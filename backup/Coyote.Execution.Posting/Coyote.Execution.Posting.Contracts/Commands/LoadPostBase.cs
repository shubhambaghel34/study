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
namespace Coyote.Execution.Posting.Contracts.Commands
{
    using Coyote.Execution.Posting.Common.Extensions;
    using Coyote.Execution.Posting.Contracts.Models;
    using System;

    public class LoadPostBase
    {
        #region " Public fields "
        public int CreateByUserId { get; set; }
        public int UserId { get; set; }
        public ExternalLoadPostCredential Credential { get; set; }
        public int LoadId { get; set; }
        public City Origin { get; set; }
        public City Destination { get; set; }
        public string EquipmentType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "UpDate")]
        public DateTime PickUpDate { get; set; }
        public bool IsLoadPartial { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Rate { get; set; }
        public int NumberOfStops { get; set; }
        public string Notes { get; set; }
        public bool Team { get; set; }
        public bool HazMat { get; set; }
        public decimal EquipmentLength { get; set; }
        public int ITSPostStatus { get; set; }
        public int DATPostStatus { get; set; }
        public int PostEverywherePostStatus { get; set; }
        public int PostedAsUserId { get; set; }
        public int ExternalLoadPostActionId { get; set; }
        public bool IsPostedWhenCovered { get; set; }
        #endregion

        #region " Constructor "
        public LoadPostBase()
        {
        }

        public LoadPostBase(LoadPost loadPost)
        {
            loadPost.ThrowIfArgumentNull(nameof(loadPost));

            Credential = loadPost.Credential;
            DATPostStatus = loadPost.DATPostStatus;
            EquipmentType = loadPost.EquipmentType;
            EquipmentLength = loadPost.EquipmentLength;
            HazMat = loadPost.HazMat;
            IsLoadPartial = loadPost.IsLoadPartial;
            ITSPostStatus = loadPost.ITSPostStatus;
            LoadId = loadPost.LoadId;
            Notes = loadPost.Notes;
            NumberOfStops = loadPost.NumberOfStops;
            PickUpDate = loadPost.PickUpDate;
            PostedAsUserId = loadPost.PostedAsUserId;
            PostEverywherePostStatus = loadPost.PostEverywherePostStatus;
            Rate = loadPost.Rate;
            Team = loadPost.Team;
            UserId = loadPost.UserId;
            Weight = loadPost.Weight;
            CreateByUserId = loadPost.CreateByUserId;
            ExternalLoadPostActionId = loadPost.ExternalLoadPostActionId;
            IsPostedWhenCovered = loadPost.IsPostedWhenCovered;
        }
        #endregion
    }
}