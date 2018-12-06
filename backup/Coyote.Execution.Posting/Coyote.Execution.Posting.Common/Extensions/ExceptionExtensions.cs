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
namespace Coyote.Execution.Posting.Common.Extensions
{
    using global::Coyote.Execution.Posting.Common.Attributes;
    using System;

    public static class ExceptionExtensions
    {
        public static T ThrowIfArgumentNull<T>([ValidatedNotNull] this T value, string variableName) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(variableName);
            }

            return value;
        }

        public static string ThrowIfArgumentNullOrEmpty(this string value, string variableName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(variableName);
            }
            return value;
        }

        public static T ThrowIfArgumentEqualTo<T>([ValidatedNotNull] this T value, string variableName, T lessThanOrEqualTo) where T : IComparable
        {
            if (value == null)
            {
                throw new ArgumentNullException(variableName);
            }
            if (value.CompareTo(lessThanOrEqualTo) == 0)
            {
                throw new ArgumentException("Invalid", variableName);
            }
            return value;
        }

        public static T ThrowIfArgumentLessThanOrEqualTo<T>([ValidatedNotNull] this T value, string variableName, T lessThanOrEqualTo) where T : IComparable
        {
            if (value == null)
            {
                throw new ArgumentNullException(variableName);
            }
            if (value.CompareTo(lessThanOrEqualTo) <= 0)
            {
                throw new ArgumentException("Invalid", variableName);
            }
            return value;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static T ThrowIfNull<T>(this T value, string variableName) where T : class
        {
            if (value == null)
            {
                throw new NullReferenceException(variableName);
            }

            return value;
        }
    }
}