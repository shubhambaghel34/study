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
namespace Coyote.Execution.Posting.Contracts.Models.PostEverywhere
{
    using System.Xml.Serialization;

    [XmlRoot("PostLoads.Many")]
    public class PostEverywhereResponseModel
    {
        [XmlElement("ResponseDO")]
        public ResponseDoModel ResponseDo { get; set; }

        [XmlElement("PostingErrors")]
        public PostingErrorsModel PostingErrors { get; set; }
    }
}