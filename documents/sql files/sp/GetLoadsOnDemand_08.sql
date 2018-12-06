/*
// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2012 - 2018
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////
*/

CREATE PROCEDURE [dbo].[GetLoadsOnDemand_08]
(
        @LoadProgressTypeLowerTL INT,
        @LoadProgressTypeUpperTL INT,
        @LoadProgressTypeLowerIMDL INT,
        @LoadProgressTypeUpperIMDL INT,
        @StartDate AS DATETIME,
        @EndDate AS DATETIME,
        @CompanyIDList AS VARCHAR(100) = '1',
        @CultureInfoName NCHAR(5) = 'en-US'
)

AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
    SET NOCOUNT ON;

	DECLARE @StartDt AS DATETIME
		, @EndDt AS DATETIME;

	SET @StartDt = CAST(CAST(@StartDate AS DATE) AS DATETIME)
	SET @EndDt = DATEADD(MINUTE, 1439, CAST(CAST(@EndDate AS DATE) AS DATETIME))
 	

	BEGIN TRY

	 	CREATE TABLE #Results
		(
			LoadID INT, --[000]
			Mode INT,
			Region VARCHAR(50),
			LDDate DATETIME,
			CustomerID INT,
			Cust VARCHAR(50),
			CustRepID INT,
			CustRep VARCHAR(50),
			CustRepGroupID INT,
			CustRepOpsOrigin INT,
			OriginCityID INT, --[010]

			OriginCity VARCHAR(100), 
			OriginCityName VARCHAR(50),
			OriginState VARCHAR(5),
			OrigLat FLOAT,
			OrigLong FLOAT,
			Dist DECIMAL(8,2),
			DestCityID INT,
			DestCity VARCHAR(100),
			DestCityName VARCHAR(50), 
			DestState VARCHAR(5),--[020]

			DestLat FLOAT,
			DestLong FLOAT,
			Equipment VARCHAR(50),
            LDReadyDt DATETIME ,
            LoadByDt DATETIME ,
            ReadyHours VARCHAR(25),
            Appointment INT ,
            LoadDtTime VARCHAR(25) ,
            [Hours] VARCHAR(25), 
			DelOpen DATETIME ,--[030]

            DelBy DATETIME ,
            DelHours VARCHAR(50) ,
            Appointment2 INT ,
			Rate MONEY,
            RateLevel INT,
            Stops SMALLINT, 
            CarrierID INT ,
            Carrier VARCHAR(50),
            CarrierRep VARCHAR(50),
            CarrRepID INT, --[040]

            ETADtTime VARCHAR(50),
            MTDtTime DATETIME ,
            MTCity VARCHAR(100) ,
            ToDist DECIMAL(8,2),
            Booked DATETIME, 
            DispDtTime VARCHAR(25),
 			ExpectedPickupDateTime DATETIME ,
            ExpectedDeliveryDateTime DATETIME ,
            BookByUserID INT ,
            CarrPhone VARCHAR(50) ,--[050]
	
	        [Weight] DECIMAL(12,3) ,
            ProgressType INT , 
            StateType INT ,
            IsRolled BIT ,
			EquipType VARCHAR(25) ,
			EquipLength DECIMAL(5,2) ,
            EquipWidth DECIMAL(5,2) ,
            EquipHeight DECIMAL(5,2) ,
			PickUpDateTime DATETIME ,
            LoadTrackingCityState VARCHAR(50) ,--[060]

			LoadTrackingNotes VARCHAR(250) , 
			LoadTrackingCreateDate DATETIME ,
            LoadTrackingNextCallDate DATETIME ,
            LoadTrackingAction INT ,
		    AssignmentRepID INT ,
            AssignmentGroupID INT ,
			AssignmentConferenceID INT,
			FacilityIDShipper INT , 
            FacilityIDConsignee INT ,
            OriginDrayman VARCHAR(50) ,--[070]

			OriginRamp VARCHAR(50) ,
            LineHaulIntermodalVendor VARCHAR(50) ,
            DestRamp VARCHAR(50) ,
            DestDrayman VARCHAR(50) ,
            PlanType INT ,
            IMDLOriginHrsCode SMALLINT ,
            IMDLOriginDateCode SMALLINT ,
            IMDLDestHrsCode SMALLINT , 
            IMDLDestDateCode SMALLINT ,
			IMDLLHICode SMALLINT , --[080]

            IMDLORampCode SMALLINT ,
            IMDLDRampCode SMALLINT ,
			IMDLODrayCode SMALLINT ,
			IMDLDDrayCode SMALLINT ,
			EquipmentCode SMALLINT ,
			ShipperName VARCHAR(50) ,
			ConsigneeName VARCHAR(50) ,
			ContainerNumber VARCHAR(60) , 
            IMDLEquipment VARCHAR(50) ,
            TrainReservationNumber VARCHAR(100) ,--[090]

            ScheduleOpenDateShipper DATETIME ,
            ScheduleCloseDateShipper DATETIME ,
            ArriveDateTimeShipper DATETIME ,
            DepartDateTimeShipper DATETIME ,
			ScheduleOpenDateConsignee DATETIME,
            ScheduleCloseDateConsignee DATETIME , 
            IngateDate DATETIME ,
            ETADate DATETIME ,
            NotificationDate DATETIME ,
            OutGateDate DATETIME ,--[100]        

            ScheduleCloseTimeFirstDelivery DATETIME ,
            LHIConfirmationSent BIT ,
           	LoadInd INT ,
            Liquor BIT ,
            TWIC BIT ,
            Hazmat BIT ,
			ConferenceID INT,
			[Source] INT,
			CreateDate DATETIME,
			RoutingRankType INT, --[110]

            OpsGroup INT,
            BookedNonQualifiedCarrier BIT,
            IMDLOriginRampGroup VARCHAR(50),
            IMDLDestRampGroup VARCHAR(50),
			InboundTrackingMethodType SMALLINT, 
			OriginAppointmentRequest BIT,
			DestinationAppointmentRequest BIT,
			MaxPay MONEY,
			LoadType INT,
			Division INT, --[120]

			Cost MONEY,
			TMSPrivateCarrier BIT,
			HRHV BIT,
			SalesStatusType INT,
			MinTemp DECIMAL(6,2),
			MaxTemp DECIMAL(6,2),
			HOT BIT,
			ProtectFromFreeze BIT,
			Duraplate BIT,
			ExchangeRateImportId INT,--[130]

			MainLoadCarrierCurrencyTypeId INT,
			LoadValue MONEY,
			MainLoadCustomerCurrencyTypeId INT,
			LaneType VARCHAR(100),
			OriginAlpha2CountryCode VARCHAR(5),
			DestAlpha2CountryCode VARCHAR(5),
			ShipmentType INT,
			Blind BIT, --'LB.Concealed'
			BondedCarrier BIT, 
			FloodRelief BIT, --[140]

			Government BIT,
			Seal BIT,
			OverDimension BIT,
			CTPAT BIT,
			TankerEndorsement BIT,
			Team BIT, 
			DriverCellRequired BIT, 
			RateQuoteID INT,
			CarrRepGroupID INT --[149]
		);

		CREATE TABLE #RateQuotes
		(
			LoadID INT,
			RateQuoteID INT,
			RateQuoteOriginFacilityName VARCHAR(50),  
			RateQuoteDestinationFacilityName VARCHAR(50), 
			RateQuoteDestinationCityState VARCHAR(50), 
			RateQuoteOriginState VARCHAR(3),  
			RateQuoteDestinationState VARCHAR(3),
			OriginRegion VARCHAR(50), 
			DestRegion VARCHAR(50), 
			RateQuoteOriginZipCode VARCHAR(50),
			RateQuoteDestinationZipCode VARCHAR(50)  
		);

		CREATE TABLE #MainCutomerOpsReps
         (
			LoadId int,
			MainCustomerReps varchar(2000)
         );

		INSERT INTO #Results 
		(
			LoadID, Mode, Region, LDDate, CustomerID, Cust, CustRepID, CustRep, CustRepGroupID, CustRepOpsOrigin,
	       	 OriginCityID, OriginCity, OriginCityName, OriginState, OrigLat, OrigLong, Dist, DestCityID, DestCity,
			DestCityName, DestState, DestLat, DestLong, Equipment, LDReadyDt, LoadByDt, ReadyHours, Appointment, LoadDtTime,
            [Hours],   DelOpen, DelBy, DelHours, Appointment2, Rate, RateLevel,
            Stops, CarrierID, Carrier, CarrierRep, CarrRepID, ETADtTime, MTDtTime, MTCity, ToDist,
			Booked, DispDtTime,   ExpectedPickupDateTime, ExpectedDeliveryDateTime, BookByUserID, CarrPhone, [Weight],
			ProgressType, StateType, IsRolled, EquipType, EquipLength, EquipWidth, EquipHeight, PickUpDateTime,  LoadTrackingCityState,
			LoadTrackingNotes,  LoadTrackingCreateDate, LoadTrackingNextCallDate, LoadTrackingAction,  AssignmentRepID, AssignmentGroupID, AssignmentConferenceID,
			FacilityIDShipper, FacilityIDConsignee, OriginDrayman, OriginRamp, LineHaulIntermodalVendor, DestRamp, DestDrayman, PlanType, IMDLOriginHrsCode, IMDLOriginDateCode,
			IMDLDestHrsCode, IMDLDestDateCode, IMDLLHICode, IMDLORampCode, IMDLDRampCode, IMDLODrayCode, IMDLDDrayCode, EquipmentCode, ShipperName, ConsigneeName,
			ContainerNumber, IMDLEquipment, TrainReservationNumber, ScheduleOpenDateShipper, ScheduleCloseDateShipper,   ArriveDateTimeShipper, DepartDateTimeShipper, ScheduleOpenDateConsignee,
			ScheduleCloseDateConsignee, IngateDate, ETADate, NotificationDate, OutGateDate,ScheduleCloseTimeFirstDelivery,     
			 LHIConfirmationSent,LoadInd, Liquor, TWIC,Hazmat, ConferenceID, [Source], CreateDate, RoutingRankType, OpsGroup, BookedNonQualifiedCarrier, IMDLOriginRampGroup, IMDLDestRampGroup, 
			InboundTrackingMethodType, OriginAppointmentRequest, DestinationAppointmentRequest, MaxPay, LoadType, Division, Cost,  TMSPrivateCarrier, HRHV,
			SalesStatusType,  MinTemp, MaxTemp, HOT, ProtectFromFreeze,    Duraplate,
			 ExchangeRateImportId, MainLoadCarrierCurrencyTypeId, LoadValue, MainLoadCustomerCurrencyTypeId, LaneType, OriginAlpha2CountryCode, DestAlpha2CountryCode, ShipmentType, Blind, 
			BondedCarrier, FloodRelief, Government, Seal, OverDimension, CTPAT, TankerEndorsement,Team,DriverCellRequired,RateQuoteID,CarrRepGroupID
		)
		SELECT
			LoadID, Mode, Region, LDDate, CustomerID, Cust, CustRepID, CustRep, CustRepGroupID, CustRepOpsOrigin,
			 OriginCityID, OriginCity, OriginCityName, OriginState, OrigLat, OrigLong, Dist, DestCityID, DestCity,
			DestCityName, DestState, DestLat, DestLong, Equipment, LDReadyDt, LoadByDt, ReadyHours, Appointment, LoadDtTime,
			[Hours],    DelOpen, DelBy, DelHours, Appointment2, Rate, RateLevel,
	        Stops, CarrierID, Carrier, CarrierRep, CarrRepID,  ETADtTime, MTDtTime, MTCity, ToDist,
			Booked, DispDtTime,   ExpectedPickupDateTime, ExpectedDeliveryDateTime, BookByUserID,  CarrPhone, [Weight],
			ProgressType, StateType, IsRolled, EquipType, EquipLength, EquipWidth, EquipHeight, PickUpDateTime,  LoadTrackingCityState,
			LoadTrackingNotes,  LoadTrackingCreateDate, LoadTrackingNextCallDate, LoadTrackingAction,  AssignmentRepID, AssignmentGroupID, AssignmentConferenceID,
			FacilityIDShipper, FacilityIDConsignee, OriginDrayman, OriginRamp, LineHaulIntermodalVendor, DestRamp, DestDrayman, PlanType, IMDLOriginHrsCode, IMDLOriginDateCode,
			IMDLDestHrsCode, IMDLDestDateCode, IMDLLHICode, IMDLORampCode, IMDLDRampCode, IMDLODrayCode, IMDLDDrayCode, EquipmentCode, ShipperName, ConsigneeName,
			ContainerNumber, IMDLEquipment, TrainReservationNumber, ScheduleOpenDateShipper, ScheduleCloseDateShipper,   ArriveDateTimeShipper, DepartDateTimeShipper, ScheduleOpenDateConsignee,
			ScheduleCloseDateConsignee, IngateDate, ETADate, NotificationDate, OutGateDate,ScheduleCloseTimeFirstDelivery,     
		    LHIConfirmationSent, LoadInd, Liquor, TWIC,Hazmat, ConferenceID, [Source], CreateDate, RoutingRankType, OpsGroup, BookedNonQualifiedCarrier, IMDLOriginRampGroup, IMDLDestRampGroup, 
			InboundTrackingMethodType, OriginAppointmentRequest, DestinationAppointmentRequest, MaxPay, LoadType, Division, Cost,  TMSPrivateCarrier, HRHV,
			SalesStatusType,  MinTemp, MaxTemp, HOT, ProtectFromFreeze,    Duraplate,
			 ExchangeRateImportId, MainLoadCarrierCurrencyTypeId, LoadValue, MainLoadCustomerCurrencyTypeId, LaneType, OriginAlpha2CountryCode, DestAlpha2CountryCode, ShipmentType, Concealed, 
			BondedCarrier, FloodRelief, Govt, Seal, OverDimension, CTPAT, TankerEndorsement,Team,DriverCellRequired,RateQuoteID,CarrRepGroupID
		FROM LoadBoard
		WHERE
			StateType != 2 AND StateType != 6
			AND LDDate BETWEEN @StartDt AND @EndDt
			AND CompanyID IN (SELECT item FROM fcn_ListToIntTable(@CompanyIDList,','))
			AND 
			(
				(Mode != 3 AND ProgressType BETWEEN @LoadProgressTypeLowerTL AND @LoadProgressTypeUpperTL)
				OR
				(Mode = 3 AND ProgressType BETWEEN @LoadProgressTypeLowerIMDL AND @LoadProgressTypeUpperIMDL)
			);
		INSERT INTO #MainCutomerOpsReps
		(
			LoadId,
			MainCustomerReps
		)

		SELECT x.LoadID, STUFF((SELECT ',' + EMP.code
				FROM [dbo].[LoadRep] LREP
				INNER JOIN [dbo].[Employee] EMP ON LREP.LoadID = LC.LoadID
				AND  LREP.EntityType = 12  -- LoadCustomer Entity
				AND  LREP.EntityID = LC.ID
				AND  EMP.ID=LREP.EmployeeID
				AND LC.Main= 1
				FOR XML PATH('')), 1, 1,'') MainCustomerReps 
		FROM [LoadCustomer] LC INNER JOIN #Results x ON LC.LoadID=x.LoadID 
		WHERE LC.Main= 1 AND X.Mode IN(1,2);

		INSERT INTO #RateQuotes (LoadID, RateQuoteID)
		SELECT LoadID, RateQuoteID FROM #Results;

		UPDATE #RateQuotes
		SET RateQuoteOriginFacilityName = RQ.OriginFacilityName, 
			RateQuoteDestinationFacilityName = RQ.DestFacilityName, 
			RateQuoteDestinationCityState = RQ.DestinationCityState,
			RateQuoteOriginState = OST.[Code],		
			RateQuoteDestinationState = DST.[Code],
			OriginRegion = CRO.[Code],
			DestRegion = CRD.[CODE],
			RateQuoteOriginZipCode = RQ.OriginZipCode,
			RateQuoteDestinationZipCode = RQ.DestinationZipCode
		FROM RateQuote RQ
			JOIN #RateQuotes x ON RQ.ID = x.RateQuoteID
			LEFT JOIN LocationState OST ON RQ.OriginStateID = OST.LocationStateId
			LEFT JOIN LocationState DST ON RQ.DestinationStateID = DST.LocationStateId
			LEFT JOIN dbo.CustomerRegion CRO ON RQ.OriginCustomerRegionId = CRO.CustomerRegionId
			LEFT JOIN dbo.CustomerRegion CRD ON RQ.DestinationCustomerRegionId = CRD.CustomerRegionId;

		
		WITH CTE_CarrFscAmount AS
		(	 
			SELECT  LoadId ,
					EntityId,
					SUM(Per) AS TotalFscAmount 
			FROM    [dbo].LoadRateDetail 
			WHERE   EntityType = 13 --Load Carrier
					AND EDIDataElementCode = '405' -- Fuel Surcharge
			GROUP BY LoadID,EntityId
		)

       SELECT 
			x.*,--[000-149]
			rq.RateQuoteOriginFacilityName,
			rq.RateQuoteDestinationFacilityName,--[151]

			rq.RateQuoteDestinationCityState,
			rq.RateQuoteOriginState, 
			rq.RateQuoteDestinationState, 
			rq.OriginRegion, 
			rq.DestRegion,
			rq.RateQuoteOriginZipCode,
			rq.RateQuoteDestinationZipCode,
			LR.[MainCustomerReps] CustomerOpsRep,
	        CONVERT(NUMERIC(19, 2), ( SELECT CASE Dist WHEN 0 THEN 0 ELSE x.Rate / Dist END), 1) RatePerMile,          
			CONVERT(NUMERIC(19, 2), ( SELECT CASE Dist WHEN 0 THEN 0 ELSE x.MaxPay / Dist END), 1) MaxPayPerMile, --[161]

            CONF.[Description] ConfIdName,
			([EquipType] + ',' + CONVERT (VARCHAR,CONVERT(DOUBLE PRECISION,[EquipLength]))) LoadEQType, 
			[dbo].[fnCulturalFormatToShortDateTimeWithComma]([ScheduleCloseTimeFirstDelivery], [ScheduleCloseTimeFirstDelivery], @CultureInfoName) DelApptDtTm,
			CUS.[SafetyRating] SafetyRating,
			LRD.[Per] CustomerFsc,
			(
				CASE YEAR(LS1.ScheduleOpenTime) 
					WHEN 1753 THEN
						[dbo].[fnCulturalFormatToShortDateTimeWithPipe](LS1.ReadyDate, LS1.OpenTime, @CultureInfoName)
					ELSE
						[dbo].[fnCulturalFormatToShortDateTimeWithPipe](LS1.ScheduleOpenTime, LS1.ScheduleOpenTime, @CultureInfoName)
					END 
				+
				CASE LS1.ScheduleType
					WHEN 1 THEN ' A '
					WHEN 2 THEN ' N '
					WHEN 3 THEN ' O '
                END 
			) PickUpApptDateTime,
           	CASE LCARR.RouteType WHEN 2 THEN LCARR.[BookNumber] END BookNumber, 
            LATTR.[HVHRPlus],
			LATTR.TradeShow,
			ELP.[IsActive] LoadPostedExternally,--[171]

			ELP.[Rate] PostedRate,
			CASE YEAR(x.MTDtTime) WHEN 1753 THEN '' ELSE LEFT(CONVERT(VARCHAR(8),x.MTDtTime, 8), 5) END EmptyTime,
			LP.[IsPriority],
			LATTR.[IntraMexico],
			CUS.[Division] MainCustomerDivisionId,
			CTECarrFsc.TotalFscAmount AS CarrierFSC , --0 AS CarrierFSC,
			LCARR.ExpectedEquipmentType TruckEQ,
			LREP1.[EmployeeID] CarrierOpsRepID, 
			(LCARR.ExpectedEquipmentType + ',' + CONVERT (VARCHAR,CONVERT(DOUBLE PRECISION,LCARR.ExpectedEquipmentLength))) TruckEquipType,--[180]
			
			LCARR.Trailer CarrierTrailerNumber,
			Incidents.[Count],
			LS1.ZipCode OrigZip,
			LS2.ZipCode DestZip,
			CARR.Division CarrDiv,
			LATTR.AirRide,
			LATTR.DisasterRelief,
			LATTR.EnglishSpeaking, 
			LATTR.ExitPass,
			LATTR.PalletJack,--[190]

			LATTR.PowerOnly,
			LATTR.PPE,
			LATTR.TCR,
			LATTR.TrailerControl,
			LATTR.CoyoteGO,
			LATTR.DoubleTrailer,
			LATTR.TripleTrailer, 
			LATTR.Drayage,
			LATTR.UIIA --[199]
	  FROM #Results x

		JOIN #RateQuotes rq ON x.LoadID = rq.LoadID
	    LEFT JOIN #MainCutomerOpsReps LR on x.LoadID=LR.LoadId
		LEFT JOIN dbo.Conference CONF ON x.AssignmentConferenceID = CONF.ID
        LEFT JOIN dbo.LoadCustomer LCUS ON x.LoadID = LCUS.LoadID AND LCUS.Main = 1
        LEFT JOIN dbo.Customer CUS ON LCUS.CustomerID = CUS.ID
		LEFT JOIN dbo.LoadRateDetail LRD ON x.LoadID = LRD.LoadID AND LRD.EntityID = LCUS.ID AND LRD.EDIDataElementCode = '405' --Fuel Surcharge
		LEFT JOIN dbo.LoadStop AS LS1 ON x.LoadID = LS1.LoadID AND x.FacilityIDShipper = LS1.FacilityID
		LEFT JOIN dbo.LoadStop AS LS2 ON x.LoadID = LS2.LoadID AND x.FacilityIDConsignee = LS2.FacilityID
		LEFT JOIN dbo.LoadAttributes LATTR ON x.LoadID = LATTR.LoadID
		LEFT JOIN dbo.LoadCarrier LCARR 
			JOIN Carrier CARR ON LCARR.CarrierID = CARR.ID 
		ON x.LoadID = LCARR.LoadID AND LCARR.Main=1
        LEFT JOIN [dbo].[ExternalLoadPost] ELP ON x.LoadID = ELP.LoadID AND  ELP.[IsActive] = 1
			AND 1 IN (ELP.[ITSPostStatus], ELP.[DATPostStatus], ELP.[PostEverywherePostStatus])
		LEFT JOIN dbo.LoadPriorityXref LP ON x.LoadID = LP.LoadID AND LP.PriorityTypeId = 1
		LEFT JOIN CTE_CarrFscAmount CTECarrFsc ON  x.LoadID = CTECarrFsc.LoadID AND CTECarrFsc.EntityId = LCARR.ID
		LEFT JOIN dbo.LoadRep LREP1 ON x.LoadID = LREP1.LoadID AND LREP1.EntityID = LCARR.ID
			AND  LREP1.EntityType = 13 /*LoadCarrier*/ AND  LREP1.RepType = 4 /*Carrier Ops rep*/
			CROSS APPLY
			(
				SELECT COUNT(1) [Count]
				FROM LoadIncident
				WHERE LoadID = x.LoadID
				AND YEAR(ResolvedDate) = 1753
				AND ResolvedBy = 0
			) Incidents

			ORDER BY 1;

    END TRY
    BEGIN CATCH
        EXEC [dbo].[spClawRethrowError];
    END CATCH

	DROP TABLE #MainCutomerOpsReps;
	DROP TABLE #RateQuotes;
	DROP TABLE #Results;
END
GO

EXEC sys.sp_addextendedproperty
    @name = N'MS_Description'
  , @value = N'Stored procedure used to get Load details.'
  , @level0type = N'SCHEMA'
  , @level0name = N'dbo'
  , @level1type = N'PROCEDURE'
  , @level1name = N'GetLoadsOnDemand_08';
GO