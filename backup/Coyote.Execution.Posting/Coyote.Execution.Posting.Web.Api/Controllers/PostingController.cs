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
namespace Coyote.Execution.Posting.Web.Api.Controllers
{
    using Coyote.Execution.Posting.Common.Coyote.Types;
    using Coyote.Execution.Posting.Common.Extensions;
    using Coyote.Execution.Posting.Contracts;
    using Coyote.Execution.Posting.Contracts.Commands;
    using Coyote.Execution.Posting.Contracts.Models;
    using Coyote.Execution.Posting.Contracts.Storage;
    using Coyote.Execution.Posting.Domain.Engines;
    using log4net;
    using NServiceBus;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class PostingController : ApiController
    {
        #region " Private fields "
        private readonly ILog _log;
        private readonly IEndpointInstance _endpointInstance;
        private IPostingRepository _postingRepository;
        private IRuntimeSettings _runtimeSettings;
        #endregion

        #region " Constructor "
        public PostingController(ILog log, IEndpointInstance endpointInstance, IPostingRepository postingRepository, IRuntimeSettings runtimeSettings)
        {
            _log = log.ThrowIfArgumentNull(nameof(log));
            _endpointInstance = endpointInstance.ThrowIfArgumentNull(nameof(endpointInstance));
            _postingRepository = postingRepository.ThrowIfArgumentNull(nameof(postingRepository));
            _runtimeSettings = runtimeSettings.ThrowIfArgumentNull(nameof(runtimeSettings));
        }
        #endregion

        #region " Publice methods "

        /// <summary>
        /// This api call is to Repost the Load to PostEverywhere, InternetTruckStop and DialATruck with blank amount using last posted user's credentials.
        /// </summary>
        /// <param name="loadPostInfo"></param>
        [Route("v1/posting/repostloadwithoutcredential")]
        public async Task<HttpResponseMessage> RepostLoadWithoutCredential(LoadPostInfo loadPostInfo)
        {
            try
            {
                loadPostInfo.ThrowIfArgumentNull(nameof(loadPostInfo));

                if (loadPostInfo.LoadId <= 0) throw new ArgumentException($"Invalid LoadId #{loadPostInfo.LoadId}.");

                _log.Info($"Posting Load #{loadPostInfo.LoadId}.");

                LoadPost loadPost = await _postingRepository.ExternalLoadPostRepository.GetActivePostDetailsByLoadId(loadPostInfo.LoadId);

                if (PostUnpostValidationEngine.ShouldRepostOnBounce(loadPost))
                {
                    loadPost.EquipmentType = loadPostInfo.Equipment;
                    loadPost.EquipmentLength = loadPostInfo.EquipmentLength;
                    loadPost.HazMat = loadPostInfo.HazMat;
                    loadPost.Team = loadPostInfo.Team;
                    loadPost.NumberOfStops = loadPostInfo.NumberOfStops;
                    loadPost.ExternalLoadPostActionId = (int)ExternalLoadPostAction.Post;
                    loadPost.UserId = loadPost.CreateByUserId;
                    loadPost.Rate = null;
                    loadPost.IsPostedWhenCovered = false;

                    await _endpointInstance.Send(GetPostLoadCommand(loadPost)).ConfigureAwait(false);

                    return Request.CreateResponse(HttpStatusCode.OK, true);
                }
                else
                {
                    _log.Info($"Cannot repost Load #{loadPostInfo.LoadId} since it is not valid.");
                    return Request.CreateResponse(HttpStatusCode.OK, false);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Could not Repost Load.", ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        /// <summary>
        /// This api call is to Auto-Repost the Load to PostEverywhere, InternetTruckStop and DialATruk.
        /// </summary>
        /// <param name="autoRefreshLoadPostInfo"></param>
        [Route("v1/posting/repostloadonautorefresh")]
        public async Task<HttpResponseMessage> RepostLoadOnAutoRefresh(AutoRefreshLoadPostInfo autoRefreshLoadPostInfo)
        {
            try
            {
                autoRefreshLoadPostInfo.ThrowIfArgumentNull(nameof(autoRefreshLoadPostInfo));
                if (autoRefreshLoadPostInfo.LoadId <= 0) throw new ArgumentException($"Invalid LoadId #{autoRefreshLoadPostInfo.LoadId}");

                var loadPostForAutoRefresh = await _postingRepository.ExternalLoadPostRepository.GetActivePostDetailsByLoaIdForAutoRefresh(autoRefreshLoadPostInfo.LoadId);

                if (PostUnpostValidationEngine.ShouldRepostOnAutoRefresh(loadPostForAutoRefresh))
                {
                    loadPostForAutoRefresh.ExternalLoadPostActionId = (int)ExternalLoadPostAction.AutoRefresh;
                    loadPostForAutoRefresh.UserId = _runtimeSettings.ServiceUserId;
                    loadPostForAutoRefresh.IsPostedWhenCovered = false;

                    PostLoadCommand postLoadCommand = GetPostLoadCommand(loadPostForAutoRefresh);
                    postLoadCommand.PostToITS = autoRefreshLoadPostInfo.PostToITS;
                    postLoadCommand.PostToPostEverywhere = autoRefreshLoadPostInfo.PostToPostEverywhere;
                    postLoadCommand.PostToDAT = autoRefreshLoadPostInfo.PostToDAT;

                    await _endpointInstance.Send(postLoadCommand).ConfigureAwait(false);
                    _log.Info($"Repost on auto-refresh for Load #{loadPostForAutoRefresh.LoadId}.");
                    return Request.CreateResponse(HttpStatusCode.OK, true);
                }
                else
                {
                    _log.Info($"Invalid Load #{autoRefreshLoadPostInfo.LoadId} for repost on auto-refresh.");
                    return Request.CreateResponse(HttpStatusCode.OK, false);
                }
            }
            catch (Exception ex)
            {
                _log.Error($"Could not repost Load on auto-refresh for Load #{autoRefreshLoadPostInfo.LoadId}.", ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        #endregion

        #region " Private methods "
        private PostLoadCommand GetPostLoadCommand(LoadPost loadPost)
        {
            PostLoadCommand postLoadCommand = new PostLoadCommand(loadPost);
            postLoadCommand.Origin = _postingRepository.LocationCountryRepository.GetCityDetailsByCityId(loadPost.OriginCityId);
            postLoadCommand.Destination = _postingRepository.LocationCountryRepository.GetCityDetailsByCityId(loadPost.DestinationCityId);
            return postLoadCommand;
        }
        #endregion
    }
}