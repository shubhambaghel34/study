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
    using Coyote.Execution.Posting.Contracts.Commands;
    using Coyote.Execution.Posting.Contracts.Models;
    using Coyote.Execution.Posting.Contracts.Storage;
    using log4net;
    using NServiceBus;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Unposting")]
    public class UnpostingController : ApiController
    {
        #region " Private fields "
        private readonly ILog _log;
        private readonly IEndpointInstance _endpointInstance;
        private readonly IPostingRepository _postingRepository;
        #endregion

        #region " Constructor "
        public UnpostingController(ILog log, IEndpointInstance endpointInstance, IPostingRepository postingRepository)
        {
            _log = log.ThrowIfArgumentNull(nameof(log));
            _endpointInstance = endpointInstance.ThrowIfArgumentNull(nameof(endpointInstance));
            _postingRepository = postingRepository.ThrowIfArgumentNull(nameof(postingRepository));
        }
        #endregion

        #region " Public methods "
        /// <summary>
        /// This api call is to unpost the Load from PostEverywhere, InternetTruckStop and DialATruck when Load progress is changed.
        /// </summary>
        /// <param name="loadPostInfo"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [Route("v1/unposting/unpostload")]
        public async Task<HttpResponseMessage> UnpostLoad(LoadPostInfo loadPostInfo)
        {
            try
            {
                ValidateLoadPostInfo(loadPostInfo);

                _log.Info($"Unposting Load #{loadPostInfo.LoadId} on progress change.");

                LoadPost loadPost = await GetLoadPost(loadPostInfo.LoadId);
                if (loadPost == null)
                {
                    _log.Info($"Load #{loadPostInfo.LoadId} is never posted. Cannot unpost on Load progress change.");
                    return Request.CreateResponse(HttpStatusCode.OK, false);
                }

                loadPost.IsPostedWhenCovered = Domain.Engines.PostUnpostValidationEngine.IsLoadPosted(loadPost);
                loadPost.UserId  = loadPostInfo.UserId;

                await BuildAndSendUnpostLoadCommand(loadPost);
                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch (Exception ex)
            {
                _log.Error($"Unable to unpost Load on Load progress change.", ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        /// <summary>
        /// This api call is to unpost the Load from PostEverywhere, InternetTruckStop and DialATruck when Load state is changed.
        /// </summary>
        /// <param name="loadPostInfo"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [Route("v1/unposting/unpostloadonstatechange")]
        public async Task<HttpResponseMessage> UnpostLoadOnStateChange(LoadPostInfo loadPostInfo)
        {
            try
            {
                ValidateLoadPostInfo(loadPostInfo);

                _log.Info($"Unposting Load #{loadPostInfo?.LoadId} on state change.");

                LoadPost loadPost = await GetLoadPost(loadPostInfo.LoadId);
                if (loadPost == null)
                {

                    _log.Info($"Load #{loadPostInfo.LoadId} is never posted. Cannot unpost on Load state change.");
                    return Request.CreateResponse(HttpStatusCode.OK, false);
                }

                loadPost.UserId = loadPostInfo.UserId;
                await BuildAndSendUnpostLoadCommand(loadPost);
                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch (Exception ex)
            {
                _log.Error($"Unable to unpost Load.", ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        /// <summary>
        /// This api call is to unpost the Load from PostEverywhere, InternetTruckStop and DialATruck when Load date is changed.
        /// </summary>
        /// <param name="loadPostInfo"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [Route("v1/unposting/unpostloadonrolled")]
        public async Task<HttpResponseMessage> UnpostLoadOnRolled(LoadPostInfo loadPostInfo)
        {
            try
            {
                ValidateLoadPostInfo(loadPostInfo);

                _log.Info($"Unposting Load #{loadPostInfo?.LoadId} on rolled.");

                LoadPost loadPost = await GetLoadPost(loadPostInfo.LoadId);
                if (loadPost == null)
                {
                    _log.Info($"Load #{loadPostInfo.LoadId} is never posted. Cannot unpost on rolled.");
                    return Request.CreateResponse(HttpStatusCode.OK, false);
                }

                loadPost.UserId = loadPostInfo.UserId;
                loadPost.Rate = null;
                await BuildAndSendUnpostLoadCommand(loadPost);
                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch (Exception ex)
            {
                _log.Error($"Unable to unpost Load.", ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        #endregion

        #region " Private methods "
        private async Task BuildAndSendUnpostLoadCommand(LoadPost loadPost)
        {
            UnpostLoadCommand unpostLoadCommand = new UnpostLoadCommand(loadPost);
            unpostLoadCommand.Origin = new City() { Id = loadPost.OriginCityId };
            unpostLoadCommand.Destination = new City() { Id = loadPost.DestinationCityId };

            await _endpointInstance.Send(unpostLoadCommand).ConfigureAwait(false);
        }

        private async Task<LoadPost> GetLoadPost(int loadId)
        {
            LoadPost loadPost = await _postingRepository.ExternalLoadPostRepository.GetActivePostDetailsByLoadId(loadId);

            if (loadPost != null)
            loadPost.ExternalLoadPostActionId = (int)ExternalLoadPostAction.Unpost;

            return loadPost;
        }

        private static void ValidateLoadPostInfo(LoadPostInfo loadPostInfo)
        {
            loadPostInfo.ThrowIfArgumentNull(nameof(loadPostInfo));
            if (loadPostInfo.LoadId <= 0) throw new ArgumentException($"Invalid LoadId #{loadPostInfo.LoadId}.");
        }
        #endregion
    }
}