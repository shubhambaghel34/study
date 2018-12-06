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
    public class CheckCallNotificationRecord
    {
        public int CarrierId { get; set; }
        public string Email { get; set; }
        public string EmailName { get; set; }
        public int LoadId { get; set; }
        public DateTime LoadDate { get; set; }
        public string OriginCityName { get; set; }
        public string OriginState { get; set; }
        public string DestCityName { get; set; }
        public string DestState { get; set; }
        public DateTime? ScheduleOpenTime { get; set; }
        public DateTime? ScheduleCloseTime { get; set; }
    }
}
