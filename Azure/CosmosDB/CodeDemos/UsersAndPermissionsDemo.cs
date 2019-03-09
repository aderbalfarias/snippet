using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDb.DotNetSdk.Demos
{
	public static class UsersAndPermissionsDemo
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
				ViewUsers(client);

				var alice = await CreateUser(client, "Alice");
				var tom = await CreateUser(client, "Tom");
				ViewUsers(client);

				ViewPermissions(client, alice);
				ViewPermissions(client, tom);

				var sql = "SELECT VALUE c._self FROM c WHERE c.id = 'mystore'";
				string collectionSelfLink = client.CreateDocumentCollectionQuery(MyDbDatabaseUri, sql).AsEnumerable().First().Value;

				var alicePerm = await CreatePermission(client, alice, "AliceCollectionAccess", PermissionMode.All, collectionSelfLink);
				var tomPerm = await CreatePermission(client, tom, "TomCollectionAccess", PermissionMode.Read, collectionSelfLink);

				ViewPermissions(client, alice);
				ViewPermissions(client, tom);

				await TestPermissions(client, alice, collectionSelfLink);
				await TestPermissions(client, tom, collectionSelfLink);

				await DeletePermission(client, alice, alicePerm);
				await DeletePermission(client, tom, tomPerm);

				await DeleteUser(client, "Alice");
				await DeleteUser(client, "Tom");
			}
		}

		// Users

		private static void ViewUsers(DocumentClient client)
		{
			Console.WriteLine();
			Console.WriteLine(">>> View Users in mydb <<<");

			var users = client.CreateUserQuery(MyDbDatabaseUri).ToList();

			var i = 0;
			foreach (var user in users)
			{
				i++;
				Console.WriteLine();
				Console.WriteLine($" User #{i}");
				ViewUser(user);
			}

			Console.WriteLine();
			Console.WriteLine($"Total users in database mydb: {users.Count}");
		}

		private static void ViewUser(User user)
		{
			Console.WriteLine($"          User ID: {user.Id}");
			Console.WriteLine($"      Resource ID: {user.ResourceId}");
			Console.WriteLine($"        Self Link: {user.SelfLink}");
			Console.WriteLine($" Permissions Link: {user.PermissionsLink}");
			Console.WriteLine($"        Timestamp: {user.Timestamp}");
		}

		private async static Task<User> CreateUser(DocumentClient client, string userId)
		{
			Console.WriteLine();
			Console.WriteLine($">>> Create User {userId} <<<");

			var userDefinition = new User { Id = userId };
			var result = await client.CreateUserAsync(MyDbDatabaseUri, userDefinition);
			var user = result.Resource;

			Console.WriteLine("Created new user");
			ViewUser(user);

			return user;
		}

		private async static Task DeleteUser(DocumentClient client, string userId)
		{
			Console.WriteLine();
			Console.WriteLine($">>> Delete User {userId} <<<");

			var userUri = UriFactory.CreateUserUri("mydb", userId);
			await client.DeleteUserAsync(userUri);

			Console.WriteLine($"Deleted user {userId}");
		}

		// Permissions

		private static void ViewPermissions(DocumentClient client, User user)
		{
			Console.WriteLine();
			Console.WriteLine($">>> View Permissions for {user.Id} <<<");

			var perms = client.CreatePermissionQuery(user.PermissionsLink).ToList();

			var i = 0;
			foreach (var perm in perms)
			{
				i++;
				Console.WriteLine();
				Console.WriteLine($"Permission #{i}");
				ViewPermission(perm);
			}

			Console.WriteLine();
			Console.WriteLine($"Total permissions for {user.Id}: {perms.Count}");
		}

		private static void ViewPermission(Permission perm)
		{
			Console.WriteLine($"    Permission ID: {perm.Id}");
			Console.WriteLine($"      Resource ID: {perm.ResourceId}");
			Console.WriteLine($"  Permission Mode: {perm.PermissionMode}");
			Console.WriteLine($"            Token: {perm.Token}");
			Console.WriteLine($"        Timestamp: {perm.Timestamp}");
		}

		private async static Task<Permission> CreatePermission(DocumentClient client, User user, string permId, PermissionMode permissionMode, string resourceLink)
		{
			Console.WriteLine();
			Console.WriteLine($">>> Create Permission {permId} for {user.Id} <<<");

			var permDefinition = new Permission { Id = permId, PermissionMode = permissionMode, ResourceLink = resourceLink };
			var result = await client.CreatePermissionAsync(user.SelfLink, permDefinition);
			var perm = result.Resource;

			Console.WriteLine("Created new permission");
			ViewPermission(perm);

			return perm;
		}

		private async static Task DeletePermission(DocumentClient client, User user, string permId)
		{
			Console.WriteLine();
			Console.WriteLine($">>> Delete Permission {permId} from {user.Id} <<<");

			var permUri = UriFactory.CreatePermissionUri("mydb", "mystore", permId);
			await client.DeletePermissionAsync(permUri);

			Console.WriteLine("Deleted permission {permId} from user {user.Id}");
		}

		private async static Task DeletePermission(DocumentClient client, User user, Permission perm)
		{
			Console.WriteLine();
			Console.WriteLine($">>> Delete Permission {perm.Id} from {user.Id} <<<");

			await client.DeletePermissionAsync(perm.SelfLink);

			Console.WriteLine("Deleted permission {permId} from user {user.Id}");
		}

		private async static Task TestPermissions(DocumentClient client, User user, string collectionLink)
		{
			var perm = client.CreatePermissionQuery(user.PermissionsLink)
				.AsEnumerable()
				.First(p => p.ResourceLink == collectionLink);

			var resourceToken = perm.Token;

			dynamic documentDefinition = new
			{
				name = "New Customer 1",
				address = new
				{
					addressType = "Main Office",
					addressLine1 = "123 Main Street",
					location = new
					{
						city = "Brooklyn",
						stateProvinceName = "New York"
					},
					postalCode = "11229",
					countryRegionName = "United States"
				},
			};

			Console.WriteLine();
			Console.WriteLine($"Trying to create & delete document as user {user.Id}");
			try
			{
				var endpoint = ConfigurationManager.AppSettings["CosmosDbEndpoint"];
				using (var restrictedClient = new DocumentClient(new Uri(endpoint), resourceToken))
				{
					var document = await restrictedClient.CreateDocumentAsync(collectionLink, documentDefinition);
					Console.WriteLine($"Successfully created document: {document.Resource.id}");

					var options = new RequestOptions { PartitionKey = new PartitionKey("11229") };
					await restrictedClient.DeleteDocumentAsync(document.Resource._self, options);
					Console.WriteLine($"Successfully deleted document: {document.Resource.id}");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"ERROR: {ex.Message}");
			}
		}

	}
}
