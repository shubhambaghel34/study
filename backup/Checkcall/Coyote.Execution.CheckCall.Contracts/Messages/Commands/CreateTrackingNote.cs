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
    using System.Collections.ObjectModel;

    public class CreateTrackingNote
    {
        public string To { get; set; }
        public LoadInfo LoadInfo { get; set; }
        public int CarrierId { get; set; }

        public CreateTrackingNote(string to, LoadInfo loadInfo, int carrierId)
        {
            To = to;
            LoadInfo = loadInfo;
            CarrierId = carrierId;
        }
    }
}
