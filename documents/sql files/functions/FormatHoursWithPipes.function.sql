
CREATE Function [dbo].[FormatHoursWithPipes]
	(
		@datFirstTime AS datetime,
		@datSecondTime as datetime
		
	)
RETURNS varchar(11)

AS

BEGIN

	DECLARE @DateAndTime varchar(11)

	select @DateAndTime =
	   RIGHT('0' + CAST(DATEPART(hh, @datFirstTime) AS VARCHAR(2)),2) +
	   ':' +
	   RIGHT('0' + CAST(DATEPART(mi, @datFirstTime) AS VARCHAR(2)),2) +
	   '|' +
	   RIGHT('0' + CAST(DATEPART(hh, @datSecondTime) AS VARCHAR(2)),2) +
	   ':' +
	   RIGHT('0' + CAST(DATEPART(mi, @datSecondTime) AS VARCHAR(2)),2)

	RETURN @DateAndTime

END 

