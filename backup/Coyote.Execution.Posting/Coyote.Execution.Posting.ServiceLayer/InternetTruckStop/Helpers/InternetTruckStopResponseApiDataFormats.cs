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
namespace Coyote.Execution.Posting.ServiceLayer.InternetTruckStop.Helpers
{
    using Coyote.Execution.Posting.Common.Exceptions;
    using Coyote.Execution.Posting.Common.Extensions;
    using log4net;
    using System.Linq;

    public static class InternetTruckStopResponseApiDataFormats
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "UpDate")]
        public static void HandleExternalServiceResponseAdd(ILog log, WebServices.ReturnBase result, string equipment)
        {
            log.ThrowIfArgumentNull(nameof(log));

            if (result == null) { return; }
            if (result.Errors == null) { return; }
            if (result.Errors.Length == 0) { return; }

            string errors = string.Join(" ", result.Errors.Select(e => e.ErrorMessage));
            log.ErrorFormat($"Internet Truck Stop errors received: {errors}");

            if (result.Errors.Any(x => x.ErrorMessage.Contains("Failed Authentication"))) { throw new ExternalServiceException(ExternalServiceMessages.InvalidUserCredentialsExceptionMessage(), Common.Coyote.Types.ExternalService.InternetTruckStop); }
            if (result.Errors.Any(x => x.ErrorMessage.Contains("Missing UserName"))) { throw new ExternalServiceException(ExternalServiceMessages.InvalidUserCredentialsExceptionMessage(), Common.Coyote.Types.ExternalService.InternetTruckStop); }
            if (result.Errors.Any(x => x.ErrorMessage.Contains("Missing Password"))) { throw new ExternalServiceException(ExternalServiceMessages.InvalidUserCredentialsExceptionMessage(), Common.Coyote.Types.ExternalService.InternetTruckStop); }
            if (result.Errors.Any(x => x.ErrorMessage.Contains("Missing IntegrationID"))) { throw new ExternalServiceException(ExternalServiceMessages.InvalidUserCredentialsExceptionMessage(), Common.Coyote.Types.ExternalService.InternetTruckStop); }

            if (result.Errors.Any(x => x.ErrorMessage.Contains("Unable To find Origin City"))) { throw new ExternalServiceException(ExternalServiceMessages.InvalidOriginExceptionMessage(), Common.Coyote.Types.ExternalService.InternetTruckStop); }
            if (result.Errors.Any(x => x.ErrorMessage.Contains("Unable To find Destination City"))) { throw new ExternalServiceException(ExternalServiceMessages.InvalidDestinationExceptionMessage(), Common.Coyote.Types.ExternalService.InternetTruckStop); }
            if (result.Errors.Any(x => x.ErrorMessage.Contains("Equipment Type is Required"))) { throw new ExternalServiceException(ExternalServiceMessages.UnknownEquipmentExceptionMessage(equipment), Common.Coyote.Types.ExternalService.InternetTruckStop); }
            if (result.Errors.Any(x => x.ErrorMessage.Contains("Invalid Date, dates can not be in the past"))) { throw new ExternalServiceException(ExternalServiceMessages.InvalidPickUpDateExceptionMessage(), Common.Coyote.Types.ExternalService.InternetTruckStop); }
            if (result.Errors.Any(x => x.ErrorMessage.Contains("Invalid Date, dates cannot be more than 30 days in the future"))) { throw new ExternalServiceException(ExternalServiceMessages.InvalidPickUpDateExceptionMessage(), Common.Coyote.Types.ExternalService.InternetTruckStop); }

            throw new ExternalServiceException(ExternalServiceMessages.UnknownErrorExceptionMessage(), Common.Coyote.Types.ExternalService.InternetTruckStop);
        }

        public static void HandleExternalServiceResponseDelete(ILog log, WebServices.ReturnBase result)
        {
            log.ThrowIfArgumentNull(nameof(log));

            if (result == null) { return; }
            if (result.Errors == null) { return; }
            if (result.Errors.Length == 0) { return; }

            if (result.Errors.Any(x => x.ErrorMessage.Contains("Load no longer exists"))) { return; }   // Load does not exist, this is not an error in delete
            if (result.Errors.Any(x => x.ErrorMessage.Contains("Load does not exist"))) { return; }     // Load does not exist, this is not an error in delete

            string errors = string.Join(" ", result.Errors.Select(e => e.ErrorMessage));
            log.ErrorFormat($"Internet Truck Stop errors received: {errors}");

            if (result.Errors.Any(x => x.ErrorMessage.Contains("Failed Authentication"))) { throw new ExternalServiceException(ExternalServiceMessages.InvalidUserCredentialsExceptionMessage(), Common.Coyote.Types.ExternalService.InternetTruckStop); }
            if (result.Errors.Any(x => x.ErrorMessage.Contains("Missing UserName"))) { throw new ExternalServiceException(ExternalServiceMessages.InvalidUserCredentialsExceptionMessage(), Common.Coyote.Types.ExternalService.InternetTruckStop); }
            if (result.Errors.Any(x => x.ErrorMessage.Contains("Missing Password"))) { throw new ExternalServiceException(ExternalServiceMessages.InvalidUserCredentialsExceptionMessage(), Common.Coyote.Types.ExternalService.InternetTruckStop); }
            if (result.Errors.Any(x => x.ErrorMessage.Contains("Missing IntegrationID"))) { throw new ExternalServiceException(ExternalServiceMessages.InvalidUserCredentialsExceptionMessage(), Common.Coyote.Types.ExternalService.InternetTruckStop); }

            throw new ExternalServiceException(ExternalServiceMessages.UnknownErrorExceptionMessage(), Common.Coyote.Types.ExternalService.InternetTruckStop);
        }
    }
}