CREATE Function [dbo].[FormatDate]
	(
		 @Date datetime
	)
RETURNS varchar(50)

AS

BEGIN

	DECLARE @ReturnDate varchar(50)
	select @ReturnDate =	
	case 
		when @Date <'1/1/1900' then ''
		else 		
			--Convert(varchar(25), @Date, 101)
			Convert(varchar(25), @Date, 103)
		end
	RETURN @ReturnDate
END