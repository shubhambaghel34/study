/*
// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2014 - 2014
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
CREATE TRIGGER [dbo].[TR_ExternalLoadPostCredential_U] ON [dbo].[ExternalLoadPostCredential]
    FOR UPDATE
AS
    BEGIN
        BEGIN TRY
            SET NOCOUNT ON
            UPDATE  [dbo].[ExternalLoadPostCredential]
            SET     [UpdateDateUTC] = GETUTCDATE() ,
                    [UpdateDate] = GETDATE()
            FROM    INSERTED
            WHERE   [dbo].[ExternalLoadPostCredential].[InternalEmployeeId] = INSERTED.InternalEmployeeId
        END TRY
        BEGIN CATCH
            EXEC dbo.spClawRethrowError
        END CATCH
    END
GO