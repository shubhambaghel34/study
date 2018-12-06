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

namespace Coyote.Execution.CheckCall.Tests.Integration.EndpointBuilders
{
    using Autofac;
    using Coyote.Execution.CheckCall.Storage;
    using log4net;
    using log4net.Config;
    using NServiceBus;
    using NServiceBus.AcceptanceTesting.Support;
    using NServiceBus.Config.ConfigurationSource;
    using NServiceBus.Log4Net;
    using System;
    using System.Configuration;
    using System.Reflection;

    public class CheckCallServer : IEndpointSetupTemplate
    {
        public BusConfiguration GetConfiguration(RunDescriptor runDescriptor, EndpointConfiguration endpointConfiguration, IConfigurationSource configSource, Action<BusConfiguration> configurationBuilderCustomization)
        {
            var config = new BusConfiguration();

            XmlConfigurator.Configure();
            NServiceBus.Logging.LogManager.Use<Log4NetFactory>();

            config.EndpointName(endpointConfiguration?.EndpointName);

            config.Conventions()
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("Coyote.") && t.Namespace.EndsWith(".Commands"))
                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith("Coyote.") && t.Namespace.EndsWith(".Messages"))
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("Coyote.") && t.Namespace.EndsWith(".Events"));

            config.AssembliesToScan(
                Assembly.Load("Coyote.Execution.CheckCall.Endpoint"),
                Assembly.Load("Coyote.Execution.CheckCall.Contracts"),
                Assembly.Load("NServiceBus.Transports.RabbitMQ"));

            config.CustomConfigurationSource(configSource);
            config.Transactions().Enable();
            config.UsePersistence<InMemoryPersistence>();
            config.UseSerialization<JsonSerializer>();
            config.UseTransport<RabbitMQTransport>();
            config.PurgeOnStartup(false);

            config.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(GetAutoFacBuilder().Build()));

            config.RegisterComponents(r =>
            {
                r.RegisterSingleton(runDescriptor.ScenarioContext.GetType(), runDescriptor.ScenarioContext);
                r.RegisterSingleton(typeof(DailyCheckCallTestContext), runDescriptor.ScenarioContext);
            });

            if (configurationBuilderCustomization != null)
            {
                configurationBuilderCustomization(config);
            }
            return config;
        }

        private static ContainerBuilder GetAutoFacBuilder()
        {
            ContainerBuilder builder = new ContainerBuilder();
            
            builder.RegisterInstance<ILog>(LogManager.GetLogger(typeof(CheckCallServer)));
            var bazookaConnectionString = ConfigurationManager.ConnectionStrings["Bazooka"]?.ConnectionString;
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
                .SingleInstance()
                .AsImplementedInterfaces();
            builder.RegisterModule(new SqlModule(bazookaConnectionString));
            return builder;
        }
    }
}
