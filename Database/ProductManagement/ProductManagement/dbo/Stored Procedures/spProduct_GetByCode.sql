CREATE PROCEDURE [dbo].[spProduct_GetByCode]
	@Code varchar(10)
AS
BEGIN
	SELECT 
				ProductId,
				Code,
				Name,
				ReleaseDate,
				[Description],
				Price,
				StarRating,
				ImageUrl	
	From Product
	WHERE Code = @Code;
END
GO
