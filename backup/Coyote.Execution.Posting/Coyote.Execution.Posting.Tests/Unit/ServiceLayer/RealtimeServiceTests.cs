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
    using Coyote.Common.Finance;
    using Coyote.Execution.Posting.Common.Coyote.Types;
    using Coyote.Execution.Posting.Contracts;
    using Coyote.Execution.Posting.Contracts.Commands;
    using Coyote.Execution.Posting.Contracts.Storage;
    using Coyote.Execution.Posting.ServiceLayer.Realtime;
    using Coyote.Execution.Posting.Tests.Unit.Helper;
    using Coyote.Systems.Bazooka.Realtime.Contracts.Enums;
    using Coyote.Systems.Bazooka.Realtime.Contracts.Tracers;
    using Coyote.Systems.Bazooka.Realtime.Info.Contracts;
    using log4net;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;

    [TestClass]
    public class RealtimeServiceTests
    {
        #region " Private Fields "
        private Mock<IRuntimeSettings> _mockRuntimeSettings;
        private Mock<IPostingRepository> _mockPostingRepository;
        private Mock<ILog> _mockLog;

        private RealtimeService _realtimeService;
        private LoadPostBase _loadPostBase;
        #endregion

        #region " Setup "
        [TestInitialize()]
        public void RealtimeServiceTestsInitialize()
        {
            _mockRuntimeSettings = new Mock<IRuntimeSettings>();
            _mockRuntimeSettings.SetupGet(s => s.RealtimeUpdateServiceAddress).Returns("http://test.com");
            _mockPostingRepository = new Mock<IPostingRepository>();
            _mockLog = new Mock<ILog>();

            _realtimeService = new RealtimeService(_mockPostingRepository.Object, _mockRuntimeSettings.Object, _mockLog.Object);
            _loadPostBase = ObjectHelpers.CreatePostLoadCommand();
        }
        #endregion

        #region " Test cases "
        [TestMethod, TestCategory("Unit")]
        public void RealtimeService_ThrowsExceptionForInvalidData()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            { new RealtimeService(null, _mockRuntimeSettings.Object, _mockLog.Object); }, "Should throw exception of Type ArgumentNullException");

            Assert.ThrowsException<ArgumentNullException>(() =>
            { new RealtimeService(_mockPostingRepository.Object, null, _mockLog.Object); }, "Should throw exception of Type ArgumentNullException");

            Assert.ThrowsException<ArgumentNullException>(() =>
            { new RealtimeService(_mockPostingRepository.Object, _mockRuntimeSettings.Object, null); }, "Should throw exception of Type ArgumentNullException");

            _mockRuntimeSettings.SetupGet(s => s.RealtimeUpdateServiceAddress).Returns(string.Empty);
            Assert.ThrowsException<UriFormatException>(() =>
            { new RealtimeService(_mockPostingRepository.Object, _mockRuntimeSettings.Object, _mockLog.Object); }, "Should throw exception of Type UriFormatException");
        }

        [TestMethod, TestCategory("Unit")]
        public void RealtimeService_BuildRealtimeLoadInfo_Success()
        {
            var dicRealTimeLoadInfo = (Dictionary<string, object>)TestManager.RunInstanceMethod(typeof(RealtimeService), "BuildRealTimeLoadInfo", _realtimeService, new object[1] { _loadPostBase });

            Assert.IsNotNull(dicRealTimeLoadInfo, "Dictionary of RealTimeLoadInfo should not be null.");
            Assert.IsNotNull(dicRealTimeLoadInfo[nameof(IRealTimeLoadInfo.LoadID)], "LoadId should not be null.");
            Assert.IsNotNull(dicRealTimeLoadInfo[nameof(IRealTimeLoadInfo.PostedRate)], "PostedRate should not be null.");
            Assert.IsNotNull(dicRealTimeLoadInfo[nameof(IRealTimeLoadInfo.LoadPostedExternally)], "LoadPostedExternally should not be null.");

            MoneyBind postedRate = (MoneyBind)dicRealTimeLoadInfo[nameof(IRealTimeLoadInfo.PostedRate)];
            bool loadPostedExternally = (_loadPostBase.PostEverywherePostStatus == (int)ExternalLoadPostStatus.Posted ||
                                        _loadPostBase.DATPostStatus == (int)ExternalLoadPostStatus.Posted ||
                                        _loadPostBase.ITSPostStatus == (int)ExternalLoadPostStatus.Posted);

            Assert.IsTrue((bool)dicRealTimeLoadInfo[nameof(IRealTimeLoadInfo.LoadPostedExternally)] == loadPostedExternally, $"LoadPostedExternally should be {loadPostedExternally}.");
            Assert.IsTrue((int)dicRealTimeLoadInfo[nameof(IRealTimeLoadInfo.LoadID)] == _loadPostBase.LoadId , $"LoadId should be {_loadPostBase.LoadId}");
            Assert.IsTrue(postedRate.Amount == _loadPostBase.Rate, $"PostedRate should be {_loadPostBase.Rate}.");
        }

        [TestMethod, TestCategory("Unit")]
        public void RealtimeService_BuildRealtimeTracer_Success()
        {
            var realtimeTracer = (RealtimeTracer)TestManager.RunInstanceMethod(typeof(RealtimeService), "BuildRealtimeTracer", _realtimeService, new object[1] { _loadPostBase });

            Assert.IsNotNull(realtimeTracer, "RealtimeTracer should not be null.");
            Assert.IsNotNull(realtimeTracer.Data, "RealtimeTracer Data should not be null.");
            Assert.IsNotNull(realtimeTracer.Origin, "RealtimeTracer Origin should not be null.");
            Assert.IsTrue(realtimeTracer.Command == (int)BazookaRealtimeUpdating.Load, $"RealtimeTracer Command should be {BazookaRealtimeUpdating.Load}.");
            Assert.IsTrue(realtimeTracer.Domain == (int)Domain.BazookaRealtimeUpdating, $"RealtimeTracer Domain should be {Domain.BazookaRealtimeUpdating}.");
            Assert.IsTrue(realtimeTracer.Version == 2.0M, "RealtimeTracer Version should be 2.0.");
        }

        [TestMethod, TestCategory("Unit")]
        public void RealtimeService_BuildRealtimeContent_Success()
        {
            var content = (string)TestManager.RunInstanceMethod(typeof(RealtimeService), "BuildRealtimeContent", _realtimeService, new object[1] { _loadPostBase });

            Assert.IsNotNull(content, "Content should not be null.");
        }
        #endregion
    }
}