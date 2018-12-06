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
namespace Coyote.Execution.CheckCall.Contracts.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Coyote.Execution.CheckCall.Domain.Models;

    public class LoadActivationCancellationEmailCommand
    {
        public int LoadId { get; set; }
        public string CustomerName { get; set; }
        public string OriginCity { get; set; }
        public string DestinationCity { get; set; }
        public string OriginStateCode { get; set; }
        public string DestinationStateCode { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<String> CarrierRepEmailAddresses { get; set; }
        public LoadStateType LoadState { get; set; }
        public DateTime LoadDate { get; set; }
        public string DoNotReplyFromAddress { get; set; }
        public Division Division { get; set; }
        public string OriginCountryCode { get; set; }
        public string DestinationCountryCode { get; set; }
        public LoadModeType Mode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public LoadActivationCancellationEmailCommand(Load load, string doNotReplyFromAddress)
        {
            LoadId = load.Id;
            CustomerName = load.MainLoadCustomer.Name;
            OriginCity = load.OriginCityName;
            DestinationCity = load.DestinationCityName;
            OriginStateCode = load.OriginStateCode;
            DestinationStateCode = load.DestinationStateCode;
            OriginCountryCode = load.OriginCountryCode;
            DestinationCountryCode = load.DestinationCountryCode;
            Division = load.Division;
            Mode = load.Mode;
            LoadState = load.StateType;
            LoadDate = load.LoadDate;
            CarrierRepEmailAddresses = load.MainLoadCarrier.LoadReps.Select(s => s.EmailWork).ToList();
            DoNotReplyFromAddress = doNotReplyFromAddress;
        }
    }
}
