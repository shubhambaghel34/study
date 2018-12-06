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
    using Coyote.Common.Extensions;
    using Coyote.Execution.CheckCall.Contracts.Commands;
    using NServiceBus;
    using NServiceBus.AcceptanceTesting;
    using System.Linq;

    public class DailyCheckCall_IntegrationTest_Email_Endpoint : EndpointConfigurationBuilder
    {
        public DailyCheckCall_IntegrationTest_Email_Endpoint()
        {
            EndpointSetup<CheckCallEmailServer>()
                .AddMapping<SendDailyCheckCallEmail>(typeof(DailyCheckCall_IntegrationTest_Email_Endpoint));            
        }

        class SendDailyCheckCallInspector : NServiceBus.MessageMutator.IMutateIncomingMessages, NServiceBus.INeedInitialization
        {
            public DailyCheckCallTestContext TestContext { get; set; }

            public object MutateIncoming(object message)
            {
                if (message is SendDailyCheckCallEmail)
                {
                    var command = (SendDailyCheckCallEmail)message;

                    TestContext.MessagesSent.Add(command.CarrierId, from loads in command.Loads select loads.LoadId);
                }

                return message;
            }

            public void Customize(BusConfiguration configuration)
            {
                configuration.ThrowIfNull("configuration");
                configuration.RegisterComponents(c => c.ConfigureComponent<SendDailyCheckCallInspector>(DependencyLifecycle.InstancePerCall));
            }

        }
    }
}
