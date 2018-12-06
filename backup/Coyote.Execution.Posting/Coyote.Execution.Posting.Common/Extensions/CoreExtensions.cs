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
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    public static class CoreExtensions
    {
        public static string GetEnumDescription(this Enum value)
        {
            if (value != null)
            {
                IReadOnlyCollection<Attribute> attributes = value.GetEnumAttributes();
                DescriptionAttribute descriptionAttribute = attributes.OfType<DescriptionAttribute>().FirstOrDefault();
                return descriptionAttribute != null ? descriptionAttribute.Description : value.ToString();
            }

            return null;
        }
        public static IReadOnlyCollection<Attribute> GetEnumAttributes(this Enum value)
        {
            value.ThrowIfArgumentNull("value");

            FieldInfo field = value.GetType().GetField(value.ToString());
            return Attribute.GetCustomAttributes(field);
        }
    }
}