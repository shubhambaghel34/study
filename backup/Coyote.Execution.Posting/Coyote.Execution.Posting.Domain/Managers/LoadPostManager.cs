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
namespace Coyote.Execution.Posting.Domain.Managers
{
    using Coyote.Execution.Posting.Common.Coyote.Types;
    using Coyote.Execution.Posting.Common.Extensions;
    using Coyote.Execution.Posting.Contracts.Commands;
    using Coyote.Execution.Posting.Contracts.Managers;
    using Coyote.Execution.Posting.Contracts.Models;
    using Coyote.Execution.Posting.Contracts.ServiceLayer;
    using Coyote.Execution.Posting.Contracts.Storage;
    using log4net;
    using System;
    using System.Threading.Tasks;

    public class LoadPostManager : ILoadPostManager
    {
        #region " Public properties "
        private readonly IPostingRepository _postingRepository;
        private ILog _log;
        private readonly IExternalService _externalService;
        private readonly IRealtimeService _realtimeService;
        #endregion

        #region " Constructor "
        public LoadPostManager(IPostingRepository postingRepository, IExternalService externalService, ILog log, IRealtimeService realtimeService)
        {
            _postingRepository = postingRepository.ThrowIfArgumentNull(nameof(postingRepository));
            _externalService = externalService.ThrowIfArgumentNull(nameof(externalService));
            _log = log.ThrowIfArgumentNull(nameof(log));
            _realtimeService = realtimeService.ThrowIfArgumentNull(nameof(realtimeService));
        }
        #endregion

        #region " Public methods "
        public async Task<bool> RepostLoadWithoutCredential(PostLoadCommand postLoadCommand)
        {
            postLoadCommand.ThrowIfArgumentNull(nameof(postLoadCommand));

            ExternalLoadPostCredential externalLoadPostCredential = await _postingRepository.ExternalLoadPostRepository.GetExternalLoadPostCredentialByInternalEmployeeId(postLoadCommand.PostedAsUserId);
            postLoadCommand.Credential = externalLoadPostCredential ?? throw new NullReferenceException($"Unable to get LoadPost credentials for UserId#{postLoadCommand.PostedAsUserId}.");

            if (PostLoad(postLoadCommand))
            {
                var result = await _postingRepository.ExternalLoadPostRepository.InsertAndUpdateExternalLoadPost(postLoadCommand);
                await _realtimeService.SendRealtimeUpdatesAsync(postLoadCommand);

                if (postLoadCommand.Rate != null)
                    await _externalService.UpdateMaxPayService.UpdateMaxPayAsync(postLoadCommand.LoadId, (decimal)postLoadCommand.Rate);

                return true;
            }
            else
                return false;
        }
        #endregion

        #region " Private Methods "
        private bool PostLoad(PostLoadCommand postLoadCommand)
        {
            int successCount = 0;

            if (postLoadCommand.PostToPostEverywhere && postLoadCommand.Credential.CanPostToPostEverywhere() && Execute(postLoadCommand, _externalService.PostEverywhereExternalService.PostLoad))
            {
                successCount++;
                postLoadCommand.PostEverywherePostStatus = (int)ExternalLoadPostStatus.Posted;
            }

            if (postLoadCommand.PostToITS && postLoadCommand.Credential.CanPostToITS() && Execute(postLoadCommand, _externalService.InternetTruckStopExternalService.PostLoad))
            {
                successCount++;
                postLoadCommand.ITSPostStatus = (int)ExternalLoadPostStatus.Posted;
            }

            if (postLoadCommand.PostToDAT && postLoadCommand.Credential.CanPostToDAT() && Execute(postLoadCommand, _externalService.DATWrapperService.PostLoad))
            {
                successCount++;
                postLoadCommand.DATPostStatus = (int)ExternalLoadPostStatus.Posted;
            }

            if (successCount > 0)
                return true;
            else
                return false;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private bool Execute(PostLoadCommand postLoadCommand, Func<PostLoadCommand, bool> function)
        {
            try
            {
                return function.Invoke(postLoadCommand);
            }
            catch (Exception exception)
            {
                _log.Error(exception.Message, exception);
                return false;
            }
        }
        #endregion
    }
}