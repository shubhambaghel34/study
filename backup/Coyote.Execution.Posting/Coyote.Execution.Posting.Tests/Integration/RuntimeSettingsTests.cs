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
namespace Coyote.Execution.Posting.Tests.Integration
{
    using Coyote.Execution.Posting.Contracts;
    using Coyote.Execution.Posting.Storage;
    using log4net;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Configuration;

    [TestClass]
    public class RuntimeSettingsTests
    {
        #region " Private Fields "
        private IRuntimeSettings _runtimeSettings;
        private static Mock<ILog> MockLog;
        private string _connectionString;
        #endregion

        #region " Setup "
        [TestInitialize()]
        public void RuntimeSettingsTestInitialize()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["Integrated.BazookaDbContext"]?.ConnectionString;
            MockLog = new Mock<ILog>();
        }
        #endregion

        #region " Tests "
        [TestMethod, TestCategory("Integration")]
        public void RuntimeSettings_Populate_Success()
        {
            _runtimeSettings = new RuntimeSettings(MockLog.Object, _connectionString);

            Assert.IsNotNull(_runtimeSettings, "RuntimeSettings should not be null.");
            Assert.IsTrue(_runtimeSettings.IsPopulated, "IsPopulated should be true.");
            Assert.IsFalse(string.IsNullOrEmpty(_runtimeSettings.DATLoadPostingWebUrl), "DATLoadPostingWebUrl should not be null or empty.");
            Assert.IsFalse(string.IsNullOrEmpty(_runtimeSettings.RealtimeUpdateServiceAddress), "RealtimeUpdateServiceAddress should not be null or empty.");
            Assert.IsTrue(_runtimeSettings.ServiceUserId.Equals(-158), "ServiceUserId should be -158.");

            var DATLoadPostingWebUrl = new Uri(_runtimeSettings.DATLoadPostingWebUrl);
            var RealtimeUpdateServiceAddress = new Uri(_runtimeSettings.RealtimeUpdateServiceAddress);

            Assert.IsTrue(DATLoadPostingWebUrl.IsAbsoluteUri, "DATLoadPostingWebUrl should be absolute.");
            Assert.IsTrue(RealtimeUpdateServiceAddress.IsAbsoluteUri, "RealtimeUpdateServiceAddress should be absolute.");

        }
        #endregion
    }
}