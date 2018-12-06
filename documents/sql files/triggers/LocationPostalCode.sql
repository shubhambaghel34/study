/*
// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2013 - 2018
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
CREATE TRIGGER [dbo].[TR_LocationPostalCode_I] ON [dbo].[LocationPostalCode]
    FOR INSERT NOT FOR REPLICATION
AS
    BEGIN
        BEGIN TRY
            IF UPDATE(CreateByUserId)
                BEGIN
                    IF EXISTS ( SELECT  1
                                FROM    INSERTED
                                WHERE   CreateByUserId NOT IN ( SELECT
                                                              UserID
                                                              FROM
                                                              SystemUser ) )
                        BEGIN
                            RAISERROR('Invalid CreateByUserId', 16, 3)
                        END
                END

            IF UPDATE(UpdateByUserId)
                BEGIN
                    IF EXISTS ( SELECT  1
                                FROM    INSERTED
                                WHERE   UpdateByUserId NOT IN ( SELECT
                                                              UserID
                                                              FROM
                                                              SystemUser ) )
                        BEGIN
                            RAISERROR('Invalid UpdateByUserId', 16, 3)
                        END
                END

            DECLARE @PostalCode AS VARCHAR(11)
            DECLARE @MainPostalCode AS VARCHAR(50)
            DECLARE @CityId AS INT

            --Guard against inserting duplicate postal codes
            SELECT
                @CityId = CONVERT(VARCHAR(50), lpc.CityId),
                @PostalCode = lpc.PostalCode
            FROM	INSERTED AS i
            JOIN	LocationPostalCode AS lpc
                ON lpc.PostalCode = i.PostalCode AND lpc.CityId = i.CityId AND lpc.LocationPostalCodeId != i.LocationPostalCodeId

            IF @PostalCode IS NOT NULL
            BEGIN
                RAISERROR (N'Attempted add duplicate LocationPostalCode record for city %d: %s', 11, 1, @CityId, @PostalCode)
            END

            --Guard against setting more than one postal code as Main
            SELECT
                @CityId = i.CityId,
                @PostalCode = i.PostalCode,
                @MainPostalCode = lpc.PostalCode
            FROM INSERTED i
            JOIN	LocationPostalCode AS lpc
                        ON lpc.Main = 1 AND lpc.CityId = i.CityId AND lpc.LocationPostalCodeId != i.LocationPostalCodeId
            WHERE i.Main = 1

            IF @MainPostalCode IS NOT NULL
            BEGIN
                RAISERROR (N'Attempted add second main postal code %s for city %d with Main postal code %s', 11, 1, @PostalCode, @CityId, @MainPostalCode)
            END

            CREATE TABLE #RegionIDs (PostalCodeId INT, ThreeDigitZipCode VARCHAR(3), RegionId INT)

            INSERT INTO #RegionIDs
            SELECT
                LPC.LocationPostalCodeId,
                LEFT(PostalCode, 3),
                MIN(RD.RegionID) as RegionId
            FROM  dbo.RegionDetail AS RD
                JOIN LocationCity AS RegionLC (NOLOCK) ON RD.CityId = RegionLC.CityId
                JOIN LocationStateCountry AS RegionLSC (NOLOCK) ON RegionLC.LocationStateCountryId = RegionLSC.LocationStateCountryId
                JOIN LocationCountry AS RegionLCO (NOLOCK) ON RegionLSC.LocationCountryId = RegionLCO.LocationCountryId
            JOIN INSERTED as LPC on RD.ThreeDigitZip = LEFT(PostalCode, 3)
                JOIN LocationCity AS PostalLC (NOLOCK) ON LPC.CityId = PostalLC.CityId
                JOIN LocationStateCountry AS PostalLSC (NOLOCK) ON PostalLC.LocationStateCountryId = PostalLSC.LocationStateCountryId
                JOIN LocationCountry AS PostalLCO (NOLOCK) ON PostalLSC.LocationCountryId = PostalLCO.LocationCountryId
            WHERE RD.RegionID > 115 -- should this be restricted by region type instead of bounding region id?
                -- As an alternative to the following filtering based on region does it make more sense to restrict
                --	regions based on sharing a country? Such a restriction may make less sense in Europe if we
                --	use rate regions there
                ------------------------------------------------------------------------------
                -- Prevent usage of RegionDetails for existing postal codes that are in a different region than the one
                --  being inserted
                AND RegionLCO.GeoRegionId = PostalLCO.GeoRegionId
                -- Prevent usage of US region details for Mexican postal codes and vice versa
                AND (RegionLCO.GeoRegionId <> 1  OR RegionLCO.IsoCodeAlpha2 = PostalLCO.IsoCodeAlpha2)
            GROUP BY LPC.LocationPostalCodeId, LEFT(PostalCode, 3)

            INSERT INTO RegionDetail
              (	RegionID ,
                ZipCode ,
                ThreeDigitZip ,
                CityID ,
                City ,
                [State] ,
                StateID ,
                CreateDate ,
                CreateByUserID ,
                UpdateDate ,
                UpdateByUserID )
            SELECT
                r.RegionID ,
                a.PostalCode ,
                r.ThreeDigitZipCode ,
                a.CityId ,
                c.Name ,
                s.Code ,
                s.LocationStateId ,
                a.CreateDate ,
                a.CreateByUserId ,
                a.UpdateDate ,
                a.UpdateByUserId
            FROM    INSERTED a
                INNER JOIN #RegionIDs r ON r.PostalCodeId = a.LocationPostalCodeId
                INNER JOIN LocationCity c ON a.CityId = c.CityId
                INNER JOIN LocationStateCountry x ON c.LocationStateCountryId = x.LocationStateCountryId
                LEFT OUTER JOIN LocationState s ON x.LocationStateId = s.LocationStateId
                LEFT OUTER JOIN RegionDetail AS rd
                    ON rd.RegionID = r.RegionID AND rd.ZipCode = a.PostalCode AND rd.CityID = c.CityId
            WHERE rd.ID IS NULL

            UPDATE cit
            SET MainZipCode = lpc.PostalCode,
                UpdateDate = lpc.UpdateDate,
                UpdateByUserID = lpc.UpdateByUserId
            FROM City AS cit
            JOIN INSERTED AS lpc ON cit.ID = lpc.CityId
            WHERE cit.MainZipCode IS NULL OR cit.MainZipCode = '' --don't overwrite existing mainzipcode

            INSERT INTO ZIPCode
              (	CityID ,
                City ,
                [State] ,
                ZIPCode ,
                Latitude ,
                Longitude ,
                CreateDate ,
                CreateByUserID ,
                UpdateDate ,
                UpdateByUserID )
            SELECT
                a.CityId ,
                c.Name ,
                s.Code ,
                a.PostalCode ,
                c.Latitude ,
                c.Longitude ,
                a.CreateDate ,
                a.CreateByUserId ,
                a.UpdateDate ,
                a.UpdateByUserId
            FROM INSERTED a
            LEFT JOIN ZipCode AS zc ON zc.CityId = a.CityId AND zc.ZIPCode = a.PostalCode
            INNER JOIN LocationCity c ON a.CityId = c.CityId
            INNER JOIN LocationStateCountry x ON c.LocationStateCountryId = x.LocationStateCountryId
            LEFT OUTER JOIN LocationState s ON x.LocationStateId = s.LocationStateId
            WHERE zc.ID IS NULL

        END TRY
        BEGIN CATCH
            EXEC dbo.spClawRethrowError
        END CATCH
    END

GO

CREATE TRIGGER dbo.TR_LocationPostalCode_U ON LocationPostalCode
    FOR UPDATE
AS
    BEGIN
        BEGIN TRY
            IF UPDATE(CreateByUserID)
                BEGIN
                    IF EXISTS ( SELECT  1
                                FROM    INSERTED
                                        INNER JOIN DELETED ON INSERTED.LocationPostalCodeId = DELETED.LocationPostalCodeId
                                                              AND INSERTED.CreateByUserId != DELETED.CreateByUserId )
                        BEGIN
                            RAISERROR('CreateByUserID is not updateable', 16, 3)
                        END
                END

            IF UPDATE(UpdateByUserID)
                BEGIN
                    IF EXISTS ( SELECT  1
                                FROM    INSERTED
                                WHERE   UpdateByUserId NOT IN ( SELECT
                                                              UserID
                                                              FROM
                                                              SystemUser ) )
                        BEGIN
                            RAISERROR('Invalid UpdateByUserId', 16, 3)
                        END
                END

            DECLARE @CityId AS INT
            DECLARE @PostalCode AS VARCHAR(11)
            DECLARE @MainPostalCode AS VARCHAR(11)

            --Guard against setting more than one postal code as Main
            SELECT
                @CityId = i.CityId,
                @PostalCode = i.PostalCode,
                @MainPostalCode = lpc.PostalCode
            FROM INSERTED i
            JOIN DELETED d ON i.LocationPostalCodeId = d.LocationPostalCodeId
            JOIN	LocationPostalCode AS lpc
                        ON lpc.Main = 1 AND lpc.CityId = i.CityId AND lpc.LocationPostalCodeId != i.LocationPostalCodeId
            WHERE d.Main = 0 AND i.Main = 1

            IF @MainPostalCode IS NOT NULL
            BEGIN
                RAISERROR (N'Attempted add second main postal code %s for city %d with Main postal code %s', 11, 1, @PostalCode, @CityId, @MainPostalCode)
            END

            IF UPDATE(CityID)
                BEGIN
                    UPDATE  dbo.ZIPCode
                    SET     CityID = a.CityID ,
                            ZIPCode = a.PostalCode ,
                            UpdateDate = a.UpdateDate ,
                            UpdateByUserID = a.UpdateByUserId
                    FROM    dbo.ZIPCode z
                            INNER JOIN INSERTED a ON z.ID = a.LocationPostalCodeID
                END

            UPDATE  LocationPostalCode
            SET     UpdateDate = GETDATE()
            FROM    LocationPostalCode
                    INNER JOIN INSERTED ON LocationPostalCode.LocationPostalCodeId = INSERTED.LocationPostalCodeId

        END TRY
        BEGIN CATCH
            EXEC dbo.spClawRethrowError
        END CATCH
    END
GO

CREATE TRIGGER [dbo].[TR_LocationPostalCode_D] 
ON [dbo].[LocationPostalCode]
AFTER DELETE AS
BEGIN
	BEGIN TRY
		DELETE FROM [dbo].ZipCode
		WHERE CityID IN (SELECT CityId FROM DELETED)
		AND LTRIM(RTRIM(ZIPCode)) IN (SELECT LTRIM(RTRIM(PostalCode)) FROM DELETED)
	END TRY
	BEGIN CATCH        
		EXEC dbo.spClawRethrowError
    END CATCH
END
GO