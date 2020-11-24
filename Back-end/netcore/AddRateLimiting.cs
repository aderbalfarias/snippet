public static IServiceCollection AddRateLimiting(this IServiceCollection services, IConfiguration configuration)
{
    return services
        // needed to load configuration from appsettings.json
        .AddOptions()

        // needed to store rate limit counters and ip rules
        .AddMemoryCache()

        //load general configuration from appsettings.json
        .Configure<ClientRateLimitOptions>(configuration.GetSection(RateLimiting.ClientRateLimiting))

        //load client rules from appsettings.json
        .Configure<ClientRateLimitPolicies>(configuration.GetSection(RateLimiting.ClientRateLimitPolicies))

        // inject counter and rules stores
        .AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>()
        .AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>()

        // configure the resolvers
        .AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>()

        .AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

    /*
     * If you load-balance your app, you'll need to use IDistributedCache with Redis or SQLServer so that all kestrel instances
     * will have the same rate limit store. Instead of the in-memory stores you should inject the distributed stores like this:
     */
    //.AddSingleton<IClientPolicyStore, DistributedCacheClientPolicyStore>();
    //.AddSingleton<IRateLimitCounterStore,DistributedCacheRateLimitCounterStore>();
}
