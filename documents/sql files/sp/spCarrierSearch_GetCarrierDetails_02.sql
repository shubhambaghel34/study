/*
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
*/
CREATE PROCEDURE [dbo].[spCarrierSearch_GetCarrierDetails_02]
(
	@USFMAccess					BIT = NULL,
	@Division					[dbo].[IDsTableType] READONLY,
	@CarrierCode				VARCHAR(10) = NULL,
	@CarrierName				VARCHAR(50) = NULL,
	@CityId						INT = NULL,
	@StateCode					VARCHAR(3) = NULL,
	@MainPhoneNumber			VARCHAR(50) = NULL,
	@ActiveContactName			VARCHAR(30) = NULL,
	@LastTruckEntryStartDate	DATETIME = NULL,
	@LastTruckEntryEndDate		DATETIME = NULL,
	@LastTruckEntryByUserID		INT = NULL,
	@SalesStatus				[dbo].[IDsTableType] READONLY,
	@GroupIds					[dbo].[IDsTableType] READONLY,
	@EmployeeId					INT = NULL,
	@EquipmentPowerUnits		INT = NULL,
	@CarrierCreatedStartDate	DATETIME = NULL,
	@CarrierCreatedEndDate		DATETIME = NULL,
	@CarrierCreateByUserID		INT = NULL,
	@EquipmentType				VARCHAR(25) = NULL,
	@EquipmentLength			DECIMAL(8,2) = NULL,
	@EquipmentWidth				DECIMAL(8,2) = NULL,
	@EquipmentHeight			INT = NULL,
	@WorkmansCompLimit			MONEY = NULL,
	@CargoLimit					MONEY = NULL,
	@InterchangeInsuranceLimit	MONEY = NULL,
	@LiabilityLimit				MONEY = NULL,
	@GeneralLimit				MONEY = NULL,
	@SafetyRating				INT = NULL,
	@ContractApprovalStatus		INT = NULL,
	@CarrierAttributes			[dbo].[CarrierAttributesTableType_01] READONLY,
	@CarrierLanguages			[dbo].[IDsTableType] READONLY,
	@CertificateHolder_HRHV		BIT = NULL,
	@IsHVHRCertified			BIT = NULL,
	@IsHVHRPlus					BIT = NULL,
	@Latitude					FLOAT = NULL,
	@Longitude					FLOAT = NULL,
	@LongOffset					FLOAT = NULL,
	@LatOffset					FLOAT = NULL,
	@ContactEmailAddress		VARCHAR(255) = NULL,
	@ContactPhoneNumber			VARCHAR(50) = NULL,
	@CarrierEquipments			[dbo].[StringList] READONLY
)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	
DECLARE	
	
	@CarrLangCount AS INT = 0,
	@CurrentDate AS DATETIME = DATEADD(dd, DATEDIFF(dd, 0, GETDATE()), 0),
	@CarrEquipCount AS INT = 0;
	
	SELECT @CarrLangCount = COUNT(ID) FROM @CarrierLanguages;
	
	SELECT @CarrEquipCount = COUNT(s) FROM @CarrierEquipments;
   
	CREATE TABLE #Carrier
	(
		ID					INT, 
		Code				VARCHAR(10), 
		Name				VARCHAR(50), 
		Division			INT, 
		MainPhone			VARCHAR(50), 
		SalesStatus			VARCHAR(100), 
		LastCallDate		DATETIME, 
		PowerUnits			INT, 		
		CarrierTruckNotes	VARCHAR(250), 
		SafetyRating		INT, 
		AutoLimit			MONEY, 
	    CargoLimit			MONEY, 
		GenLimit			MONEY, 
		WCLimit				MONEY,
		MainRep				VARCHAR(50),
		City				VARCHAR(50), 
		ST					CHAR(3), 
		CountryCodeAlpha2	VARCHAR(10), 		
		FirstName			VARCHAR(20),
		LastName			VARCHAR(30), 
		Phone				VARCHAR(50), 
		CityID				INT, 
		Latitude			FLOAT, 
		Longitude			FLOAT 		
	);

	CREATE TABLE #ContactTemp 
	(
		EntityId INT
	);

	CREATE TABLE #EquipCountTemp 
	(
		EquipmentV BIGINT,
		EquipmentR BIGINT,
		EquipmentF BIGINT,
		EquipmentE BIGINT,
		EquipmentZ BIGINT, 
		EntityId   INT
	);

	CREATE  TABLE #CarrEquipments
	(
		EquipCount INT,
		EntityId INT
	);

	INSERT INTO #CarrEquipments(EquipCount, EntityId)
	(SELECT COUNT(DISTINCT(EquipmentType)) CountEq, EQU.EntityID
		FROM [dbo].[Equipment] EQU
		INNER JOIN @CarrierEquipments CE ON EQU.EquipmentType = CE.s
		WHERE EQU.EntityType = 1				--EnityType =1 for carrier
		Group BY EQU.EntityId);
	
	INSERT INTO #Carrier
	(
		ID, 
		Code, 
		Name, 
		Division, 
		MainPhone, 
		SalesStatus, 
		LastCallDate, 
		PowerUnits, 		
		CarrierTruckNotes, 
		SafetyRating, 
		AutoLimit, 
	    CargoLimit, 
		GenLimit, 
		WCLimit,
		MainRep,
		City, 
		ST, 
		CountryCodeAlpha2, 		
		FirstName,
		LastName, 
		Phone, 
		CityID, 
		Latitude, 
		Longitude
	) 
	SELECT DISTINCT
		CARR.ID, 
		CARR.Code, 
		CARR.Name, 
		CARR.Division, 
		CARR.MainPhoneNumber, 
		TDET.Code, 
		CARR.LastTruckEntryDate, 
		CARR.EquipmentPowerUnits, 		
		CARR.TruckNotes, 
		CARR.SafetyRating, 
		CARR.LiabilityLimit, 
		CARR.CargoLimit, 
		CARR.GeneralLimit, 
		CARR.WorkmansCompLimit,
		EMP.Code,
		CIT.Name, 
		ST.Code, 
		CNT.[ISOCodeAlpha2],		
		CON.FirstName,
		CON.LastName, 
		CON.OfficePhoneNumber, 
		CIT.CityId, 
		CIT.Latitude, 
		CIT.Longitude		
	FROM [dbo].[Carrier] CARR
		LEFT JOIN [dbo].[Address] ADR ON ADR.EntityID = CARR.ID AND ADR.EntityType = 1 AND ADR.Main = 1 
		LEFT JOIN [dbo].[LocationCity] CIT ON ADR.CityID = CIT.CityID
		LEFT JOIN [dbo].[LocationStateCountry] SCIT ON SCIT.LocationStateCountryId = CIT.LocationStateCountryId
		LEFT JOIN [dbo].[LocationState] ST ON ST.LocationStateId = SCIT.LocationStateId
		LEFT JOIN [dbo].[LocationCountry] CNT ON CNT.LocationCountryId = SCIT.LocationCountryId		
		LEFT JOIN [dbo].[Contact_CarrierSearch] CON ON CON.EntityID = CARR.ID AND CON.EntityType = 1 AND CON.main = 1 
		LEFT JOIN [dbo].[Equipment] EQU ON EQU.EntityType = 1 and EQU.EntityID = CARR.ID 
		LEFT JOIN [dbo].[TypeDetails] TDET ON TDET.ID = CARR.SalesStatus AND TDET.TypeCategoryID = 27
		LEFT JOIN [dbo].[Rep] RP  ON RP.EntityID = CARR.ID AND RP.EntityType = 1 AND RP.Main = 1 
		LEFT JOIN [dbo].[Employee] EMP ON EMP.ID = RP.EmployeeID 
		LEFT JOIN [dbo].[CarrierAttributes] CA ON CA.CarrierId = CARR.ID
		OUTER APPLY(SELECT COUNT(CarrierLanguageID) CountCLI FROM [dbo].[CarrierLanguageAttribute] CLA 
                           INNER JOIN @CarrierLanguages CL ON CLA.CarrierLanguageID = CL.ID 
                           WHERE CarrierID = CARR.ID Group BY CarrierId) CLA
		LEFT JOIN @CarrierAttributes  CARRATTR  ON 1 = 1
		LEFT JOIN @GroupIds AS GRP ON 1 = 1	
		LEFT JOIN @SalesStatus AS SLS ON 1 = 1	
		LEFT JOIN @Division AS DIV ON 1 = 1	
		LEFT JOIN #CarrEquipments AS CARREQ ON CARREQ.EntityId = CARR.ID
	WHERE
		 CARR.SalesStatus <>  14   -- sales status 14 is for Duplicate 
		 AND ( @USFMAccess IS NULL OR CARR.CompanyID <> 3 )
		 AND ( DIV.ID IS NULL OR (CARR.Division = DIV.Id  OR (DIV.ID = 0 AND (CARR.Division <= 0 OR  CARR.Division IS NULL ))))
		 AND ( @CarrierCode IS  NULL OR CARR.Code = @CarrierCode ) 							
		 AND ( @CarrierName IS  NULL OR CARR.Name LIKE '%' + @CarrierName + '%' )
		 AND ( @MainPhoneNumber IS NULL OR CARR.MainPhoneNumber LIKE '%' + @MainPhoneNumber + '%' )	
		 AND ( @LastTruckEntryStartDate IS NULL OR (CARR.LastTruckEntryDate >= @LastTruckEntryStartDate   AND CARR.LastTruckEntryDate <= @LastTruckEntryEndDate  ))
		 AND ( @LastTruckEntryByUserID IS NULL OR CARR.LastTruckEntryByUserID = @LastTruckEntryByUserID )
		 AND ( @EquipmentPowerUnits IS NULL OR CARR.EquipmentPowerUnits >= @EquipmentPowerUnits )
		 AND ( @CarrierCreatedStartDate IS NULL OR (CARR.CreateDate >= @CarrierCreatedStartDate   AND CARR.CreateDate <= @CarrierCreatedEndDate ))
		 AND ( @CarrierCreateByUserID IS NULL OR CARR.CreateByUserID = @CarrierCreateByUserID )
		 AND ( SLS.ID IS NULL OR CARR.salesStatus =SLS.id )
		 AND ( @WorkmansCompLimit IS NULL OR CARR.WorkmansCompLimit >= @WorkmansCompLimit )
		 AND ( @CargoLimit IS NULL OR CARR.CargoLimit >= @CargoLimit )
		 AND ( @InterchangeInsuranceLimit IS NULL OR CARR.InterchangeInsuranceLimit >= @InterchangeInsuranceLimit )
		 AND ( @LiabilityLimit IS NULL OR CARR.LiabilityLimit >= @LiabilityLimit )
		 AND ( @GeneralLimit IS NULL OR CARR.GeneralLimit >= @GeneralLimit )
		 AND ( @SafetyRating IS NULL OR CARR.SafetyRating = @SafetyRating )
		 AND ( @ContractApprovalStatus IS NULL OR CARR.ContractApprovalStatus = @ContractApprovalStatus )	
		 AND ( @CertificateHolder_HRHV IS NULL OR CARR.CertificateHolder_HRHV = 1 )
		 AND ( @IsHVHRCertified IS NULL OR CARR.IsHVHRCertified = 1 )
		 AND ( @IsHVHRPlus IS NULL OR CARR.IsHVHRPlus = 1 ) 
		 AND ( @CarrLangCount = 0 OR CLA.CountCLI >= @CarrLangCount )
		 AND ( @EquipmentType IS NULL OR  EQU.EquipmentType = @EquipmentType )
		 AND ( @EquipmentLength IS NULL OR  EQU.[Length] >= @EquipmentLength )
		 AND ( @EquipmentWidth IS NULL OR EQU.Width >= @EquipmentWidth )
		 AND ( @EquipmentHeight IS NULL OR EQU.Height >= @EquipmentHeight )	
		 AND ( CARRATTR.ModeTL IS NULL OR CA.ModeTL = 1)
		 AND ( CARRATTR.ModeLTL IS NULL OR CA.ModeLTL = 1)
		 AND ( CARRATTR.ModeIMDL IS NULL OR CA.ModeIMDL = 1)
		 AND ( CARRATTR.ModeRail IS NULL OR CA.ModeRail = 1)
		 AND ( CARRATTR.ModeOcean IS NULL OR CA.ModeOcean = 1)
		 AND ( CARRATTR.ModeAir IS NULL OR CA.ModeAir = 1)
		 AND ( CARRATTR.ModePartial IS NULL OR CA.ModePartial = 1)
		 AND ( CARRATTR.ServiceDrayage IS NULL OR CA.ServiceDrayage = 1)
		 AND ( CARRATTR.CertificationANDLicenseHazMat IS NULL OR CA.CertificationANDLicenseHazMat = 1)
		 AND ( CARRATTR.EquipmentServiceTeamDrivers IS NULL OR CA.EquipmentServiceTeamDrivers = 1)
		 AND ( CARRATTR.EquipmentServiceRoadTrain IS NULL OR CA.EquipmentServiceRoadTrain = 1)
		 AND ( CARRATTR.ServiceWarehousing IS NULL OR CA.ServiceWarehousing = 1)
		 AND ( CARRATTR.EquipmentServiceDuraplate IS NULL OR CA.EquipmentServiceDuraplate = 1)
		 AND ( CARRATTR.EquipmentServiceLiftgate IS NULL OR CA.EquipmentServiceLiftgate = 1)
		 AND ( CARRATTR.EquipmentServicePowerOnly IS NULL OR CA.EquipmentServicePowerOnly = 1)
		 AND ( CARRATTR.ServiceCrossDock IS NULL OR CA.ServiceCrossDock = 1)
		 AND ( CARRATTR.ServiceCustoms IS NULL OR CA.ServiceCustoms = 1)
		 AND ( CARRATTR.CertificationANDLicenseLiquorPermit IS NULL OR CA.CertificationANDLicenseLiquorPermit = 1)
		 AND ( CARRATTR.EquipmentServiceOversize IS NULL OR CA.EquipmentServiceOversize = 1)
		 AND ( CARRATTR.EquipmentServiceAirRide IS NULL OR CA.EquipmentServiceAirRide = 1)
		 AND ( CARRATTR.SpecialPrivateFleet IS NULL OR CA.SpecialPrivateFleet = 1)
		 AND ( CARRATTR.CertificationANDLicenseTWIC IS NULL OR CA.CertificationANDLicenseTWIC = 1)
		 AND ( CARRATTR.EquipmentServiceDropTrailer IS NULL OR CA.EquipmentServiceDropTrailer = 1)
		 AND ( CARRATTR.ServiceLumper IS NULL OR CA.ServiceLumper = 1)
		 AND ( CARRATTR.CertificationANDLicenseHACCP IS NULL OR CA.CertificationANDLicenseHACCP = 1)
		 AND ( CARRATTR.EquipmentServiceLightEquipment IS NULL OR CA.EquipmentServiceLightEquipment = 1)
		 AND ( CARRATTR.CertificationANDLicenseCTPAT IS NULL OR CA.CertificationANDLicenseCTPAT = 1)
		 AND ( CARRATTR.CertificationANDLicenseFAST IS NULL OR CA.CertificationANDLicenseFAST = 1)
		 AND ( CARRATTR.CertificationANDLicensePIPPEP IS NULL OR CA.CertificationANDLicensePIPPEP = 1)
		 AND ( CARRATTR.EquipmentServiceETrac IS NULL OR CA.EquipmentServiceETrac = 1)
		 AND ( CARRATTR.EquipmentServicePalletJack IS NULL OR CA.EquipmentServicePalletJack = 1)
		 AND ( CARRATTR.SpecialDedicatedFleet IS NULL OR CA.SpecialDedicatedFleet = 1)
		 AND ( CARRATTR.CertificationANDLicenseBondedCarrier IS NULL OR CA.CertificationANDLicenseBondedCarrier = 1)
		 AND ( CARRATTR.OwnershipMinority IS NULL OR CA.OwnershipMinority = 1)
		 AND ( CARRATTR.OwnershipFemale IS NULL OR CA.OwnershipFemale = 1)
		 AND ( CARRATTR.OwnershipVeteran IS NULL OR CA.OwnershipVeteran = 1)
		 AND ( CARRATTR.OwnershipServiceDisability IS NULL OR CA.OwnershipServiceDisability = 1)
		 AND ( @EmployeeId IS NULL OR EMP.ID = @EmployeeId )
		 AND ( GRP.ID IS NULL OR GRP.ID = emp.[Group] ) 
		 AND ( @CityId IS NULL OR CIT.CityID = @CityId )
		 AND ( @Latitude IS NULL OR (ABS( ABS(CIT.Latitude) - @Latitude) <= @LongOffset AND ABS( ABS(CIT.Longitude) - @Longitude) <= @LatOffset))
		 AND ( @StateCode IS NULL OR ST.Code = @StateCode )
		 AND ( CARRATTR.Saturday IS NULL OR CA.DispatchSaturday = 1)
		 AND ( CARRATTR.Sunday IS NULL OR CA.DispatchSunday = 1)
		 AND ( CARRATTR.Tanker IS NULL OR (EQU.EquipmentType = 'TKC' OR EQU.EquipmentType = 'TKF'))
		 AND ( CARRATTR.IMDLEquipment IS NULL OR (EQU.EquipmentType like 'IM' OR EQU.EquipmentType like 'IMF'))
		 AND ( @CarrEquipCount = 0 OR CARREQ.EquipCount >= @CarrEquipCount);
		 
	INSERT INTO #ContactTemp (EntityId)
	SELECT DISTINCT EntityId
	FROM #Carrier CARR
	LEFT JOIN dbo.Contact_CarrierSearch CON ON EntityType = 1 AND Active = 1 AND EntityID = CARR.ID 
	WHERE 										 
		( @ContactEmailAddress IS NULL OR (EmailAddressWork = @ContactEmailAddress  OR EmaillAddressPersonal = @ContactEmailAddress )) 
		AND ( @ContactPhoneNumber IS NULL OR ( HomePhoneNumber LIKE  '%' + @ContactPhoneNumber + '%' OR MobilePhoneNumber LIKE  '%' + @ContactPhoneNumber + '%' OR OfficePhoneNumber LIKE  '%' + @ContactPhoneNumber + '%' )) 
		AND ( @ActiveContactName IS NULL OR ( REPLACE(NickName,' ','') LIKE '%' + @ActiveContactName + '%' OR REPLACE(CON.FirstName + CON.LastName,' ','') LIKE '%' + @ActiveContactName + '%' ));	

	INSERT INTO #EquipCountTemp 
	(
		EquipmentV,
		EquipmentR,
		EquipmentF,
		EquipmentE,
		EquipmentZ, 
		EntityId
	)
	SELECT  
		SUM(CASE WHEN  EquipmentType = 'V' THEN [Count] ELSE 0 END), 
		SUM(CASE WHEN  EquipmentType = 'R' THEN [Count] ELSE 0 END), 
		SUM(CASE WHEN  EquipmentType = 'F' THEN [Count] ELSE 0 END), 
		SUM(CASE WHEN  EquipmentType = 'E' THEN [Count] ELSE 0 END), 
		SUM(CASE WHEN  EquipmentType = 'Z' THEN [Count] ELSE 0 END),
		EntityId
	FROM #Carrier CARR
		INNER JOIN dbo.Equipment EQ ON EntityID = CARR.ID AND EntityType=1
	GROUP BY EntityId ;

	SELECT 
		CARR.ID AS EntityID, 
		CARR.Code, 
		CARR.Name, 
		CARR.Division, 
		CARR.MainPhone, 
		CARR.SalesStatus, 
		CARR.LastCallDate, 
		CARR.PowerUnits, 		
		CARR.CarrierTruckNotes, 
		CARR.SafetyRating, 
		CARR.AutoLimit, 
	    CARR.CargoLimit, 
		CARR.GenLimit, 
		CARR.WCLimit,
		CARR.MainRep,		
		CASE CARR.LastCallDate 
			WHEN @CurrentDate THEN 1 
			ELSE 0 
		END AS Called,
		1 AS 'EntityType', 		
		City, 
		ST, 
		CountryCodeAlpha2, 
		0.0 AS 'Distance', 
		FirstName + ' ' + LastName AS 'Contact', 
		Phone, 
		CityID, 
		Latitude, 
		Longitude, 		
		LEFT(MULTINUMS.MCNums, LEN(MULTINUMS.MCNums) - 1) AS 'MC#', 
		EQUICOUNT.EquipmentV AS 'EquipmentV', 
		EQUICOUNT.EquipmentR AS 'EquipmentR', 
		EQUICOUNT.EquipmentF AS 'EquipmentF',
		EQUICOUNT.EquipmentE AS 'EquipmentE', 
		EQUICOUNT.EquipmentZ AS 'EquipmentZ' 
	FROM 
		#Carrier CARR
		LEFT JOIN #ContactTemp CONACTIVE ON CONACTIVE.EntityId = CARR.Id
		LEFT JOIN #EquipCountTemp EQUICOUNT ON EQUICOUNT.EntityId = CARR.Id
		OUTER APPLY ( SELECT MN.Number + '; ' FROM [dbo].[MultiNumber] MN 
					  WHERE CARR.ID = MN.EntityID AND mn.EntityType = 1 
					  FOR XML PATH('')  ) AS MULTINUMS (MCNums)
	WHERE
		((@ContactEmailAddress IS NULL AND @ContactPhoneNumber IS NULL AND @ActiveContactName IS NULL) OR CONACTIVE.EntityID IS NOT NULL);

	DROP TABLE #Carrier;
	DROP TABLE #ContactTemp;
	DROP TABLE #EquipCountTemp;	
	DROP TABLE #CarrEquipments;
END
GO

EXEC sys.sp_addextendedproperty
	@name = N'MS_Description'
  , @value = N'This stored procedure returns carrier details based on the provided search criteria.'
  , @level0type = N'SCHEMA'
  , @level0name = N'dbo'
  , @level1type = N'PROCEDURE'
  , @level1name = N'spCarrierSearch_GetCarrierDetails_02';
GO

EXEC sys.sp_addextendedproperty
	@name = N'MS_Description',
	@value = N'@CarrierEquipments contains list of equipment selected for search criteria.',
	@level0type = N'SCHEMA',
	@level0name = N'dbo',
	@level1type = N'PROCEDURE',
	@level1name = N'spCarrierSearch_GetCarrierDetails_02',
	@level2type = N'PARAMETER',
	@level2name = N'@CarrierEquipments';
GO
