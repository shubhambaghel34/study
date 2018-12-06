// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2016 - 2017
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

    public class CarrierTrackingPreference
    {
        public int CarrierTrackingPreferenceId { get; set; }
        public int CarrierId { get; set; }
        public int OutboundDefaultCommunicationTypeId { get; set; }
        public string Email { get; set; }
        public string EmailName { get; set; }
        public bool? DailyCheckCallEmail { get; set; }
        public bool? TrackingCallEmail { get; set; }

        public virtual Carrier Carrier { get; set; }
    }
}
