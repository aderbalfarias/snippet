    

  "NServiceBusSettings": {
    "CrsHandlerEndpoint": "Test",
    "NumberOfRetries": 2,
    "SendHeartbeatTo": "Particular.ServiceControl",
    "AuditProcessedMessagesTo": "audit",
    "SendFailedMessagesTo": "error",
    "SendMetricDataToServiceControl": "Particular.Monitoring",
    "SendMetricDataToServiceControlIntervalInMilliseconds": 500,
    "RecoverabilityTimeIncreaseInSeconds": 5,
    "SubscriptionCacheForInMinutes": 1
  },

    public class NServiceBusSettings
    {
        public string CrsHandlerEndpoint { get; set; }

        public int NumberOfRetries { get; set; }

        public string SendHeartbeatTo { get; set; }

        public string AuditProcessedMessagesTo { get; set; }

        public string SendFailedMessagesTo { get; set; }

        public string SendMetricDataToServiceControl { get; set; }

        public int SendMetricDataToServiceControlIntervalInMilliseconds { get; set; }

        public int RecoverabilityTimeIncreaseInSeconds { get; set; }

        public int SubscriptionCacheForInMinutes { get; set; }
    }


<PackageReference Include="NServiceBus" Version="7.1.10" />
    <PackageReference Include="NServiceBus.Heartbeat" Version="3.0.1" />
    <PackageReference Include="NServiceBus.Metrics" Version="3.0.0" />
    <PackageReference Include="NServiceBus.Metrics.ServiceControl" Version="3.0.3" />
    <PackageReference Include="NServiceBus.MSDependencyInjection" Version="0.1.4" />
    <PackageReference Include="NServiceBus.Persistence.Sql" Version="4.6.0" />
    <PackageReference Include="NServiceBus.SqlServer" Version="4.3.0" />

NServiceBusConfig(hostContext, services);

private static Task NServiceBusConfig(HostBuilderContext hostContext, IServiceCollection services)
        {
            var serviceBusSettings = hostContext.Configuration.GetSection(nServiceBusSettings).Get<NServiceBusSettings>();

            var endpointConfiguration = new EndpointConfiguration(serviceBusSettings.CrsHandlerEndpoint);

            var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
            transport.ConnectionString(hostContext.Configuration.GetConnectionString(discConnection));
            transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);

            endpointConfiguration.AutoSubscribe();
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UsePersistence<SqlPersistence>();
            endpointConfiguration.UseContainer<ServicesBuilder>(c => c.ExistingServices(services));
            endpointConfiguration.SendHeartbeatTo(serviceBusSettings.SendFailedMessagesTo);
            endpointConfiguration.AuditProcessedMessagesTo(serviceBusSettings.AuditProcessedMessagesTo);
            endpointConfiguration.SendFailedMessagesTo(serviceBusSettings.SendFailedMessagesTo);

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(
                immediate =>
                {
                    immediate.NumberOfRetries(serviceBusSettings.NumberOfRetries);
                });
            recoverability.Delayed(
                delayed =>
                {
                    delayed.NumberOfRetries(serviceBusSettings.NumberOfRetries);
                    delayed.TimeIncrease(TimeSpan.FromSeconds(serviceBusSettings.RecoverabilityTimeIncreaseInSeconds));
                });

            //var metrics = endpointConfiguration.EnableMetrics();
            //metrics.SendMetricDataToServiceControl(nServiceBusSettings.SendMetricDataToServiceControl,
            //    TimeSpan.FromMilliseconds(nServiceBusSettings.SendMetricDataToServiceControlIntervalInMilliseconds));

            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            persistence.SqlDialect<SqlDialect.MsSqlServer>();
            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(hostContext.Configuration.GetConnectionString(discConnection));
                });

            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.CacheFor(TimeSpan.FromMinutes(serviceBusSettings.SubscriptionCacheForInMinutes));

            var endpointInstance = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            services.AddSingleton(endpointInstance);
            services.AddSingleton<IMessageSession>(endpointInstance);

            return Task.CompletedTask;
        }

____________________________________________________________________________________________________________________________


transport.Routing()
                .RegisterPublisher(typeof(EntityCommon), serviceBusSettings.CrsHandlerEndpoint);

 public class TestClass : IHandleMessages<EntityCommon>
    {


        private readonly IBaseRepository _baseRepository;
        private readonly ILogger _logger;

        public TestClass(IBaseRepository baseRepository,
            ILogger<TestClass> logger)
        {
            _baseRepository = baseRepository;
            _logger = logger;
        }


        public async Task Handle(MessageDefinitions.PpsnAllocated message, IMessageHandlerContext context)
        {
            _logger.LogInformation($"Received message, GroLifeEventId = {message.GroLifeEventId}");

            try
            {
                //do something

                _logger.LogInformation($"Message Id = { message.Id} processed successfully");
            }            
            catch(Exception e)
            {
                _logger.LogError($"Error for message Id: {id}, Exception: {e}");
            }
        }

    }

____________________________________________________________________________________________________________________________

await _messageSession
                        .Publish(new EntityCOmmon
                        {
                            Id = groLifeEventId,
                            SerilizedMessage = "fdsfsdf"
                        })
                        .ConfigureAwait(false);
 IMessageSession messageSession = injected in constructor
