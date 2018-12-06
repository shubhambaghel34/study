


CREATE PROCEDURE [dbo].[spProduct_Upsert]
	@Code VARCHAR(10),
	@Name VARCHAR(50) = NULL,
	@ReleaseDate DATETIME = NULL,
	@Description VARCHAR(MAX) = NULL,
	@Price MONEY = NULL,
	@StarRating DECIMAL(16,2) = NULL,
	@ImageUrl VARCHAR(500) = NULL
AS
BEGIN

	DECLARE @ret INT;
	SET @ret=0;
	BEGIN TRAN; 
		IF EXISTS(SELECT 1 FROM [dbo].[Product] WHERE Code = @Code) BEGIN
			  UPDATE [dbo].[Product]
				 SET	Name = @Name,
						ReleaseDate = @ReleaseDate,
						[Description] = @Description,
						Price = @Price,
						StarRating = @StarRating,
						ImageUrl = @ImageUrl
				 WHERE Code=@Code;
			  SET @ret=@@ERROR;
		      
		END ELSE BEGIN

			 INSERT INTO [dbo].[Product](
						Code,
						Name,
						ReleaseDate,
						[Description],
						Price,
						StarRating,
						ImageUrl)
			 VALUES(	@Code,
						@Name,
						@ReleaseDate,
						@Description,
						@Price,
						@StarRating,
						@ImageUrl);
			  SET @ret=@@ERROR;
		END;
	COMMIT;
RETURN @ret;
END