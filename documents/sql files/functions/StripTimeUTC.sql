CREATE Function [dbo].[StripTimeUTC]
	(
		@DateTime DATETIME2(7)
	)
RETURNS DATETIME2(7)
AS

BEGIN

	SET @DateTime = CAST(CAST(@DateTime AS DATE) AS DATETIME2(7))

	RETURN @DateTime

END