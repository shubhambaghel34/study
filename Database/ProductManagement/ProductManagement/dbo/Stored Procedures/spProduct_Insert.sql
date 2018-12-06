CREATE PROCEDURE [dbo].[spProduct_Insert]
	@Code VARCHAR(10),
	@Name VARCHAR(50) = NULL,
	@ReleaseDate DATETIME = NULL,
	@Description VARCHAR(MAX) = NULL,
	@Price MONEY = NULL,
	@StarRating DECIMAL(16,2) = NULL,
	@ImageUrl VARCHAR(500) = NULL
AS
BEGIN
 
		IF NOT EXISTS(SELECT 1 FROM [dbo].[Product] WHERE Code = @Code) BEGIN
			  INSERT INTO [dbo].[Product](
						Code,
						Name,
						ReleaseDate,
						[Description],
						Price,
						StarRating,
						ImageUrl)
				OUTPUT	ISNULL(INSERTED.[ProductId], -1)
				VALUES(	@Code,
						@Name,
						@ReleaseDate,
						@Description,
						@Price,
						@StarRating,
						@ImageUrl);
		END;
END