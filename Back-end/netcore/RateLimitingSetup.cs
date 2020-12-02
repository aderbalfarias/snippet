public static async Task Main(string[] args)
{
    var host = CreateWebHostBuilder(args).Build();

    await SeedRateLimitPolicies(host);

    await host.RunAsync();
}

// Other stuff related to CreateWebHostBuilder

private static async Task SeedRateLimitPolicies(IHost host)
{
    // manually seed the appsettings.json rate limiting policies
    using (var scope = host.Services.CreateScope())
    {
        // get the ClientPolicyStore instance
        var clientPolicyStore = scope.ServiceProvider.GetRequiredService<IClientPolicyStore>();

        // seed client data from appsettings
        await clientPolicyStore.SeedAsync();
    }
}
