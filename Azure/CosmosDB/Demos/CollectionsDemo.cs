using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDb.DotNetSdk.Demos
{
	public static class CollectionsDemo
	{
		public static Uri MyDbDatabaseUri =>
			UriFactory.CreateDatabaseUri("mydb");

		public async static Task Run()
		{
			Debugger.Break();

			var endpoint = ConfigurationManager.AppSettings["CosmosDbEndpoint"];
			var masterKey = ConfigurationManager.AppSettings["CosmosDbMasterKey"];

			using (var client = new DocumentClient(new Uri(endpoint), masterKey))
			{
				ViewCollections(client);

				await CreateCollection(client, "MyCollection1");
				await CreateCollection(client, "MyCollection2", 25000);
				ViewCollections(client);

				await DeleteCollection(client, "MyCollection1");
				await DeleteCollection(client, "MyCollection2");
			}
		}

		private static void ViewCollections(DocumentClient client)
		{
			Console.WriteLine();
			Console.WriteLine(">>> View Collections in mydb <<<");

			var collections = client
				.CreateDocumentCollectionQuery(MyDbDatabaseUri)
				.ToList();

			var i = 0;
			foreach (var collection in collections)
			{
				i++;
				Console.WriteLine();
				Console.WriteLine($" Collection #{i}");
				ViewCollection(collection);
			}

			Console.WriteLine();
			Console.WriteLine($"Total collections in mydb database: {collections.Count}");
		}

		private static void ViewCollection(DocumentCollection collection)
		{
			Console.WriteLine($"    Collection ID: {collection.Id}");
			Console.WriteLine($"      Resource ID: {collection.ResourceId}");
			Console.WriteLine($"        Self Link: {collection.SelfLink}");
			Console.WriteLine($"            E-Tag: {collection.ETag}");
			Console.WriteLine($"        Timestamp: {collection.Timestamp}");
		}

		private async static Task CreateCollection(
			DocumentClient client,
			string collectionId,
			int reservedRUs = 1000,
			string partitionKey = "/partitionKey")
		{
			Console.WriteLine();
			Console.WriteLine($">>> Create Collection {collectionId} in mydb <<<");
			Console.WriteLine();
			Console.WriteLine($" Throughput: {reservedRUs} RU/sec");
			Console.WriteLine($" Partition key: {partitionKey}");
			Console.WriteLine();

			var partitionKeyDefinition = new PartitionKeyDefinition();
			partitionKeyDefinition.Paths.Add(partitionKey);

			var collectionDefinition = new DocumentCollection
			{
				Id = collectionId,
				PartitionKey = partitionKeyDefinition
			};
			var options = new RequestOptions { OfferThroughput = reservedRUs };

			var result = await client.CreateDocumentCollectionAsync(MyDbDatabaseUri, collectionDefinition, options);
			var collection = result.Resource;

			Console.WriteLine("Created new collection");
			ViewCollection(collection);
		}

		private async static Task DeleteCollection(DocumentClient client, string collectionId)
		{
			Console.WriteLine();
			Console.WriteLine($">>> Delete Collection {collectionId} in mydb <<<");

			var collectionUri = UriFactory.CreateDocumentCollectionUri("mydb", collectionId);
			await client.DeleteDocumentCollectionAsync(collectionUri);

			Console.WriteLine($"Deleted collection {collectionId} from database mydb");
		}

	}
}
