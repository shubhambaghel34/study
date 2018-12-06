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
namespace Coyote.Execution.Posting.ServiceLayer
{
    using Coyote.Execution.Posting.Common.Extensions;
    using Coyote.Execution.Posting.Contracts.ServiceLayer;

    public class ExternalService : IExternalService
    {
        #region Fields
        public IPostEverywhereExternalService PostEverywhereExternalService { get; set; }

        public IInternetTruckStopExternalService InternetTruckStopExternalService { get; set; }

        public IDATWrapperService DATWrapperService { get; set; }

        public IUpdateMaxPayService UpdateMaxPayService { get; set; }
        #endregion

        #region Constructor
        public ExternalService(IPostEverywhereExternalService postEverywhereExternalService, IInternetTruckStopExternalService internetTruckStopExternalService, IDATWrapperService datWrapperService, IUpdateMaxPayService updateMaxPayService)
        {
            PostEverywhereExternalService = postEverywhereExternalService.ThrowIfArgumentNull(nameof(postEverywhereExternalService));
            InternetTruckStopExternalService = internetTruckStopExternalService.ThrowIfArgumentNull(nameof(internetTruckStopExternalService));
            DATWrapperService = datWrapperService.ThrowIfArgumentNull(nameof(datWrapperService));
            UpdateMaxPayService = updateMaxPayService.ThrowIfArgumentNull(nameof(updateMaxPayService));
        }
        #endregion
    }
}