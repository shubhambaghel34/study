CREATE PROCEDURE [dbo].[spEntity_Insert]
	@EntityType INT,
	@Name VARCHAR(10),
	@Description VARCHAR(100) = NULL
AS
BEGIN
 
		IF NOT EXISTS(SELECT 1 FROM [dbo].[Entity] WHERE Name = @Name AND EntityType = @EntityType)
		BEGIN
			  INSERT INTO [dbo].[Entity](
						EntityType,
						Name,
						[Description])
				OUTPUT	ISNULL(INSERTED.[EntityType],-1)
				VALUES(	@EntityType,
						@Name,
						@Description);
		END;
END