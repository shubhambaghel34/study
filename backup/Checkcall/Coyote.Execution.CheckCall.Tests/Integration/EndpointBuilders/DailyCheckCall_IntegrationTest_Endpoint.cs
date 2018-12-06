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

namespace Coyote.Execution.CheckCall.Tests.Integration.EndpointBuilders
{
    using Coyote.Execution.CheckCall.Contracts.Commands;
    using NServiceBus.AcceptanceTesting;

    public class DailyCheckCall_IntegrationTest_Endpoint : EndpointConfigurationBuilder
    {
        public DailyCheckCall_IntegrationTest_Endpoint()
        {
            EndpointSetup<CheckCallServer>()
                .AddMapping<ProcessDailyCheckCallEmails>(typeof(DailyCheckCall_IntegrationTest_Endpoint))
                .AddMapping<ProcessPriorDayCheckCallEmails>(typeof(DailyCheckCall_IntegrationTest_Endpoint))
                .AddMapping<SendDailyCheckCallEmail>(typeof(DailyCheckCall_IntegrationTest_Email_Endpoint));
        }
    }

}

