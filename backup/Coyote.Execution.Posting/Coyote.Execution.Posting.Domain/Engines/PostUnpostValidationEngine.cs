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

namespace Coyote.Execution.Posting.Domain.Engines
{
    using Coyote.Execution.Posting.Common.Coyote.Types;
    using Coyote.Execution.Posting.Contracts.Models;
    using System;

    public static class PostUnpostValidationEngine
    {
        public static bool ShouldRepostOnBounce(LoadPost loadPost)
        {
            return loadPost != null && (loadPost.IsPostedWhenCovered || IsLoadPosted(loadPost)) && IsPickupDateValid(loadPost.PickUpDate);
        }

        public static bool ShouldRepostOnAutoRefresh(LoadPost loadPost)
        {
            return loadPost != null
                && IsLoadPosted(loadPost)
                && IsPickupDateValid(loadPost.PickUpDate)
                && loadPost.ProgressType == (int)LoadProgressType.Available
                && (loadPost.StateType == (int)LoadState.Active || loadPost.StateType == (int)LoadState.NonComm);
        }

        public static bool IsLoadPosted(LoadPost loadPost)
        {
            return loadPost!= null && (loadPost.ITSPostStatus == (int)ExternalLoadPostStatus.Posted ||
                                    loadPost.DATPostStatus == (int)ExternalLoadPostStatus.Posted ||
                                    loadPost.PostEverywherePostStatus == (int)ExternalLoadPostStatus.Posted);
        }

        private static bool IsPickupDateValid(DateTime pickupDate)
        {
            return pickupDate.Date >= DateTime.Now.Date;
        }
    }
}
