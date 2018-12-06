CREATE PROCEDURE [dbo].[spProducts_GetAll]
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
    SET NOCOUNT ON;

	BEGIN TRY
	
        SELECT 
				ProductId,
				Code,
				Name,
				ReleaseDate,
				[Description],
				Price,
				StarRating,
				ImageUrl
        FROM Product  
		ORDER BY ProductId

    END TRY
    BEGIN CATCH
    END CATCH
END

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Stored procedure used to get Product details.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'spProducts_GetAll';

