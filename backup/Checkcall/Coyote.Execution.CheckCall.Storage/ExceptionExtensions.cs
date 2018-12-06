﻿// /////////////////////////////////////////////////////////////////////////////////////
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
namespace Coyote.Execution.CheckCall.Storage
{
    using System; 
    public static class ExceptionExtensions
    {
        public static string ThrowIfArgumentNullOrEmpty(this string value, string variableName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(variableName);
            }
            return value;
        }
    }
}
