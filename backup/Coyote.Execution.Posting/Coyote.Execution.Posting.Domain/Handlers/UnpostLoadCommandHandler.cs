// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2017 - 2017
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////
namespace Coyote.Execution.Posting.Contracts.Handlers
{
    using Coyote.Execution.Posting.Common.Extensions;
    using Coyote.Execution.Posting.Contracts.Commands;
    using Coyote.Execution.Posting.Contracts.Managers;
    using log4net;
    using NServiceBus;
    using System;
    using System.Threading.Tasks;

    public class UnpostLoadCommandHandler : IHandleMessages<UnpostLoadCommand>
    {
        #region " Private fields "
        private readonly ILog _log;
        private ILoadUnpostManager _loadUnpostManager { get; set; }
        #endregion

        #region " Constructor "
        public UnpostLoadCommandHandler(ILog log, ILoadUnpostManager loadUnpostManager)
        {
            _log = log.ThrowIfArgumentNull(nameof(log));
            _loadUnpostManager = loadUnpostManager.ThrowIfArgumentNull(nameof(loadUnpostManager));
        }
        #endregion

        #region " Handler "
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public async Task Handle(UnpostLoadCommand message, IMessageHandlerContext context)
        {
            message.ThrowIfArgumentNull(nameof(message));
            if (message.LoadId <= 0) throw new ArgumentException($"Invalid LoadId #{message.LoadId}.");

            _log.Info($"Unposting Load #{message.LoadId}.");
            await _loadUnpostManager.UnpostLoad(message);
        }
        #endregion
    }
}