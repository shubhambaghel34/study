CREATE Function [dbo].[GetHourMin]
	(
		 @Date datetime
	)
RETURNS varchar(50)
AS
BEGIN

	DECLARE @ReturnDate varchar(50)
	
	select @ReturnDate =right('0' + Convert(varchar(2), DATEPART(hh, @Date)), 2) + ':' +  right('0' + Convert(varchar(5),DATEPART(mi, @Date)), 2)  

	RETURN @ReturnDate
END