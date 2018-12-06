CREATE PROCEDURE [dbo].[spUser_GetAll]
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
    SET NOCOUNT ON;

	BEGIN TRY
	
        SELECT 
				Id,
				Code,
				FirstName,
				LastName,
				Email,
				Mobile
				
        FROM dbo.[User]  
		ORDER BY Id

    END TRY
    BEGIN CATCH
    END CATCH
END
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Stored procedure used to get all User.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'spUser_GetAll';

