using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDb.DotNetSdk.Demos
{
	public static class IndexingDemo
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
				await AutomaticIndexing(client);
				await ManualIndexing(client);
				await SetIndexPaths(client);
			}
		}

		private async static Task AutomaticIndexing(DocumentClient client)
		{
			Console.WriteLine();
			Console.WriteLine(">>> Override Automatic Indexing <<<");

			// Create collection with automatic indexing
			var partitionKeyDefinition = new PartitionKeyDefinition();
			partitionKeyDefinition.Paths.Add("/id");
			var collectionDefinition = new DocumentCollection
			{
				Id = "autoindexing",
				PartitionKey = partitionKeyDefinition
			};
			var options = new RequestOptions { OfferThroughput = 1000 };
			var collection = await client.CreateDocumentCollectionAsync(MyDbDatabaseUri, collectionDefinition, options);
			var collectionUri = UriFactory.CreateDocumentCollectionUri("mydb", "autoindexing");

			// Add a document (indexed)
			dynamic indexedDocumentDefinition = new
			{
				id = "JOHN",
				firstName = "John",
				lastName = "Doe",
				addressLine = "123 Main Street",
				city = "Brooklyn",
				state = "New York",
				zip = "11229",
			};
			Document indexedDocument =
				await client.CreateDocumentAsync(collectionUri, indexedDocumentDefinition);

			// Add another document (request no indexing)
			dynamic unindexedDocumentDefinition = new
			{
				id = "JANE",
				firstName = "Jane",
				lastName = "Doe",
				addressLine = "123 Main Street",
				city = "Brooklyn",
				state = "New York",
				zip = "11229",
			};
			var requestOptions = new RequestOptions { IndexingDirective = IndexingDirective.Exclude };
			Document unindexedDocument =
				await client.CreateDocumentAsync(collectionUri, unindexedDocumentDefinition, requestOptions);

			var feedOptions = new FeedOptions { EnableCrossPartitionQuery = true };

			// Unindexed document won't get returned when querying on non-ID (or self-link) property

			var doeDocs = client
				.CreateDocumentQuery(collectionUri, "SELECT * FROM c WHERE c.lastName = 'Doe'", feedOptions)
				.ToList();

			Console.WriteLine($"Documents WHERE lastName = 'Doe': {doeDocs.Count}");
			foreach (var doeDoc in doeDocs)
			{
				Console.WriteLine($" ID: {doeDoc.id}, Name: {doeDoc.firstName} {doeDoc.lastName}");
			}
			Console.WriteLine();

			// Unindexed document will get returned when using no WHERE clause
			var allDocs = client.CreateDocumentQuery(collectionUri, "SELECT * FROM c", feedOptions).ToList();
			Console.WriteLine($"All documents: {allDocs.Count}");
			foreach (var doc in allDocs)
			{
				Console.WriteLine($" ID: {doc.id}, Name: {doc.firstName} {doc.lastName}");
			}
			Console.WriteLine();

			// Unindexed document will get returned when querying by ID (or self-link) property
			var janeDoc = client
				.CreateDocumentQuery(collectionUri, "SELECT * FROM c WHERE c.id = 'JANE'", feedOptions)
				.AsEnumerable()
				.FirstOrDefault();

			Console.WriteLine($"Unindexed document:");
			Console.WriteLine($" ID: {janeDoc.id}, Name: {janeDoc.firstName} {janeDoc.lastName}");

			// Delete the collection
			await client.DeleteDocumentCollectionAsync(collectionUri);
		}

		private async static Task ManualIndexing(DocumentClient client)
		{
			Console.WriteLine();
			Console.WriteLine(">>> Manual Indexing <<<");

			// Create collection with manual indexing
			var partitionKeyDefinition = new PartitionKeyDefinition();
			partitionKeyDefinition.Paths.Add("/id");
			var collectionDefinition = new DocumentCollection
			{
				Id = "manualindexing",
				PartitionKey = partitionKeyDefinition,
				IndexingPolicy = new IndexingPolicy
				{
					Automatic = false,
				},
			};
			var options = new RequestOptions { OfferThroughput = 1000 };
			var collection = await client.CreateDocumentCollectionAsync(MyDbDatabaseUri, collectionDefinition, options);
			var collectionUri = UriFactory.CreateDocumentCollectionUri("mydb", "manualindexing");

			// Add a document (unindexed)
			dynamic unindexedDocumentDefinition = new
			{
				id = "JOHN",
				firstName = "John",
				lastName = "Doe",
				addressLine = "123 Main Street",
				city = "Brooklyn",
				state = "New York",
				zip = "11229",
			};
			Document unindexedDocument = await client.CreateDocumentAsync(collectionUri, unindexedDocumentDefinition);

			// Add another document (request indexing)
			dynamic indexedDocumentDefinition = new
			{
				id = "JANE",
				firstName = "Jane",
				lastName = "Doe",
				addressLine = "123 Main Street",
				city = "Brooklyn",
				state = "New York",
				zip = "11229",
			};
			var requestOptions = new RequestOptions { IndexingDirective = IndexingDirective.Include };
			Document indexedDocument = await client.CreateDocumentAsync(collectionUri, indexedDocumentDefinition, requestOptions);

			var feedOptions = new FeedOptions { EnableCrossPartitionQuery = true };

			// Unindexed document won't get returned when querying on non-ID (or self-link) property
			var doeDocs = client
				.CreateDocumentQuery(collectionUri, "SELECT * FROM c WHERE c.lastName = 'Doe'", feedOptions)
				.ToList();

			Console.WriteLine($"Documents WHERE lastName = 'Doe': {doeDocs.Count}");
			foreach (var doeDoc in doeDocs)
			{
				Console.WriteLine($" ID: {doeDoc.id}, Name: {doeDoc.firstName} {doeDoc.lastName}");
			}
			Console.WriteLine();

			// Unindexed document will get returned when using no WHERE clause
			var allDocs = client.CreateDocumentQuery(collectionUri, "SELECT * FROM c", feedOptions).ToList();
			Console.WriteLine($"All documents: {allDocs.Count}");
			foreach (var doc in allDocs)
			{
				Console.WriteLine($" ID: {doc.id}, Name: {doc.firstName} {doc.lastName}");
			}
			Console.WriteLine();

			// Unindexed document will get returned when querying by ID (or self-link) property
			var johnDoc = client
				.CreateDocumentQuery(collectionUri, "SELECT * FROM c WHERE c.id = 'JOHN'")
				.AsEnumerable()
				.FirstOrDefault();

			Console.WriteLine($"Unindexed document:");
			Console.WriteLine($" ID: {johnDoc.id}, Name: {johnDoc.firstName} {johnDoc.lastName}");

			// Delete the collection
			await client.DeleteDocumentCollectionAsync(collectionUri);
		}

		private async static Task SetIndexPaths(DocumentClient client)
		{
			Console.WriteLine();
			Console.WriteLine(">>> Set Custom Index Paths <<<");

			// Create collection with custom indexing paths
			var partitionKeyDefinition = new PartitionKeyDefinition();
			partitionKeyDefinition.Paths.Add("/id");
			var collectionDefinition = new DocumentCollection
			{
				Id = "customindexing",
				PartitionKey = partitionKeyDefinition,
				IndexingPolicy = new IndexingPolicy
				{
					IncludedPaths = new Collection<IncludedPath> 
					{
						// The Title property in the root is the only string property we need to sort on
						new IncludedPath
						{
							Path = "/title/?",
							Indexes = new Collection<Index>
							{
								new RangeIndex(DataType.String)
							}
						},
						// Every property (also the Title) gets a hash index on strings, and a range index on numbers
						new IncludedPath
						{
							Path = "/*",
							Indexes = new Collection<Index>
							{
								new HashIndex(DataType.String),
								new RangeIndex(DataType.Number),
							}
						}
					},
					ExcludedPaths = new Collection<ExcludedPath>
					{
						new ExcludedPath
						{
							Path = "/misc/*",
						}
					}
				},
			};
			var options = new RequestOptions { OfferThroughput = 1000 };
			var collection = await client.CreateDocumentCollectionAsync(MyDbDatabaseUri, collectionDefinition, options);
			var collectionUri = UriFactory.CreateDocumentCollectionUri("mydb", "customindexing");

			// Add some documents
			dynamic doc1Definition = new
			{
				id = "SW4",
				title = "Star Wars IV - A New Hope",
				rank = 600,
				category = "Sci-Fi",
				misc = new
				{
					year = 1977,
					length = "2hr 1min"
				}
			};
			Document doc1 = await client.CreateDocumentAsync(collectionUri, doc1Definition);

			dynamic doc2Definition = new
			{
				id = "GF",
				title = "Godfather",
				rank = 500,
				category = "Crime Drama",
				misc = new {
					year = 1972,
					length = "2hr 55min"
				}
			};
			Document doc2 = await client.CreateDocumentAsync(collectionUri, doc2Definition);

			dynamic doc3Definition = new
			{
				id = "LOTR1",
				title = "Lord Of The Rings - Fellowship of the Ring",
				rank = 700,
				category = "Fantasy",
				misc = new
				{
					year = 2001,
					length = "2hr 58min"
				}
			};
			Document doc3 = await client.CreateDocumentAsync(collectionUri, doc3Definition);

			// All the queries in this demo are cross-partition queries
			var allowCrossPartitionQuery = new FeedOptions { EnableCrossPartitionQuery = true };

			// When trying a range query when a range index is not availalbe, must also explicitly enable scan in query
			var allowCrossPartitionAndScanQuery = new FeedOptions { EnableCrossPartitionQuery = true, EnableScanInQuery = true };


			// *** Querying (WHERE) ***

			// Equality on title string property (range and hash index available)
			var sql = "SELECT * FROM c WHERE c.title = 'Godfather'";
			var queryByTitle = client.CreateDocumentQuery(collectionUri, sql, allowCrossPartitionQuery).ToList();

			// Equality on category string property (hash index available)
			sql = "SELECT * FROM c WHERE c.category = 'Fantasy'";
			var queryByCategory = client.CreateDocumentQuery(collectionUri, sql, allowCrossPartitionQuery).ToList();

			// Range on category string property (hash index can't be used, no range index available)
			sql = "SELECT * FROM c WHERE c.category >= 'Fantasy'";
			try
			{
				var queryByCategoryRange = client.CreateDocumentQuery(collectionUri, sql, allowCrossPartitionQuery).ToList();
			}
			catch
			{
				var queryByCategoryRange = client.CreateDocumentQuery(collectionUri, sql, allowCrossPartitionAndScanQuery).ToList();
			}

			// Equality on rank number property (range index available)
			sql = "SELECT * FROM c WHERE c.rank = 500";
			var queryByRank = client.CreateDocumentQuery(collectionUri, sql, allowCrossPartitionQuery).ToList();

			// Range on rank number property (range index available)
			sql = "SELECT * FROM c WHERE c.rank > 500";
			var queryByRankRange = client.CreateDocumentQuery(collectionUri, sql, allowCrossPartitionQuery).ToList();

			// Equality on year number property (no index available; scan required)
			sql = "SELECT * FROM c WHERE c.misc.year = 2001";
			try
			{
				var queryByYear = client.CreateDocumentQuery(collectionUri, sql, allowCrossPartitionQuery).ToList();
			}
			catch
			{
				var queryByYear = client.CreateDocumentQuery(collectionUri, sql, allowCrossPartitionAndScanQuery).ToList();
			}

			// Equality on length string property (no index available; scan required)
			sql = "SELECT * FROM c WHERE c.misc.length = '2hr 58min'";
			try
			{
				var queryByLength = client.CreateDocumentQuery(collectionUri, sql, allowCrossPartitionQuery).ToList();
			}
			catch
			{
				var queryByLength = client.CreateDocumentQuery(collectionUri, sql, allowCrossPartitionAndScanQuery).ToList();
			}


			// *** Sorting (ORDER BY) ***

			// Works with range index on title property strings
			sql = "SELECT * FROM c ORDER BY c.title";
			var sortByTitle = client.CreateDocumentQuery(collectionUri, sql, allowCrossPartitionQuery).ToList();

			// Doesn't works without range index on category property strings (returns 0 documents, but doesn't throw error!)
			sql = "SELECT * FROM c ORDER BY c.category";
			var sortByCategory = client.CreateDocumentQuery(collectionUri, sql, allowCrossPartitionQuery).ToList();

			// Works with range index on rank property numbers
			sql = "SELECT * FROM c ORDER BY c.rank";
			var sortByRank = client.CreateDocumentQuery(collectionUri, sql, allowCrossPartitionQuery).ToList();

			// Delete the collection
			await client.DeleteDocumentCollectionAsync(collectionUri);
		}

	}
}
