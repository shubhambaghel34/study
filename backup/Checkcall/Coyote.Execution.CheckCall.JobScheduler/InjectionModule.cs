// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2016 - 2018
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////

namespace Coyote.Execution.CheckCall.JobScheduler
{
    using System.Collections.Generic;
    using System.Linq;
    using Autofac;
    using Autofac.Core;
    using Common.Logging;
    using NServiceBus;
    using NServiceBus.Log4Net;
    using NServiceBus.Transports.RabbitMQ;
    using Coyote.Execution.CheckCall.Contracts.Commands;

    public class InjectionModule : Module
    {
        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            if (registration != null)
                registration.Preparing += OnComponentPreparing;
        }

        protected static void OnComponentPreparing(object sender, PreparingEventArgs eventArgs)
        {
            if (eventArgs != null)
            {
                var activatorType = eventArgs.Component.Activator.LimitType;
                eventArgs.Parameters = eventArgs.Parameters.Union(new[]
                {
                    new ResolvedParameter((p, i) => p.ParameterType == typeof(ILog), (p, i) => LogManager.GetLogger(activatorType)) ,
                    new ResolvedParameter((p, i) => p.ParameterType == typeof(IBus), (p, i) => Bus.CreateSendOnly(ConfigureBus()))
                });
            }
        }

        private static BusConfiguration ConfigureBus()
        {
            NServiceBus.Logging.LogManager.Use<Log4NetFactory>();
            var busConfiguration = new BusConfiguration();
            busConfiguration.AssembliesToScan(GetAssemblyNames().Select(System.Reflection.Assembly.Load));
            busConfiguration.EndpointName(typeof(InjectionModule).Assembly.GetName().Name);
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.UseTransport<RabbitMQTransport>();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.PurgeOnStartup(false);
            busConfiguration.Conventions()
            .DefiningCommandsAs(
                t => t.Namespace != null && t.Namespace.StartsWith("Coyote.") && t.Namespace.EndsWith(".Commands"));

            busConfiguration.RegisterComponents(components =>
            {
                components.ConfigureComponent(() =>
                    log4net.LogManager.GetLogger(typeof(InjectionModule)),
                    DependencyLifecycle.SingleInstance);
            });
            return busConfiguration;
        }

        private static IEnumerable<string> GetAssemblyNames()
        {
            return new[]
            {
                 typeof(DailyCheckCallJob).Assembly.GetName().Name,
                 typeof(NServiceBus.Logging.LogManager).Assembly.GetName().Name,
                 typeof(ProcessDailyCheckCallEmails).Assembly.GetName().Name,
                 typeof(IManageRabbitMqConnections).Assembly.GetName().Name
            };
        }
    }
}
