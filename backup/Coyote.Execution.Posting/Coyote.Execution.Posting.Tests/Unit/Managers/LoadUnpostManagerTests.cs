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
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Threading.Tasks;

    [TestClass]
    public class LoadUnpostManagerTests : TestBase
    {
        private Mock<IPostingRepository> PostingRepository { get; set; }
        private Mock<IExternalLoadPostRepository> LoadPostRepository { get; set; }
        private Mock<IExternalService> ExternalService { get; set; }
        private ILoadUnpostManager LoadUnpostManager { get; set; }
        private Mock<IRealtimeService> RealtimeService { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            BaseInitialize();
            PostingRepository = new Mock<IPostingRepository>();
            ExternalService = new Mock<IExternalService>();
            LoadPostRepository = new Mock<IExternalLoadPostRepository>();
            RealtimeService = new Mock<IRealtimeService>();

            LoadUnpostManager = new LoadUnpostManager(PostingRepository.Object, ExternalService.Object, LogMock.Object, RealtimeService.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            LoadPostRepository.VerifyAll();
            PostingRepository.VerifyAll();
            ExternalService.VerifyAll();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task LoadUnpostManager_NotPostedLoad_ShouldNotUnpostLoad()
        {
            UnpostLoadCommand unpostLoadCommand = new UnpostLoadCommand() { LoadId = 1 };
            PostingRepository.SetupGet(r => r.ExternalLoadPostRepository).Returns(LoadPostRepository.Object);
            var result = await LoadUnpostManager.UnpostLoad(unpostLoadCommand);
            Assert.IsFalse(result, "The load cannot be unposted since its not yet posted");
        }

        [TestMethod, TestCategory("Unit")]
        public async Task LoadUnpostManager_WithoutCredentials_ShouldNotUnpostLoad()
        {
            UnpostLoadCommand unpostLoadCommand = new UnpostLoadCommand() { LoadId = 1,
                PostedAsUserId = 1,
                ITSPostStatus = (int)ExternalLoadPostStatus.Posted,
                DATPostStatus = (int)ExternalLoadPostStatus.Posted,
                PostEverywherePostStatus = (int)ExternalLoadPostStatus.Posted
            };

            LoadPostRepository
                .Setup(r => r.GetExternalLoadPostCredentialByInternalEmployeeId(unpostLoadCommand.PostedAsUserId))
                .ReturnsAsync(new ExternalLoadPostCredential());
            PostingRepository.SetupGet(r => r.ExternalLoadPostRepository).Returns(LoadPostRepository.Object);
            var result = await LoadUnpostManager.UnpostLoad(unpostLoadCommand);
            Assert.IsFalse(result, "The load cannot be unpost since no valid credentials found from the ActivePost");
        }
    }
}