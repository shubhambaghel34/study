
CREATE Function GetFirstDayofCurrentMonth()
RETURNS datetime

AS

BEGIN

	RETURN CAST(RIGHT(CONVERT(VARCHAR(4), datepart(yy, getdate())), 2) + RIGHT('0' + CAST(MONTH(getdate()) as VARCHAR(2)), 2) + '01' + ' 00:00:00' AS DATETIME)

END

