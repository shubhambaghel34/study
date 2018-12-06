 
CREATE Function [dbo].[EndOfDay]
	(
		@DateTime datetime
	)
RETURNS datetime
AS

BEGIN

	SET @DateTime = DATEADD(hh, -(DATEPART(hh, @DateTime)), @DateTime)
	SET @DateTime = DATEADD(mi, -(DATEPART(mi, @DateTime)), @DateTime)	
	SET @DateTime = DATEADD(ss, -(DATEPART(ss, @DateTime)), @DateTime)

	RETURN CAST(CONVERT(VARCHAR(10),@DateTime,112) + ' 23:58:00' AS DATETIME)

END
