var defaultFactory = LogManager.Use<DefaultFactory>();
            defaultFactory.Directory("C:\\App\\Logs");
            
            
var routing = transport.Routing();
routing.RouteToEndpoint(typeof(RegisterX), Configuration.GetSection("NServiceBusRouteToEndpoint").Value);

await base.MessageSession.Send(xCommand).ConfigureAwait(false);

IHandleMessages<RegisterX>

endpointConfiguration.RegisterComponents(
                registration: configureComponents =>
                {
                    configureComponents.RegisterSingleton(hostContext.Configuration.GetSection(appSettings).Get<AppSettings>());
                });
