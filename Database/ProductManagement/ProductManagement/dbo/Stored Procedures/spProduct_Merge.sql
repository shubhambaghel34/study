
CREATE PROCEDURE [dbo].[spProduct_Merge]
	@Code VARCHAR(10),
	@Name VARCHAR(50) = NULL,
	@ReleaseDate DATETIME = NULL,
	@Description VARCHAR(MAX) = NULL,
	@Price MONEY = NULL,
	@StarRating DECIMAL(16,2) = NULL,
	@ImageUrl VARCHAR(500) = NULL
AS
BEGIN

	BEGIN TRAN; 
		MERGE dbo.[Product] AS [Target]
		USING (SELECT @Code AS Code) AS [Source] 
		ON [Target].Code = [Source].Code
		WHEN MATCHED THEN UPDATE SET
						Name = @Name,
						ReleaseDate = @ReleaseDate,
						[Description] = @Description,
						Price = @Price,
						StarRating = @StarRating,
						ImageUrl = @ImageUrl
		WHEN NOT MATCHED THEN  INSERT (
						Name,
						ReleaseDate,
						[Description],
						Price,
						StarRating,
						ImageUrl)
						VALUES(	
						@Name,
						@ReleaseDate,
						@Description,
						@Price,
						@StarRating,
						@ImageUrl);
	COMMIT;
END