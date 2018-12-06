CREATE PROCEDURE [dbo].[spAddress_Insert]
	@EntityType INT,
	@EntityId INT,
	@City VARCHAR(100) = NULL,
	@State VARCHAR(100) = NULL,
	@Country VARCHAR(100) = NULL,
	@AddressLine VARCHAR(100) = NULL,
	@Street VARCHAR(100) = NULL,
	@AddressType INT = NULL
AS
BEGIN
 
		IF NOT EXISTS(SELECT 1 FROM [dbo].[Address] WHERE EntityType = @EntityType AND EntityId = @EntityId )
		BEGIN
			  INSERT INTO [dbo].[Address](
						EntityType,
						EntityId,
						City,
						[State],
						Country,
						AddressLine,
						Street,
						AddressType)
				OUTPUT	ISNULL(INSERTED.[Id], -1)
				VALUES(	@EntityType,
						@EntityId,
						@City,
						@State,
						@Country,
						@AddressLine,
						@Street,
						@AddressType);
		END;
END