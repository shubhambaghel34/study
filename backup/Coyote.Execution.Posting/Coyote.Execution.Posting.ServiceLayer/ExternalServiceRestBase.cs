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
namespace Coyote.Execution.Posting.ServiceLayer
{
    using Coyote.Execution.Posting.Contracts.Storage;
    using log4net;
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    public class ExternalServiceRestBase: ExternalServiceBase
    {
        #region " Constructor "
        public ExternalServiceRestBase(IPostingRepository postingRepository, Uri uri, Common.Coyote.Types.ExternalService externalService, ILog log)
            : base(postingRepository, uri, externalService, log)
        {
        }
        #endregion

        #region " Static Methods "
        protected static string SerializeObjectToXml(object valueToSerialize)
        {
            return SerializeObjectToXml(valueToSerialize, string.Empty);
        }

        protected static string SerializeObjectToXml(object valueToSerialize, string xmlNamespace)
        {
            if (valueToSerialize == null) { return string.Empty; }

            using (var stringWriter = new StringWriter())
            {
                var settings = new XmlWriterSettings { OmitXmlDeclaration = true };
                var xmlWriter = XmlWriter.Create(stringWriter, settings);
                var serializer = new XmlSerializer(valueToSerialize.GetType(), xmlNamespace);
                var serializerNameSpaces = new XmlSerializerNamespaces();
                serializerNameSpaces.Add("", xmlNamespace);
                serializer.Serialize(xmlWriter, valueToSerialize, serializerNameSpaces);
                return stringWriter.ToString();
            }
        }

        protected static T DeserializeFromXml<T>(string valueToDeserialize)
        {
            if (string.IsNullOrEmpty(valueToDeserialize)) { return default(T); }

            using (var reader = new StringReader(valueToDeserialize))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(reader);
            }
        } 
        #endregion
    }
}