-- // /////////////////////////////////////////////////////////////////////////////////////
-- //                           Copyright (c) 2012 - 2012
-- //                            Coyote Logistics L.L.C.
-- //                          All Rights Reserved Worldwide
-- //
-- // WARNING:  This program (or document) is unpublished, proprietary
-- // property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
-- // Unauthorized reproduction, distribution or disclosure of this program
-- // (or document), or any program (or document) derived from it is
-- // prohibited by State and Federal law, and by local law outside of the U.S.
-- // //////////////////////////////////////////////////////////////////////////////////////

CREATE Function [dbo].[FormatPhone]
	(
		 @PhoneNumber varchar(50)
	)
RETURNS varchar(50)

AS

BEGIN

	DECLARE @ReturnValue varchar(50)
	DECLARE @GeoRegion int
	SELECT @GeoRegion = SettingValue FROM SystemSettings (NOLOCK) WHERE SettingName = 'GeoRegion'

	IF @PhoneNumber IS NULL OR @PhoneNumber = '' OR @PhoneNumber = '|1|'
		RETURN ''

	DECLARE @CountryCode varchar(3)
	DECLARE @Number varchar(20)
	DECLARE @Extension varchar(10)

	DECLARE @Index int
	DECLARE @Index2 int
	SELECT @Index = CHARINDEX('|', @PhoneNumber, 2)
	
	IF @Index <= 0 RETURN @PhoneNumber

	SET @CountryCode = SUBSTRING(@PhoneNumber, 2, @Index-2)

	SELECT @Index2 = CHARINDEX('|', @PhoneNumber, @Index+1)
	SET @Number = SUBSTRING(@PhoneNumber, @Index+1, @Index2-@Index-1)

	SELECT @Index = CHARINDEX('|', @PhoneNumber, @Index2+1)
	SET @Extension = SUBSTRING(@PhoneNumber, @Index2+1, @Index-@Index2-1)

	SET @ReturnValue = ''

	IF @GeoRegion = 1
	BEGIN
		IF @CountryCode <> '1' SET @ReturnValue = '+' + @CountryCode + ' '
		IF LTRIM(RTRIM(@Number)) <> '' SET @ReturnValue = @ReturnValue + '(' + SUBSTRING(@Number, 1, 3) + ') ' + SUBSTRING(@Number, 4, 3) + '-' + SUBSTRING(@Number, 7, 4)
		IF LTRIM(RTRIM(@Extension)) <> '' SET @ReturnValue = @ReturnValue + ' x' +  @Extension
	END

	ELSE
	BEGIN
		SET @ReturnValue = '+' + @CountryCode + ' ' + @Number
		IF @Extension <> '' SET @ReturnValue = @ReturnValue + ' x' +  @Extension
	END

	RETURN @ReturnValue

END



