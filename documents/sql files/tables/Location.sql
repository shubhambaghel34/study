CREATE TABLE [dbo].[Location] (
    [LocationId]				INT         IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL
        CONSTRAINT PK_Location
        PRIMARY KEY (LocationId),
    [LocationNo]				INT          NULL,
    [LocationType]				TINYINT      NULL,
    [Longitude]					FLOAT        NULL,
    [Latitude]					FLOAT        NULL,
    [CountryId]					SMALLINT     NULL,
    [ProvinceId]				SMALLINT     NULL,
    [PlaceName]					VARCHAR (80) NULL,
    [PlaceUnencoded]			VARCHAR (80) NULL,
    [PostCodePlaceName]			VARCHAR (90) NULL,
    [TrunkNo]					INT          NULL,
    [LocalTownName]				VARCHAR (80) NULL,
    [ProvinceName]				VARCHAR (60) NULL,
    [CountryCode]				CHAR (3)     NULL,
    [ExpDistToNext]				SMALLINT     NULL,
    [ExpTimeToNext]				SMALLINT     NULL,
    [ExpTollToNext]				SMALLINT     NULL,
    [ZoneId]					SMALLINT     NULL,
    [LocationCityId]			INT          NULL
        CONSTRAINT [FK_Location_LocationCity]
        FOREIGN KEY (LocationCityId) REFERENCES dbo.LocationCity (CityId),
    [LocationPostalCodeId]		INT			 NULL
        CONSTRAINT [FK_Location_LocationPostalCode]
        FOREIGN KEY (LocationPostalCodeId) REFERENCES dbo.LocationPostalCode (LocationPostalCodeId),
    [CreateDateUTC]             [DATETIME]   NULL
        CONSTRAINT DF_Location_01 DEFAULT GETUTCDATE(),
    [CreateByUserId]            [INT]        NULL,
    [UpdateDateUTC]             [DATETIME]   NULL
        CONSTRAINT DF_Location_02 DEFAULT GETUTCDATE(),
    [UpdateByUserId]            [INT]        NULL,
);
GO

CREATE INDEX [IX_Location_LocationNo]
ON [dbo].[Location] ([LocationNo])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = ON, DROP_EXISTING = OFF, ONLINE = ON, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90)
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description',
    @value = N'Contains information about physical locations, including ones that are not cities',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Location';
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ID of this entry (Primary Key)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Location',
    @level2type = N'COLUMN',
    @level2name = N'LocationId';
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Number of this entry in FreightEx''s database',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Location',
    @level2type = N'COLUMN',
    @level2name = N'LocationNo';
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Specifies what type of location''s data is stored in the row, delineated in table LocationType. 11 = Large city, 17 = Tiny Village, 30 = Railway station, etc.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Location',
    @level2type = N'COLUMN',
    @level2name = N'LocationType';
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Latitude of this location',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Location',
    @level2type = N'COLUMN',
    @level2name = N'Latitude';
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Longitude of this location',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Location',
    @level2type = N'COLUMN',
    @level2name = N'Longitude';
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This column contains the CityId of the LocationCity associated with this location',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Location',
    @level2type = N'COLUMN',
    @level2name = N'LocationCityId';
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This column contains the LocationPostalCodeId of the LocationPostalCode associated with this location',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Location',
    @level2type = N'COLUMN',
    @level2name = N'LocationPostalCodeId';
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'UTC Date that this entry was created',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Location',
    @level2type = N'COLUMN',
    @level2name = N'CreateDateUTC';
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ID of the user that created this entry',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Location',
    @level2type = N'COLUMN',
    @level2name = N'CreateByUserId';
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'UTC Date that this entry was last updated',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Location',
    @level2type = N'COLUMN',
    @level2name = N'UpdateDateUTC';
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ID of the user that last updated this entry',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Location',
    @level2type = N'COLUMN',
    @level2name = N'UpdateByUserId';
GO
