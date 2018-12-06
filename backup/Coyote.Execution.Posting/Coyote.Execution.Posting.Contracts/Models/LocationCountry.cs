// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2017 - 2017
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

    public class LocationCountry
    {
        public int LocationCountryId { get; set; }
        public int GeoRegionId { get; set; }
        public string ISOCodeAlpha2 { get; set; }
        public string ISOCodeAlpha3 { get; set; }
        public string PhoneCode { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdateByUserId { get; set; }
    }
}