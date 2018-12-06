/*
// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2015 - 2015
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
CREATE TRIGGER [dbo].[TR_LoadEvent_U] ON [dbo].[LoadEvent]
    FOR UPDATE
AS
    BEGIN
        SET NOCOUNT ON;

        BEGIN TRY
            UPDATE  [dbo].[LoadEvent]
            SET     UpdateDate = GETDATE() ,
                    UpdateDateUTC = GETUTCDATE()
            FROM    INSERTED
            WHERE   [dbo].[LoadEvent].LoadEventId = INSERTED.LoadEventId;
        END TRY

        BEGIN	CATCH
            EXEC dbo.spClawRethrowError;
        END CATCH
    END
GO