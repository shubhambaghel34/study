// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2016 - 2018
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////

namespace Coyote.Execution.CheckCall.Domain.Models
{    
    using System;    

    public class Load
    {

        public int Id { get; set; }
        public DateTime LoadDate { get; set; }
        public LoadModeType Mode { get; set; }
        public LoadStateType StateType { get; set; }
        public int ProgressType { get; set; }
        public string EquipmentType { get; set; }            
        public string OriginCityName { get; set; }
        public string OriginStateCode { get; set; }
        public string DestinationCityName { get; set; }
        public string DestinationStateCode { get; set; }
        public  LoadCarrier MainLoadCarrier { get; set; }
        public  LoadCustomer MainLoadCustomer { get; set; }
        public Division Division { get; set; }
        public string OriginCountryCode { get; set; }
        public string DestinationCountryCode { get; set; }

    }
}
