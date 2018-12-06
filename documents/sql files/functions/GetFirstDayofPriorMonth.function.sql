
CREATE Function [dbo].[GetFirstDayofPriorMonth]()
RETURNS datetime

AS

BEGIN

	RETURN (dateadd(month, datediff(month, -1, GETDATE()) - 2, -1) + 1)
END
