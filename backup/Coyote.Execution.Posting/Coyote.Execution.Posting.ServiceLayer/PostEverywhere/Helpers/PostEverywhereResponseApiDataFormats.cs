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
namespace Coyote.Execution.Posting.ServiceLayer.PostEverywhere.Helpers
{
    using Coyote.Execution.Posting.Common.Coyote.Types;
    using Coyote.Execution.Posting.Common.Exceptions;
    using System;

    public static class PostEverywhereResponseApiDataFormats
    {
        #region " Public methods "
        public static void HandleExternalServiceResponseAdd(Tuple<bool /*success*/, string /*errorMessage*/> result, decimal? rate, decimal length, string equipment)
        {
            if (result == null) { throw new ExternalServiceException(ExternalServiceMessages.UnknownErrorExceptionMessage(), ExternalService.PostEverywhere); }
            if (result.Item1) { return; }

            string error = result.Item2;
            HandleGeneralExternalServiceResponse(error);

            if (error.Contains("originState is not valid")) { throw new ExternalServiceException(ExternalServiceMessages.InvalidOriginExceptionMessage(), ExternalService.PostEverywhere); }
            if (error.Contains("destState is not valid")) { throw new ExternalServiceException(ExternalServiceMessages.InvalidDestinationExceptionMessage(), ExternalService.PostEverywhere); }
            if (error.Contains("Please enter a valid length between 1 and 57")) { throw new ExternalServiceException(ExternalServiceMessages.InvalidLengthExceptionMessage(length), ExternalService.PostEverywhere); }
            if (error.Contains("Please enter a valid number of stops between 0 and 999")) { throw new ExternalServiceException(ExternalServiceMessages.InvalidNumberOfStopsExceptionMessage(999), ExternalService.PostEverywhere); }
            if (error.Contains("truckType is not valid")) { throw new ExternalServiceException(ExternalServiceMessages.UnknownEquipmentExceptionMessage(equipment), ExternalService.PostEverywhere); }
            if (error.Contains("Please enter a valid rate"))
            {
                if (rate.HasValue)
                {
                    throw new ExternalServiceException(ExternalServiceMessages.InvalidRateExceptionMessage(), ExternalService.PostEverywhere);
                }

                throw new ExternalServiceException("Please enter a valid rate", ExternalService.PostEverywhere);
            }

            throw new ExternalServiceException(ExternalServiceMessages.UnknownErrorExceptionMessage(), ExternalService.PostEverywhere);
        }

        public static void HandleExternalServiceResponseDelete(Tuple<bool /*success*/, string /*errorMessage*/> result)
        {
            if (result == null) { throw new ExternalServiceException(ExternalServiceMessages.UnknownErrorExceptionMessage(), ExternalService.PostEverywhere); }
            if (result.Item1) { return; }

            string error = result.Item2;
            HandleGeneralExternalServiceResponse(error);

            throw new ExternalServiceException(ExternalServiceMessages.UnknownErrorExceptionMessage(), ExternalService.PostEverywhere);
        }
        #endregion

        #region " Private methods "
        private static void HandleGeneralExternalServiceResponse(string error)
        {
            if (string.IsNullOrEmpty(error)) { throw new ExternalServiceException(ExternalServiceMessages.UnknownErrorExceptionMessage(), ExternalService.PostEverywhere); }
            if (error.Contains("Invalid login for PostEverywhere")) { throw new ExternalServiceException(ExternalServiceMessages.InvalidUserCredentialsExceptionMessage(), ExternalService.PostEverywhere); }
        }
        #endregion
    }
}