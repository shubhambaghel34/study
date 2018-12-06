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
namespace Coyote.Execution.Posting.Contracts.ServiceLayer
{
    public interface IExternalService
    {
        IPostEverywhereExternalService PostEverywhereExternalService { get; set; }

        IInternetTruckStopExternalService InternetTruckStopExternalService { get; set; }

        IDATWrapperService DATWrapperService { get; set; }

        IUpdateMaxPayService UpdateMaxPayService { get; set; }
    }
}