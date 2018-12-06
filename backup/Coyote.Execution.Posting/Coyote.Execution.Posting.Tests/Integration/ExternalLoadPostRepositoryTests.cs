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
    using Coyote.Execution.Posting.Common.Coyote.Types;
    using Coyote.Execution.Posting.Contracts.Commands;
    using Coyote.Execution.Posting.Contracts.Models;
    using Coyote.Execution.Posting.Contracts.Storage;
    using Coyote.Execution.Posting.Storage.Repositories;
    using Coyote.Execution.Posting.Tests.Integration.Managers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Configuration;

    [TestClass]
    public class ExternalLoadPostRepositoryTests
    {
        #region " Private Fields "
        private const string strTestUrl = "http://test:8080/";
        private TestBucket _testBucket = null;
        private IExternalLoadPostRepository _externalLoadPostRepository = null;
        private string _connectionString = string.Empty;
        #endregion

        #region " Setup "
        [TestInitialize()]
        public void MyTestInitialize()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["Integrated.BazookaDbContext"]?.ConnectionString;

            _testBucket = new TestBucket(_connectionString);
            _externalLoadPostRepository = new ExternalLoadPostRepository(_connectionString);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _testBucket.Close();
        }
        #endregion

        #region " Tests "
        [TestMethod, TestCategory("Integration")]
        public void GetActivePostDetailsByLoadId_IsValid()
        {
            int loadId = ExternalLoadPostRepositoryManager.GetValidLoadIdToRetrieveExternalLoadPost(_connectionString);
            LoadPost loadPost = _externalLoadPostRepository.GetActivePostDetailsByLoadId(loadId).Result;

            Assert.IsNotNull(loadPost, "LoadPost should not be null.");
            Assert.IsTrue(loadPost.PostEverywherePostStatus >= 0, $"PostEverywherePostStatus should be valid.");
            Assert.IsTrue(loadPost.ITSPostStatus >= 0, $"ITSPostStatus should be valid.");
            Assert.IsTrue(loadPost.DATPostStatus >= 0, $"DATPostStatus should be valid.");
            Assert.IsTrue(loadPost.DestinationCityId > 0, "There should be DestinationCityId.");
            Assert.IsTrue(loadPost.OriginCityId > 0, "There should be OriginCityId.");
        }

        [TestMethod, TestCategory("Integration")]
        public void GetActivePostDetailsByLoadId_IsInvalid()
        {
            LoadPost loadPost = _externalLoadPostRepository.GetActivePostDetailsByLoadId(-1).Result;

            Assert.IsNull(loadPost, "ExternalLoadPost should be null.");
        }

        [TestMethod, TestCategory("Integration")]
        public void GetExternalLoadPostCredentialByInternalEmployeeId_IsValid()
        {
            int userId = ExternalLoadPostRepositoryManager.GetValidInternalEmployeeIdToRetrieveExternalLoadPostCredential(_connectionString);
            ExternalLoadPostCredential externalLoadPostCredential = _externalLoadPostRepository.GetExternalLoadPostCredentialByInternalEmployeeId(userId).Result;

            Assert.IsNotNull(externalLoadPostCredential, "ExternalLoadPostCredential should not be null.");
            Assert.IsTrue(externalLoadPostCredential.InternalEmployeeId == userId, $"InternalEmployeeId should be {userId}.");
            Assert.IsTrue(!string.IsNullOrEmpty(externalLoadPostCredential.ITSLogin), "ITSLogin should not be null or empty");
            Assert.IsTrue(!string.IsNullOrEmpty(externalLoadPostCredential.ITSPassword), $"ITSPassword should not be null or empty.");
            Assert.IsTrue(!string.IsNullOrEmpty(externalLoadPostCredential.DATLogin), $"DATLogin should not be null or empty.");
            Assert.IsTrue(!string.IsNullOrEmpty(externalLoadPostCredential.DATPassword), $"DATPassword should not be null or empty.");
            Assert.IsTrue(!string.IsNullOrEmpty(externalLoadPostCredential.DATThirdPartyId), $"DATThirdPartyId should not be null or empty.");
            Assert.IsTrue(!string.IsNullOrEmpty(externalLoadPostCredential.PostEverywhereCustomerKey), $"PostEverywhereCustomerKey should not be null or empty.");
            Assert.IsTrue(!string.IsNullOrEmpty(externalLoadPostCredential.PostEverywhereLogin), $"PostEverywhereLogin should not be null or empty.");
            Assert.IsTrue(!string.IsNullOrEmpty(externalLoadPostCredential.PostEverywherePassword), $"PostEverywherePassword should not be null or empty.");
            Assert.IsTrue(!string.IsNullOrEmpty(externalLoadPostCredential.PostEverywherePhonenumber), $"PostEverywherePhonenumber should not be null or empty.");
            Assert.IsTrue(!string.IsNullOrEmpty(externalLoadPostCredential.PostEverywhereServiceKey), $"PostEverywhereServiceKey should not be null or empty.");
        }

        [TestMethod, TestCategory("Integration")]
        public void GetExternalLoadPostCredentialByInternalEmployeeId_IsInvalid()
        {
            ExternalLoadPostCredential externalLoadPostCredential = _externalLoadPostRepository.GetExternalLoadPostCredentialByInternalEmployeeId(-1).Result;

            Assert.IsNull(externalLoadPostCredential, "ExternalLoadPostCredential should be null.");
        }

        [TestMethod, TestCategory("Integration")]
        public void InsertAndUpdateExternalLoadPost_Success()
        {
            int loadId = ExternalLoadPostRepositoryManager.GetValidLoadIdToRetrieveExternalLoadPost(_connectionString);
            LoadPost retrievedLoadPost = _externalLoadPostRepository.GetActivePostDetailsByLoadId(loadId).Result;
            var LoadPost = new LoadPostBase()
            {
                DATPostStatus = (int)ExternalLoadPostStatus.Posted,
                IsLoadPartial = retrievedLoadPost.IsLoadPartial,
                IsPostedWhenCovered = true,
                ITSPostStatus = (int)ExternalLoadPostStatus.Posted,
                Notes = retrievedLoadPost.Notes,
                PickUpDate = retrievedLoadPost.PickUpDate,
                PostedAsUserId = retrievedLoadPost.PostedAsUserId,
                PostEverywherePostStatus = (int)ExternalLoadPostStatus.Posted,
                Rate = retrievedLoadPost.Rate,
                Weight = retrievedLoadPost.Weight,
                EquipmentType = "V",
                EquipmentLength = 53,
                ExternalLoadPostActionId = 1,
                HazMat = true,
                Team = true,
                NumberOfStops = 2,
                UserId = retrievedLoadPost.CreateByUserId,
                LoadId = loadId,
                Destination = new City() { Id = retrievedLoadPost.DestinationCityId},
                Origin = new City() { Id = retrievedLoadPost.OriginCityId }
            };

            int numberofRowsAffected = _externalLoadPostRepository.InsertAndUpdateExternalLoadPost(LoadPost).Result;
            Assert.IsTrue(numberofRowsAffected > 0, "Should be able to insert & update ExternalLoadPost records.");

            LoadPost expectedLoadPost = _externalLoadPostRepository.GetActivePostDetailsByLoadId(loadId).Result;
            Assert.IsNotNull(expectedLoadPost, "LoadPost should not be null.");
            Assert.IsTrue(expectedLoadPost.IsPostedWhenCovered, "IsPostedWhenCovered should be true.");
            Assert.IsTrue(expectedLoadPost.DATPostStatus == (int)ExternalLoadPostStatus.Posted, "DATPostStatus should be Posted.");
            Assert.IsTrue(expectedLoadPost.PostEverywherePostStatus == (int)ExternalLoadPostStatus.Posted, "PostEverywherePostStatus should be Posted.");
            Assert.IsTrue(expectedLoadPost.ITSPostStatus == (int)ExternalLoadPostStatus.Posted, "ITSPostStatus should be Posted.");
            Assert.IsTrue(expectedLoadPost.Notes.Equals(retrievedLoadPost.Notes), $"Notes should be {retrievedLoadPost.Notes}.");
            Assert.IsTrue(expectedLoadPost.Rate ==retrievedLoadPost.Rate, $"Rate should be {retrievedLoadPost.Rate}.");
            Assert.IsTrue(expectedLoadPost.Weight == retrievedLoadPost.Weight, $"Weight should be {retrievedLoadPost.Rate}.");
            Assert.IsTrue(expectedLoadPost.DestinationCityId == retrievedLoadPost.DestinationCityId, $"DestinationCityId should be {retrievedLoadPost.DestinationCityId}.");
            Assert.IsTrue(expectedLoadPost.OriginCityId == retrievedLoadPost.OriginCityId, $"OriginCityId should be {retrievedLoadPost.OriginCityId}.");

            _testBucket.TakeExternalLoadPostOwnership(expectedLoadPost.Id, retrievedLoadPost.Id);
        }

        [TestMethod, TestCategory("Integration")]
        public void GetActivePostDetailsByLoaIdForAutoRefresh_IsValid()
        {
            int loadId = ExternalLoadPostRepositoryManager.GetValidLoadIdToRetrieveExternalLoadPost(_connectionString);
            LoadPost loadPost = _externalLoadPostRepository.GetActivePostDetailsByLoaIdForAutoRefresh(loadId).Result;

            Assert.IsNotNull(loadPost, "LoadPost should not be null.");
            Assert.IsTrue(loadPost.PostEverywherePostStatus >= 0, $"PostEverywherePostStatus should be valid.");
            Assert.IsTrue(loadPost.ITSPostStatus >= 0, $"ITSPostStatus should be valid.");
            Assert.IsTrue(loadPost.DATPostStatus >= 0, $"DATPostStatus should be valid.");
            Assert.IsTrue(loadPost.DestinationCityId > 0, "There should be DestinationCityId.");
            Assert.IsTrue(loadPost.OriginCityId > 0, "There should be OriginCityId.");
        }

        [TestMethod, TestCategory("Integration")]
        public void GetActivePostDetailsByLoaIdForAutoRefresh_IsInvalid()
        {
            LoadPost loadPost = _externalLoadPostRepository.GetActivePostDetailsByLoaIdForAutoRefresh(-1).Result;

            Assert.IsNull(loadPost, "LoadPost should be null.");
        }

        #endregion
    }
}