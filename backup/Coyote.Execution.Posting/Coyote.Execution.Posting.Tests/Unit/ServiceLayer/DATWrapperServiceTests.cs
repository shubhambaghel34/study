// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2018 - 2018
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////
namespace Coyote.Execution.Posting.Tests.Unit.ServiceLayer
{
    using Coyote.Execution.Posting.Contracts;
    using Coyote.Execution.Posting.Contracts.Storage;
    using Coyote.Execution.Posting.ServiceLayer.DAT;
    using Coyote.Execution.Posting.Tests.Unit.Helper;
    using log4net;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Net.Http;

    [TestClass]
    public class DATWrapperServiceTests
    {
        #region " Private Fields "
        private string strTestUrl = "https://test:8080/";

        private Mock<IRuntimeSettings> _mockRuntimeSettings;
        private Mock<IPostingRepository> _mockPostingRepository;
        private Mock<ILog> _mockLog;
        private Mock<DATWrapperService> _mockDATWrapperService;
        #endregion

        #region " Setup "
        [TestInitialize()]
        public void DATWrapperServiceTestsInitialize()
        {

            _mockRuntimeSettings = new Mock<IRuntimeSettings>();
            _mockRuntimeSettings.SetupGet(s=>s.DATLoadPostingWebUrl).Returns(strTestUrl);

            _mockPostingRepository = new Mock<IPostingRepository>();

            _mockLog = new Mock<ILog>();

            _mockDATWrapperService = new Mock<DATWrapperService>(_mockLog.Object, _mockPostingRepository.Object, _mockRuntimeSettings.Object);
            var mockhttpResponseMessage = new Mock<HttpResponseMessage>();
            _mockDATWrapperService.Setup(s => s.PostContentAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(mockhttpResponseMessage.Object);
        }
        #endregion

        #region " Test cases "
        [TestMethod, TestCategory("Unit")]
        public void DATWrapperService_ThrowsExceptionForInvalidData()
        {
            const string errorMessage = "Should throw exception.";
            Assert.ThrowsException<ArgumentNullException>(() =>
            { new DATWrapperService(null, _mockPostingRepository.Object, _mockRuntimeSettings.Object); }, errorMessage);

            Assert.ThrowsException<ArgumentNullException>(() =>
            { new DATWrapperService(_mockLog.Object, null, _mockRuntimeSettings.Object); }, errorMessage);

            Assert.ThrowsException<ArgumentNullException>(() =>
            { new DATWrapperService(_mockLog.Object, _mockPostingRepository.Object, null); }, errorMessage);

            _mockRuntimeSettings.SetupGet(s => s.DATLoadPostingWebUrl).Returns(string.Empty);
            Assert.ThrowsException<UriFormatException>(() =>
            { new DATWrapperService(_mockLog.Object, _mockPostingRepository.Object, _mockRuntimeSettings.Object); }, errorMessage);
        }

        [TestMethod, TestCategory("Unit")]
        public void DATWrapperService_PostLoad_ThrowsExceptionForInvalidData()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            { _mockDATWrapperService.Object.PostLoad(null); }, "Should throw exception of type ArgumentNullException.");
        }

        [TestMethod, TestCategory("Unit")]
        public void DATWrapperService_PostLoad_Success()
        {
            var result = _mockDATWrapperService.Object.PostLoad(ObjectHelpers.CreatePostLoadCommand());
            Assert.IsTrue(result, "Result should be true.");
        }

        [TestMethod, TestCategory("Unit")]
        public void DATWrapperService_DeleteLoadPost_ThrowsExceptionForInvalidData()
        {
            const string errorMessage = "Should throw exception.";

            Assert.ThrowsException<ArgumentNullException>(() =>
            { _mockDATWrapperService.Object.DeleteLoadPost(101, null, 1001); }, errorMessage);

            Assert.ThrowsException<ArgumentException>(() =>
            { _mockDATWrapperService.Object.DeleteLoadPost(1, ObjectHelpers.CreateExternalLoadPostCredential(), 0); }, errorMessage);
        }

        [TestMethod, TestCategory("Unit")]
        public void DATWrapperService_DeleteLoadPost_Success()
        {
            _mockDATWrapperService.Object.DeleteLoadPost(101, ObjectHelpers.CreateExternalLoadPostCredential(), 1001);
        }
        #endregion
    }
}