/*
// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2016 - 2016
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
BEGIN
    BEGIN TRY
  
        DECLARE @MaxCustomerID        INT;
        DECLARE @BatchSize    INT = 10000;
        DECLARE @BatchStartID INT;

		SELECT  @MaxCustomerID = (SELECT  MAX(ID) FROM [dbo].[Customer])
		FROM    [dbo].[Customer];
		
		SELECT  @BatchStartID = (SELECT MIN (ID) FROM [dbo].[Customer])
		FROM    [dbo].[Customer];

		--Total number of records updated 129957
		WHILE @BatchStartID < @MaxCustomerID 
		BEGIN
			BEGIN TRANSACTION Customer_Division

			UPDATE TOP (@BatchSize) Cust
			SET 	Division = 1		--Division 1 = COY
					FROM [dbo].[Customer] Cust 
					WHERE Cust.ID BETWEEN ( @BatchStartID )
					AND ( @BatchStartID + (@BatchSize - 1)); 
		  
			 COMMIT TRANSACTION Customer_Division  			                           
			 SET @BatchStartID = ( @BatchStartID + @BatchSize );
		END
					 
	   PRINT '';
	   PRINT 'Processing Complete';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 
            ROLLBACK TRANSACTION Customer_Division
        EXECUTE dbo.spClawRethrowError;
    END CATCH
END
GO

