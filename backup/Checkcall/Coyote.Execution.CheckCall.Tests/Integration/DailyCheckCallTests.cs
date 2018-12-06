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

namespace Coyote.Execution.CheckCall.Tests.Integration
{
    using Coyote.Execution.CheckCall.Contracts.Commands;
    using Coyote.Execution.CheckCall.Domain.Models;
    using Coyote.Execution.CheckCall.Tests.Integration.EndpointBuilders;
    using Coyote.Execution.CheckCall.Tests.Integration.Support;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NServiceBus.AcceptanceTesting;
    using System;
    using System.Linq;

    [TestClass]
    public class DailyCheckCallTests
    {
        private TestBucket TestBucket { get; set; }
        private DateTime _senarioTimeout;
        private Carrier _testCarrier = null;
        private Customer _testCustomer = null;
        private Load _load = null;

        [TestInitialize]
        public void Init()
        {
            TestBucket = new TestBucket();
            _senarioTimeout = DateTime.Now.AddSeconds(20);
            PopulateCheckCallRequiredDetails();
        }

        [TestCleanup]
        public void CleanUp()
        {
            TestBucket.Close();
        }

        [TestMethod, TestCategory("Integration")]
        [TestCategory("IgnoreInSIT")]
        public void DailyCheckCall_OnlyOneLoadForTestCarrier()
        {
            _load = LoadManagement.CreateLoad(new Load()
            {
                StateType = LoadStateType.Active,
                Mode = LoadModeType.TL,
                EquipmentType = "V",
                LoadDate = DateTime.Today,
                ProgressType = 2 //covered
            }, _testCarrier, _testCustomer);
            TestBucket.TakeOwnership(_load);
            SendCheckCallEmail(true);
        }

        [TestMethod, TestCategory("Integration")]
        public void DailyPriorDayCheckCall_OnlyOneLoadForTestCarrier()
        {
            _load = LoadManagement.CreateLoad(new Load()
            {
                StateType = LoadStateType.Active,
                Mode = LoadModeType.TL,
                EquipmentType = "V",
                LoadDate = DateTime.Today.AddDays(1),
                ProgressType = 2 //covered
            }, _testCarrier, _testCustomer);
            TestBucket.TakeOwnership(_load);
            SendCheckCallEmail(false);
        }

        private void SendCheckCallEmail(bool isLoadDateToday)
        {
            object message = null;
            if (isLoadDateToday)
                message = new ProcessDailyCheckCallEmails();
            else
                message = new ProcessPriorDayCheckCallEmails();

            var testContext =
           Scenario.Define<DailyCheckCallTestContext>()
           .WithEndpoint<DailyCheckCall_IntegrationTest_Email_Endpoint>()
               .WithEndpoint<DailyCheckCall_IntegrationTest_Endpoint>(b =>
                   b.When(bus => bus.Send(message))
               )
                                    .Done(context => (context.MessagesSent.ContainsKey(_testCarrier.Id) && context.MessagesSent[_testCarrier.Id].Contains(_load.Id) || DateTime.Now > _senarioTimeout))
               .Run();

            Assert.IsTrue(testContext.MessagesSent.ContainsKey(_testCarrier.Id), "No Send Email Command sent for Test Carrier.");
            Assert.IsTrue(testContext.MessagesSent[_testCarrier.Id].Contains(_load.Id), "Load List in Message for Test Carrier contains test created LoadId");

        }
        private void PopulateCheckCallRequiredDetails()
        {
            _testCustomer = CustomerManagement.CreateCustomer("DCC Test Customer 1", "DCCTestCU1");
            TestBucket.TakeOwnership(_testCustomer);

            _testCarrier = CarrierManagement.AddNewCarrier("DCC Test Carrier 1", "DCCTestC1");
            TestBucket.TakeOwnership(_testCarrier);

            var testRep = UserManagement.AddCarrierRep(_testCarrier, true);
            TestBucket.TakeOwnership(testRep);

            CarrierManagement.InsertCarrierTrackingPreference(_testCarrier.Id, 1, "integration@fake.dat", true, false);
        }
    }
}
