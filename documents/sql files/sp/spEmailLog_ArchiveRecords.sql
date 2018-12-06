/*
// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2015 - 2016
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
CREATE PROCEDURE [dbo].[spEmailLog_ArchiveRecords]
    (
      @DaysToKeep INT ,
      @BatchSize INT ,
      @Delay DATETIME
    )
AS
    BEGIN
        SET NOCOUNT ON;

        DECLARE @ArchivePurgeCriteria DATETIME;
        DECLARE @BatchCount INT;

        SET @ArchivePurgeCriteria = DATEADD(DAY, -@DaysToKeep, GETDATE()); 
        SET @ArchivePurgeCriteria = CAST(CAST(@ArchivePurgeCriteria AS DATE) AS DATETIME);
        SET @BatchCount = @BatchSize;

        WHILE @BatchCount = @BatchSize
            BEGIN
                DELETE TOP ( @BatchSize )
                FROM    dbo.EmailLog
                OUTPUT  Deleted.ID ,
                        Deleted.EmailType ,
                        Deleted.EntityType ,
                        Deleted.EntityID ,
                        Deleted.ToAddress ,
                        Deleted.FromUserID ,
                        Deleted.[Subject] ,
                        Deleted.Body ,
                        Deleted.Attachments ,
                        Deleted.CreateDate ,
                        Deleted.CreateByUserID ,
                        Deleted.UpdateDate ,
                        Deleted.UpdateByUserID
                        INTO dbo.EmailLog_Archive ( ID, EmailType, EntityType,
                                                    EntityID, ToAddress,
                                                    FromUserID, [Subject],
                                                    Body, Attachments,
                                                    CreateDate, CreateByUserID,
                                                    UpdateDate, UpdateByUserID )
                WHERE   CreateDate < @ArchivePurgeCriteria;

                SELECT  @BatchCount = @@ROWCOUNT;

                IF @BatchCount = @BatchSize
				BEGIN
                    WAITFOR DELAY @Delay;
				END
            END
    END
GO

EXEC sys.sp_addextendedproperty
	@name = N'MS_Description'
  , @value = N'Archives any EmailLog records older than current timestamp + DaysToKeep param.'
  , @level0type = N'SCHEMA'
  , @level0name = N'dbo'
  , @level1type = N'PROCEDURE'
  , @level1name = N'spEmailLog_ArchiveRecords';
GO