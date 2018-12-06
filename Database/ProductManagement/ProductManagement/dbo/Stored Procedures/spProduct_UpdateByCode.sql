
CREATE PROCEDURE [dbo].[spProduct_UpdateByCode]
	@Code VARCHAR(10),
	@Name VARCHAR(50) = NULL,
	@ReleaseDate DATETIME = NULL,
	@Description VARCHAR(MAX) = NULL,
	@Price MONEY = NULL,
	@StarRating DECIMAL(16,2) = NULL,
	@ImageUrl VARCHAR(500) = NULL
AS
BEGIN	
		IF EXISTS(SELECT 1 FROM [dbo].[Product] WHERE Code = @Code) BEGIN
			  UPDATE [dbo].[Product]
				 SET	Name = @Name,
						ReleaseDate = @ReleaseDate,
						[Description] = @Description,
						Price = @Price,
						StarRating = @StarRating,
						ImageUrl = @ImageUrl
				 WHERE Code=@Code;
		END;
END