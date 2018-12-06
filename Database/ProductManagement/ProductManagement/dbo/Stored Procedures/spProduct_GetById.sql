
CREATE PROCEDURE [dbo].[spProduct_GetById]
	@Id INT
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
	WHERE ProductId = @Id;
END