// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2016 - 2017
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////

namespace Coyote.Execution.CheckCall.Services
{
    using System;
    using log4net;
    using Newtonsoft.Json;
    using Coyote.Execution.CheckCall.Contracts;
    using Coyote.Execution.CheckCall.Contracts.Services;
    using Coyote.Execution.CheckCall.Domain.Models;  
    using Coyote.Common.Extensions;

    public class ExecutionCommandService : ServiceBase, IExecutionCommandService
    {
        public ExecutionCommandService(ILog logger)
            :base(logger, "ExecutionCommandServiceUri")
        { }

        public void CreateLoadTrackingNote(int auditUserId, LoadInfo load, int carrierId, int mainRepId, string messageTo, DateTime? nextCallbackDateTime)
        {
            load.ThrowIfNull("load");
            Logger.InfoFormat("Tracking Note Created for Load : {0}", load.LoadId);

            var note = new TrackingNote()
            {
                LoadId = load.LoadId,
                Notes = "Automated check call email sent to " + messageTo,
                CarrierId = carrierId,
                CityId = 0,
                CityState = string.Empty,
                Action = TrackingActionType.CheckCall,
                EntryDate = DateTime.Now,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                EmailReps = false,
                NotificationSent = false,
                Private = true,
                TrackingInfoSource = TrackingInfoSourceType.Undefined,
                TrackingMethodType = TrackingMethodType.System,
                ParentEntityType = EntityType.Undefined,
                CreateByUserId = auditUserId,
                UpdateByUseId = auditUserId,
                NextCallUserId = mainRepId,
                NextCallDate = nextCallbackDateTime
            };

            PostContent(auditUserId, "/CreateLoadTrackingNote", JsonConvert.SerializeObject(note, new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat }));
        }
        
        private class TrackingNote
        {
            public int LoadId { get; set; }
            public int CarrierId { get; set; }
            public int CityId { get; set; }
            public string CityState { get; set; }
            public int CreateByUserId { get; set; }
            public DateTime? CreateDate { get; set; }
            public int UpdateByUseId { get; set; }
            public DateTime? UpdateDate { get; set; }
            public TrackingActionType Action { get; set; }
            public bool Private { get; set; }
            public TrackingInfoSourceType TrackingInfoSource { get; set; }
            public TrackingMethodType TrackingMethodType { get; set; }
            public DateTime EntryDate { get; set; }
            public int? NextCallUserId { get; set; }
            public DateTime? NextCallDate { get; set; }
            public Decimal? NextStopDistanceMiles { get; set; }
            public bool EmailReps { get; set; }
            public bool NotificationSent { get; set; }
            public EntityType ParentEntityType { get; set; }
            public string Notes { get; set; }

        }

    }
}
