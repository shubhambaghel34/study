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
namespace Coyote.Execution.Posting.Tests.Unit.CustomChecks
{
    using Coyote.Execution.Posting.Contracts.ServiceLayer;
    using Coyote.Execution.Posting.Endpoint.CustomChecks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using ServiceControl.Plugin.CustomChecks;
    using System.Threading.Tasks;

    /// <summary>
    /// Summary description for InternetTruckStopExternalServiceCheckTests
    /// </summary>
    [TestClass]
    public class InternetTruckStopExternalServiceCheckTests
    {
        private Mock<IInternetTruckStopExternalService> _internetTruckStopExternalService;
        private Endpoint.CustomChecks.InternetTruckStopExternalServiceCheck internetTruckStopExternalServiceCheck;

        [TestInitialize]
        public void Init()
        {
            _internetTruckStopExternalService = new Mock<IInternetTruckStopExternalService>();
            internetTruckStopExternalServiceCheck = new InternetTruckStopExternalServiceCheck(_internetTruckStopExternalService.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _internetTruckStopExternalService.VerifyAll();
        }


        [TestMethod, TestCategory("Unit")]
        public async Task InternetTruckStopExternalServiceCheck_PingAsync_Pass()
        {
            _internetTruckStopExternalService.Setup(x => x.PingAsync()).ReturnsAsync(true);
            var result = await internetTruckStopExternalServiceCheck.PerformCheck().ConfigureAwait(false);
            Assert.AreEqual(CheckResult.Pass, result, "Should pass");
        }

        [TestMethod, TestCategory("Unit")]
        public async Task InternetTruckStopExternalServiceCheck_PingAsync_Fail()
        {
            _internetTruckStopExternalService.Setup(x => x.PingAsync()).ReturnsAsync(false);
            var result = await internetTruckStopExternalServiceCheck.PerformCheck().ConfigureAwait(false);
            Assert.IsTrue(result.HasFailed, "Should fail");
        }

        [TestMethod, TestCategory("Unit")]
        public void InternetTruckStopExternalServiceCheck_VerifyCategoryAndId_ExpectHttp()
        {
            var expectedCategory = "HTTP Service";
            var expectedId = "Coyote Execution Posting - Endpoint - InternetTruckStop";
            Assert.AreEqual(expectedCategory, internetTruckStopExternalServiceCheck.Category);
            Assert.AreEqual(expectedId, internetTruckStopExternalServiceCheck.Id);
        }
    }
}