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
namespace Coyote.Execution.Posting.ServiceLayer.DAT
{
    using Coyote.Execution.Posting.Common.Coyote.Types;
    using Coyote.Execution.Posting.Common.Exceptions;
    using Coyote.Execution.Posting.Common.Extensions;
    using Coyote.Execution.Posting.Contracts;
    using Coyote.Execution.Posting.Contracts.Commands;
    using Coyote.Execution.Posting.Contracts.Models;
    using Coyote.Execution.Posting.Contracts.ServiceLayer;
    using Coyote.Execution.Posting.Contracts.Storage;
    using log4net;
    using Newtonsoft.Json;
    using System;

    public class DATWrapperService : ExternalServiceRestBase, IDATWrapperService
    {
        #region " Constructor "
        public DATWrapperService(ILog log, IPostingRepository postingRepository, IRuntimeSettings runtimeSettings)
            : base(postingRepository, new Uri(runtimeSettings?.DATLoadPostingWebUrl), Common.Coyote.Types.ExternalService.DAT, log)
        {
            runtimeSettings.ThrowIfArgumentNull(nameof(runtimeSettings));
        }
        #endregion

        #region IDATWrapperService operations

        public bool PostLoad(LoadPostBase loadPost)
        {
            loadPost.ThrowIfArgumentNull(nameof(loadPost));

            var datPostLoad = new
            {
                loadPost.UserId,
                loadPost.Credential,
                loadPost.LoadId,
                loadPost.Origin,
                loadPost.Destination,
                Equipment = loadPost.EquipmentType,
                loadPost.PickUpDate,
                loadPost.IsLoadPartial,
                loadPost.Weight,
                loadPost.Rate,
                Length = loadPost.EquipmentLength,
                loadPost.NumberOfStops,
                loadPost.Notes,
                IsPosted = (loadPost.DATPostStatus == (int)ExternalLoadPostStatus.Posted ? true: false),
                loadPost.Team,
                loadPost.HazMat
            };

            return PostContentAsync(JsonConvert.SerializeObject(datPostLoad),
                                    "v1/datposting/postloadtodat",
                                    loadPost.UserId).Result.IsSuccessStatusCode;
        }

        public void DeleteLoadPost(int userId, ExternalLoadPostCredential credential, int loadId)
        {
            credential.ThrowIfArgumentNull(nameof(credential));
            loadId.ThrowIfArgumentLessThanOrEqualTo(nameof(loadId), 0);

            var datUnpostLoad = new
            {
                userId,
                credential,
                loadId
            };

            if (!PostContentAsync(JsonConvert.SerializeObject(datUnpostLoad),
                              "v1/datposting/unpostloadfromdat",
                              userId).Result.IsSuccessStatusCode)
                throw new ExternalServiceException($"Unable to unpost Load #{loadId} from DAT", ExternalService.DAT);

        }
        #endregion
    }
}