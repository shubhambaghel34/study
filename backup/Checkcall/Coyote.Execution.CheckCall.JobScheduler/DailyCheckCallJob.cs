// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2016 - 2018
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
    using NServiceBus;
    using Quartz;
    using Coyote.Execution.CheckCall.Contracts.Commands;    
    using Coyote.Execution.CheckCall.Domain.Models;

    public class DailyCheckCallJob : IJob
    {
        private readonly IBus _bus;
        private readonly ILog _logger;
        public DailyCheckCallJob(IBus bus, ILog log)
        {
            if (bus == null) throw new AggregateException("bus");
            if (log == null) throw new AggregateException("log");
            _bus = bus;
            _logger = log;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public void Execute(IJobExecutionContext context)
        {            
            if (context.Trigger.Key.Name == CheckCallTrigger.DailyCheckCallTrigger.ToString())
            {
                _logger.Info("Initiating Daily Check Call Email Generation.");
                _bus.Send(new ProcessDailyCheckCallEmails());
                _logger.Info("Daily Check Call Email Generation request completed.");
            }
            else if (context.Trigger.Key.Name == CheckCallTrigger.PriorDayCheckCallTrigger.ToString())
            {
                _logger.Info("Initiating Prior Day Check Call Email Generation.");
                _bus.Send(new ProcessPriorDayCheckCallEmails());
                _logger.Info("Prior Day Check Call Email Generation request completed.");
            }
        }
    }
}
