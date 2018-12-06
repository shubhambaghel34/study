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
namespace Coyote.Execution.Posting.ServiceLayer.InternetTruckStop.Enums
{
    using System.ComponentModel;

    public enum Country
    {
        [Description("USA")]
        UnitedStates,
        [Description("CAN")]
        Canada,
        [Description("MEX")]
        Mexico
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
        [Description("NS")]
        NovaScotia,
        [Description("NT")]
        NorthwestTerritories,
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
        BajaCaliforniaNorte,
        [Description("BS")]
        BajaCaliforniaSur,
        [Description("CH")]
        Chihuahua,
        [Description("CL")]
        Colima,
        [Description("CM")]
        Campeche,
        [Description("CO")]
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
        Zacatecas
    }

    public enum EquipmentCode
    {
        [Description("2F")]
        Two24Or28FootFlats,
        [Description("ANIM")]
        AnimalCarrier,
        [Description("AUTO")]
        AutoCarrier,
        [Description("B-TR")]
        BTrainOrSuperTrain,
        [Description("BDMP")]
        BellyDump,
        [Description("BEAM")]
        Beam,
        [Description("BELT")]
        ConveyorBelt,
        [Description("BOAT")]
        BoatHaulingTrailer,
        [Description("CH")]
        ConvertibleHopper,
        [Description("CONG")]
        Conestoga,
        [Description("CONT")]
        ContainerTrailer,
        [Description("CV")]
        CurtainVan,
        [Description("DA")]
        DriveAway,
        [Description("DD")]
        DoubleDrop,
        [Description("DDE")]
        DoubleDropExtendable,
        [Description("DUMP")]
        DumpTrucks,
        [Description("ENDP")]
        EndDump,
        [Description("F")]
        Flatbed,
        [Description("FA")]
        FlatbedAirRide,
        [Description("FEXT")]
        StretchTrailersOrExtendableFlatbed,
        [Description("FINT")]
        FlatbedIntermodal,
        [Description("FO")]
        FlatbedOverDimensionLoads,
        [Description("FRV")]
        FlatbedReeferOrVan,
        [Description("FSD")]
        FlatOrStepDeck,
        [Description("FSDV")]
        FlatbedStepDeckOrVan,
        [Description("FV")]
        FlatbedOrVan,
        [Description("FVR")]
        FlatbedVanOrReefer,
        [Description("FVV")]
        FlatbedOrVentedVan,
        [Description("FVVR")]
        FlatbedVentedVanOrReefer,
        [Description("FWS")]
        FlatbedWithSides,
        [Description("HOPP")]
        HopperBottom,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "HotShot")]
        [Description("HS")]
        HotShot,
        [Description("HTU")]
        HaulAndTowUnit,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Landoll")]
        [Description("LAF")]
        LandollFlatbed,
        [Description("LB")]
        Lowboy,
        [Description("LBO")]
        LowboyOverDimensionLoad,
        [Description("LDOT")]
        LoadOut,
        [Description("LIVE")]
        LiveBottomTrailer,
        [Description("MAXI")]
        MaxiOrDoubleFlatTrailers,
        [Description("MBHM")]
        MobileHome,
        [Description("PNEU")]
        Pneumatic,
        [Description("PO")]
        PowerOnly,
        [Description("R")]
        Reefer,
        [Description("RFV")]
        ReeferFlatbedOrVan,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi")]
        [Description("RGN")]
        RemovableGooseNeckAndMultiAxleHeavyHaulers,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "RGN")]
        [Description("RGNE")]
        RGNExtendable,
        [Description("RINT")]
        RefrigeratedIntermodal,
        [Description("ROLL")]
        RollTopConestoga,
        [Description("RPD")]
        RefrigeratedCarrierWithPlantDecking,
        [Description("RV")]
        ReeferOrVan,
        [Description("RVF")]
        ReeferVanOrFlatbed,
        [Description("SD")]
        StepDeck,
        [Description("SDL")]
        StepDeckWithLoadingRamps,
        [Description("SDO")]
        StepDeckOverDimensionLoad,
        [Description("SDRG")]
        StepDeckOrRemovableGooseneck,
        [Description("SPEC")]
        UnspecifiedSpecializedTrailers,
        [Description("SV")]
        StraightVan,
        [Description("TANK")]
        Tanker,
        [Description("V")]
        Van,
        [Description("V-OT")]
        OpenTopVan,
        [Description("VA")]
        VanAirRide,
        [Description("VB")]
        BlanketWrapVan,
        [Description("VCAR")]
        CargoVans,
        [Description("VF")]
        VanOrFlatbed,
        [Description("VFR")]
        VanFlatbedOrReefer,
        [Description("VINT")]
        VanIntermodal,
        [Description("VIV")]
        VentedInsulatedVan,
        [Description("VIVR")]
        VentedInsulatedVanOrRefrigerated,
        [Description("VLG")]
        VanWithLiftGate,
        [Description("VM")]
        MovingVan,
        [Description("VR")]
        VanOrReefer,
        [Description("VRDD")]
        VanReeferOrDoubleDrop,
        [Description("VRF")]
        VanReeferOrFlatbed,
        [Description("VV")]
        VentedVan,
        [Description("VVR")]
        VentedVanOrRefrigerated,
        [Description("WALK")]
        WalkingFloor
    }
}