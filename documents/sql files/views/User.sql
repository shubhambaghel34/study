/*
// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2013 - 2017
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

CREATE VIEW dbo.[User]
AS
    SELECT  SystemUser.UserID AS ID ,
            CASE
				WHEN SystemUser.UserID <= 0 THEN 57
				WHEN InternalEmployee.EmployeeID IS NOT NULL THEN 4
				ELSE 2
            END AS EntityType,
            SystemUser.UserID AS EntityID ,
            SystemUser.Code ,
            Person.FirstName ,
            Person.LastName ,
            Person.MiddleInitial AS MI ,
            0 AS [Type],
            SystemUser.CreateDate ,
            SystemUser.CreateByUserID ,
            SystemUser.WebOnly ,
            SystemUser.WebPassword ,
            SystemUser.WebLoginEnabled ,
            SystemUser.WebLastLoginDate ,
            SystemUser.UpdateDate ,
            SystemUser.UpdateByUserID ,
            SystemUser.[Password] ,
            SystemUser.PasswordReset ,
            CASE
                WHEN InternalEmployee.Active IS NOT NULL THEN InternalEmployee.Active
                ELSE CONVERT(BIT, 0)
            END AS Active ,
            CONVERT(varchar(36), '') AS WebLoginGUID ,
            SystemUser.ClawUserName,
			SystemUser.ActiveDirectoryDomainId
    FROM    dbo.SystemUser
            INNER JOIN dbo.Person ON SystemUser.UserID = Person.PersonID	   
			LEFT OUTER JOIN dbo.InternalEmployee ON SystemUser.UserID = InternalEmployee.EmployeeID
GO
