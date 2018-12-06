CREATE PROCEDURE [dbo].[spUser_GetByCode]
	@Code varchar(10)
AS
BEGIN
	SELECT 
				Id,
				Code,
				FirstName,
				LastName,
				Mobile,
				Email					
	From dbo.[User]
	WHERE Code = @Code;
END