// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2018 - 2018
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////
namespace Coyote.Execution.Posting.Tests.Unit.Helper
{
    using Coyote.Execution.Posting.Common.Extensions;
    using System;
    using System.Reflection;

    public static class TestManager
    {
        public static object RunInstanceMethod(Type type, string methodName, object typeInstance, object[] parameters)
        {
            type.ThrowIfArgumentNull(nameof(type));
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

            MethodInfo methodInfo;
            try
            {
                methodInfo = type.GetMethod(methodName, bindingFlags);
                if (methodInfo == null)
                {
                    throw new ArgumentException("There is no method as '" +
                     methodName + "' of type '" + type.ToString() + "'.");
                }

                object result = methodInfo.Invoke(typeInstance, parameters);
                return result;
            }
            catch
            {
                throw;
            }
        }
    }
}