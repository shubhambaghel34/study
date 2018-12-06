// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2017 - 2017
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
    using Coyote.Execution.CheckCall.Contracts.Services;
    using Coyote.Execution.CheckCall.Domain;
    using Coyote.Execution.CheckCall.Email.Endpoint.Email;
    using Coyote.Execution.CheckCall.Email.Endpoint.Processor.Command;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using NServiceBus.Testing;
    using Storage.Repositories;
    using System;
    using System.Collections.Generic;

    [TestClass]
    public class LoadActivationCancellationEmailCommandHandlerTests : NServiceBusHandlerUnitTestBase
    {
        Mock<IAuditUserRepository> AuditUserRepositoryMock { get; set; }
        Mock<IEmailApi> EmailApiMock { get; set; }

        private int _auditUserId;

        [TestInitialize]
        public void TestInit()
        {
            BaseTestInit();

            _auditUserId = 123;
            AuditUserRepositoryMock = new Mock<IAuditUserRepository>();
            AuditUserRepositoryMock.Setup(r => r.GetAuditUserId(It.IsAny<string>())).ReturnsAsync(_auditUserId);

            EmailApiMock = new Mock<IEmailApi>();
            EmailApiMock.Setup(e => e.SendEmail(It.IsAny<int>(), It.IsAny<EmailMessage>())).Returns(true);

            Test.Initialize();
        }

        [TestMethod, TestCategory("Unit")]
        public void LoadActivationCancellationEmailCommandHandler_EmailSent()
        {
            var command = GetLoadActivationCancellationEmailCommand();

            GetForTestHandlerForProcessor()
                .OnMessage(command);

            EmailApiMock.Verify(e => e.SendEmail(It.IsAny<int>(), It.IsAny<LoadActivationCancellationEmail>()), Times.Once);
        }

        private Handler<LoadActivationCancellationEmailCommandHandler> GetForTestHandlerForProcessor()
        {
            return Test.Handler<LoadActivationCancellationEmailCommandHandler>(bus => new LoadActivationCancellationEmailCommandHandler(
                LogMock.Object,
                AuditUserRepositoryMock.Object,
                EmailApiMock.Object));
        }

        private static Contracts.Commands.LoadActivationCancellationEmailCommand GetLoadActivationCancellationEmailCommand()
        {
            return Test.CreateInstance<Contracts.Commands.LoadActivationCancellationEmailCommand>(m =>
            {
                m.LoadId = 1;
                m.CarrierRepEmailAddresses = new List<string> { "one@fake.dat", "two@fake.dat" };
                m.LoadDate = DateTime.Now.Date;
                m.OriginCity = "One";
                m.OriginStateCode = "IL";
                m.DestinationCity = "One";
                m.DestinationStateCode = "IL";
            });
        }
    }
}
