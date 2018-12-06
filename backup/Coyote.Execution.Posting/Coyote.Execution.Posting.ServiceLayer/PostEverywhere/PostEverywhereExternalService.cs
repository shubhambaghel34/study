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
namespace Coyote.Execution.Posting.ServiceLayer.PostEverywhere
{
    using Coyote.Execution.Posting.Common.Coyote.Types;
    using Coyote.Execution.Posting.Common.Exceptions;
    using Coyote.Execution.Posting.Common.Extensions;
    using Coyote.Execution.Posting.Contracts.Commands;
    using Coyote.Execution.Posting.Contracts.Models;
    using Coyote.Execution.Posting.Contracts.Models.PostEverywhere;
    using Coyote.Execution.Posting.Contracts.ServiceLayer;
    using Coyote.Execution.Posting.Contracts.Storage;
    using Coyote.Execution.Posting.ServiceLayer;
    using Coyote.Execution.Posting.ServiceLayer.PostEverywhere.Helpers;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;

    public class PostEverywhereExternalService: ExternalServiceRestBase, IPostEverywhereExternalService
    {
        #region " Constants "
        private const string ContentType = "application/x-www-form-urlencoded";
        private const string Post = "POST";
        #endregion

        #region " Constructor "
        public PostEverywhereExternalService(IPostingRepository postingRepository, Uri uri, ILog log)
            : base(postingRepository, uri, Common.Coyote.Types.ExternalService.PostEverywhere, log)
        {
        }
        #endregion

        #region " Public methods "
        public bool PostLoad(LoadPostBase loadPost)
        {
            loadPost.ThrowIfArgumentNull(nameof(loadPost));
            loadPost.Origin.ThrowIfArgumentNull(nameof(loadPost.Origin));
            loadPost.Destination.ThrowIfArgumentNull(nameof(loadPost.Destination));

            if (loadPost.DATPostStatus == (int)ExternalLoadPostStatus.Posted)
            {
                DeleteLoadPost(loadPost.UserId, loadPost.Credential, loadPost.LoadId);
            }

            var originCountry = RetrieveCountry(loadPost.Origin);
            var destinationCountry = RetrieveCountry(loadPost.Destination);

            var loadRate = loadPost.Rate.HasValue ? PostEverywhereRequestApiDataFormats.FormatRate(loadPost.Rate.Value) : (decimal?)null;
            var loadWeight = loadPost.Weight.HasValue ? PostEverywhereRequestApiDataFormats.FormatWeight(loadPost.Weight.Value) : (decimal?)null;

            var postLoad = new PostLoadModel
            {
                Phone = loadPost.Credential.PostEverywherePhonenumber,
                PostAction = PostAction.Add.GetEnumDescription(),
                ImportRef = loadPost.LoadId,
                OriginCity = loadPost.Origin.Name,
                OriginState = PostEverywhereRequestApiDataFormats.FormatState(loadPost.Origin.StateCode, originCountry),
                DestCity = loadPost.Destination.Name,
                DestState = PostEverywhereRequestApiDataFormats.FormatState(loadPost.Destination.StateCode, destinationCountry),
                PickUpDate = loadPost.PickUpDate.Date,
                PickUpTime = loadPost.PickUpDate.TimeOfDay,
                TruckType = PostEverywhereRequestApiDataFormats.FormatEquipment(loadPost.EquipmentType, loadPost.HazMat),
                FullOrPartial = loadPost.IsLoadPartial ? FullOrPartial.Partial : FullOrPartial.Full,
                Stops = loadPost.NumberOfStops,
                Rate = loadRate,
                Weight = loadWeight,
                Length = (int)Math.Round(loadPost.EquipmentLength, MidpointRounding.ToEven),
                Comment = Regex.Replace(PostEverywhereRequestApiDataFormats.FormatNotes(loadPost.Notes, loadPost.EquipmentType, loadPost.Team, loadPost.HazMat), @"\t|\n|\r", " ")
            };

            var postEverywhereModel = new PostEverywhereModel
            {
                LoadModels = new List<PostLoadModel>()
            };
            postEverywhereModel.LoadModels.Add(postLoad);

            var result = WebRequestPost(SerializeObjectToXml(postEverywhereModel), loadPost.Credential);
            PostEverywhereResponseApiDataFormats.HandleExternalServiceResponseAdd(result, loadRate, loadPost.EquipmentLength, loadPost.EquipmentType);
            if (result.Item1)
            {
                Log.Info($"Load #{loadPost.LoadId} is Posted to Post Everywhere.");
                return true;
            }
            return false;
        }

        public void DeleteLoadPost(int userId, ExternalLoadPostCredential credential, int loadId)
        {
            if (credential == null) throw new ArgumentNullException(nameof(credential));

            var postLoad = new PostLoadModel
            {
                PostAction = PostAction.Delete.GetEnumDescription(),
                ImportRef = loadId
            };
            var postEverywhereModel = new PostEverywhereModel
            {
                LoadModels = new List<PostLoadModel>()
            };
            postEverywhereModel.LoadModels.Add(postLoad);

            var result = WebRequestPost(SerializeObjectToXml(postEverywhereModel), credential);
            PostEverywhereResponseApiDataFormats.HandleExternalServiceResponseDelete(result);
        }
        #endregion

        #region " Private methods "
        private Tuple<bool /*success*/, string /*errorMessage*/> WebRequestPost(string xmlMessage, ExternalLoadPostCredential credential)
        {
            // Update Url with serviceKey and customer key credentials
            var uriBuilder = new UriBuilder(Uri)
            {
                Query = string.Format("ServiceKey={0}&CustomerKey={1}&ServiceAction=Many", credential.PostEverywhereServiceKey, credential.PostEverywhereCustomerKey)
            };

            var request = (HttpWebRequest)WebRequest.Create(uriBuilder.Uri);
            request.Method = Post;

            var byteArray = Encoding.UTF8.GetBytes(xmlMessage);
            request.ContentType = ContentType;
            request.ContentLength = byteArray.Length;

            using (var putStream = request.GetRequestStream())
            {
                putStream.Write(byteArray, 0, byteArray.Length);
            }

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var responseStream = response.GetResponseStream();
                if (responseStream == null) { return new Tuple<bool /*success*/, string /*errorMessage*/>(false, null); }

                using (var reader = new StreamReader(responseStream))
                {
                    var responseXml = reader.ReadToEnd();

                    var postEverywhereResponseModel = DeserializeFromXml<PostEverywhereResponseModel>(responseXml);
                    if (postEverywhereResponseModel != null)
                    {
                        if (postEverywhereResponseModel.PostingErrors != null)
                        {
                            var error = postEverywhereResponseModel.PostingErrors.ErrorDos.FirstOrDefault();
                            if (error == null)
                            {
                                return new Tuple<bool /*success*/, string /*errorMessage*/>(false, null);
                            }

                            Log.Error($"Post Everywhere Posting Error: {error.RespMsg}");
                            return new Tuple<bool /*success*/, string /*errorMessage*/>(false, error.RespMsg);
                        }

                        if (postEverywhereResponseModel.ResponseDo.Status.ToUpper() == "DENIED")
                        {
                            Log.Error($"Post Everywhere Posting Error: {postEverywhereResponseModel.ResponseDo.DisplayMsg}");
                            return new Tuple<bool /*success*/, string /*errorMessage*/>(false, postEverywhereResponseModel.ResponseDo.DisplayMsg);
                        }
                        return new Tuple<bool /*success*/, string /*errorMessage*/>(true, null);
                    }
                    return new Tuple<bool /*success*/, string /*errorMessage*/>(false, "Post Everywhere Posting Error: Failed to read response");
                }
            }
        }

        private LocationCountry RetrieveCountry(City city)
        {
            var country = PostingRepository.LocationCountryRepository.GetLocationCountryByCityId(city.Id);
            if (country == null)
            {
                Log.Error( $"Failed to retrieve the country for city {city.CityState}");
                throw new ExternalServiceException(ExternalServiceMessages.UnknownCountryForCityExceptionMessage(city), Common.Coyote.Types.ExternalService.PostEverywhere);
            }

            return country;
        }
        #endregion
    }
}