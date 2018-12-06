// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2017 - 2018
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////
namespace Coyote.Execution.Posting.Endpoint
{
    using Autofac;
    using Coyote.Execution.Posting.Common.Extensions;
    using log4net.Config;
    using NServiceBus;
    using NServiceBus.Logging;
    using NServiceBus.Persistence;

    public class EndpointConfig : IConfigureThisEndpoint
    {

        public void Customize(EndpointConfiguration configuration)
        {
            configuration.ThrowIfArgumentNull(nameof(configuration));
            XmlConfigurator.Configure();

            EndpointInitializer.ConfigureBus(configuration);
        }

        private static class EndpointInitializer
        {
            public static void ConfigureBus(EndpointConfiguration endpointConfiguration)
            {
                endpointConfiguration.ThrowIfArgumentNull(nameof(endpointConfiguration));

                IContainer container = AutofacConfig.GetAutofacContainerBuilder();
                LogManager.Use<Log4NetFactory>();
                endpointConfiguration.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(container));
                endpointConfiguration.UsePersistence<NHibernatePersistence, StorageType.Timeouts>();
                endpointConfiguration.UsePersistence<InMemoryPersistence>();
                endpointConfiguration.UseSerialization<JsonSerializer>();
                endpointConfiguration.UseTransport<RabbitMQTransport>();
                endpointConfiguration.PurgeOnStartup(false);
                endpointConfiguration.Recoverability()
                    .Delayed(delayed => { delayed.NumberOfRetries(0); })
                    .Immediate((immediate => { immediate.NumberOfRetries(0); }));

                endpointConfiguration.Conventions()
                    .DefiningCommandsAs(t =>
                        t.Namespace != null
                        && t.Namespace.StartsWith("Coyote.")
                        && t.Namespace.EndsWith(".Commands"));
            }
        }
    }
}