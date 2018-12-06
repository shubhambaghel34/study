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
namespace Coyote.Execution.Posting.ServiceLayer.InternetTruckStop
{
    using Coyote.Execution.Posting.Common.Coyote.Types;
    using Coyote.Execution.Posting.Common.Exceptions;
    using Coyote.Execution.Posting.Common.Extensions;
    using Coyote.Execution.Posting.Contracts.Commands;
    using Coyote.Execution.Posting.Contracts.Models;
    using Coyote.Execution.Posting.Contracts.ServiceLayer;
    using Coyote.Execution.Posting.Contracts.Storage;
    using Coyote.Execution.Posting.ServiceLayer.InternetTruckStop.Helpers;
    using log4net;
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    public class InternetTruckStopExternalService : ExternalServicesSoapBase<ILoadPosting>, IInternetTruckStopExternalService
    {
        #region " Private properties "
        private readonly Binding _binding;
        private readonly EndpointAddress _serviceAddress;
        #endregion

        #region " Constructor "
        public InternetTruckStopExternalService(IPostingRepository postingRepository,Uri uri, ILog log)
            : base(postingRepository, uri, Common.Coyote.Types.ExternalService.InternetTruckStop, log)
        {
            _serviceAddress = new EndpointAddress(uri);
            _binding = new BasicHttpBinding();
        }
        #endregion

        #region " IInternetTruckStopExternalService operations "

        public bool PostLoad(LoadPostBase loadPost)
        {
            loadPost.ThrowIfArgumentNull(nameof(loadPost));

            loadPost.Credential.ThrowIfArgumentNull(nameof(loadPost.Credential));
            loadPost.Origin.ThrowIfArgumentNull(nameof(loadPost.Origin));
            loadPost.Destination.ThrowIfArgumentNull(nameof(loadPost.Destination));

            if (loadPost.ITSPostStatus == (int)ExternalLoadPostStatus.Posted)
            {
                DeleteLoadPost(loadPost.UserId, loadPost.Credential, loadPost.LoadId);
            }

            var originCountry = RetrieveCountry(loadPost.Origin);
            var destinationCountry = RetrieveCountry(loadPost.Destination);

            loadPost.PickUpDate = DateTime.SpecifyKind(loadPost.PickUpDate, DateTimeKind.Unspecified);

            using (var client = new LoadPostingClient(_binding, _serviceAddress))
            {
                var loadWeight = loadPost.Weight.HasValue ? InternetTruckStopRequestApiDataFormats.FormatWeight(loadPost.Weight.Value) : null;

                var equipmentOptionsField = new System.Collections.Generic.List<Truckstop2.Objects.TrailerOptionType>();
                if (loadPost.Team) equipmentOptionsField.Add(Truckstop2.Objects.TrailerOptionType.Team);
                if (loadPost.HazMat) equipmentOptionsField.Add(Truckstop2.Objects.TrailerOptionType.Hazardous);

                var load = new WebServices.Objects.Load
                {
                    //Affiliate
                    //Credit
                    //DeliveryDate
                    //DeliveryTime
                    DestinationCity = loadPost.Destination.Name,
                    DestinationState = InternetTruckStopRequestApiDataFormats.FormatState(loadPost.Destination.StateCode, destinationCountry),
                    DestinationCountry = InternetTruckStopRequestApiDataFormats.FormatCountry(destinationCountry),
                    //Distance
                    //EquipmentID
                    //Truckstop2.Objects.TrailerOptionType[] EquipmentOptions
                    //FuelPrice
                    //IsDaily
                    //IsFavorite
                    IsLoadFull = !loadPost.IsLoadPartial,
                    Length = loadPost.EquipmentLength.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    //LoadId = loadId,
                    LoadNumber = loadPost.LoadId.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    //Mileage
                    OriginCity = loadPost.Origin.Name,
                    OriginState = InternetTruckStopRequestApiDataFormats.FormatState(loadPost.Origin.StateCode, originCountry),
                    OriginCountry = InternetTruckStopRequestApiDataFormats.FormatCountry(originCountry),
                    PaymentAmount = InternetTruckStopRequestApiDataFormats.FormatPaymentAmount(loadPost.Rate),
                    PickUpDate = loadPost.PickUpDate,
                    //PickUpTime
                    //Quantity
                    SpecInfo = loadPost.Notes,
                    Stops = InternetTruckStopRequestApiDataFormats.FormatNumberOfStops(loadPost.NumberOfStops),
                    TypeOfEquipment = InternetTruckStopRequestApiDataFormats.FormatEquipment(loadPost.EquipmentType),
                    Weight = loadWeight
                    //Width
                };
                if (equipmentOptionsField.Count > 0)
                    load.EquipmentOptions = equipmentOptionsField.ToArray();

                var request = new WebServices.Posting.LoadPostingRequest
                {
                    UserName = loadPost.Credential.ITSLogin,
                    Password = loadPost.Credential.ITSPassword,
                    FullImport = false, // We are posting JUST for this load, this is not an all-encompassing view of what we expect out there
                    Loads = new[] { load }
                };

                if (loadPost.Credential.ITSIntegrationId.HasValue)
                {
                    request.IntegrationId = loadPost.Credential.ITSIntegrationId.Value;
                }

                var result = client.PostLoads(request);
                InternetTruckStopResponseApiDataFormats.HandleExternalServiceResponseAdd(Log, result, loadPost.EquipmentType);
                if (result.Errors.Length == 0)
                {
                    Log.Info($"Load #{loadPost.LoadId} is Posted to Internet Truck Stop.");
                    return true;
                }
                return false;
            }
        }

        public void DeleteLoadPost(int userId, ExternalLoadPostCredential credential, int loadId)
        {
            credential.ThrowIfArgumentNull("credential");

            using (var client = new LoadPostingClient(_binding, _serviceAddress))
            {
                var request = new WebServices.Posting.LoadDeleteByLoadNumberRequest
                {
                    UserName = credential.ITSLogin,
                    Password = credential.ITSPassword,
                    LoadNumbers = new[] { loadId.ToString(System.Globalization.CultureInfo.InvariantCulture) }
                };

                if (credential.ITSIntegrationId.HasValue)
                {
                    request.IntegrationId = credential.ITSIntegrationId.Value;
                }

                WebServices.Objects.LoadPostingReturn result = client.DeleteLoadsByLoadNumber(request);
                InternetTruckStopResponseApiDataFormats.HandleExternalServiceResponseDelete(Log, result);
            }
        }
        #endregion

        #region Private operations
        private LocationCountry RetrieveCountry(City city)
        {
            var country = PostingRepository.LocationCountryRepository.GetLocationCountryByCityId(city.Id);
            if (country == null)
            {
                Log.Error($"Failed to retrieve the country for city {city.CityState}");
                throw new ExternalServiceException(ExternalServiceMessages.UnknownCountryForCityExceptionMessage(city), Common.Coyote.Types.ExternalService.InternetTruckStop);
            }

            return country;
        }
        #endregion
    }
}