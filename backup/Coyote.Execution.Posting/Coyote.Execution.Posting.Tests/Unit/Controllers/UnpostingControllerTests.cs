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
namespace Coyote.Execution.Posting.Tests.Unit.Controllers
{
    using Coyote.Execution.Posting.Common.Coyote.Types;
    using Coyote.Execution.Posting.Contracts.Commands;
    using Coyote.Execution.Posting.Contracts.Models;
    using Coyote.Execution.Posting.Contracts.Storage;
    using Coyote.Execution.Posting.Tests.Unit.Helper;
    using Coyote.Execution.Posting.Web.Api.Controllers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using NServiceBus.Testing;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable"),
     System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Unposting")]
    [TestClass]
    public class UnpostingControllerTests : NServiceBusTestBase
    {
        private UnpostingController _unpostingController;
        private TestableEndpointInstance _endpointInstance;
        private Mock<IPostingRepository> _mockPostingRepository;

        [TestInitialize]
        public void Initialize()
        {
            NServiceBusTestInitialize();
            _endpointInstance = new TestableEndpointInstance();
            _mockPostingRepository = new Mock<IPostingRepository>();
            _mockPostingRepository.Setup(s => s.ExternalLoadPostRepository.GetActivePostDetailsByLoadId(101)).ReturnsAsync(ObjectHelpers.CreateLoadPost());
            _mockPostingRepository.Setup(s => s.ExternalLoadPostRepository.GetActivePostDetailsByLoadId(102)).ReturnsAsync(null);
            _mockPostingRepository.Setup(s => s.ExternalLoadPostRepository.GetActivePostDetailsByLoadId(103)).ReturnsAsync(ObjectHelpers.CreateLoadPost_NotPosted());
            _unpostingController = new UnpostingController(LogMock.Object, _endpointInstance, _mockPostingRepository.Object);
            _unpostingController.Configuration = new HttpConfiguration();
            _unpostingController.Request = new HttpRequestMessage();
        }

        [TestCleanup]
        public void Cleanup()
        {
            LogMock.VerifyAll();
            _unpostingController.Dispose();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task PostingController_SendUnpostLoadCommand_Success()
        {

            LoadPostInfo loadPostInfo = ObjectHelpers.CreateLoadPostInfo();
            
            await _unpostingController.UnpostLoad(loadPostInfo);
            var sentMessages = _endpointInstance.SentMessages;
            Assert.AreEqual(1, sentMessages.Length);
            Assert.IsInstanceOfType(sentMessages[0].Message, typeof(UnpostLoadCommand));

            var message = (UnpostLoadCommand)sentMessages[0].Message;
            Assert.IsTrue(message.IsPostedWhenCovered, "IsPostedWhenCovered should be true.");
            Assert.IsTrue(message.ExternalLoadPostActionId == (int)ExternalLoadPostAction.Unpost, "ExternalLoadPostActionId should be Unpost.");

        }

        [TestMethod, TestCategory("Unit")]
        public async Task PostingController_SendUnpostLoadCommand_Failed()
        {
            LoadPostInfo loadPostInfo = ObjectHelpers.CreateLoadPostInfo();
            loadPostInfo.LoadId = 102;
            var httpResponse = await _unpostingController.UnpostLoad(loadPostInfo);
            var sentMessages = _endpointInstance.SentMessages;
            Assert.AreEqual(0, sentMessages.Length);

            Assert.IsNotNull(httpResponse, "Response should not be null.");
            Assert.IsTrue(httpResponse.IsSuccessStatusCode, "StatusCode should be success.");

            var result = httpResponse.Content.ReadAsAsync<bool>();
            Assert.IsNotNull(result, "Result should not be null.");
            Assert.IsFalse(result.Result, "Result should be false.");
        }

        [TestMethod, TestCategory("Unit")]
        public async Task PostingController_SendUnpostLoadCommand_Exception()
        {
            var httpResponse = await _unpostingController.UnpostLoad(null);
            var sentMessages = _endpointInstance.SentMessages;
            Assert.AreEqual(0, sentMessages.Length);

            Assert.IsNotNull(httpResponse, "Response should not be null.");
            Assert.IsFalse(httpResponse.IsSuccessStatusCode, "IsSuccessStatusCode should be false.");
            Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest, "StatusCode should be BadRequest.");

            var error = httpResponse.Content.ReadAsAsync<HttpError>();
            Assert.IsNotNull(error, "Should throw an error.");
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UnpostLoadOnStateChange_SendUnpostLoadCommand_Success()
        {

            LoadPostInfo loadPostInfo = ObjectHelpers.CreateLoadPostInfo();
            loadPostInfo.LoadId = 103;
            await _unpostingController.UnpostLoadOnStateChange(loadPostInfo);
            var sentMessages = _endpointInstance.SentMessages;
            Assert.AreEqual(1, sentMessages.Length);
            Assert.IsInstanceOfType(sentMessages[0].Message, typeof(UnpostLoadCommand));

            var message = (UnpostLoadCommand)sentMessages[0].Message;
            Assert.IsFalse(message.IsPostedWhenCovered, "IsPostedWhenCovered should be false.");
            Assert.IsTrue(message.ExternalLoadPostActionId == (int)ExternalLoadPostAction.Unpost, "ExternalLoadPostActionId should be Unpost.");
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UnpostLoadOnStateChange_SendUnpostLoadCommand_Failed()
        {
            LoadPostInfo loadPostInfo = ObjectHelpers.CreateLoadPostInfo();
            loadPostInfo.LoadId = 102;
            var httpResponse = await _unpostingController.UnpostLoadOnStateChange(loadPostInfo);
            var sentMessages = _endpointInstance.SentMessages;
            Assert.AreEqual(0, sentMessages.Length);

            Assert.IsNotNull(httpResponse, "Response should not be null.");
            Assert.IsTrue(httpResponse.IsSuccessStatusCode, "StatusCode should be success.");

            var result = httpResponse.Content.ReadAsAsync<bool>();
            Assert.IsNotNull(result, "Result should not be null.");
            Assert.IsFalse(result.Result, "Result should be false.");
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UnpostLoadOnStateChange_SendUnpostLoadCommand_Exception()
        {
            var httpResponse = await _unpostingController.UnpostLoadOnStateChange(null);
            var sentMessages = _endpointInstance.SentMessages;
            Assert.AreEqual(0, sentMessages.Length);

            Assert.IsNotNull(httpResponse, "Response should not be null.");
            Assert.IsFalse(httpResponse.IsSuccessStatusCode, "IsSuccessStatusCode should be false.");
            Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest, "StatusCode should be BadRequest.");

            var error = httpResponse.Content.ReadAsAsync<HttpError>();
            Assert.IsNotNull(error, "Should throw an error.");
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UnpostLoadOnRolled_SendUnpostLoadCommand_Success()
        {

            LoadPostInfo loadPostInfo = ObjectHelpers.CreateLoadPostInfo();
            loadPostInfo.LoadId = 103;
            await _unpostingController.UnpostLoadOnRolled(loadPostInfo);
            var sentMessages = _endpointInstance.SentMessages;
            Assert.AreEqual(1, sentMessages.Length);
            Assert.IsInstanceOfType(sentMessages[0].Message, typeof(UnpostLoadCommand));

            var message = (UnpostLoadCommand)sentMessages[0].Message;
            Assert.IsTrue(message.ExternalLoadPostActionId == (int)ExternalLoadPostAction.Unpost, "ExternalLoadPostActionId should be Unpost.");
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UnpostLoadOnRolled_SendUnpostLoadCommand_Failed()
        {
            LoadPostInfo loadPostInfo = ObjectHelpers.CreateLoadPostInfo();
            loadPostInfo.LoadId = 102;
            var httpResponse = await _unpostingController.UnpostLoadOnRolled(loadPostInfo);
            var sentMessages = _endpointInstance.SentMessages;
            Assert.AreEqual(0, sentMessages.Length);

            Assert.IsNotNull(httpResponse, "Response should not be null.");
            Assert.IsTrue(httpResponse.IsSuccessStatusCode, "Status Code should be success.");

            var result = httpResponse.Content.ReadAsAsync<bool>();
            Assert.IsNotNull(result, "Result should not be null.");
            Assert.IsFalse(result.Result, "Result should be false.");
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UnpostLoadOnRolled_SendUnpostLoadCommand_Exception()
        {
            var httpResponse = await _unpostingController.UnpostLoadOnRolled(null);
            var sentMessages = _endpointInstance.SentMessages;
            Assert.AreEqual(0, sentMessages.Length);

            Assert.IsNotNull(httpResponse, "Response should not be null.");
            Assert.IsFalse(httpResponse.IsSuccessStatusCode, "IsSuccessStatusCode should be false.");
            Assert.IsTrue(httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest, "Status Code should be BadRequest.");

            var error = httpResponse.Content.ReadAsAsync<HttpError>();
            Assert.IsNotNull(error, "Should throw an error.");
        }
    }
}