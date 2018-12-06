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
namespace Coyote.Execution.Posting.ServiceLayer
{
    using Coyote.Execution.Posting.Contracts.Models;

    public static class ExternalServiceMessages
    {
        #region " Static Methods "
        public static string UnknownErrorExceptionMessage()
        {
            return "General server error occurred.";
        }

        public static string InvalidUserCredentialsExceptionMessage()
        {
            return "User credentials are invalid.";
        }

        public static string InvalidStateCodeExceptionMessage()
        {
            return "State code is invalid.";
        }

        public static string UnsupportedStateCodeExceptionMessage()
        {
            return "State code is not supported by the external load board.";
        }

        public static string UnknownStateCodeExceptionMessage()
        {
            return "State code is not supported in Bazooka.";
        }

        public static string UnknownCountryForCityExceptionMessage(City city)
        {
            return string.Format("Failed to find a country for city {0}.", city != null ? city.CityState : "unknown");
        }

        public static string UnknownCountryExceptionMessage(string country)
        {
            return string.Format("Country {0} is not supported.", country);
        }

        public static string InvalidEquipmentExceptionMessage()
        {
            return "Equipment is invalid.";
        }

        public static string UnknownEquipmentExceptionMessage(string equipment)
        {
            return string.Format("Equipment {0} is not supported.", equipment);
        }

        public static string InvalidRateExceptionMessage()
        {
            return "Rate is invalid.";
        }

        public static string InvalidWeightExceptionMessage()
        {
            return "Weight is invalid.";
        }

        public static string InvalidLengthExceptionMessage(decimal length)
        {
            return string.Format("Length {0} is invalid; Should be between 1 and 57.", length);
        }

        public static string InvalidOriginExceptionMessage()
        {
            return "Origin is invalid.";
        }

        public static string InvalidDestinationExceptionMessage()
        {
            return "Destination is invalid.";
        }

        public static string InvalidNumberOfStopsExceptionMessage(int maximumNumberOfStops)
        {
            return string.Format("Number of stops is invalid; Should be between 0 and {0}.", maximumNumberOfStops);
        }

        public static string InvalidCountryExceptionMessage()
        {
            return "Country is invalid.";
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "UpDate")]
        public static string InvalidPickUpDateExceptionMessage()
        {
            return "Pickup date is invalid.";
        }

        public static string InvalidLocationExceptionMessage()
        {
            return "Either origin or destination is not valid.";
        }
        #endregion
    }
}