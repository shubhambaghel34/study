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
namespace Coyote.Execution.Posting.Tests.Unit.Controllers
{
    using Coyote.Execution.Posting.Contracts;
    using Coyote.Execution.Posting.Contracts.Commands;
    using Coyote.Execution.Posting.Contracts.Models;
    using Coyote.Execution.Posting.Contracts.Storage;
    using Coyote.Execution.Posting.Tests.Unit.Helper;
    using Coyote.Execution.Posting.Web.Api.Controllers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using NServiceBus.Testing;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    [TestClass]
    public class PostingControllerTests : NServiceBusTestBase
    {
        private PostingController _postingController;
        private TestableEndpointInstance _endpointInstance;
        private Mock<IPostingRepository> _mockPostingRepository;
        private Mock<IRuntimeSettings> _mockRuntimeSettings;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        [TestInitialize]
        public void Initialize()
        {
            NServiceBusTestInitialize();
            _endpointInstance = new TestableEndpointInstance();

            _mockPostingRepository = new Mock<IPostingRepository>();
            _mockPostingRepository.Setup(s => s.ExternalLoadPostRepository.GetActivePostDetailsByLoaIdForAutoRefresh(1001)).ReturnsAsync(ObjectHelpers.CreateLoadPost_Posted());
            _mockPostingRepository.Setup(s => s.ExternalLoadPostRepository.GetActivePostDetailsByLoaIdForAutoRefresh(1002)).ReturnsAsync(ObjectHelpers.CreateLoadPost_NotPosted());
            _mockPostingRepository.Setup(s => s.ExternalLoadPostRepository.GetActivePostDetailsByLoaIdForAutoRefresh(1003)).Throws(new Exception());
            _mockPostingRepository.Setup(s => s.LocationCountryRepository.GetCityDetailsByCityId(1)).Returns(ObjectHelpers.CreateCity());
            _mockPostingRepository.Setup(s => s.LocationCountryRepository.GetCityDetailsByCityId(2)).Returns(ObjectHelpers.CreateCity());
            _mockPostingRepository.Setup(s => s.ExternalLoadPostRepository.GetActivePostDetailsByLoadId(101)).ReturnsAsync(ObjectHelpers.CreateLoadPost_Posted());
            _mockPostingRepository.Setup(s => s.ExternalLoadPostRepository.GetActivePostDetailsByLoadId(102)).ReturnsAsync(ObjectHelpers.CreateLoadPost_NotPosted());

            _mockRuntimeSettings = new Mock<IRuntimeSettings>();
            _postingController = new PostingController(LogMock.Object, _endpointInstance, _mockPostingRepository.Object, _mockRuntimeSettings.Object);
            _postingController.Configuration = new HttpConfiguration();
            _postingController.Request = new HttpRequestMessage();
        }

        [TestCleanup]
        public void Cleanup()
        {
            LogMock.VerifyAll();
            _postingController.Dispose();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task RepostLoadWithoutCredential_PostLoadCommand_Success()
        {
            var loadPostInfo = ObjectHelpers.CreateLoadPostInfo();
            await _postingController.RepostLoadWithoutCredential(loadPostInfo);
            var sentMessages = _endpointInstance.SentMessages;
            Assert.AreEqual(1, sentMessages.Length);
            Assert.IsInstanceOfType(sentMessages[0].Message, typeof(PostLoadCommand));
        }

        [TestMethod, TestCategory("Unit")]
        public async Task RepostLoadWithoutCredential_PostLoadCommand_Failed()
        {
            var loadPostInfo = ObjectHelpers.CreateLoadPostInfo();
            loadPostInfo.LoadId = 102;
            await _postingController.RepostLoadWithoutCredential(loadPostInfo);
            var sentMessages = _endpointInstance.SentMessages;
            Assert.AreEqual(0, sentMessages.Length);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task RepostLoadWithoutCredential_PostLoadCommand_Exception()
        {
            await _postingController.RepostLoadWithoutCredential(null);
            var sentMessages = _endpointInstance.SentMessages;
            Assert.AreEqual(0, sentMessages.Length);
        }

        [TestMethod, TestCategory("Unit")]
        public void RepostLoadOnAutoRefresh_PostLoadCommand_Sent()
        {
            var httpResponse = _postingController.RepostLoadOnAutoRefresh(ObjectHelpers.CreateAutoRefreshLoadPostInfo_PostToAll()).Result;
            var sentMessages = _endpointInstance.SentMessages;

            Assert.AreEqual(1, sentMessages.Length, "Should send AutoRefreshCommand.");
            Assert.IsInstanceOfType(sentMessages[0].Message, typeof(PostLoadCommand));

            Assert.IsNotNull(httpResponse, "Response should not be null.");
            Assert.IsTrue(httpResponse.IsSuccessStatusCode, "StatusCode should be success.");

            var result = httpResponse.Content.ReadAsAsync<bool>();
            Assert.IsNotNull(result, "Result should not be null.");
            Assert.IsTrue(result.Result, "Result should be true.");
        }

        [TestMethod, TestCategory("Unit")]
        public void RepostLoadOnAutoRefresh_PostLoadCommand_NotSent()
        {
            AutoRefreshLoadPostInfo autoRefreshLoadPostInfo = ObjectHelpers.CreateAutoRefreshLoadPostInfo_PostToAll();
            autoRefreshLoadPostInfo.LoadId = 1002;
            var httpResponse = _postingController.RepostLoadOnAutoRefresh(autoRefreshLoadPostInfo).Result;
            var sentMessages = _endpointInstance.SentMessages;

            Assert.AreEqual(0, sentMessages.Length, "Should not send any command.");
            Assert.IsNotNull(httpResponse, "Response should not be null.");
            Assert.IsTrue(httpResponse.IsSuccessStatusCode, "StatusCode should be success.");

            var result = httpResponse.Content.ReadAsAsync<bool>();
            Assert.IsNotNull(result, "Result should not be null.");
            Assert.IsFalse(result.Result, "Result should be false.");
        }

        [TestMethod, TestCategory("Unit")]
        public void RepostLoadOnAutoRefresh_ThrowException()
        {
            AutoRefreshLoadPostInfo autoRefreshLoadPostInfo = ObjectHelpers.CreateAutoRefreshLoadPostInfo_PostToAll();
            autoRefreshLoadPostInfo.LoadId = 0;
            var httpResponse = _postingController.RepostLoadOnAutoRefresh(autoRefreshLoadPostInfo).Result;
            Assert.IsNotNull(httpResponse, "Response should not be null.");
            Assert.IsFalse(httpResponse.IsSuccessStatusCode, "IsSuccessStatusCode should not be false.");
            Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest, "StatusCode should be BadRequest.");

            var error = httpResponse.Content.ReadAsAsync<HttpError>();
            Assert.IsNotNull(error, "Should throw an error.");

            autoRefreshLoadPostInfo.LoadId = 1003;
            httpResponse = _postingController.RepostLoadOnAutoRefresh(autoRefreshLoadPostInfo).Result;
            Assert.IsNotNull(httpResponse, "Response should not be null.");
            Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest, "StatusCode should be BadRequest.");

            error = httpResponse.Content.ReadAsAsync<HttpError>();
            Assert.IsNotNull(error, "Should throw an error.");
        }

        [TestMethod, TestCategory("Unit")]
        public void RepostLoadOnAutoRefresh_ShouldNotRepostToDAT()
        {
            AutoRefreshLoadPostInfo autoRefreshLoadPostInfo = ObjectHelpers.CreateAutoRefreshLoadPostInfo_PostToAll();
            autoRefreshLoadPostInfo.LoadId = 1001;
            autoRefreshLoadPostInfo.PostToDAT = false;

            var httpResponse = _postingController.RepostLoadOnAutoRefresh(autoRefreshLoadPostInfo).Result;
            var sentMessages = _endpointInstance.SentMessages;

            Assert.AreEqual(1, sentMessages.Length, "Should send PostLoadCommand.");
            Assert.IsInstanceOfType(sentMessages[0].Message, typeof(PostLoadCommand));

            PostLoadCommand postLoadCommand = (PostLoadCommand) sentMessages[0].Message;

            Assert.IsTrue(postLoadCommand.PostToITS, "PostToITS should be true.");
            Assert.IsTrue(postLoadCommand.PostToPostEverywhere, "PostToPostEverywhere should be true.");
            Assert.IsFalse(postLoadCommand.PostToDAT, "PostToDAT should be true.");
        }
    }
}