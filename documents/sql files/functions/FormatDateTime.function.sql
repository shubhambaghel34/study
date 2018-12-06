
CREATE Function [dbo].[FormatDateTime]
(
	@DateTime datetime,
	@Format int = 101
)
RETURNS varchar(50)
AS
BEGIN

	DECLARE @ReturnDate varchar(50)
	
	DECLARE @GeoRegion int
	SELECT @GeoRegion = SettingValue FROM SystemSettings (NOLOCK) WHERE SettingName = 'GeoRegion'
	select @ReturnDate =	
	case 
		when @DateTime <'1/1/1900' then ''
		else 		
			case @GeoRegion
				When 1 Then
					Convert(varchar(25), @DateTime, @Format) + ' ' + dbo.GetTime(@DateTime)
				else
					  Convert(varchar(25), @DateTime, 103) + ' ' + dbo.GetTime(@DateTime)
			end
		end
	RETURN @ReturnDate

END
