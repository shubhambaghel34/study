// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2016 - 2016
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////

namespace Coyote.Carrier.Ops.CheckCall.Storage.Repositories
{
    using Coyote.Common.Extensions;
    using log4net;

    public class RepositoryBase
    {
        protected CheckCallDbContext DbContext { get; private set; }
        protected ILog Logger { get; private set; }

        public RepositoryBase(ILog logger)
        {
            Logger = logger.ThrowIfNull("logger");
            DbContext = new CheckCallDbContext();
        }
    }
}

