CREATE PROCEDURE [dbo].[spAddress_GetByTypeAndId]
	@EntityType INT,
	@EntityId INT
AS
BEGIN

	SELECT 
				Id,
				@EntityId EntityId,
				@EntityType EntityType,
				City,
				[State],
				Country,
				AddressLine,
				Street,
				AddressType	
	From dbo.[Address]
	WHERE EntityId = @EntityId AND EntityType = @EntityType;
END