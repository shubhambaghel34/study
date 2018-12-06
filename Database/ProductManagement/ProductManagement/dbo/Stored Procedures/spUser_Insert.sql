
CREATE PROCEDURE [dbo].[spUser_Insert]
	@Code VARCHAR(10),
	@FirstName VARCHAR(100) = NULL,
	@LastName VARCHAR(100) = NULL,
	@Email VARCHAR(100) = NULL,
	@Mobile VARCHAR(13) = NULL
AS
BEGIN
 
		IF NOT EXISTS(SELECT 1 FROM [dbo].[User] WHERE Code = @Code OR Email = @Email )
		BEGIN
			  INSERT INTO [dbo].[User](
						Code,
						FirstName,
						LastName,
						Email,
						Mobile)
				OUTPUT	ISNULL(INSERTED.[Id],-1)
				VALUES(	@Code,
						@FirstName,
						@LastName,
						@Email,
						@Mobile);
		END;
END