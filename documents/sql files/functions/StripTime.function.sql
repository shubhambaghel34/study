/*
// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2015 - 2015
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////
*/

CREATE Function [dbo].[StripTime]
	(
		@DateTime datetime
	)
RETURNS datetime
AS

BEGIN

	SET @DateTime = CAST(CAST(@DateTime AS DATE) AS DATETIME)

	RETURN @DateTime

END

