 public static class CustomServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomOpenIddict(this IServiceCollection services,
            IHostingEnvironment hostingEnvironment)
        {
            var openIdOptions = services.BuildServiceProvider().GetRequiredService<OIDCAuthorizationServerOptions>();
            var authenticationOptions = services.BuildServiceProvider().GetRequiredService<AuthenticationOptions>();

            services
                .AddDbContext<DbContext>(options =>
                {
                    // Configure the context to use an in-memory store.
                    options.UseInMemoryDatabase(nameof(DbContext));

                    // Register the entity sets needed by OpenIddict.
                    // Note: use the generic overload if you need
                    // to replace the default OpenIddict entities.
                    options.UseOpenIddict();
                })
                .AddOpenIddict()
                .AddCore(options =>
                {
                    // Configure OpenIddict to use the Entity Framework Core stores and entities.
                    options.UseEntityFrameworkCore()
                        .UseDbContext<DbContext>();
                })
                .AddServer(options =>
                {
                    // Register the ASP.NET Core MVC binder used by OpenIddict.
                    // Note: if you don't call this method, you won't be able to
                    // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                    options.UseMvc();

                    // Enable the authorization/token endpoints (required to use the code flow).
                    options
                        .EnableTokenEndpoint("/connect/token");
                    //.EnableAuthorizationEndpoint("/connect/authorize");

                    // Allow client applications to use the grant_type=client_credentials flow.
                    options.AllowClientCredentialsFlow();

                    // During development, you can disable the HTTPS requirement.
                    if (hostingEnvironment.IsDevelopment()) options.DisableHttpsRequirement();

                    // Accept token requests that don't specify a client_id.
                    // options.AcceptAnonymousClients();

                    options.EnableRequestCaching();

                    // Note: to use JWT access tokens instead of the default
                    // encrypted format, the following lines are required:
                    //
                    options.UseJsonWebTokens();

                    // Register a new ephemeral key, that is discarded when the application
                    // shuts down. Tokens signed using this key are automatically invalidated.
                    // This method should only be used during development.
                    //options.AddEphemeralSigningKey();

                    // On production, using a X.509 certificate stored in the machine store is recommended.
                    options.AddSigningCertificate(LoadCertificate(services));

                    var expiryInSeconds = openIdOptions.AccessTokenExpiration;
                    options.SetAccessTokenLifetime(TimeSpan.FromSeconds(Convert.ToDouble(expiryInSeconds)));

                    // Note: if you don't want to use permissions, you can disable
                    // permission enforcement by uncommenting the following lines:
                    //
                    // options.IgnoreEndpointPermissions()
                    //        .IgnoreGrantTypePermissions()
                    //        .IgnoreScopePermissions();
                })

                // Register the OpenIddict validation handler.
                // Note: the OpenIddict validation handler is only compatible with the
                // default token format or with reference tokens and cannot be used with
                // JWT tokens. For JWT tokens, use the Microsoft JWT bearer handler.
                //.AddValidation()
                .Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    //options.Authority = authenticationOptions.Issuer;
                    //options.Audience = authenticationOptions.Audience;
                    options.RequireHttpsMetadata = !hostingEnvironment.IsDevelopment();
                    options.IncludeErrorDetails = hostingEnvironment.IsDevelopment(); // sets to false otherwise
                    options.SaveToken = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidIssuer = authenticationOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = authenticationOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new X509SecurityKey(LoadCertificate(services))
                    };
                });

            return services;
        }

        public static X509Certificate2 LoadCertificate(IServiceCollection services)
        {
            var openIdOptions = services.BuildServiceProvider().GetRequiredService<OIDCAuthorizationServerOptions>();
            CertificateOptions options = openIdOptions.SigningCertificate;

            if (options != null)
            {
                if (options.Store != null && options.Location != null)
                {
                    using (var store = new X509Store(options.Store, (StoreLocation)Enum.Parse(typeof(StoreLocation), options.Location)))
                    {
                        store.Open(OpenFlags.ReadOnly);
                        var certificate = store.Certificates.Find(
                            X509FindType.FindBySubjectName,
                            options.Subject,
                            validOnly: !options.AllowInvalid);

                        if (certificate.Count == 0)
                        {
                            throw new InvalidOperationException($"Certificate not found for {options.Subject}.");
                        }

                        return certificate[0];
                    }
                }
            }

            throw new InvalidOperationException("No valid certificate configuration found for the current endpoint.");
        }
    }
