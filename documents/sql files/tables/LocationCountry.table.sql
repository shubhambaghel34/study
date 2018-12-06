/*
// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2013 - 2017
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
CREATE TABLE [dbo].[LocationCountry](
	[LocationCountryId]	INT			IDENTITY(1,1) NOT FOR REPLICATION NOT NULL
		 CONSTRAINT PK_LocationCountry PRIMARY KEY CLUSTERED (LocationCountryId),
	[CreateDate]		DATETIME	NOT NULL
		CONSTRAINT DF_LocationCountry_01 DEFAULT getdate(),
	[CreateByUserId]	INT			NOT NULL,
	[UpdateDate]		DATETIME	NOT NULL
		CONSTRAINT DF_LocationCountry_02 DEFAULT getdate(),
	[UpdateByUserId]	INT			NOT NULL,
	[GeoRegionId]		INT			NULL,
	[ISOCodeAlpha2]		VARCHAR(2)	NULL,
	[ISOCodeAlpha3]		VARCHAR(3)	NULL,
	[PhoneCode]			VARCHAR(4)	NULL,
	[Name]				VARCHAR(75) NOT NULL,
    [NumericCode]		INT			NULL,
    [FIPS]				CHAR (2)	NULL,);
GO

CREATE  NONCLUSTERED INDEX [IX_LocationCountry_01] 
ON [dbo].[LocationCountry] ([GeoRegionId]) 
INCLUDE ([ISOCodeAlpha2])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = ON, DROP_EXISTING = OFF, ONLINE = ON, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90);
GO

EXECUTE sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Contains information about Countries',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationCountry';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'ID of this entry (Primary Key)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationCountry',
    @level2type = N'COLUMN',
    @level2name = N'LocationCountryId';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Date that this entry was created',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationCountry',
    @level2type = N'COLUMN',
    @level2name = N'CreateDate';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'ID of the user that created this entry',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationCountry',
    @level2type = N'COLUMN',
    @level2name = N'CreateByUserId';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Date that this entry was last updated',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationCountry',
    @level2type = N'COLUMN',
    @level2name = N'UpdateDate';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'ID of the user that last updated this entry',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationCountry',
    @level2type = N'COLUMN',
    @level2name = N'UpdateByUserId';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'The ID of the GeoRegion this country belongs to',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationCountry',
    @level2type = N'COLUMN',
    @level2name = N'GeoRegionId';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'2-character ISO code for this country',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationCountry',
    @level2type = N'COLUMN',
    @level2name = N'ISOCodeAlpha2';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'3-character ISO code for this country',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationCountry',
    @level2type = N'COLUMN',
    @level2name = N'ISOCodeAlpha3';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Phone code for the country',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationCountry',
    @level2type = N'COLUMN',
    @level2name = N'PhoneCode';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Name of this State',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationCountry',
    @level2type = N'COLUMN',
    @level2name = N'Name';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'The Coyote assigned numeric code for the country',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationCountry',
    @level2type = N'COLUMN',
    @level2name = N'NumericCode';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'The FIPS code for the country',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationCountry',
    @level2type = N'COLUMN',
    @level2name = N'FIPS';
GO