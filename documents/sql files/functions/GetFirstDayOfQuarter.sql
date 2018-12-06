/*
// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2013 - 2013
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
CREATE FUNCTION [dbo].[GetFirstDayOfQuarter] ( @inputDate DATETIME )
RETURNS DATE
    BEGIN
        RETURN CAST(YEAR(@inputDate) AS VARCHAR(4)) + '/'
            + CAST(DATEPART(Q, @inputDate) * 3 - 2 AS VARCHAR(2)) + '/01'
    END
GO