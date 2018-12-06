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


namespace Coyote.Execution.CheckCall.Tests.Integration.Support
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class RandomDataGeneration
    {
        private static readonly Random _random = new Random();
        private static readonly object _syncLock = new object();

        public static string String(int length)
        {
            string result = System.IO.Path.GetRandomFileName();
            result = result.Replace(".", "");
            return result.Substring(0, System.Math.Min(result.Length, length));
        }

        public static string String(int length, bool exactLength)
        {
            string result = String(length);
            if (exactLength)
            {
                while (result.Length != length)
                {
                    result = String(length);
                }
            }
            return result;
        }

        public static string StringFromCharacterSet(int length, string allowedCharacters)
        {
            if (allowedCharacters == null)
                throw new ArgumentNullException("allowedCharacters");

            var result = "";
            for (int i = 0; i < length; i++)
            {
                result += allowedCharacters[Number(allowedCharacters.Length)];
            }
            return result;
        }

        public static int Number(int maxValue)
        {
            return Number(1, maxValue);
        }

        /// <remarks>
        /// Need to use single instance to make sure numbers are random when called
        ///  in tight loop - then need to synchronize in case multiple threads calling it
        /// http://stackoverflow.com/a/768001
        /// </remarks>
        public static int Number(int minValue, int maxValue)
        {
            lock (_syncLock)
            {
                return _random.Next(minValue, maxValue);
            }
        }

        public static T EnumValue<T>(params T[] disallowed) where T : struct
        {
            var values = Enum.GetValues(typeof(T))
                .Cast<T>()
                .Where(v => !disallowed.Contains(v))
                .ToArray();

            if (values.Length == 0)
                throw new InvalidOperationException("No allowed values found.");

            var index = Number(0, values.Length);
            return values[index];
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object")]
        public static void FillObject(object emptyObject)
        {
            if (emptyObject == null)
            {
                return;
            }

#if DEBUG
            if (emptyObject.GetType().IsPrimitive || emptyObject.GetType().IsEnum)
            {
                System.Diagnostics.Debug.Assert(false, "Unsupported types through the FillObject function");
                return;
            }
#endif

            PropertyInfo[] properties = emptyObject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (PropertyInfo propertyInfo in properties)
            {
                object propertyValue = null;

                Type propType = propertyInfo.PropertyType;
                if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    Type[] typeCol = propType.GetGenericArguments();
                    Type nullableType;
                    if (typeCol.Length > 0)
                    {
                        nullableType = typeCol[0];
                        if (nullableType.BaseType == typeof(Enum))
                        {
                            object o = Enum.Parse(nullableType, "1");
                            propertyInfo.SetValue(emptyObject, o, null);
                        }
                    }
                }
                else
                {

                    var underlyingType = GetUnderlyingType(propertyInfo.PropertyType);
                    switch (Type.GetTypeCode(underlyingType))
                    {
                        case TypeCode.Byte:
                            propertyValue = Convert.ToByte(Number(SByte.MaxValue));
                            break;
                        case TypeCode.SByte:
                            propertyValue = Convert.ToSByte(Number(SByte.MaxValue));
                            break;
                        case TypeCode.Decimal:
                            propertyValue = Convert.ToDecimal(Number(Int16.MaxValue));
                            break;
                        case TypeCode.Double:
                            propertyValue = Convert.ToDouble(Number(Int16.MaxValue));
                            break;
                        case TypeCode.Int16:
                            propertyValue = Convert.ToInt16(Number(Int16.MaxValue));
                            break;
                        case TypeCode.Int32:
                            propertyValue = Number(Int16.MaxValue);
                            break;
                        case TypeCode.Int64:
                            propertyValue = Convert.ToInt64(Number(Int16.MaxValue));
                            break;
                        case TypeCode.Single:
                            propertyValue = Convert.ToSingle(Number(Int16.MaxValue));
                            break;
                        case TypeCode.UInt16:
                            propertyValue = Convert.ToUInt16(Number(Int16.MaxValue));
                            break;
                        case TypeCode.UInt32:
                            propertyValue = Convert.ToUInt32(Number(Int16.MaxValue));
                            break;
                        case TypeCode.UInt64:
                            propertyValue = Convert.ToUInt64(Number(Int16.MaxValue));
                            break;
                        case TypeCode.String:
                            propertyValue = String(50);
                            break;
                        case TypeCode.DateTime:
                            propertyValue = DateTime.Now.AddDays(Number(365));
                            break;
                    }
                    if (propertyValue != null)
                    {
                        propertyInfo.SetValue(emptyObject, propertyValue);
                    }
                }
            }
        }


        private static Type GetUnderlyingType(Type propertyType)
        {
            return propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
                       ? Nullable.GetUnderlyingType(propertyType)
                       : propertyType;
        }

        public static bool Boolean()
        {
            return (Number(0, 9) % 2 == 0);
        }

        public static decimal Decimal(int scale, decimal minValue, decimal maxValue)
        {
            int multipler = (int)Math.Pow(10, scale);
            int number = Number((int)(minValue * multipler), (int)(maxValue * multipler));
            return (decimal)number / (decimal)multipler;
        }
    }
}
