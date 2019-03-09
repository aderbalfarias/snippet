using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDb.ServerSide.Demos
{
	public static class UserDefinedFunctionsDemo
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
				await CreateUserDefinedFunctions(client);

				ViewUserDefinedFunctions(client);

				Execute_udfRegEx(client);
				Execute_udfIsNorthAmerica(client);
				Execute_udfFormatCityStateZip(client);

				await DeleteUserDefinedFunctions(client);
			}
		}

		private async static Task CreateUserDefinedFunctions(DocumentClient client)
		{
			Console.WriteLine();
			Console.WriteLine(">>> Create User Defined Functions <<<");
			Console.WriteLine();

			await CreateUserDefinedFunction(client, "udfRegEx");
			await CreateUserDefinedFunction(client, "udfIsNorthAmerica");
			await CreateUserDefinedFunction(client, "udfFormatCityStateZip");
		}

		private async static Task<UserDefinedFunction> CreateUserDefinedFunction(DocumentClient client, string udfId)
		{
			var udfBody = File.ReadAllText($@"..\..\Server\{udfId}.js");
			var udfDefinition = new UserDefinedFunction
			{
				Id = udfId,
				Body = udfBody
			};

			var result = await client.CreateUserDefinedFunctionAsync(MyStoreCollectionUri, udfDefinition);
			var udf = result.Resource;
			Console.WriteLine($" Created user defined function {udf.Id}; RID: {udf.ResourceId}");

			return udf;
		}

		private static void ViewUserDefinedFunctions(DocumentClient client)
		{
			Console.WriteLine();
			Console.WriteLine(">>> View UDFs <<<");
			Console.WriteLine();

			var udfs = client
				.CreateUserDefinedFunctionQuery(MyStoreCollectionUri)
				.ToList();

			foreach (var udf in udfs)
			{
				Console.WriteLine($" User defined function {udf.Id}; RID: {udf.ResourceId}");
			}
		}

		private static void Execute_udfRegEx(DocumentClient client)
		{
			var sql = "SELECT c.id, c.name FROM c WHERE udf.udfRegEx(c.name, 'Rental') != null";

			Console.WriteLine();
			Console.WriteLine("Querying for Rental customers");
			var options = new FeedOptions { EnableCrossPartitionQuery = true };
			var documents = client.CreateDocumentQuery(MyStoreCollectionUri, sql, options).ToList();

			Console.WriteLine($"Found {documents.Count} Rental customers:");
			foreach (var document in documents)
			{
				Console.WriteLine($" {document.name} ({document.id})");
			}
		}

		private static void Execute_udfIsNorthAmerica(DocumentClient client)
		{
			var sql = @"
				SELECT c.name, c.address.countryRegionName
				FROM c
				WHERE udf.udfIsNorthAmerica(c.address.countryRegionName) = true";

			Console.WriteLine();
			Console.WriteLine("Querying for North American customers");
			var options = new FeedOptions { EnableCrossPartitionQuery = true };
			var documents = client.CreateDocumentQuery(MyStoreCollectionUri, sql, options).ToList();

			Console.WriteLine($"Found {documents.Count} North American customers; first 20:");
			foreach (var document in documents.Take(20))
			{
				Console.WriteLine($" {document.name}, {document.countryRegionName}");
			}

			sql = @"
				SELECT c.name, c.address.countryRegionName
				FROM c
				WHERE udf.udfIsNorthAmerica(c.address.countryRegionName) = false";

			Console.WriteLine();
			Console.WriteLine("Querying for non North American customers");
			documents = client.CreateDocumentQuery(MyStoreCollectionUri, sql, options).ToList();

			Console.WriteLine($"Found {documents.Count} non North American customers; first 20:");
			foreach (var document in documents.Take(20))
			{
				Console.WriteLine($" {document.name}, {document.countryRegionName}");
			}
		}

		private static void Execute_udfFormatCityStateZip(DocumentClient client)
		{
			var sql = "SELECT c.name, udf.udfFormatCityStateZip(c) AS csz FROM c";

			Console.WriteLine();
			Console.WriteLine("Listing names with city, state, zip (first 20)");

			var options = new FeedOptions { EnableCrossPartitionQuery = true };
			var documents = client.CreateDocumentQuery(MyStoreCollectionUri, sql, options).ToList();
			foreach (var document in documents.Take(20))
			{
				Console.WriteLine($" {document.name} located in {document.csz}");
			}
		}

		private async static Task DeleteUserDefinedFunctions(DocumentClient client)
		{
			Console.WriteLine();
			Console.WriteLine(">>> Delete User Defined Functions <<<");
			Console.WriteLine();

			await DeleteUserDefinedFunction(client, "udfRegEx");
			await DeleteUserDefinedFunction(client, "udfIsNorthAmerica");
			await DeleteUserDefinedFunction(client, "udfFormatCityStateZip");
		}

		private async static Task DeleteUserDefinedFunction(DocumentClient client, string udfId)
		{
			var udfUri = UriFactory.CreateUserDefinedFunctionUri("mydb", "mystore", udfId);

			await client.DeleteUserDefinedFunctionAsync(udfUri);

			Console.WriteLine($"Deleted user defined function: {udfId}");
		}

	}
}
