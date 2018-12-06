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
namespace Coyote.Execution.Posting.Contracts.Models.PostEverywhere
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class PostingErrorsModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [XmlElement("ErrorDO")]
        public List<ErrorDoModel> ErrorDos { get; set; }
    }
}