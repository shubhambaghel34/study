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
    using Coyote.Execution.Posting.Common.Coyote.Types;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    public class PostLoadModel
    {
        [XmlElement("phone")]
        public string Phone { get; set; }

        [XmlElement("postAction")]
        public string PostAction { get; set; }

        [XmlElement("importRef")]
        public long? ImportRef { get; set; }

        [XmlElement("originCity")]
        public string OriginCity { get; set; }

        [XmlElement("originState")]
        public string OriginState { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "UpDate")]
        [XmlElement("pickupDate")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy-MM-dd}")]
        public DateTime? PickUpDate { get; set; }

        [XmlElement("destCity")]
        public string DestCity { get; set; }

        [XmlElement("destState")]
        public string DestState { get; set; }

        [XmlElement("truckType")]
        public string TruckType { get; set; }

#pragma warning disable CS3003 // Type is not CLS-compliant
        [XmlElement("fullOrPartial")]
        public FullOrPartial FullOrPartial { get; set; }
#pragma warning restore CS3003

        [XmlElement("length")]
        public int? Length { get; set; }

        [XmlElement("weight")]
        public decimal? Weight { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "UpTime")]
        [XmlElement("pickupTime")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{hh:mm}")]
        public TimeSpan? PickUpTime { get; set; }

        [XmlElement("deliveryDate")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy-MM-dd}")]
        public DateTime? DeliveryDate { get; set; }

        [XmlElement("deliveryTime")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{hh:mm}")]
        public TimeSpan? DeliveryTime { get; set; }

        [XmlElement("miles")]
        public string Miles { get; set; }

        [XmlElement("quantity")]
        public string Quantity { get; set; }

        [XmlElement("rate")]
        public decimal? Rate { get; set; }

        [XmlElement("stops")]
        public int? Stops { get; set; }

        [XmlElement("comment")]
        public string Comment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Def")]
        [XmlElement("userDef1")]
        public string UserDef1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Def")]
        [XmlElement("userDef2")]
        public string UserDef2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Def")]
        [XmlElement("userDef3")]
        public string UserDef3 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Def")]
        [XmlElement("userDef4")]
        public string UserDef4 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Def")]
        [XmlElement("userDef5")]
        public string UserDef5 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Def")]
        [XmlElement("userDef6")]
        public string UserDef6 { get; set; }

        [XmlElement("note")]
        public string Note { get; set; }
    }
}