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

namespace Coyote.Execution.CheckCall.Tests.Unit.Processor
{
    using System.Collections.Generic;
    using Coyote.Execution.CheckCall.Contracts.Commands;
    using Coyote.Execution.CheckCall.Domain.Models;
    using Coyote.Execution.CheckCall.Endpoint.Processors;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using NServiceBus.Testing;
    using Storage.Repositories;

    [TestClass]
    public class ProcessDailyCheckCallHandlerTests : NServiceBusHandlerUnitTestBase
    {
        Mock<ICarrierRepository> CarrierRepositoryMock { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            BaseTestInit();
            CarrierRepositoryMock = new Mock<ICarrierRepository>();
        }


        [TestMethod, TestCategory("Unit")]
        public void ProcessDailyCheckCallHandler_NoCarrier_NoEmailCommand()
        {
            SetCarrierRepositoryMock_NoCarrier_NoEmailCommand(true);

            GetTestHandlerForProcessor()
                .ExpectNotSend<SendDailyCheckCallEmail>(c => GetExpectFalseCallback(c, f =>
                {
                    Assert.Fail("No Send Daily Check Call Email Command should have been issued");
                }))
                .OnMessage(getCommand());
        }


        [TestMethod, TestCategory("Unit")]
        public void ProcessDailyCheckCallHandler_SingleCarrier_SingleCommand()
        {
           SetCarrierRepositoryMock_SingleCarrier_SingleCommand(true);

            int messageCount = 0;
            GetTestHandlerForProcessor()
                .ExpectSend<SendDailyCheckCallEmail>(c => GetExpectCallback<SendDailyCheckCallEmail>(c, f =>
                {
                    messageCount++;
                }))
                .OnMessage(getCommand());

            Assert.AreEqual(1, messageCount);
        }

        [TestMethod, TestCategory("Unit")]
        public void ProcessDailyCheckCallHandler_MultipleCarriers_MultipleCommands()
        {
            SetCarrierRepositoryMock_MultipleCarriers_MultipleCommands(true);

            int messageCount = 0;
            GetTestHandlerForProcessor()
                .ExpectSend<SendDailyCheckCallEmail>(c => GetExpectCallback<SendDailyCheckCallEmail>(c, f =>
                {
                    messageCount++;
                    if (c.CarrierId == 1)
                    {
                        Assert.AreEqual(3, c.Loads.Count, "Unexpected Number of Loads for Carrier 1");
                    }
                    else if (c.CarrierId == 2)
                    {
                        Assert.AreEqual(2, c.Loads.Count, "Unexpected Number of Loads for Carrier 2");
                    }
                    else if (c.CarrierId == 3)
                    {
                        Assert.AreEqual(1, c.Loads.Count, "Unexpected Number of Loads for Carrier 3");
                    }
                }))
                .OnMessage(getCommand());

            Assert.AreEqual(3, messageCount, "Unexpected Number of messages.");
        }

        [TestMethod, TestCategory("Unit")]
        public void ProcessPriorDayCheckCallHandler_NoCarrier_NoEmailCommand()
        {
            SetCarrierRepositoryMock_NoCarrier_NoEmailCommand(false);

            GetTestHandlerForProcessor()
                .ExpectNotSend<SendDailyCheckCallEmail>(c => GetExpectFalseCallback(c, f =>
                {
                    Assert.Fail("No Send Prior Day Check Call Email Command should have been issued");
                }))
                .OnMessage(getPriorDayCommand());
        }

        [TestMethod, TestCategory("Unit")]
        public void ProcessPriorDayCheckCallHandler_SingleCarrier_SingleCommand()
        {
            SetCarrierRepositoryMock_SingleCarrier_SingleCommand(false);

            int messageCount = 0;
            GetTestHandlerForProcessor()
                .ExpectSend<SendDailyCheckCallEmail>(c => GetExpectCallback<SendDailyCheckCallEmail>(c, f =>
                {
                    messageCount++;
                }))
                .OnMessage(getPriorDayCommand());

            Assert.AreEqual(1, messageCount);
        }

        [TestMethod, TestCategory("Unit")]
        public void ProcessPriorDayCheckCallHandler_MultipleCarriers_MultipleCommands()
        {
            SetCarrierRepositoryMock_MultipleCarriers_MultipleCommands(false);

            int messageCount = 0;
            GetTestHandlerForProcessor()
                .ExpectSend<SendDailyCheckCallEmail>(c => GetExpectCallback<SendDailyCheckCallEmail>(c, f =>
                {
                    messageCount++;
                    if (c.CarrierId == 1)
                    {
                        Assert.AreEqual(3, c.Loads.Count, "Unexpected Number of Loads for Carrier 1");
                    }
                    else if (c.CarrierId == 2)
                    {
                        Assert.AreEqual(2, c.Loads.Count, "Unexpected Number of Loads for Carrier 2");
                    }
                    else if (c.CarrierId == 3)
                    {
                        Assert.AreEqual(1, c.Loads.Count, "Unexpected Number of Loads for Carrier 3");
                    }
                }))
                .OnMessage(getPriorDayCommand());

            Assert.AreEqual(3, messageCount, "Unexpected Number of messages.");
        }
        
        private Handler<ProcessDailyCheckCallHandler> GetTestHandlerForProcessor()
        {
            return Test.Handler<ProcessDailyCheckCallHandler>(bus => new ProcessDailyCheckCallHandler(LogMock.Object, bus, CarrierRepositoryMock.Object));
        }

        private static ProcessDailyCheckCallEmails getCommand()
        {
            return new ProcessDailyCheckCallEmails();
        }

        private static ProcessPriorDayCheckCallEmails getPriorDayCommand()
        {
            return new ProcessPriorDayCheckCallEmails();
        }

        private void SetCarrierRepositoryMock_NoCarrier_NoEmailCommand(bool isLoadDateToday)
        {
            CarrierRepositoryMock.Setup(e => e.GetCarrierCheckCallNotificationRecords(isLoadDateToday)).ReturnsAsync(new List<CheckCallNotificationRecord>());
        }
        private void SetCarrierRepositoryMock_SingleCarrier_SingleCommand(bool isLoadDateToday)
        {
            CarrierRepositoryMock.Setup(e => e.GetCarrierCheckCallNotificationRecords(isLoadDateToday)).ReturnsAsync(new List<CheckCallNotificationRecord>()
            {
                new CheckCallNotificationRecord()
                {
                    CarrierId = 1,
                    LoadId = 11
                },
                new CheckCallNotificationRecord()
                {
                    CarrierId = 1,
                    LoadId = 12
                },
                new CheckCallNotificationRecord()
                {
                    CarrierId = 1,
                    LoadId = 13
                }
            });
        }
        private void SetCarrierRepositoryMock_MultipleCarriers_MultipleCommands(bool isLoadDateToday)
        {
            CarrierRepositoryMock.Setup(e => e.GetCarrierCheckCallNotificationRecords(isLoadDateToday)).ReturnsAsync(new List<CheckCallNotificationRecord>()
            {
                new CheckCallNotificationRecord()
                {
                    CarrierId = 1,
                    LoadId = 11
                },
                new CheckCallNotificationRecord()
                {
                    CarrierId = 1,
                    LoadId = 12
                },
                new CheckCallNotificationRecord()
                {
                    CarrierId = 1,
                    LoadId = 13
                },
                new CheckCallNotificationRecord()
                {
                    CarrierId = 2,
                    LoadId = 14
                },
                new CheckCallNotificationRecord()
                {
                    CarrierId = 2,
                    LoadId = 15
                },
                new CheckCallNotificationRecord()
                {
                    CarrierId = 3,
                    LoadId = 16
                }
            });
        }
    }
}
