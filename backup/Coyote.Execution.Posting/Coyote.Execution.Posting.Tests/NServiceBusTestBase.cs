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
namespace Coyote.Execution.Posting.Tests
{
    using NServiceBus.Testing;

    public abstract class NServiceBusTestBase : TestBase
    {
        protected TestableMessageHandlerContext Context { get; private set; }

        protected void NServiceBusTestInitialize()
        {
            BaseInitialize();
            Context = new TestableMessageHandlerContext();
        }
    }
}