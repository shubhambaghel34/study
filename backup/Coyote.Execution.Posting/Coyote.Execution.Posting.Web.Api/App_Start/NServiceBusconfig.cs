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
namespace Coyote.Execution.Posting.Web.Api.AppStart
{
    using NServiceBus;
    using NServiceBus.Logging;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly")]
    public static class NServiceBusconfig
    {
        public static IEndpointInstance SetupEndpoint()
        {
            var endpointConfiguration = new EndpointConfiguration(WebApiApplication.ServiceName);

            var builder = AutofacConfig.GetAutofacContainerBuilder();

            endpointConfiguration.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(builder.Build()));

            // Apply transport
            endpointConfiguration.UseTransport<RabbitMQTransport>();
            endpointConfiguration.UseSerialization<JsonSerializer>();

            //no defer messages from api so can use in memory
            endpointConfiguration.UsePersistence<InMemoryPersistence>();

            endpointConfiguration.Conventions()
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("Coyote.") && t.Namespace.Contains(".Commands"));

            endpointConfiguration.EnableInstallers();
            LogManager.Use<Log4NetFactory>();

            endpointConfiguration.SendOnly();

            return Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
        }
    }
}