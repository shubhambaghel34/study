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
namespace Coyote.Execution.Posting.Tests.Unit.Models
{
    using Coyote.Execution.Posting.Tests.Unit.Helper;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ExternalLoadPostCredentialTests
    {
        [TestMethod, TestCategory("Unit")]
        public void ExternalLoadPostCredential_CanPostToITS_Valid()
        {
            var externalLoadPostCredential = ObjectHelpers.CreateExternalLoadPostCredential();

            Assert.IsTrue(externalLoadPostCredential.CanPostToITS());
        }

        [TestMethod, TestCategory("Unit")]
        public void ExternalLoadPostCredential_CanPostToITS_InvalidITSIntegrationId()
        {
            var externalLoadPostCredential = ObjectHelpers.CreateExternalLoadPostCredential();
            externalLoadPostCredential.ITSIntegrationId = null;
            Assert.IsFalse(externalLoadPostCredential.CanPostToITS());
        }

        [TestMethod, TestCategory("Unit")]
        public void ExternalLoadPostCredential_CanPostToITS_InvalidITSPassword()
        {
            var externalLoadPostCredential = ObjectHelpers.CreateExternalLoadPostCredential();
            externalLoadPostCredential.ITSPassword = string.Empty;
            Assert.IsFalse(externalLoadPostCredential.CanPostToITS());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
        [TestMethod, TestCategory("Unit")]
        public void ExternalLoadPostCredential_CanPostToITS_InvalidITSLogin()
        {
            var externalLoadPostCredential = ObjectHelpers.CreateExternalLoadPostCredential();
            externalLoadPostCredential.ITSLogin = string.Empty;
            Assert.IsFalse(externalLoadPostCredential.CanPostToITS());
        }

        [TestMethod, TestCategory("Unit")]
        public void ExternalLoadPostCredential_CanPostToDAT_Valid()
        {
            var externalLoadPostCredential = ObjectHelpers.CreateExternalLoadPostCredential();

            Assert.IsTrue(externalLoadPostCredential.CanPostToDAT());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
        [TestMethod, TestCategory("Unit")]
        public void ExternalLoadPostCredential_CanPostToDAT_InvalidDATLogin()
        {
            var externalLoadPostCredential = ObjectHelpers.CreateExternalLoadPostCredential();
            externalLoadPostCredential.DATLogin = string.Empty;
            Assert.IsFalse(externalLoadPostCredential.CanPostToDAT());
        }

        [TestMethod, TestCategory("Unit")]
        public void ExternalLoadPostCredential_CanPostToDAT_InvalidDATPassword()
        {
            var externalLoadPostCredential = ObjectHelpers.CreateExternalLoadPostCredential();
            externalLoadPostCredential.DATPassword = string.Empty;
            Assert.IsFalse(externalLoadPostCredential.CanPostToDAT());
        }

        [TestMethod, TestCategory("Unit")]
        public void ExternalLoadPostCredential_CanPostToPostEverywhere_Valid()
        {
            var externalLoadPostCredential = ObjectHelpers.CreateExternalLoadPostCredential();

            Assert.IsTrue(externalLoadPostCredential.CanPostToPostEverywhere());
        }

        [TestMethod, TestCategory("Unit")]
        public void ExternalLoadPostCredential_CanPostToPostEverywhere_InvalidPostEverywhereCustomerKey()
        {
            var externalLoadPostCredential = ObjectHelpers.CreateExternalLoadPostCredential();
            externalLoadPostCredential.PostEverywhereCustomerKey = string.Empty;
            Assert.IsFalse(externalLoadPostCredential.CanPostToPostEverywhere());
        }

        [TestMethod, TestCategory("Unit")]
        public void ExternalLoadPostCredential_CanPostToPostEverywhere_InvalidPostEverywhereServiceKey()
        {
            var externalLoadPostCredential = ObjectHelpers.CreateExternalLoadPostCredential();
            externalLoadPostCredential.PostEverywhereServiceKey = string.Empty;
            Assert.IsFalse(externalLoadPostCredential.CanPostToPostEverywhere());
        }

        [TestMethod, TestCategory("Unit")]
        public void ExternalLoadPostCredential_CanPostToPostEverywhere_InvalidPostEverywherePhoneNumber()
        {
            var externalLoadPostCredential = ObjectHelpers.CreateExternalLoadPostCredential();
            externalLoadPostCredential.PostEverywherePhonenumber = string.Empty;
            Assert.IsFalse(externalLoadPostCredential.CanPostToPostEverywhere());
        }
    }
}