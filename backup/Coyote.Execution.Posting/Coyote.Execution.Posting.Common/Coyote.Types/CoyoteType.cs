// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2017 - 2018
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
//
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////
namespace Coyote.Execution.Posting.Common.Coyote.Types
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    [DataContract]
    public enum ExternalService
    {
        [EnumMember]
        Undefined = 0,
        [EnumMember]
        InternetTruckStop = 2,
        [EnumMember]
        DAT = 3,
        [EnumMember]
        PostEverywhere = 4
    }

    public enum PostAction
    {
        [Description("A")]
        Add,
        [Description("U")]
        Update,
        [Description("D")]
        Delete
    }

    public enum FullOrPartial
    {
        Full,
        Partial
    }

    public enum StateCode
    {
        // United States
        [Description("AL")]
        Alabama,
        [Description("AK")]
        Alaska,
        [Description("AZ")]
        Arizona,
        [Description("AR")]
        Arkansas,
        [Description("CA")]
        California,
        [Description("CO")]
        Colorado,
        [Description("CT")]
        Connecticut,
        [Description("DE")]
        Delaware,
        [Description("DC")]
        DistrictOfColumbia,
        [Description("FL")]
        Florida,
        [Description("GA")]
        Georgia,
        [Description("HI")]
        Hawaii,
        [Description("ID")]
        Idaho,
        [Description("IL")]
        Illinois,
        [Description("IN")]
        Indiana,
        [Description("IA")]
        Iowa,
        [Description("KS")]
        Kansas,
        [Description("KY")]
        Kentucky,
        [Description("LA")]
        Louisiana,
        [Description("MA")]
        Massachusetts,
        [Description("ME")]
        Maine,
        [Description("MD")]
        Maryland,
        [Description("MI")]
        Michigan,
        [Description("MN")]
        Minnesota,
        [Description("MS")]
        Mississippi,
        [Description("MO")]
        Missouri,
        [Description("MT")]
        Montana,
        [Description("NE")]
        Nebraska,
        [Description("NV")]
        Nevada,
        [Description("NH")]
        NewHampshire,
        [Description("NJ")]
        NewJersey,
        [Description("NM")]
        NewMexico,
        [Description("NY")]
        NewYork,
        [Description("NC")]
        NorthCarolina,
        [Description("ND")]
        NorthDakota,
        [Description("OH")]
        Ohio,
        [Description("OK")]
        Oklahoma,
        [Description("OR")]
        Oregon,
        [Description("PA")]
        Pennsylvania,
        [Description("RI")]
        RhodeIsland,
        [Description("SC")]
        SouthCarolina,
        [Description("SD")]
        SouthDakota,
        [Description("TN")]
        Tennessee,
        [Description("TX")]
        Texas,
        [Description("UT")]
        Utah,
        [Description("VT")]
        Vermont,
        [Description("VA")]
        Virginia,
        [Description("WA")]
        Washington,
        [Description("WV")]
        WestVirginia,
        [Description("WI")]
        Wisconsin,
        [Description("WY")]
        Wyoming,
        // Canada
        [Description("AB")]
        Alberta,
        [Description("BC")]
        BritishColumbia,
        [Description("MB")]
        Manitoba,
        [Description("NB")]
        NewBrunswick,
        [Description("NL")]
        NewfoundlandAndLabrador,
        [Description("NT")]
        NorthwestTerritories,
        [Description("NU")]
        Nunavut,
        [Description("ON")]
        Ontario,
        [Description("PE")]
        PrinceEdwardIsland,
        [Description("QC")]
        Quebec,
        [Description("SK")]
        Saskatchewan,
        [Description("YT")]
        Yukon,
        // Mexico
        [Description("AG")]
        Aguascalientes,
        [Description("BC")]
        BajaCalifornia,
        [Description("BN")]
        BajaCaliforniaNorte,
        [Description("BS")]
        BajaCaliforniaSur,
        [Description("CH")]
        Chihuahua,
        [Description("CL")]
        Colima,
        [Description("CM")]
        Campeche,
        [Description("CU")]
        Coahuila,
        [Description("CS")]
        Chiapas,
        [Description("DF")]
        DistritoFederal,
        [Description("DG")]
        Durango,
        [Description("GR")]
        Guerrero,
        [Description("GT")]
        Guanajuato,
        [Description("HG")]
        Hidalgo,
        [Description("JA")]
        Jalisco,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Michoacan")]
        [Description("MI")]
        Michoacan,
        [Description("MO")]
        Morelos,
        [Description("MX")]
        Mexico,
        [Description("NA")]
        Nayarit,
        [Description("NL")]
        NuevoLeon,
        [Description("OA")]
        Oaxaca,
        [Description("PU")]
        Puebla,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Quintanaroo")]
        [Description("QR")]
        Quintanaroo,
        [Description("QT")]
        Queretaro,
        [Description("SI")]
        Sinaloa,
        [Description("SL")]
        SanLuisPotosi,
        [Description("SO")]
        Sonora,
        [Description("TB")]
        Tabasco,
        [Description("TL")]
        Tlaxcala,
        [Description("TM")]
        Tamaulipas,
        [Description("VE")]
        Veracruz,
        [Description("YU")]
        Yucatan,
        [Description("ZT")]
        Zacatecas,
        // US territories
        [Description("GU")]
        Guam,
        [Description("PR")]
        PuertoRico,
        [Description("VI")]
        USVirginIslands
    }

    public enum EquipmentCode
    {
        [Description("AUTO")]
        AutoCarrier,
        [Description("DD")]
        DoubleDrop,
        [Description("DT")]
        DumpTrailer,
        [Description("F")]
        Flatbed,
        [Description("FWS")]
        FlatbedWithSides,
        [Description("FD")]
        FlatbedOrStepDeck,
        [Description("FT")]
        FlatbedWithTarps,
        [Description("FX")]
        FlatbedWithPalletExchange,
        [Description("FZ")]
        FlatbedHazardous,
        [Description("HB")]
        HopperBottom,
        [Description("HS")]
        Hotshot,
        [Description("LB")]
        Lowboy,
        [Description("MX")]
        Maxi,
        [Description("PO")]
        PowerOnly,
        [Description("R")]
        Reefer,
        [Description("RG")]
        RemovableGooseneck,
        [Description("RX")]
        ReeferWithPalletExchange,
        [Description("RZ")]
        ReeferHazardous,
        [Description("SD")]
        StepDeck,
        [Description("TA")]
        Tanker,
        [Description("V")]
        Van,
        [Description("VA")]
        VanAirRide,
        [Description("VC")]
        VanWithCurtains,
        [Description("VF")]
        VanOrFlatbed,
        [Description("VFR")]
        VanReeferFlatbed,
        [Description("VR")]
        VanOrReefer,
        [Description("VV")]
        VanVented,
        [Description("VX")]
        VanWithPalletExchange,
        [Description("VZ")]
        VanHazardous
    }

    [DataContract]
    public enum EntityType
    {
        [EnumMember]
        Undefined = 0,
        [EnumMember]
        Carrier = 1,
        [EnumMember]
        Contact = 2,
        [EnumMember]
        Customer = 3,
        [EnumMember]
        Employee = 4,
        [EnumMember]
        Facility = 5,
        [EnumMember]
        Insurer = 6,
        [EnumMember]
        Vendor = 7,
        [EnumMember]
        City = 8,
        [EnumMember]
        Company = 9,
        [EnumMember]
        CarrierTruck = 10,
        [EnumMember]
        Load = 11,
        [EnumMember]
        LoadCustomer = 12,
        [EnumMember]
        LoadCarrier = 13,
        [EnumMember]
        LoadShipper = 14,
        [EnumMember]
        LoadConsignee = 15,
        [EnumMember]
        Credit = 16,
        [EnumMember]
        Lead = 17,
        [EnumMember]
        Offer = 18,
        [EnumMember]
        SystemSettings = 19,
        [EnumMember]
        CostQuote = 20,
        [EnumMember]
        Security = 21,
        [EnumMember]
        User = 22,
        [EnumMember]
        Opportunity = 23,
        [EnumMember]
        State = 24,
        [EnumMember]
        LoadRoute = 25,
        [EnumMember]
        CompanyBranch = 26,
        [EnumMember]
        CompanyInvoiceTemplate = 27,
        [EnumMember]
        CustomerInvoiceTemplate = 28,
        [EnumMember]
        FuelSurchargeTemplate = 29,
        [EnumMember]
        ReferenceNumber = 30,
        [EnumMember]
        PickupNumber = 31,
        [EnumMember]
        BillOfLading = 32,
        [EnumMember]
        DeliveryNumber = 33,
        [EnumMember]
        LoadCommodity = 34,
        [EnumMember]
        StandardTransportationCommodityCode = 35,
        [EnumMember]
        CostQuoteImportLocation = 36,
        [EnumMember]
        LoadInvoice = 37,
        [EnumMember]
        LoadVoucher = 38,
        [EnumMember]
        LoadRate = 39,
        [EnumMember]
        LoadTracking = 40,
        [EnumMember]
        LoadStop = 41,
        [EnumMember]
        LoadAccountingNotes = 42,
        [EnumMember]
        GreatPlainsBridgeCarrier = 43,
        [EnumMember]
        GreatPlainsBridgeCustomer = 44,
        [EnumMember]
        LoadDiaryNotes = 45,
        [EnumMember]
        Edi204 = 46,
        [EnumMember]
        Edi210 = 47,
        [EnumMember]
        Edi214 = 48,
        [EnumMember]
        Edi322 = 49,
        [EnumMember]
        Edi404 = 50,
        [EnumMember]
        Edi410 = 51,
        [EnumMember]
        Edi418 = 52,
        [EnumMember]
        Edi824 = 53,
        [EnumMember]
        Edi990 = 54,
        [EnumMember]
        Edi997 = 55,
        [EnumMember]
        AutoTrux = 56,
        [EnumMember]
        SystemUser = 57,
        [EnumMember]
        ConsolidatedCustomerInvoice = 58,
        [EnumMember]
        Order = 59,
        [EnumMember]
        OrderStop = 60,
        [EnumMember]
        OrderCommodity = 61,
        [EnumMember]
        ManagedLoadOfferHistory = 62,
        [EnumMember]
        ManagedLoadOfferQueue = 63,
        [EnumMember]
        EDI856 = 64,
        [EnumMember]
        FactoringCompany = 65,
        [EnumMember]
        CarrierAccountingTrackingNote = 66,
        [EnumMember]
        CustomerDoNotUseCarrier = 67,
        [EnumMember]
        Shipment = 68,
        [EnumMember]
        Regions = 69
    }

    public enum ExternalLoadPostAction
    {
        [Description("Unknown")]
        Unknown = 0,
        [Description("Post")]
        Post = 1,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Unpost")]
        [Description("Unpost")]
        Unpost = 2,
        [Description("Auto-Refresh")]
        AutoRefresh = 3
    }

    public enum ExternalLoadPostStatus
    {
        [Description("Never Posted")]
        NeverPosted = 0,
        [Description("Posted")]
        Posted = 1,
        [ Description("Not Posted") ]
        NotPosted = 2
    }

    public enum LoadProgressType
    {
        Unassigned = 0,
        Available = 1,
        Covered = 2,
        Dispatched = 3,
        LoadingBegin = 4,
        PickedUp = 5,
        UnloadingBegin = 6,
        Delivered = 7,
        Invoiced = 8
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum LoadState
    {
        Active = 1,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Cancelled")]
        Cancelled = 2,
        Hold = 3,
        NonComm = 4,
        Pending = 5,
        Tendered = 6,
        Rejected = 7
    }
}