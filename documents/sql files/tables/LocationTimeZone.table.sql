/*
// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright © 2017 - 2017
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
CREATE TABLE [dbo].[LocationTimeZone]
    (
    [LocationTimeZoneId] int NOT NULL IDENTITY (1, 1),
    [CreateDate] [datetime] NOT NULL
        CONSTRAINT DF_LocationTimeZone_01 DEFAULT getdate(),
    [CreateByUserId] [int] NOT NULL,
    [UpdateDate] [datetime] NOT NULL
        CONSTRAINT DF_LocationTimeZone_02 DEFAULT getdate(),
    [UpdateByUserId] [int] NOT NULL,
    [IANATimeZoneName] nvarchar(50) NOT NULL UNIQUE

    )  ON [PRIMARY]
GO

EXECUTE sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Contains information about TimeZones',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationTimeZone';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'ID of this entry (Primary Key)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationTimeZone',
    @level2type = N'COLUMN',
    @level2name = N'LocationTimeZoneId';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Date that this entry was created',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationTimeZone',
    @level2type = N'COLUMN',
    @level2name = N'CreateDate';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'ID of the user that created this entry',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationTimeZone',
    @level2type = N'COLUMN',
    @level2name = N'CreateByUserId';
GO


EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Date that this entry was last updated',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationTimeZone',
    @level2type = N'COLUMN',
    @level2name = N'UpdateDate';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'ID of the user that last updated this entry',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationTimeZone',
    @level2type = N'COLUMN',
    @level2name = N'UpdateByUserId';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'IANA time zone name (id) of this entry. Used for lookup of time zone information from external libraries using the IANA Time Zone Database',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationTimeZone',
    @level2type = N'COLUMN',
    @level2name = N'IANATimeZoneName';
GO
