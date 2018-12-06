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
namespace Coyote.Execution.Posting.Tests.Unit.Helper
{
    using Coyote.Execution.Posting.Common.Coyote.Types;
    using Coyote.Execution.Posting.Contracts.Commands;
    using Coyote.Execution.Posting.Contracts.Models;
    using Ploeh.AutoFixture;
    using System;

    public static class ObjectHelpers
    {
        public static LoadPost CreateLoadPost()
        {
            var fixture = new Fixture();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var loadPost = fixture.Create<LoadPost>();
            loadPost.LoadId = 1234;
            loadPost.PickUpDate = DateTime.Now;
            loadPost.IsPostedWhenCovered = true;
            loadPost.ITSPostStatus = 1;

            return loadPost;
        }

        public static ExternalLoadPostCredential CreateExternalLoadPostCredential()
        {
            var fixture = new Fixture();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var externalLoadPostCredential = fixture.Create<ExternalLoadPostCredential>();
            externalLoadPostCredential.InternalEmployeeId = 1234;
            externalLoadPostCredential.DATLogin = "UserName";
            externalLoadPostCredential.DATPassword = "Password";
            externalLoadPostCredential.DATThirdPartyId = "ID";
            externalLoadPostCredential.ITSIntegrationId = 123;
            externalLoadPostCredential.ITSLogin = "UserName";
            externalLoadPostCredential.ITSPassword = "Password";
            externalLoadPostCredential.PostEverywhereServiceKey = "Key";
            externalLoadPostCredential.PostEverywhereCustomerKey = "Customer";
            externalLoadPostCredential.PostEverywhereLogin = "UserName";
            externalLoadPostCredential.PostEverywherePassword = "Password";
            externalLoadPostCredential.PostEverywherePhonenumber = "1234567890";

            return externalLoadPostCredential;
        }

        public static PostLoadCommand CreatePostLoadCommand()
        {
            var fixture = new Fixture();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var postLoadCommand = fixture.Create<PostLoadCommand>();
            postLoadCommand.LoadId = 1;
            postLoadCommand.PostedAsUserId = 1;
            postLoadCommand.PickUpDate = DateTime.Now.AddDays(1);
            postLoadCommand.Rate = 20M;
            postLoadCommand.PostEverywherePostStatus = (int)ExternalLoadPostStatus.Posted;
            postLoadCommand.DATPostStatus = (int)ExternalLoadPostStatus.Posted;
            postLoadCommand.ITSPostStatus = (int)ExternalLoadPostStatus.Posted;

            return postLoadCommand;
        }

        public static LoadPost CreateLoadPost_Posted()
        {
            var fixture = new Fixture();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var loadPost = fixture.Create<LoadPost>();
            loadPost.PostEverywherePostStatus = (int)ExternalLoadPostStatus.Posted;
            loadPost.DATPostStatus = (int)ExternalLoadPostStatus.Posted;
            loadPost.ITSPostStatus = (int)ExternalLoadPostStatus.Posted;
            loadPost.IsPostedWhenCovered = true;
            loadPost.OriginCityId = 1;
            loadPost.DestinationCityId = 2;
            loadPost.PickUpDate = DateTime.Now;
            loadPost.ProgressType = (int)LoadProgressType.Available;
            loadPost.StateType = (int)LoadState.Active;
            return loadPost;
        }

        public static LoadPost CreateLoadPost_NotPosted()
        {
            var fixture = new Fixture();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var loadPost = fixture.Create<LoadPost>();
            loadPost.IsPostedWhenCovered = false;
            loadPost.PostEverywherePostStatus = (int)ExternalLoadPostStatus.NotPosted;
            loadPost.DATPostStatus = (int)ExternalLoadPostStatus.NotPosted;
            loadPost.ITSPostStatus = (int)ExternalLoadPostStatus.NotPosted;

            return loadPost;
        }

        public static PostLoadCommand CreatePostLoadCommand_InvalidDate()
        {
            var fixture = new Fixture();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var postLoadCommand = fixture.Create<PostLoadCommand>();
            postLoadCommand.PickUpDate = DateTime.Now.AddDays(-1);
            postLoadCommand.PostedAsUserId = 1;
            return postLoadCommand;
        }

        public static City CreateCity()
        {
            var fixture = new Fixture();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var city = fixture.Create<City>();

            return city;
        }

        public static LoadPostInfo CreateLoadPostInfo()
        {
            var fixture = new Fixture();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var loadPostInfo = fixture.Create<LoadPostInfo>();
            loadPostInfo.LoadId = 101;

            return loadPostInfo;
        }

        public static AutoRefreshLoadPostInfo CreateAutoRefreshLoadPostInfo_PostToAll()
        {
            var fixture = new Fixture();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var autoRefreshLoadPostInfo = fixture.Create<AutoRefreshLoadPostInfo>();
            autoRefreshLoadPostInfo.LoadId = 1001;
            autoRefreshLoadPostInfo.PostToDAT = true;
            autoRefreshLoadPostInfo.PostToITS = true;
            autoRefreshLoadPostInfo.PostToPostEverywhere = true;

            return autoRefreshLoadPostInfo;
        }
    }
}