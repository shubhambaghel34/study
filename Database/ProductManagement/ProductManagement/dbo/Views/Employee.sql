CREATE VIEW dbo.Employee
AS
    SELECT  sysu.Id,
            sysu.Code,
            sysu.Email
            
    FROM    dbo.SystemUser sysu
