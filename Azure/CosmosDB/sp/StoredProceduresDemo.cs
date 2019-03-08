using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDb.ServerSide.Demos
{
	public static class StoredProceduresDemo
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
				await CreateStoredProcedures(client);

				ViewStoredProcedures(client);

				await ExecuteStoredProcedures(client);

				await DeleteStoredProcedures(client);
			}
		}

		// Create stored procedures

		private async static Task CreateStoredProcedures(DocumentClient client)
		{
			Console.WriteLine();
			Console.WriteLine(">>> Create Stored Procedures <<<");
			Console.WriteLine();

			await CreateStoredProcedure(client, "spHelloWorld");
			await CreateStoredProcedure(client, "spSetNorthAmerica");
			await CreateStoredProcedure(client, "spEnsureUniqueId");
			await CreateStoredProcedure(client, "spBulkInsert");
			await CreateStoredProcedure(client, "spBulkDelete");
		}

		private async static Task<StoredProcedure> CreateStoredProcedure(DocumentClient client, string sprocId)
		{
			var sprocBody = File.ReadAllText($@"..\..\Server\{sprocId}.js");

			var sprocDefinition = new StoredProcedure
			{
				Id = sprocId,
				Body = sprocBody
			};

			var result = await client.CreateStoredProcedureAsync(MyStoreCollectionUri, sprocDefinition);
			var sproc = result.Resource;
			Console.WriteLine($"Created stored procedure {sproc.Id}; RID: {sproc.ResourceId}");

			return result;
		}

		// View stored procedures

		private static void ViewStoredProcedures(DocumentClient client)
		{
			Console.WriteLine();
			Console.WriteLine(">>> View Stored Procedures <<<");
			Console.WriteLine();

			var sprocs = client
				.CreateStoredProcedureQuery(MyStoreCollectionUri)
				.ToList();

			foreach (var sproc in sprocs)
			{
				Console.WriteLine($"Stored procedure {sproc.Id}; RID: {sproc.ResourceId}");
			}
		}

		// Execute stored procedures

		private async static Task ExecuteStoredProcedures(DocumentClient client)
		{
			await Execute_spHelloWorld(client);
			await Execute_spSetNorthAmerica1(client);
			await Execute_spSetNorthAmerica2(client);
			await Execute_spSetNorthAmerica3(client);
			await Execute_spEnsureUniqueId(client);
			await Execute_spBulkInsert(client);
			await Execute_spBulkDelete(client);
		}

		private async static Task Execute_spHelloWorld(DocumentClient client)
		{
			Console.WriteLine();
			Console.WriteLine("Execute spHelloWorld stored procedure");

			var uri = UriFactory.CreateStoredProcedureUri("mydb", "mystore", "spHelloWorld");
			var options = new RequestOptions { PartitionKey = new PartitionKey(string.Empty) };
			var result = await client.ExecuteStoredProcedureAsync<string>(uri, options);
			var message = result.Response;

			Console.WriteLine($"Result: {message}");
		}

		private async static Task Execute_spSetNorthAmerica1(DocumentClient client)
		{
			Console.WriteLine();
			Console.WriteLine("Execute spSetNorthAmerica (country = United States)");

			// Should succeed with isNorthAmerica = true
			dynamic documentDefinition = new
			{
				name = "John Doe",
				address = new
				{
					countryRegionName = "United States",
					postalCode = "12345"
				}
			};
			var uri = UriFactory.CreateStoredProcedureUri("mydb", "mystore", "spSetNorthAmerica");
			var options = new RequestOptions { PartitionKey = new PartitionKey("12345") };
			var result = await client.ExecuteStoredProcedureAsync<object>(uri, options, documentDefinition, true);
			var document = result.Response;

			var id = document.id;
			var country = document.address.countryRegionName;
			var isNA = document.address.isNorthAmerica;

			Console.WriteLine("Result:");
			Console.WriteLine($" Id = {id}");
			Console.WriteLine($" Country = {country}");
			Console.WriteLine($" Is North America = {isNA}");

			string documentLink = document._self;
			await client.DeleteDocumentAsync(documentLink, options);
		}

		private async static Task Execute_spSetNorthAmerica2(DocumentClient client)
		{
			Console.WriteLine();
			Console.WriteLine("Execute spSetNorthAmerica (country = United Kingdom)");

			// Should succeed with isNorthAmerica = false
			dynamic documentDefinition = new
			{
				name = "John Doe",
				address = new
				{
					countryRegionName = "United Kingdom",
					postalCode = "RG41 1QW"
				}
			};
			var uri = UriFactory.CreateStoredProcedureUri("mydb", "mystore", "spSetNorthAmerica");
			var options = new RequestOptions { PartitionKey = new PartitionKey("RG41 1QW") };
			var result = await client.ExecuteStoredProcedureAsync<object>(uri, options, documentDefinition, true);
			var document = result.Response;

			// Deserialize new document as JObject (use dictionary-style indexers to access dynamic properties)
			var documentObject = JsonConvert.DeserializeObject(document.ToString());

			var id = documentObject["id"];
			var country = documentObject["address"]["countryRegionName"];
			var isNA = documentObject["address"]["isNorthAmerica"];

			Console.WriteLine("Result:");
			Console.WriteLine($" Id = {id}");
			Console.WriteLine($" Country = {country}");
			Console.WriteLine($" Is North America = {isNA}");

			string documentLink = document._self;
			await client.DeleteDocumentAsync(documentLink, options);
		}

		private async static Task Execute_spSetNorthAmerica3(DocumentClient client)
		{
			Console.WriteLine();
			Console.WriteLine("Execute spSetNorthAmerica (no country)");

			// Should fail with no country and enforceSchema = true
			try
			{
				dynamic documentDefinition = new
				{
					name = "James Smith",
					address = new
					{
						postalCode = "12345"
					}
				};
				var uri = UriFactory.CreateStoredProcedureUri("mydb", "mystore", "spSetNorthAmerica");
				var options = new RequestOptions { PartitionKey = new PartitionKey("12345") };
				var result = await client.ExecuteStoredProcedureAsync<object>(uri, options, documentDefinition, true);
			}
			catch (DocumentClientException ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}
		}

		private async static Task Execute_spEnsureUniqueId(DocumentClient client)
		{
			Console.WriteLine();
			Console.WriteLine("Execute spEnsureUniqueId");

			dynamic documentDefinition1 = new { id = "DUPEJ", name = "James Dupe", address = new { postalCode = "12345" } };
			dynamic documentDefinition2 = new { id = "DUPEJ", name = "John Dupe", address = new { postalCode = "12345" } };
			dynamic documentDefinition3 = new { id = "DUPEJ", name = "Justin Dupe", address = new { postalCode = "12345" } };

			var uri = UriFactory.CreateStoredProcedureUri("mydb", "mystore", "spEnsureUniqueId");
			var options = new RequestOptions { PartitionKey = new PartitionKey("12345") };

			var result1 = await client.ExecuteStoredProcedureAsync<object>(uri, options, documentDefinition1);
			var document1 = result1.Response;
			Console.WriteLine($"New document ID: {document1.id}");

			var result2 = await client.ExecuteStoredProcedureAsync<object>(uri, options, documentDefinition2);
			var document2 = result2.Response;
			Console.WriteLine($"New document ID: {document2.id}");

			var result3 = await client.ExecuteStoredProcedureAsync<object>(uri, options, documentDefinition3);
			var document3 = result3.Response;
			Console.WriteLine($"New document ID: {document3.id}");

			// cleanup
			await client.DeleteDocumentAsync(document1._self.ToString(), options);
			await client.DeleteDocumentAsync(document2._self.ToString(), options);
			await client.DeleteDocumentAsync(document3._self.ToString(), options);
		}

		private async static Task Execute_spBulkInsert(DocumentClient client)
		{
			Console.WriteLine();
			Console.WriteLine("Execute spBulkInsert");

			var docs = new List<dynamic>();
			var total = 5000;
			for (var i = 1; i <= total; i++)
			{
				dynamic doc = new
				{
					name = $"Bulk inserted doc {i}",
					address = new
					{
						postalCode = "12345"
					}
				};
				docs.Add(doc);
			}

			var uri = UriFactory.CreateStoredProcedureUri("mydb", "mystore", "spBulkInsert");
			var options = new RequestOptions { PartitionKey = new PartitionKey("12345") };

			var totalInserted = 0;
			while (totalInserted < total)
			{
				var result = await client.ExecuteStoredProcedureAsync<int>(uri, options, docs);
				var inserted = result.Response;
				totalInserted += inserted;
				var remaining = total - totalInserted;
				Console.WriteLine($"Inserted {inserted} documents ({totalInserted} total, {remaining} remaining)");
				docs = docs.GetRange(inserted, docs.Count - inserted);
			}
		}

		private async static Task Execute_spBulkDelete(DocumentClient client)
		{
			Console.WriteLine();
			Console.WriteLine("Execute spBulkDelete");

			// query retrieves self-links for documents to bulk-delete
			var sql = "SELECT VALUE c._self FROM c WHERE STARTSWITH(c.name, 'Bulk inserted doc ') = true";
			var count = await Execute_spBulkDelete(client, sql);
			Console.WriteLine($"Deleted bulk inserted documents; count: {count}");
			Console.WriteLine();
		}

		private async static Task<int> Execute_spBulkDelete(DocumentClient client, string sql)
		{
			var uri = UriFactory.CreateStoredProcedureUri("mydb", "mystore", "spBulkDelete");
			var options = new RequestOptions { PartitionKey = new PartitionKey("12345") };

			var continuationFlag = true;
			var totalDeleted = 0;
			while (continuationFlag)
			{
				var result = await client.ExecuteStoredProcedureAsync<spBulkDeleteResponse>(uri, options, sql);
				var response = result.Response;
				continuationFlag = response.ContinuationFlag;
				var deleted = response.Count;
				totalDeleted += deleted;
				Console.WriteLine($"Deleted {deleted} documents ({totalDeleted} total, more: {continuationFlag})");
			}

			return totalDeleted;
		}

		// Delete stored procedures

		private async static Task DeleteStoredProcedures(DocumentClient client)
		{
			Console.WriteLine();
			Console.WriteLine(">>> Delete Stored Procedures <<<");
			Console.WriteLine();

			await DeleteStoredProcedure(client, "spHelloWorld");
			await DeleteStoredProcedure(client, "spSetNorthAmerica");
			await DeleteStoredProcedure(client, "spEnsureUniqueId");
			await DeleteStoredProcedure(client, "spBulkInsert");
			await DeleteStoredProcedure(client, "spBulkDelete");
		}

		private async static Task DeleteStoredProcedure(DocumentClient client, string sprocId)
		{
			var sprocUri = UriFactory.CreateStoredProcedureUri("mydb", "mystore", sprocId);

			await client.DeleteStoredProcedureAsync(sprocUri);

			Console.WriteLine($"Deleted stored procedure: {sprocId}");
		}

	}

	public class spBulkDeleteResponse
	{
		[JsonProperty(PropertyName = "count")]
		public int Count { get; set; }

		[JsonProperty(PropertyName = "continuationFlag")]
		public bool ContinuationFlag { get; set; }
	}

}
