﻿// /////////////////////////////////////////////////////////////////////////////////////
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
    using System.Collections.Generic;

    public class Carrier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContractVersion { get; set; }

        public CarrierTrackingPreference CarrierTrackingPreference { get; set; }
        public ICollection<Rep> Reps { get; set; }
    }
}
