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

namespace Coyote.Execution.CheckCall.Tests.Unit
{
    using System;
    using log4net;
    using Moq;
    using NServiceBus.Testing;

    public class NServiceBusHandlerUnitTestBase
    {
        protected Mock<ILog> LogMock { get; set; }
        
        protected void BaseTestInit()
        {
            LogMock = new Mock<ILog>();

            Test.Initialize();
        }

        public static bool GetExpectFalseCallback<T>(T obj, Action<T> callback)
        {
            if (callback != null)
            {
                callback.Invoke(obj);
            }
            return false;
        }
        public static bool GetExpectCallback<T>(T obj, Action<T> callback)
        {
            if (callback != null)
            {
                callback.Invoke(obj);
            }
            return true;
        }

    }
}
