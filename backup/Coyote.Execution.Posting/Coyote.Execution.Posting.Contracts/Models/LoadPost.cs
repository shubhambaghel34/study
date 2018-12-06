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
namespace Coyote.Execution.Posting.Contracts.Models
{
    using System;

    public class LoadPost
    {
        public int Id { get; set; }
        public int CreateByUserId { get; set; }
        public int UserId { get; set; }
        public ExternalLoadPostCredential Credential { get; set; }
        public int LoadId { get; set; }
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
        public int DestinationCityId { get; set; }
        public int OriginCityId { get; set; }
        public int ExternalLoadPostActionId { get; set; }
        public bool IsPostedWhenCovered { get; set; }
        public int ProgressType { get; set; }
        public int StateType { get; set; }
    }
}