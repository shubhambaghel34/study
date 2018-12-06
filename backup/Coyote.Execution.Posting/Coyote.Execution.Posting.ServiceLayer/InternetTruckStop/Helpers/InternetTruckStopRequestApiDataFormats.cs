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
namespace Coyote.Execution.Posting.ServiceLayer.InternetTruckStop.Helpers
{
    using Coyote.Execution.Posting.Common.Exceptions;
    using Coyote.Execution.Posting.Common.Extensions;
    using Coyote.Execution.Posting.Contracts.Models;
    using Coyote.Execution.Posting.ServiceLayer.InternetTruckStop.Enums;

    public static class InternetTruckStopRequestApiDataFormats
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public static string FormatState(string stateCode, LocationCountry country)
        {
            if (string.IsNullOrEmpty(stateCode)) { throw new ExternalServiceException(ExternalServiceMessages.InvalidStateCodeExceptionMessage(), Common.Coyote.Types.ExternalService.InternetTruckStop); }

            // This is based on the state information found in LocationState (select * from dbo.LocationState)
            switch (stateCode)
            {
                // United States
                case "AK": { return Common.Coyote.Types.StateCode.Alaska.GetEnumDescription(); }
                case "AL": { return StateCode.Alabama.GetEnumDescription(); }
                case "AR": { return StateCode.Arkansas.GetEnumDescription(); }
                case "AZ": { return StateCode.Arizona.GetEnumDescription(); }
                case "CA": { return StateCode.California.GetEnumDescription(); }
                case "CO": { return StateCode.Colorado.GetEnumDescription(); }
                case "CT": { return StateCode.Connecticut.GetEnumDescription(); }
                case "DE": { return StateCode.Delaware.GetEnumDescription(); }
                case "DC": { return StateCode.DistrictOfColumbia.GetEnumDescription(); }
                case "FL": { return StateCode.Florida.GetEnumDescription(); }
                case "GA": { return StateCode.Georgia.GetEnumDescription(); }
                case "HI": { return StateCode.Hawaii.GetEnumDescription(); }
                case "IA": { return StateCode.Iowa.GetEnumDescription(); }
                case "ID": { return StateCode.Idaho.GetEnumDescription(); }
                case "IL": { return StateCode.Illinois.GetEnumDescription(); }
                case "IN": { return StateCode.Indiana.GetEnumDescription(); }
                case "KS": { return StateCode.Kansas.GetEnumDescription(); }
                case "KY": { return StateCode.Kentucky.GetEnumDescription(); }
                case "LA": { return StateCode.Louisiana.GetEnumDescription(); }
                case "MA": { return StateCode.Massachusetts.GetEnumDescription(); }
                case "MD": { return StateCode.Maryland.GetEnumDescription(); }
                case "ME": { return StateCode.Maine.GetEnumDescription(); }
                case "MI": { return StateCode.Michigan.GetEnumDescription(); }
                case "MN": { return StateCode.Minnesota.GetEnumDescription(); }
                case "MO": { return StateCode.Missouri.GetEnumDescription(); }
                case "MS": { return StateCode.Mississippi.GetEnumDescription(); }
                case "MT": { return StateCode.Montana.GetEnumDescription(); }
                case "NC": { return StateCode.NorthCarolina.GetEnumDescription(); }
                case "ND": { return StateCode.NorthDakota.GetEnumDescription(); }
                case "NE": { return StateCode.Nebraska.GetEnumDescription(); }
                case "NH": { return StateCode.NewHampshire.GetEnumDescription(); }
                case "NJ": { return StateCode.NewJersey.GetEnumDescription(); }
                case "NM": { return StateCode.NewMexico.GetEnumDescription(); }
                case "NV": { return StateCode.Nevada.GetEnumDescription(); }
                case "NY": { return StateCode.NewYork.GetEnumDescription(); }
                case "OH": { return StateCode.Ohio.GetEnumDescription(); }
                case "OK": { return StateCode.Oklahoma.GetEnumDescription(); }
                case "OR": { return StateCode.Oregon.GetEnumDescription(); }
                case "PA": { return StateCode.Pennsylvania.GetEnumDescription(); }
                case "RI": { return StateCode.RhodeIsland.GetEnumDescription(); }
                case "SC": { return StateCode.SouthCarolina.GetEnumDescription(); }
                case "SD": { return StateCode.SouthDakota.GetEnumDescription(); }
                case "TN": { return StateCode.Tennessee.GetEnumDescription(); }
                case "TX": { return StateCode.Texas.GetEnumDescription(); }
                case "UT": { return StateCode.Utah.GetEnumDescription(); }
                case "VA": { return StateCode.Virginia.GetEnumDescription(); }
                case "VT": { return StateCode.Vermont.GetEnumDescription(); }
                case "WA": { return StateCode.Washington.GetEnumDescription(); }
                case "WI": { return StateCode.Wisconsin.GetEnumDescription(); }
                case "WV": { return StateCode.WestVirginia.GetEnumDescription(); }
                case "WY": { return StateCode.Wyoming.GetEnumDescription(); }
                // Canada
                case "AB": { return StateCode.Alberta.GetEnumDescription(); }
                case "BC": { return StateCode.BritishColumbia.GetEnumDescription(); }
                case "MB": { return StateCode.Manitoba.GetEnumDescription(); }
                case "NB": { return StateCode.NewBrunswick.GetEnumDescription(); }
                // case "NL": { return StateCode.NewfoundlandAndLabrador.GetEnumDescription(); }    Duplicated stateCode in Mexico, so fix it another way
                case "NS": { return StateCode.NovaScotia.GetEnumDescription(); }
                case "NT": { return StateCode.NorthwestTerritories.GetEnumDescription(); }
                case "NU": { throw new ExternalServiceException(ExternalServiceMessages.UnsupportedStateCodeExceptionMessage(), Common.Coyote.Types.ExternalService.InternetTruckStop); }
                case "ON": { return StateCode.Ontario.GetEnumDescription(); }
                case "PE": { return StateCode.PrinceEdwardIsland.GetEnumDescription(); }
                case "QC": { return StateCode.Quebec.GetEnumDescription(); }
                case "SK": { return StateCode.Saskatchewan.GetEnumDescription(); }
                case "YT": { return StateCode.Yukon.GetEnumDescription(); }
                // Mexico
                case "AG": { return StateCode.Aguascalientes.GetEnumDescription(); }
                case "BJ": { return StateCode.BajaCaliforniaNorte.GetEnumDescription(); }
                case "BS": { return StateCode.BajaCaliforniaSur.GetEnumDescription(); }
                case "CP": { return StateCode.Campeche.GetEnumDescription(); }
                case "CH": { return StateCode.Chiapas.GetEnumDescription(); }
                case "CI": { return StateCode.Chihuahua.GetEnumDescription(); }
                case "CU": { return StateCode.Coahuila.GetEnumDescription(); }
                case "CL": { return StateCode.Colima.GetEnumDescription(); }
                case "DF": { return StateCode.DistritoFederal.GetEnumDescription(); }
                case "DG": { return StateCode.Durango.GetEnumDescription(); }
                case "GJ": { return StateCode.Guanajuato.GetEnumDescription(); }
                case "GR": { return StateCode.Guerrero.GetEnumDescription(); }
                case "HG": { return StateCode.Hidalgo.GetEnumDescription(); }
                case "JA": { return StateCode.Jalisco.GetEnumDescription(); }
                case "EM": { return StateCode.Mexico.GetEnumDescription(); }
                case "MH": { return StateCode.Michoacan.GetEnumDescription(); }
                case "MR": { return StateCode.Morelos.GetEnumDescription(); }
                case "NA": { return StateCode.Nayarit.GetEnumDescription(); }
                // case "NL": { return StateCode.NuevoLeon.GetEnumDescription(); }  Duplicated stateCode in Canada, so fix it another way
                case "OA": { return StateCode.Oaxaca.GetEnumDescription(); }
                case "PU": { return StateCode.Puebla.GetEnumDescription(); }
                case "QA": { return StateCode.Queretaro.GetEnumDescription(); }
                case "QR": { return StateCode.Quintanaroo.GetEnumDescription(); }   // LocationStateId = 379 is being ignored treated like LocationStateId = 311 (since both have the same country)
                case "SL": { return StateCode.SanLuisPotosi.GetEnumDescription(); }
                case "SI": { return StateCode.Sinaloa.GetEnumDescription(); }
                case "SO": { return StateCode.Sonora.GetEnumDescription(); }
                case "TA": { return StateCode.Tabasco.GetEnumDescription(); }
                case "TM": { return StateCode.Tamaulipas.GetEnumDescription(); }
                case "TL": { return StateCode.Tlaxcala.GetEnumDescription(); }
                case "VZ": { return StateCode.Veracruz.GetEnumDescription(); }
                case "YC": { return StateCode.Yucatan.GetEnumDescription(); }
                case "ZT": { return StateCode.Zacatecas.GetEnumDescription(); }
                //case "QR": { return StateCode.Queretaro.GetEnumDescription(); } LocationStateId = 379 is being ignored treated like LocationStateId = 311 (since both have the same country)
                // US territories
                case "PR": { throw new ExternalServiceException(ExternalServiceMessages.UnsupportedStateCodeExceptionMessage(), Common.Coyote.Types.ExternalService.InternetTruckStop); }
                // Duplicates
                case "NL": { return ResolveNLStateCode(country); }
                default: { throw new ExternalServiceException(ExternalServiceMessages.UnknownStateCodeExceptionMessage(), Common.Coyote.Types.ExternalService.InternetTruckStop); }
            }
        }

        private static string ResolveNLStateCode(LocationCountry country)
        {
            if (country == null) { throw new ExternalServiceException(ExternalServiceMessages.UnknownStateCodeExceptionMessage(), Common.Coyote.Types.ExternalService.InternetTruckStop); }

            switch (country.Name)
            {
                case "Canada": { return StateCode.NewfoundlandAndLabrador.GetEnumDescription(); }
                case "Mexico": { return StateCode.NuevoLeon.GetEnumDescription(); }
                default: { throw new ExternalServiceException(ExternalServiceMessages.UnknownStateCodeExceptionMessage(), Common.Coyote.Types.ExternalService.InternetTruckStop); }
            }
        }

        public static string FormatCountry(LocationCountry country)
        {
            if (country == null) { throw new ExternalServiceException(ExternalServiceMessages.InvalidCountryExceptionMessage(), Common.Coyote.Types.ExternalService.InternetTruckStop); }
            if (string.IsNullOrEmpty(country.Name)) { throw new ExternalServiceException(ExternalServiceMessages.InvalidCountryExceptionMessage(), Common.Coyote.Types.ExternalService.InternetTruckStop); }

            switch (country.Name)
            {
                case "United States": { return Country.UnitedStates.GetEnumDescription(); }
                case "Canada": { return Country.Canada.GetEnumDescription(); }
                case "Mexico": { return Country.Mexico.GetEnumDescription(); }
                default: { throw new ExternalServiceException(ExternalServiceMessages.UnknownCountryExceptionMessage(country.Name), Common.Coyote.Types.ExternalService.InternetTruckStop); }
            }
        }

        public static string FormatPaymentAmount(decimal? rate)
        {
            if (rate.HasValue)
            {
                if (rate.Value > int.MaxValue) { throw new ExternalServiceException(ExternalServiceMessages.InvalidRateExceptionMessage(), Common.Coyote.Types.ExternalService.InternetTruckStop); }

                // Per spec: string PaymentAmount – an integer dollar amount, containing only digits 0-9, and no other punctuation. 
                // Non-conforming values will be set to an empty-string (ie, will display as blank on the Load board).
                return ((int)rate.Value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            }

            return string.Empty;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public static string FormatEquipment(string equipment)
        {
            if (string.IsNullOrEmpty(equipment)) { throw new ExternalServiceException(ExternalServiceMessages.InvalidEquipmentExceptionMessage(), Common.Coyote.Types.ExternalService.InternetTruckStop); }

            // This is based on the equipment drop down within the Load (select * from dbo.TypeDetails where TypeCategoryID=107)
            switch (equipment)
            {
                case "V": { return EquipmentCode.Van.GetEnumDescription(); }
                case "R": { return EquipmentCode.Reefer.GetEnumDescription(); }
                case "F": { return EquipmentCode.Flatbed.GetEnumDescription(); }
                case "V,R": { return EquipmentCode.VanOrReefer.GetEnumDescription(); }
                case "V,F": { return EquipmentCode.FlatbedOrVan.GetEnumDescription(); }
                case "V,R,F": { return EquipmentCode.VanFlatbedOrReefer.GetEnumDescription(); }
                case "E": { return EquipmentCode.Van.GetEnumDescription(); }
                case "T": { return EquipmentCode.Van.GetEnumDescription(); }
                case "B": { return EquipmentCode.Van.GetEnumDescription(); }
                case "Q": { return EquipmentCode.Van.GetEnumDescription(); }
                case "E,M": { return EquipmentCode.Van.GetEnumDescription(); }
                case "E,M,F": { return EquipmentCode.Van.GetEnumDescription(); }
                case "T,E,M": { return EquipmentCode.Van.GetEnumDescription(); }
                case "T,E,Q": { return EquipmentCode.Van.GetEnumDescription(); }
                case "T,E,Q,M": { return EquipmentCode.Van.GetEnumDescription(); }
                case "T,E,Q,M,F": { return EquipmentCode.Van.GetEnumDescription(); }
                case "T,E,Q,V,R,M,F": { return EquipmentCode.Van.GetEnumDescription(); }
                case "T,E,V,R,M,F": { return EquipmentCode.Van.GetEnumDescription(); }
                case "M": { return EquipmentCode.Van.GetEnumDescription(); }
                case "C": { return EquipmentCode.ContainerTrailer.GetEnumDescription(); }
                case "Z": { return EquipmentCode.Tanker.GetEnumDescription(); }
                case "DD": { return EquipmentCode.DoubleDrop.GetEnumDescription(); }
                case "SD": { return EquipmentCode.StepDeck.GetEnumDescription(); }
                case "DF": { return EquipmentCode.Flatbed.GetEnumDescription(); }
                case "SS": { return EquipmentCode.CurtainVan.GetEnumDescription(); }
                case "CNRU": { return EquipmentCode.Van.GetEnumDescription(); }
                case "CPPU": { return EquipmentCode.Van.GetEnumDescription(); }
                case "CSXU": { return EquipmentCode.Van.GetEnumDescription(); }
                case "EMHU": { return EquipmentCode.Van.GetEnumDescription(); }
                case "EMPU": { return EquipmentCode.Van.GetEnumDescription(); }
                case "EMWU": { return EquipmentCode.Van.GetEnumDescription(); }
                case "EPTY": { return EquipmentCode.Van.GetEnumDescription(); }
                case "PACU": { return EquipmentCode.Van.GetEnumDescription(); }
                case "W": { return EquipmentCode.VentedVan.GetEnumDescription(); }
                case "G": { return EquipmentCode.RemovableGooseNeckAndMultiAxleHeavyHaulers.GetEnumDescription(); }
                case "K": { return EquipmentCode.Conestoga.GetEnumDescription(); }
                case "P": { return EquipmentCode.PowerOnly.GetEnumDescription(); }
                case "DR": { return EquipmentCode.RefrigeratedCarrierWithPlantDecking.GetEnumDescription(); }
                case "H": { return EquipmentCode.Van.GetEnumDescription(); }
                case "R,W": { return EquipmentCode.Reefer.GetEnumDescription(); }
                case "F,SD": { return EquipmentCode.FlatOrStepDeck.GetEnumDescription(); }
                case "G,SD": { return EquipmentCode.RemovableGooseNeckAndMultiAxleHeavyHaulers.GetEnumDescription(); }
                case "FWS": { return EquipmentCode.FlatbedWithSides.GetEnumDescription(); }
                case "F,FWS": { return EquipmentCode.FlatbedWithSides.GetEnumDescription(); }
                case "BT": { return EquipmentCode.WalkingFloor.GetEnumDescription(); }
                case "T,E,M,R": { return EquipmentCode.Van.GetEnumDescription(); }
                case "T,E,V,R": { return EquipmentCode.Van.GetEnumDescription(); }
                case "T,E": { return EquipmentCode.Van.GetEnumDescription(); }
                case "ZS": { return EquipmentCode.Tanker.GetEnumDescription(); }
                case "XM": { return EquipmentCode.Van.GetEnumDescription(); }
                case "MV": { return EquipmentCode.Van.GetEnumDescription(); }
                case "MF": { return EquipmentCode.MaxiOrDoubleFlatTrailers.GetEnumDescription(); }
                case "FM": { return EquipmentCode.Van.GetEnumDescription(); }
                case "DV": { return EquipmentCode.Van.GetEnumDescription(); }
                case "R,F": { return EquipmentCode.Reefer.GetEnumDescription(); }
                case "LALL": { return EquipmentCode.LandollFlatbed.GetEnumDescription(); }
                case "F,SD,K": { return EquipmentCode.FlatOrStepDeck.GetEnumDescription(); }
                case "F,SD,K,FWS": { return EquipmentCode.FlatOrStepDeck.GetEnumDescription(); }
                case "SD,G": { return EquipmentCode.RemovableGooseNeckAndMultiAxleHeavyHaulers.GetEnumDescription(); }
                case "F,K": { return EquipmentCode.Flatbed.GetEnumDescription(); }
                case "K,SD": { return EquipmentCode.StepDeck.GetEnumDescription(); }
                case "F,K,FWS": { return EquipmentCode.FlatOrStepDeck.GetEnumDescription(); }
                case "F,SD,G": { return EquipmentCode.FlatOrStepDeck.GetEnumDescription(); }
                case "SDL": { return EquipmentCode.StepDeckWithLoadingRamps.GetEnumDescription(); }
                case "A": { return EquipmentCode.AutoCarrier.GetEnumDescription(); }
                default: { throw new ExternalServiceException(ExternalServiceMessages.UnknownEquipmentExceptionMessage(equipment), Common.Coyote.Types.ExternalService.InternetTruckStop); }
            }
        }

        public static string FormatWeight(decimal weight)
        {
            if (weight < 0) { throw new ExternalServiceException(ExternalServiceMessages.InvalidWeightExceptionMessage(), Common.Coyote.Types.ExternalService.InternetTruckStop); }
            if (weight >= 1000000) { throw new ExternalServiceException(ExternalServiceMessages.InvalidWeightExceptionMessage(), Common.Coyote.Types.ExternalService.InternetTruckStop); }

            return weight.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string FormatNumberOfStops(int numberOfStops)
        {
            const int maximumNumberOfStops = 99;

            if (numberOfStops < 0) { throw new ExternalServiceException(ExternalServiceMessages.InvalidNumberOfStopsExceptionMessage(maximumNumberOfStops), Common.Coyote.Types.ExternalService.InternetTruckStop); }
            if (numberOfStops > maximumNumberOfStops) { throw new ExternalServiceException(ExternalServiceMessages.InvalidNumberOfStopsExceptionMessage(maximumNumberOfStops), Common.Coyote.Types.ExternalService.InternetTruckStop); }

            return numberOfStops.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}