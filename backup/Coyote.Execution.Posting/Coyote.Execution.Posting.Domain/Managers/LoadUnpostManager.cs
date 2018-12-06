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

    public class LoadUnpostManager : ILoadUnpostManager
    {
        #region " Public properties "
        private ILog _log;
        private readonly IPostingRepository _postingRepository;
        private readonly IExternalService _externalService;
        private readonly IRealtimeService _realtimeService;
        #endregion

        #region " Constructor "
        public LoadUnpostManager(IPostingRepository postingRepository, IExternalService externalService, ILog log, IRealtimeService realtimeService)
        {
            _postingRepository = postingRepository.ThrowIfArgumentNull(nameof(postingRepository));
            _externalService = externalService.ThrowIfArgumentNull(nameof(externalService));
            _log = log.ThrowIfArgumentNull(nameof(log));
            _realtimeService = realtimeService.ThrowIfArgumentNull(nameof(realtimeService));
        }
        #endregion

        #region " Public methods "
        public async Task<bool> UnpostLoad(UnpostLoadCommand unpostLoadCommand)
        {
            unpostLoadCommand.ThrowIfArgumentNull(nameof(unpostLoadCommand));

            unpostLoadCommand.Credential = await _postingRepository.ExternalLoadPostRepository.GetExternalLoadPostCredentialByInternalEmployeeId(unpostLoadCommand.UserId);
            if (unpostLoadCommand.Credential == null)
            {
                unpostLoadCommand.Credential = await _postingRepository.ExternalLoadPostRepository.GetExternalLoadPostCredentialByInternalEmployeeId(unpostLoadCommand.PostedAsUserId);

                if (unpostLoadCommand.Credential == null)
                {
                    _log.Info($"Cannot fetch credentials for currentuser/postedasuser. Cannot unpost.");
                    return false;
                }
            }
            else
                unpostLoadCommand.PostedAsUserId = unpostLoadCommand.UserId;

            int successCount = 0;
            if (unpostLoadCommand.Credential.CanPostToPostEverywhere() && Execute(unpostLoadCommand, _externalService.PostEverywhereExternalService.DeleteLoadPost))
            {
                successCount++;
                unpostLoadCommand.PostEverywherePostStatus = (int)ExternalLoadPostStatus.NotPosted;
            }

            if (unpostLoadCommand.Credential.CanPostToITS() && Execute(unpostLoadCommand, _externalService.InternetTruckStopExternalService.DeleteLoadPost))
            {
                successCount++;
                unpostLoadCommand.ITSPostStatus = (int)ExternalLoadPostStatus.NotPosted;
            }

            if (unpostLoadCommand.Credential.CanPostToDAT() && Execute(unpostLoadCommand, _externalService.DATWrapperService.DeleteLoadPost))
            {
                successCount++;
                unpostLoadCommand.DATPostStatus = (int)ExternalLoadPostStatus.NotPosted;
            }

            if (successCount > 0)
            {
                var result = await _postingRepository.ExternalLoadPostRepository.InsertAndUpdateExternalLoadPost(unpostLoadCommand);
                await _realtimeService.SendRealtimeUpdatesAsync(unpostLoadCommand);
                return true;
            }
            else
                return false;
        }
        #endregion

        #region " Private methods "
         [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private bool Execute(UnpostLoadCommand unpostLoadCommand, Action<int, ExternalLoadPostCredential, int> function)
        {
            try
            {
                function.Invoke(unpostLoadCommand.UserId, unpostLoadCommand.Credential, unpostLoadCommand.LoadId);
                return true;
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