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

namespace Coyote.Execution.CheckCall.JobScheduler
{
    using System;
    using Common.Logging;
    using Quartz;

    public class DailyCheckCallEmailService
    {
        private static readonly ILog _logger = LogManager.GetLogger<DailyCheckCallEmailService>();
        private readonly IScheduler _scheduler;

        public DailyCheckCallEmailService(IScheduler scheduler)
        {
            if (scheduler == null) throw new ArgumentNullException("scheduler");
            _scheduler = scheduler;
        }

        public bool Start()
        {
            _logger.Info("DailyCheckCallEmailService Starting.");

            if (!_scheduler.IsStarted)
            {
                _scheduler.Start();
            }

            _logger.Info("DailyCheckCallEmailService Started.");
            return true;
        }

        public bool Stop()
        {
            _logger.Info("DailyCheckCallEmailService Stopping.");

            _scheduler.Shutdown(true);

            _logger.Info("DailyCheckCallEmailService Stopped.");
            return true;
        }

    }
}
