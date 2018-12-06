
CREATE PROCEDURE [dbo].[spProduct_DeleteByCode]
	@Code VARCHAR(10)
AS
BEGIN
	
	DELETE FROM dbo.[Product] WHERE Code = @Code;
END