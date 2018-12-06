CREATE FUNCTION [dbo].[GetFirstDayOfMonth] ( @inputDate DATETIME )
RETURNS DATETIME
    BEGIN
        RETURN CAST(FLOOR(CAST(@inputDate AS DECIMAL(12, 5))) - 
               (DAY(@inputDate) - 1) AS DATETIME)
    END
