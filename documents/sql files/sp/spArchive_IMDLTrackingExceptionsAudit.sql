/*
// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2018 - 2018
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
CREATE PROCEDURE [dbo].[spArchive_IMDLTrackingExceptionsAudit]
(
		@CreateDate DATETIME2 = NULL,
		@BatchSize INT = 10000,
		@Delay DATETIME = '00:00:10'
)
AS
BEGIN
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	DECLARE @BatchCount INT;

    IF @CreateDate IS NULL
	BEGIN
			SELECT  @CreateDate = DATEADD(YEAR, -1, GETUTCDATE());
	END

    BEGIN TRY
		SET @BatchCount = @BatchSize;
			WHILE @BatchCount = @BatchSize
				BEGIN

					DELETE TOP (@BatchSize) [dbo].[IMDLTrackingExceptionsAudit]
					OUTPUT	DELETED.[IMDLTrackingExceptionsAuditId],
							DELETED.[CreateDateUTC],
							DELETED.[CreateByUserId],
							DELETED.[ExceptionCreateDateUTC],
							DELETED.[ExceptionId],
							DELETED.[ExceptionTypeId],
							DELETED.[LoadId],
							DELETED.[IsVerified],
							DELETED.[VerifiedByUserId],
							DELETED.[VerifiedDateUTC], 
							DELETED.[ResolvedDateUTC],
							DELETED.[ResolvedByUserId],
							DELETED.[ProgressType],
							DELETED.[IMDLRepId]
					INTO [dbo].IMDLTrackingExceptionsAudit_Archive
					WHERE [CreateDateUTC] < @CreateDate;
	
				SELECT @BatchCount = @@ROWCOUNT;
				WAITFOR DELAY @Delay;
			END
    END TRY	
    BEGIN CATCH
        EXECUTE dbo.RethrowError;
    END CATCH;
END
GO

EXEC sys.sp_addextendedproperty
    @name = N'MS_Description'
  , @value = N'This store procedure is used to archive the IMDLTrackingExceptionsAudit table.'
  , @level0type = N'SCHEMA'
  , @level0name = N'dbo'
  , @level1type = N'PROCEDURE'
  , @level1name = N'spArchive_IMDLTrackingExceptionsAudit';
GO