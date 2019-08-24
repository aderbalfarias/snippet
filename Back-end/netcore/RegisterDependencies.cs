namespace ProjectNameApi.IoC
{
	public static class RegisterDependencies
	{
		public static IServiceCollection Services(this IServiceCollection services)
		{
			services.AddScoped<ITestService, TestService>();

			return services;
		}

		public static IServiceCollection Repositories(this IServiceCollection services)
		{
			services.AddScoped<ITestRepository, TestRepository>();

			return services;
		}

		public static IServiceCollection Databases(this IServiceCollection services, string connectionString)
		{
			// Add configuration for DbContext
			// Use connection string from appsettings.json file
			services.AddDbContext<Context>(options => options.UseSqlServer(connectionString));
			services.AddScoped<ILogger, Logger<Context>>();

			return services;
		}
	}
}
