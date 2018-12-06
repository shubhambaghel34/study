// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2016 - 2017
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////

namespace Coyote.Execution.CheckCall.Email.Endpoint
{
    using System;
    using System.Reflection;
    using Autofac;
    using log4net;
    using log4net.Config;
    using NServiceBus;
    using NServiceBus.Log4Net;
    using System.Configuration;
    using Coyote.Execution.CheckCall.Storage;

    [EndpointName("Coyote.Execution.CheckCall.Email.Endpoint")]
    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            //Setup Logging
            XmlConfigurator.Configure();
            NServiceBus.Logging.LogManager.Use<Log4NetFactory>();

            configuration.Conventions()
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("Coyote.") && t.Namespace.EndsWith(".Commands"))
                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith("Coyote.") && t.Namespace.EndsWith(".Messages"))
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("Coyote.") && t.Namespace.EndsWith(".Events"));

            //Scan Assemblys for Messaging Handlers and Messages
            configuration.AssembliesToScan(
                Assembly.GetExecutingAssembly(),
                Assembly.Load("Coyote.Execution.CheckCall.Contracts"),
                Assembly.Load("NServiceBus.Transports.RabbitMQ"),
                Assembly.Load("ServiceControl.Plugin.Nsb5.Heartbeat"),
                Assembly.Load("ServiceControl.Plugin.Nsb5.CustomChecks")
                );

            //Configure NServiceBus
            configuration.UseTransport<RabbitMQTransport>();
            configuration.Transactions().Enable();
            //configuration.
            configuration.UseSerialization<JsonSerializer>();
            configuration.UsePersistence<InMemoryPersistence>();
            configuration.PurgeOnStartup(false);

            configuration.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(GetAutoFacBuilder().Build()));
        }

        private static ContainerBuilder GetAutoFacBuilder()
        {
            ContainerBuilder builder = new ContainerBuilder();
            var bazookaConnectionString = ConfigurationManager.ConnectionStrings["BazookaDbContext"]?.ConnectionString;
            builder.RegisterInstance<ILog>(LogManager.GetLogger(typeof(EndpointConfig)));

            builder.RegisterAssemblyTypes(new Assembly[]
            {
                Assembly.Load("Coyote.Execution.CheckCall.Contracts"),
                Assembly.Load("Coyote.Execution.CheckCall.Services")
            })
                .Where(t => t.Namespace.Contains("Service"))
                .SingleInstance()
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assembly.Load("Coyote.Execution.CheckCall.Storage"))
                .Where(t => t.Namespace.Contains("Repositories"))
                .AsImplementedInterfaces();
            builder.RegisterModule(new SqlModule(bazookaConnectionString));
            return builder;
        }
    }
}
