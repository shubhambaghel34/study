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

namespace Coyote.Execution.CheckCall.Contracts.Commands
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Coyote.Execution.CheckCall.Domain.Models;

    public class SendDailyCheckCallEmail
    {
        public int CarrierId { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public Collection<LoadInfo> Loads { get; set; }

        public SendDailyCheckCallEmail(CheckCallNotificationRecord record, Collection<LoadInfo> loads)
        {
            CarrierId = record.CarrierId;
            Email = record.Email;
            Date = record.LoadDate.Date;
            Loads = loads;

            if (Loads == null)
            {
                Loads = new Collection<LoadInfo>();
            }

            if (Loads.Select(s => s.LoadId == record.LoadId).Count() < 1)
            {
                Loads.Add(new LoadInfo
                {
                    LoadId = record.LoadId,
                    OriginCityName = record.OriginCityName,
                    OriginState = record.OriginState,
                    DestinationCityName = record.DestCityName,
                    DestinationState = record.DestState
                });
            }
        }

    }
}
