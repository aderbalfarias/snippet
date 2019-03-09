using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDb.ServerSide.Demos
{
	public static class TriggersDemo
	{
		public static Uri MyStoreCollectionUri =>
			UriFactory.CreateDocumentCollectionUri("mydb", "mystore");

		public async static Task Run()
		{
			Debugger.Break();

			var endpoint = ConfigurationManager.AppSettings["CosmosDbEndpoint"];
			var masterKey = ConfigurationManager.AppSettings["CosmosDbMasterKey"];

			using (var client = new DocumentClient(new Uri(endpoint), masterKey))
			{
				await CreateTriggers(client);

				ViewTriggers(client);

				await Execute_trgValidateDocument(client);
				await Execute_trgUpdateMetadata(client);

				await DeleteTriggers(client);
			}
		}

		private async static Task CreateTriggers(DocumentClient client)
		{
			Console.WriteLine();
			Console.WriteLine(">>> Create Triggers <<<");
			Console.WriteLine();

			// Create pre-trigger
			var trgValidateDocument = File.ReadAllText(@"..\..\Server\trgValidateDocument.js");
			await CreateTrigger(client, "trgValidateDocument", trgValidateDocument, TriggerType.Pre, TriggerOperation.All);

			// Create post-trigger
			var trgUpdateMetadata = File.ReadAllText(@"..\..\Server\trgUpdateMetadata.js");
			await CreateTrigger(client, "trgUpdateMetadata", trgUpdateMetadata, TriggerType.Post, TriggerOperation.Create);
		}

		private async static Task<Trigger> CreateTrigger(
			DocumentClient client,
			string triggerId,
			string triggerBody,
			TriggerType triggerType,
			TriggerOperation triggerOperation)
		{
			var triggerDefinition = new Trigger
			{
				Id = triggerId,
				Body = triggerBody,
				TriggerType = triggerType,
				TriggerOperation = triggerOperation
			};

			var result = await client.CreateTriggerAsync(MyStoreCollectionUri, triggerDefinition);
			var trigger = result.Resource;
			Console.WriteLine($" Created trigger {trigger.Id}; RID: {trigger.ResourceId}");

			return trigger;
		}

		private static void ViewTriggers(DocumentClient client)
		{
			Console.WriteLine();
			Console.WriteLine(">>> View Triggers <<<");
			Console.WriteLine();

			var triggers = client
				.CreateTriggerQuery(MyStoreCollectionUri)
				.ToList();

			foreach (var trigger in triggers)
			{
				Console.WriteLine($" Trigger: {trigger.Id};");
				Console.WriteLine($" RID: {trigger.ResourceId};");
				Console.WriteLine($" Type: {trigger.TriggerType};");
				Console.WriteLine($" Operation: {trigger.TriggerOperation}");
				Console.WriteLine();
			}
		}

		private static async Task Execute_trgValidateDocument(DocumentClient client)
		{
			// Create three documents
			var doc1Link = await CreateDocumentWithValidation(client, "mon");		// Monday
			var doc2Link = await CreateDocumentWithValidation(client, "THURS");		// Thursday
			var doc3Link = await CreateDocumentWithValidation(client, "sonday");	// error - won't get created

			// Update one of them
			await UpdateDocumentWithValidation(client, doc2Link, "FRI");			// Thursday > Friday

			// Delete them
			var requestOptions = new RequestOptions { PartitionKey = new PartitionKey("12345") };
			await client.DeleteDocumentAsync(doc1Link, requestOptions);
			await client.DeleteDocumentAsync(doc2Link, requestOptions);
		}

		private async static Task<string> CreateDocumentWithValidation(DocumentClient client, string weekdayOff)
		{
			dynamic documentDefinition = new
			{
				name = "John Doe",
				address = new { postalCode = "12345" },
				weekdayOff = weekdayOff
			};

			var options = new RequestOptions { PreTriggerInclude = new[] { "trgValidateDocument" } };

			try
			{
				var result = await client.CreateDocumentAsync(MyStoreCollectionUri, documentDefinition, options);
				var document = result.Resource;

				Console.WriteLine(" Result:");
				Console.WriteLine($"  Id = {document.id}");
				Console.WriteLine($"  Weekday off = {document.weekdayOff}");
				Console.WriteLine($"  Weekday # off = {document.weekdayNumberOff}");
				Console.WriteLine();

				return document._self;
			}
			catch (DocumentClientException ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
				Console.WriteLine();

				return null;
			}
		}

		private async static Task UpdateDocumentWithValidation(DocumentClient client, string documentLink, string weekdayOff)
		{
			var sql = $"SELECT * FROM c WHERE c._self = '{documentLink}'";
			var feedOptions = new FeedOptions { EnableCrossPartitionQuery = true };
			var document = client.CreateDocumentQuery(MyStoreCollectionUri, sql, feedOptions).AsEnumerable().FirstOrDefault();

			document.weekdayOff = weekdayOff;

			var options = new RequestOptions { PreTriggerInclude = new[] { "trgValidateDocument" } };
			var result = await client.ReplaceDocumentAsync(document._self, document, options);
			document = result.Resource;

			Console.WriteLine(" Result:");
			Console.WriteLine($"  Id = {document.id}");
			Console.WriteLine($"  Weekday off = {document.weekdayOff}");
			Console.WriteLine($"  Weekday # off = {document.weekdayNumberOff}");
			Console.WriteLine();
		}

		private async static Task Execute_trgUpdateMetadata(DocumentClient client)
		{
			// Show no metadata documents
			ViewMetaDocs(client);

			// Create a bunch of documents across two partition keys
			var docs = new List<dynamic>
			{
				// 11229
				new { id = "11229a", address = new { postalCode = "11229" }, name = "New Customer ABCD" },
				new { id = "11229b", address = new { postalCode = "11229" }, name = "New Customer ABC" },
				new { id = "11229c", address = new { postalCode = "11229" }, name = "New Customer AB" },			// smallest
				new { id = "11229d", address = new { postalCode = "11229" }, name = "New Customer ABCDEF" },
				new { id = "11229e", address = new { postalCode = "11229" }, name = "New Customer ABCDEFG" },		// largest
				new { id = "11229f", address = new { postalCode = "11229" }, name = "New Customer ABCDE" },
				// 11235
				new { id = "11235a", address = new { postalCode = "11235" }, name = "New Customer AB" },
				new { id = "11235b", address = new { postalCode = "11235" }, name = "New Customer ABCDEFGHIJKL" },	// largest
				new { id = "11235c", address = new { postalCode = "11235" }, name = "New Customer ABC" },
				new { id = "11235d", address = new { postalCode = "11235" }, name = "New Customer A" },				// smallest
				new { id = "11235e", address = new { postalCode = "11235" }, name = "New Customer ABC" },
				new { id = "11235f", address = new { postalCode = "11235" }, name = "New Customer ABCDE" },
			};

			var options = new RequestOptions { PostTriggerInclude = new[] { "trgUpdateMetadata" } };
			foreach (var doc in docs)
			{
				await client.CreateDocumentAsync(MyStoreCollectionUri, doc, options);
			}

			// Show two metadata documents
			ViewMetaDocs(client);

			// Cleanup
			var sql = @"
				SELECT c._self, c.address.postalCode
				FROM c
				WHERE c.address.postalCode IN('11229', '11235')
			";

			var feedOptions = new FeedOptions { EnableCrossPartitionQuery = true };
			var documentKeys = client.CreateDocumentQuery(MyStoreCollectionUri, sql, feedOptions).ToList();
			foreach (var documentKey in documentKeys)
			{
				var requestOptions = new RequestOptions { PartitionKey = new PartitionKey(documentKey.postalCode) };
				await client.DeleteDocumentAsync(documentKey._self, requestOptions);
			}
		}

		private static void ViewMetaDocs(DocumentClient client)
		{
			var sql = @"SELECT * FROM c WHERE c.isMetaDoc";

			var feedOptions = new FeedOptions { EnableCrossPartitionQuery = true };
			var metaDocs = client.CreateDocumentQuery(MyStoreCollectionUri, sql, feedOptions).ToList();

			Console.WriteLine();
			Console.WriteLine($" Found {metaDocs.Count} metadata documents:");
			foreach (var metaDoc in metaDocs)
			{
				Console.WriteLine();
				Console.WriteLine($"  MetaDoc ID: {metaDoc.id}");
				Console.WriteLine($"  Metadata for: {metaDoc.address.postalCode}");
				Console.WriteLine($"  Smallest doc size: {metaDoc.minSize} ({metaDoc.minSizeId})");
				Console.WriteLine($"  Largest doc size: {metaDoc.maxSize} ({metaDoc.maxSizeId})");
				Console.WriteLine($"  Total doc size: {metaDoc.totalSize}");
			}
		}

		private async static Task DeleteTriggers(DocumentClient client)
		{
			Console.WriteLine();
			Console.WriteLine(">>> Delete Triggers <<<");
			Console.WriteLine();

			await DeleteTrigger(client, "trgValidateDocument");
			await DeleteTrigger(client, "trgUpdateMetadata");
		}

		private async static Task DeleteTrigger(DocumentClient client, string triggerId)
		{
			var triggerUri = UriFactory.CreateTriggerUri("mydb", "mystore", triggerId);

			await client.DeleteTriggerAsync(triggerUri);

			Console.WriteLine($"Deleted trigger: {triggerId}");
		}

	}
}
