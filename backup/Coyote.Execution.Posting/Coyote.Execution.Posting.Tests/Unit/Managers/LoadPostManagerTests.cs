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
namespace Coyote.Execution.Posting.Tests.Unit.Manager
{
    using Coyote.Execution.Posting.Common.Coyote.Types;
    using Coyote.Execution.Posting.Contracts.Commands;
    using Coyote.Execution.Posting.Contracts.Managers;
    using Coyote.Execution.Posting.Contracts.Models;
    using Coyote.Execution.Posting.Contracts.ServiceLayer;
    using Coyote.Execution.Posting.Contracts.Storage;
    using Coyote.Execution.Posting.Domain.Managers;
    using Coyote.Execution.Posting.Tests.Unit.Helper;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Threading.Tasks;

    [TestClass]
    public class LoadPostManagerTests : TestBase
    {
        private Mock<IPostingRepository> PostingRepository { get; set; }
        private Mock<IExternalLoadPostRepository> ExternalLoadPostRepository { get; set; }
        private Mock<IExternalService> ExternalService { get; set; }
        private Mock<IRealtimeService> RealtimeService { get; set; }
        private ILoadPostManager LoadPostManager { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            BaseInitialize();
            PostingRepository = new Mock<IPostingRepository>();
            ExternalService = new Mock<IExternalService>();
            ExternalLoadPostRepository = new Mock<IExternalLoadPostRepository>();
            RealtimeService = new Mock<IRealtimeService>();

            LoadPostManager = new LoadPostManager(PostingRepository.Object, ExternalService.Object, LogMock.Object,RealtimeService.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            ExternalLoadPostRepository.VerifyAll();
            PostingRepository.VerifyAll();
            ExternalService.VerifyAll();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task RepostLoadWithoutCredential_ShouldNotRepostLoad()
        {
            PostLoadCommand postLoadCommand = new PostLoadCommand() { LoadId = 1,
                PostedAsUserId = 1,
                ITSPostStatus = (int)ExternalLoadPostStatus.Posted,
                DATPostStatus = (int)ExternalLoadPostStatus.Posted,
                PostEverywherePostStatus = (int)ExternalLoadPostStatus.Posted
            };

            ExternalLoadPostRepository
                .Setup(r => r.GetExternalLoadPostCredentialByInternalEmployeeId(postLoadCommand.PostedAsUserId))
                .ReturnsAsync(new ExternalLoadPostCredential());
            PostingRepository.SetupGet(r => r.ExternalLoadPostRepository).Returns(ExternalLoadPostRepository.Object);
            var result = await LoadPostManager.RepostLoadWithoutCredential(postLoadCommand);
            Assert.IsFalse(result, "The load cannot be reposted since no valid credentials found from the ActivePost");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Unposted")]
        [TestMethod, TestCategory("Unit")]
        public void RepostLoadWithoutCredential_InvalidPickupDate_ShouldNotRepostLoad()
        {
            PostLoadCommand postLoadCommand = ObjectHelpers.CreatePostLoadCommand_InvalidDate();

            ExternalLoadPostRepository
                .Setup(r => r.GetExternalLoadPostCredentialByInternalEmployeeId(postLoadCommand.PostedAsUserId))
                .ReturnsAsync(new ExternalLoadPostCredential());

            PostingRepository.SetupGet(r => r.ExternalLoadPostRepository).Returns(ExternalLoadPostRepository.Object);

            var result = LoadPostManager.RepostLoadWithoutCredential(postLoadCommand).Result;

            Assert.IsFalse(result, "Load should not get unposted.");
        }

        [TestMethod, TestCategory("Unit")]
        public async Task RepostLoadWithoutCredential_ShouldPostToDAT()
        {
            PostLoadCommand postLoadCommand = ObjectHelpers.CreatePostLoadCommand();
            postLoadCommand.PostToITS = false;
            postLoadCommand.PostToPostEverywhere = false;
            postLoadCommand.PostToDAT = true;
            postLoadCommand.Rate = null;

            ExternalLoadPostRepository
                .Setup(r => r.GetExternalLoadPostCredentialByInternalEmployeeId(postLoadCommand.PostedAsUserId))
                .ReturnsAsync(ObjectHelpers.CreateExternalLoadPostCredential());
            ExternalLoadPostRepository
                .Setup(r => r.InsertAndUpdateExternalLoadPost(It.IsAny<LoadPostBase>()))
                .ReturnsAsync(1);

            PostingRepository.SetupGet(r => r.ExternalLoadPostRepository).Returns(ExternalLoadPostRepository.Object);
            ExternalService.Setup(s => s.DATWrapperService.PostLoad(It.IsAny<LoadPostBase>())).Returns(true);

            var result = await LoadPostManager.RepostLoadWithoutCredential(postLoadCommand);
            Assert.IsTrue(result, "The load shoult be posted to DAT.");
        }

        [TestMethod, TestCategory("Unit")]
        public async Task RepostLoadWithoutCredential_ShouldPostToITSAndPostEverywhere()
        {
            PostLoadCommand postLoadCommand = ObjectHelpers.CreatePostLoadCommand();
            postLoadCommand.PostToDAT = false;
            postLoadCommand.PostToITS = true;
            postLoadCommand.PostToPostEverywhere = true;
            postLoadCommand.Rate = null;

            ExternalLoadPostRepository
                .Setup(r => r.GetExternalLoadPostCredentialByInternalEmployeeId(postLoadCommand.PostedAsUserId))
                .ReturnsAsync(ObjectHelpers.CreateExternalLoadPostCredential());
            ExternalLoadPostRepository
                .Setup(r => r.InsertAndUpdateExternalLoadPost(It.IsAny<LoadPostBase>()))
                .ReturnsAsync(1);
            PostingRepository.SetupGet(r => r.ExternalLoadPostRepository).Returns(ExternalLoadPostRepository.Object);
            ExternalService.Setup(s => s.PostEverywhereExternalService.PostLoad(It.IsAny<LoadPostBase>())).Returns(true);
            ExternalService.Setup(s => s.InternetTruckStopExternalService.PostLoad(It.IsAny<LoadPostBase>())).Returns(true);

            var result = await LoadPostManager.RepostLoadWithoutCredential(postLoadCommand);
            Assert.IsTrue(result, "The load shoult be posted ITS and PostEverywhere.");
        }

        [TestMethod, TestCategory("Unit")]
        public async Task RepostLoadWithoutCredential_ShouldNotPost()
        {
            PostLoadCommand postLoadCommand = ObjectHelpers.CreatePostLoadCommand();
            postLoadCommand.PostToDAT = false;
            postLoadCommand.PostToITS = false;
            postLoadCommand.PostToPostEverywhere = false;

            ExternalLoadPostRepository
                .Setup(r => r.GetExternalLoadPostCredentialByInternalEmployeeId(postLoadCommand.PostedAsUserId))
                .ReturnsAsync(ObjectHelpers.CreateExternalLoadPostCredential());
            PostingRepository.SetupGet(r => r.ExternalLoadPostRepository).Returns(ExternalLoadPostRepository.Object);

            var result = await LoadPostManager.RepostLoadWithoutCredential(postLoadCommand);
            Assert.IsFalse(result, "The load shoult not be posted to any of external site.");
        }
    }
}