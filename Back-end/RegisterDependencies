namespace ProjectNameApi.IoC
{
    public static class RegisterDependencies
    {
        public static void Services(IServiceCollection services)
        {
            services.AddScoped<ITestService, TestService>();
        }

        public static void Repositories(IServiceCollection services)
        {
            services.AddScoped<ITestRepository, TestRepository>();
        }

        public static void Database(IServiceCollection services, string connectionString)
        {
            // Add configuration for DbContext
            // Use connection string from appsettings.json file
            services.AddDbContext<Context>(options => options.UseSqlServer(connectionString));
            services.AddScoped<ILogger, Logger<Context>>();
        }
    }
}
