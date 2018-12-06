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
CREATE TABLE [dbo].[Carrier] (
    [ID]                                 INT             IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Code]                               VARCHAR (10)    NULL,
    [Name]                               VARCHAR (50)    NULL,
    [ParentCarrierID]                    INT             NULL,
    [PayToVendorID]                      INT             NULL,
    [SalesStatus]                        INT             NULL,
    [ServiceRating]                      FLOAT           NULL,
    [SCAC]                               CHAR (10)       NULL,
    [MainPhoneNumber]                    VARCHAR (50)    NULL,
    [RateConfirmationFax]                VARCHAR (50)    NULL,
    [RateConfirmationEmail]              VARCHAR (50)    NULL,
    [LoadOfferFax]                       CHAR (20)       NULL,
    [LoadOfferEmail]                     VARCHAR (1000)  NULL,
    [TaxID]                              VARCHAR (20)    NULL,
    [DoTNumber]                          CHAR (20)       NULL,
    [ContractVersion]                    CHAR (15)       NULL,
    [ContractExpirationDate]             DATETIME        NULL,
    [ContractApprovalStatus]             INT             NULL,
    [LiabilityLimit]                     MONEY           NULL,
    [LiabilityAgent]                     VARCHAR (75)    NULL,
    [LiabilityCompanyName]               VARCHAR (50)    NULL,
    [LiabilityExpirationDate]            DATETIME        NULL,
    [CargoCompanyName]                   VARCHAR (50)    NULL,
    [CargoLimit]                         MONEY           NULL,
    [CargoDeductible]                    MONEY           NULL,
    [CargoAgent]                         VARCHAR (75)    NULL,
    [CargoExpirationDate]                DATETIME        NULL,
    [GeneralCompanyName]                 VARCHAR (50)    NULL,
    [GeneralLimit]                       MONEY           NULL,
    [GeneralAgent]                       VARCHAR (75)    NULL,
    [GeneralExpirationDate]              DATETIME        NULL,
    [WorkmansCompCompanyName]            VARCHAR (50)    NULL,
    [WorkmansCompLimit]                  MONEY           NULL,
    [WorkmansCompAgent]                  VARCHAR (75)    NULL,
    [WorkmansCompExpirationDate]         DATETIME        NULL,
    [EquipmentPowerUnits]                INT			 NULL,
    [EquipmentDrivers]                   INT			 NULL,
    [DefaultOriginDeadhead]              INT             NULL,
    [DefaultDestinationDeadhead]         INT             NULL,
    [LoadStatusUpdateViaType]            INT             NULL,
    [CreateDate]                         DATETIME        NOT NULL,
    [CreateByUserID]                     INT             NOT NULL,
    [UpdateDate]                         DATETIME        NOT NULL,
    [UpdateByUserID]                     INT             NOT NULL,
    [ContractDate]                       DATETIME        NULL,
    [SafetyRating]                       INT             NULL,
    [LegalName]                          VARCHAR (50)    NULL,
    [LastTruckEntryDate]                 DATETIME        NULL,
    [LastTruckEntryByUserID]             INT             NULL,
    [TruckNotes]                         VARCHAR (250)   NULL,
    [APFactoringCompanyPhone]            VARCHAR (50)    NULL,
    [UseFactoringCompany]                BIT             NULL,
    [APDefaultPaymentType]               INT             NULL,
    [LiabilityPolicyNumber]              VARCHAR (75)    NULL,
    [CargoPolicyNumber]                  VARCHAR (75)    NULL,
    [GeneralPolicyNumber]                VARCHAR (75)    NULL,
    [WorkmansCompPolicyNumber]           VARCHAR (75)    NULL,
    [LegacyAccountingIdentifier]         VARCHAR (75)    NULL,
    [TruckNotesEditedByUserID]           INT             NULL,
    [TruckNotesEditedDate]               DATETIME        NULL,
    [LegacyCode]                         VARCHAR (50)    NULL,
    [WebsiteURL]                         VARCHAR (75)    NULL,
    [WebsiteUserID]                      VARCHAR (75)    NULL,
    [WebsitePassword]                    VARCHAR (25)    NULL,
    [EDIID]                              VARCHAR (50)    NULL,
    [UseDefaultFuelTemplate]             BIT             NULL,
    [HazmatCertificateNumber]            VARCHAR (75)    NULL,
    [HazmatCertificateExpirationDate]    DATETIME        NULL,
    [EDIEnabled]                         BIT             NOT NULL,
    [TruckEntryViewType]                 INT             NOT NULL,
    [CheckName]                          VARCHAR (65)    NULL,
    [SelfDispatch]                       BIT             NULL,
    [Send1099]                           BIT             NULL,
    [CorporationType]                    INT             NULL,
    [CertificateHolder_AutoLiability]    BIT             NULL,
    [CertificateHolder_WorkmansComp]     BIT             NULL CONSTRAINT [DF_Carrier_CertificateHolder_WorkmansComp] DEFAULT(0),
    [CertificateHolder_GeneralLiability] BIT             NULL CONSTRAINT [DF_Carrier_CertificateHolder_GeneralLiability] DEFAULT(0),
    [CertificateHolder_Cargo]            BIT             NULL CONSTRAINT [DF_Carrier_CertificateHolder_Cargo] DEFAULT(0),
    [IncludeInLoadDistribution]          BIT             NULL,
    [IncludeInCarrierFuelScreen]         BIT             NULL,
    [Market]                             INT             NULL,
    [EDISenderID]                        VARCHAR (50)    NULL,
    [EDISenderIDQualifier]               VARCHAR (50)    NULL,
    [EDIReceiverID]                      VARCHAR (50)    NULL,
    [EDIReceiverIDQualifier]             VARCHAR (50)    NULL,
    [FacilityID]                         INT             NULL,
    [DiscountPercent]                    DECIMAL (16, 2) NULL,
    [FlatAmount]                         DECIMAL (16, 2) NULL,
    [ComcheckFees]                       DECIMAL (16, 2) NULL,
    [MileageSystemType]                  INT             NULL,
    [MileageRouteType]                   INT             NULL,
    [MileageBorderType]                  INT             NULL,
    [AutoDocRequest]                     BIT             NOT NULL,
    [DaysToFirstRequest]                 INT             NOT NULL,
    [UnsafeDriving]                      DECIMAL (4, 3)  NULL,
    [FatiguedDriving]                    DECIMAL (4, 3)  NULL,
    [DriverFitness]                      DECIMAL (4, 3)  NULL,
    [ControlledSubstances]               DECIMAL (4, 3)  NULL,
    [VehicleMaintenance]                 DECIMAL (4, 3)  NULL,
    [CargoRelated]                       DECIMAL (4, 3)  NULL,
    [CrashIndicator]                     DECIMAL (4, 3)  NULL,
    [CompanyID]                          INT             NULL,
    [MileageVersion]                     INT             NULL,
    [MileageInTenths]                    BIT             NULL,
    [ACEDocExpDt]                        DATETIME        NULL,
    [ComcheckApproved]                   BIT             NOT NULL,
	[ComcheckMainContactFirstName]		 VARCHAR(50)	 NULL,
	[ComcheckMainContactLastName]		 VARCHAR(50)	 NULL,
	[ComcheckMainContactPhoneNumber]	 VARCHAR(50)	 NULL,
	[CARBFormReceived]					 BIT			 NULL,
	[Division]							 INT			 NULL,
	[VINVerification]					 BIT			 NULL, 
    [CertificateHolder_HRHV]			 BIT             NULL, 
    [InterchangeInsuranceCompanyName]	 VARCHAR(50)	 NULL, 
    [InterchangeInsuranceLimit]			 MONEY			 NULL, 
    [InterchangeInsuranceAgent]			 VARCHAR(75)	 NULL, 
    [InterchangeInsuranceExpirationDate] DATETIME		 NULL, 
    [InterchangeInsurancePolicyNumber]	 VARCHAR(75)	 NULL, 
    [Disable214TrackingOverwrite]        BIT             NULL,
	[Disable214AppointmentOverwrite]     BIT             NULL,
	[FactoringCompanyId]				 INT			 NULL, 
    [CarrierAccountingNotes]			 VARCHAR(2000)	 NULL, 
    [AccountingNotesAlert]				 BIT			 NULL,
	[IsVoucherPaymentEmailNotify]		 BIT			 NOT NULL CONSTRAINT [DF_Carrier_IsVoucherPaymentEmailNotify] DEFAULT(0),
	[IsMexicoLoadsOnly]					 BIT			 NOT NULL CONSTRAINT [DF_Carrier_CanBookOnlyOnMexicanLoads] DEFAULT(0),
	[IsHVHRCertified]					 BIT			 NOT NULL CONSTRAINT [DF_Carrier_IsHVHRCertified] DEFAULT(0),
	[IsHVHRPlus]						 BIT			 NOT NULL CONSTRAINT [DF_Carrier_IsHVHRPlus] DEFAULT(0),
	[SecondaryWebsiteUrl]				 VARCHAR(75)	 NULL,		
	[PowerOnlyMatches]					 BIT             NOT NULL CONSTRAINT [DF_Carrier_PowerOnlyMatches] DEFAULT(1),
	[EDIParentCarrierID]                 INT             NULL,
	[UpdateDateExternalUser]			 DATETIME,
	[UpdateByExternalUserId]			 INT
);
GO

CREATE NONCLUSTERED INDEX [_dta_index_Carrier_9_638625318__K1_K3_48] 
ON [dbo].[Carrier] ([ID] ASC, [Name] ASC)
INCLUDE ([LastTruckEntryDate], [UpdateDate]) 
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = ON, DROP_EXISTING = ON, ONLINE = ON, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) 
GO

CREATE NONCLUSTERED INDEX [IX_Carrier_05] 
ON [dbo].[Carrier] ([ParentCarrierID], [EDIEnabled]) 
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = ON, DROP_EXISTING = OFF, ONLINE = ON, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90)
GO

CREATE NONCLUSTERED INDEX [ix_Carrier_SalesStatus_CompanyID_includes] ON [dbo].[Carrier]
(
	[SalesStatus] ASC,
	[CompanyID] ASC
)
INCLUDE ( 	[ID],
	[Code],
	[Name],
	[LegalName]) 
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = ON, DROP_EXISTING = OFF, ONLINE = ON, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90);
GO

-- created high value missing index (AZ)
CREATE NONCLUSTERED INDEX [IX_Carrier_DoTNumber_CompanyID_SalesStatus] 
ON [dbo].[Carrier] ([DoTNumber], [CompanyID], [SalesStatus])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = ON, DROP_EXISTING = OFF, ONLINE = ON, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Carrier_EDIEnabled_EDIParentCarrierID_includes] 
ON [dbo].[Carrier] ([EDIEnabled], [EDIParentCarrierID])  
INCLUDE ([EDIID]) 
WITH (ONLINE=ON, SORT_IN_TEMPDB=ON);
GO

EXEC sys.sp_addextendedproperty
    @name = N'MS_Description',
	@value = N'This column indicate the carrier can only booked on mexican load.',
	@level0type = N'SCHEMA', 
	@level0name = N'dbo', 
	@level1type = N'TABLE', 
	@level1name = N'Carrier', 
	@level2type = N'Column', 
	@level2name = N'IsMexicoLoadsOnly';
GO

EXEC sys.sp_addextendedproperty
    @name = N'MS_Description',	
	@value = N'This column indicates whether the carrier is HVHR certified',
	@level0type = N'SCHEMA',
	@level0name = N'dbo', 
	@level1type = N'TABLE',
	@level1name = N'Carrier', 
	@level2type = N'Column',
	@level2name = N'IsHVHRCertified';
GO

EXEC sys.sp_addextendedproperty
    @name = N'MS_Description',
	@value = N'This column indicates whether the carrier is HVHR Plus certified',
	@level0type = N'SCHEMA',
	@level0name = N'dbo', 
	@level1type = N'TABLE',
	@level1name = N'Carrier', 
	@level2type = N'Column',
	@level2name = N'IsHVHRPlus';
GO

EXEC sys.sp_addextendedproperty @name = N'MS_Description',
	@value = N'This column stores the secondary website url for a carrier.',
	@level0type = N'SCHEMA',
	@level0name = N'dbo',
	@level1type = N'TABLE',
	@level1name = N'Carrier',
	@level2type = N'Column',
	@level2name = N'SecondaryWebsiteUrl';
GO
GO

EXEC sys.sp_addextendedproperty
    @name = N'MS_Description',
	@value = N'This column is used to store PowerOnlyMatches attribute of Carrier',
	@level0type = N'SCHEMA',
	@level0name = N'dbo',
	@level1type = N'TABLE',
	@level1name = N'Carrier',
	@level2type = N'Column',
	@level2name = N'PowerOnlyMatches';
GO

CREATE NONCLUSTERED INDEX [IX_Carrier_LastTruckEntryDate] 
ON [dbo].[Carrier]([LastTruckEntryDate] ASC)
WITH(SORT_IN_TEMPDB=ON, ONLINE=ON);
GO

EXEC sys.sp_addextendedproperty
    @name = N'MS_Description',
	@value = N'This column is used to store EDI parent carrier ID of Carrier',
	@level0type = N'SCHEMA',
	@level0name = N'dbo',
	@level1type = N'TABLE',
	@level1name = N'Carrier',
	@level2type = N'Column',
	@level2name = N'EDIParentCarrierID';
GO

CREATE NONCLUSTERED INDEX [IX_Carrier_SalesStatus_Includes] ON [dbo].[Carrier]
(
       [SalesStatus] ASC
)
INCLUDE (     
	   [ID],
       [Code],
       [Name],
       [MainPhoneNumber],
       [ContractApprovalStatus],
       [LiabilityLimit], 
	   [CargoLimit],
       [GeneralLimit],
       [WorkmansCompLimit],
       [EquipmentPowerUnits],
       [CreateDate],
       [CreateByUserID],
       [SafetyRating],
       [LastTruckEntryDate],
       [LastTruckEntryByUserID],
       [TruckNotes],
       [CompanyID],
       [Division],
       [CertificateHolder_HRHV],
       [InterchangeInsuranceLimit],
       [IsHVHRCertified],
       [IsHVHRPlus]) 
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = ON, DROP_EXISTING = OFF, ONLINE = ON, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90);
GO 
