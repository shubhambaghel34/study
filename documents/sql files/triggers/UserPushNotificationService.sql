/*
// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2013 - 2013
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
CREATE TRIGGER TR_UserPushNotificationService_U ON [dbo].[UserPushNotificationService]
    FOR UPDATE
AS
BEGIN
	BEGIN TRY
		UPDATE UserPushNotificationService
			SET UpdateDate = GETDATE()
			FROM INSERTED
			WHERE UserPushNotificationService.UserPushNotificationServiceId = INSERTED.UserPushNotificationServiceId
	END TRY         
    BEGIN CATCH        
        EXEC dbo.spClawRethrowError
    END CATCH    
END
GO


