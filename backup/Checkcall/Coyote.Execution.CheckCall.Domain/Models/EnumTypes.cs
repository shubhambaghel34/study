// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2016 - 2018
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////
namespace Coyote.Execution.CheckCall.Domain.Models
{
    using System.Diagnostics.CodeAnalysis;

    public enum LoadModeType
    {
        Undefined = 0,
        TL = 1,
        LTL = 2,
        IMDL = 3,
        Rail = 4,
        Ocean = 5,
        Air = 6
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
    public enum LoadStateType
    {
        Undefined = 0,
        Active = 1,
        Canceled = 2,
        Hold = 3,
        NonComm = 4,
        Pending = 5,
        Tendered = 6
    }
    public enum CarrierCommunicationPreferenceType
    {
        Undefined = 0,
        Email = 1,
        Phone = 2,
        Web = 3
    }

    public enum EntityType
    {
        Undefined = 0,
        Carrier = 1,
        Contact = 2,
        Customer = 3,
        Employee = 4,
        Facility = 5,
        Insurer = 6,
        Vendor = 7,
        City = 8,
        Company = 9,
        CarrierTruck = 10,
        Load = 11,
        LoadCustomer = 12,
        LoadCarrier = 13,
        LoadShipper = 14,
        LoadConsignee = 15,
        Credit = 16,
        Lead = 17,
        Offer = 18,
        SystemSettings = 19,
        CostQuote = 20,
        Security = 21,
        User = 22,
        Opportunity = 23,
        State = 24,
        LoadRoute = 25,
        CompanyBranch = 26,
        CompanyInvoiceTemplate = 27,
        CustomerInvoiceTemplate = 28,
        FuelSurchargeTemplate = 29,
        ReferenceNumber = 30,
        PickupNumber = 31,
        BillOfLading = 32,
        DeliveryNumber = 33,
        LoadCommodity = 34,
        StandardTransportationCommodityCode = 35,
        CostQuoteImportLocation = 36,
        LoadInvoice = 37,
        LoadVoucher = 38,
        LoadRate = 39,
        LoadTracking = 40,
        LoadStop = 41,
        LoadAccountingNotes = 42,
        GreatPlainsBridgeCarrier = 43,
        GreatPlainsBridgeCustomer = 44,
        LoadDiaryNotes = 45,
        Edi204 = 46,
        Edi210 = 47,
        Edi214 = 48,
        Edi322 = 49,
        Edi404 = 50,
        Edi410 = 51,
        Edi418 = 52,
        Edi824 = 53,
        Edi990 = 54,
        Edi997 = 55,
        AutoTrux = 56,
        SystemUser = 57,
        ConsolidatedCustomerInvoice = 58,
        Order = 59,
        OrderStop = 60,
        OrderCommodity = 61,
        ManagedLoadOfferHistory = 62,
        ManagedLoadOfferQueue = 63,
        EDI856 = 64,
        FactoringCompany = 65,
        CarrierAccountingTrackingNote = 66
    }
    public enum RepType
    {
        Undefined = 0,
        Sales = 1,
        Dispatch = 2,
        CustomerOperations = 3,
        CarrierOperations = 4,
        AccountsReceivable = 5,
        AccountsPayable = 6,
        CustomerOperationsOrigin = 7,
        CustomerOperationsDestination = 8,
        ExecutiveSponsor = 9,
        Collector = 10,
        Accessorial = 11
    }

    public enum TrackingActionType
    {
        Undefined = 0,
        Dispatched = 1,
        LoadBooked = 2,
        PickedUp = 3,
        Delivered = 4,
        CheckCall = 5,
        TrackingCall = 6,
        LoadingBegin = 7,
        UnloadingBegin = 8,
        EDISystemUpdate = 9,
        TextProgramSent = 10,
        ShipmentDelayed = 11,
        ArriveOrigin = 12,
        DepartOrigin = 13,
        RailBilled = 14,
        InGateOrigin = 15,
        TrainDepart = 16,
        DispatchedDestination = 17,
        Notified = 18,
        OutGateDestination = 19,
        ArriveDestination = 20,
        DepartDestination = 21,
        Terminated = 22,
        CoyoteUpdate = 23,
        LatePickup = 24,
        LateDelivery = 25,
        FinalCostUpdate = 26,
        TMSPrivateCarrier = 27
    }

    public enum TrackingInfoSourceType
    {
        Undefined = 0,
        Driver = 1,
        Dispatch = 2,
        Shipper = 3,
        Consignee = 4,
        Customer = 5,
        Web = 6,
        EDI = 7
    }

    public enum TrackingMethodType
    {
        Undefined = 0,
        System = 1,
        IBCall = 2,
        OBCall = 3,
        Email = 4,
        CarrierWebsite = 5,
        WebPortal = 6,
        CoyoteGO = 7,
        Text = 8,
        GPS = 9,
        EDI = 10
    }

    public enum CheckCallTrigger
    {
        DailyCheckCallTrigger = 0,
        PriorDayCheckCallTrigger = 1
    }

    public enum Division
    {
        Unknown = 0,
        Coy = 1,
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Aat")]
        Aat = 2,
        Both = 3,
        Ups = 4,
        EU = 5
    }
}
