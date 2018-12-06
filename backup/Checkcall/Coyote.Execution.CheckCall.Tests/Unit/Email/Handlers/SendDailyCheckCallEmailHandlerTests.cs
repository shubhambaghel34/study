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

namespace Coyote.Execution.CheckCall.Tests.Unit.Email.Processor
{
    using System;
    using System.Collections.ObjectModel;
    using Coyote.Execution.CheckCall.Contracts.Services;
    using Coyote.Execution.CheckCall.Domain;
    using Coyote.Execution.CheckCall.Domain.Models;
    using Coyote.Execution.CheckCall.Email.Endpoint.Email;
    using Coyote.Execution.CheckCall.Email.Endpoint.Processor.Command;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using NServiceBus.Testing;
    using Storage.Repositories;

    [TestClass]
    public class SendDailyCheckCallEmailHandlerTests : NServiceBusHandlerUnitTestBase
    {
        Mock<ICarrierRepository> CarrierRepositoryMock { get; set; }
        Mock<IAuditUserRepository> AuditUserRepositoryMock { get; set; }
        Mock<IEmailApi> EmailApiMock { get; set; }          

        private int _auditUserId;
        private Carrier _carrier;       

        [TestInitialize]
        public void TestInit()
        {
            BaseTestInit();

            SetupData();

            CarrierRepositoryMock = new Mock<ICarrierRepository>();
            CarrierRepositoryMock.Setup(e => e.GetById(It.IsAny<int>())).ReturnsAsync(_carrier);

            _auditUserId = 123;
            AuditUserRepositoryMock = new Mock<IAuditUserRepository>();
            AuditUserRepositoryMock.Setup(r => r.GetAuditUserId(It.IsAny<string>())).ReturnsAsync(_auditUserId);

            EmailApiMock = new Mock<IEmailApi>();
            EmailApiMock.Setup(e => e.SendEmail(It.IsAny<int>(), It.IsAny<EmailMessage>())).Returns(true);          

            Test.Initialize();
        }

        [TestMethod, TestCategory("Unit")]
        public void SendDailyCheckCallEmailHandler_EmailSent()
        {
            var command = GetSendCheckCallCommand();

            GetForTestHandlerForProcessor()
                .OnMessage(command);

            EmailApiMock.Verify(e => e.SendEmail(It.IsAny<int>(), It.IsAny<DailyCheckCall>()), Times.Once);
        }

        private Handler<SendDailyCheckCallEmailHandler> GetForTestHandlerForProcessor()
        {
            return Test.Handler<SendDailyCheckCallEmailHandler>(bus => new SendDailyCheckCallEmailHandler(
                LogMock.Object,
                bus,
                CarrierRepositoryMock.Object,
                AuditUserRepositoryMock.Object,
                EmailApiMock.Object));
        }

        private static Contracts.Commands.SendDailyCheckCallEmail GetSendCheckCallCommand()
        {
            return Test.CreateInstance<Contracts.Commands.SendDailyCheckCallEmail>(m =>
            {
                m.CarrierId = 1;
                m.Email = " ; one@fake.dat;two@fake.date;;";
                m.Date = DateTime.Now.Date;
                m.Loads = new Collection<Contracts.LoadInfo>()
                {
                    new Contracts.LoadInfo
                    {
                        LoadId = 1234,
                        OriginCityName = "One",
                        OriginState = "IL",
                        DestinationCityName = "One",
                        DestinationState = "IL"
                    },
                    new Contracts.LoadInfo
                    {
                        LoadId = 43212,
                        OriginCityName = "Two",
                        OriginState = "CA",
                        DestinationCityName = "Two",
                        DestinationState = "IL"
                    }
                };
            });
        }

        private void SetupData()
        {
            _carrier = new Carrier()
            {
                Reps = new Collection<Rep>()
                {
                    new Rep()
                    {
                        Main = true,
                        RepType = RepType.CarrierOperations,
                        EmployeeId = 500,
                        EmailWork = "workMain@fake.dat"
                    },
                    new Rep()
                    {
                        Main = false,
                        RepType = RepType.CarrierOperations,
                        EmployeeId = 600,
                        EmailWork = "workNotMain2@fake.dat"
                    }
                },
                CarrierTrackingPreference = new CarrierTrackingPreference()
                {
                    OutboundDefaultCommunicationTypeId = 1,
                    Email = " ; one@fake.dat;two@fake.date;;"
                }
            };

        }
    }
}
