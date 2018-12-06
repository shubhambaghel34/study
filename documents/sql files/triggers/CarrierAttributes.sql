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
CREATE TRIGGER [dbo].[TR_CarrierAttributes_U]
	ON [dbo].[CarrierAttributes]
	FOR UPDATE
	AS
	BEGIN
		BEGIN TRY

			SET NOCOUNT ON
			UPDATE dbo.CarrierAttributes 
			SET UpdateDateUTC = GETUTCDATE(),
				UpdateDate = GETDATE()
			FROM INSERTED 
			WHERE dbo.CarrierAttributes.CarrierId = INSERTED.CarrierId

		END TRY
		BEGIN CATCH
			 EXEC dbo.spClawRethrowError
		END CATCH
	END
GO
