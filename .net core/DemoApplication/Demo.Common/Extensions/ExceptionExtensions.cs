namespace Demo.Common.Extensions
{
    using System;

    public static class ExceptionExtensions
    {
        public static T ThrowIfArgumentNull<T>(this T value, string variableName) where T : class
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

        public static T ThrowIfArgumentEqualTo<T>(this T value, string variableName, T lessThanOrEqualTo) where T : IComparable
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

        public static T ThrowIfArgumentLessThanOrEqualTo<T>(this T value, string variableName, T lessThanOrEqualTo) where T : IComparable
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
