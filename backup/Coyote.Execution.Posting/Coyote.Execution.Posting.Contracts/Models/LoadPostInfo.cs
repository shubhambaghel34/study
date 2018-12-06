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
namespace Coyote.Execution.Posting.Contracts.Models
{
    public class LoadPostInfo
    {
        public int LoadId { get; set; }
        public string Equipment { get; set; }
        public bool Team { get; set; }
        public bool HazMat { get; set; }
        public decimal EquipmentLength { get; set; }
        public int NumberOfStops { get; set; }
        public int UserId { get; set; }
    }
}