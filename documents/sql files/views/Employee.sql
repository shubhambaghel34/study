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

CREATE VIEW dbo.Employee
AS
    SELECT  InternalEmployee.EmployeeID AS ID ,
            SystemUser.Code ,
            CASE
                WHEN SystemUser.UserID IS NOT NULL THEN SystemUser.UserID
                ELSE 0
            END as UserID,
            InternalEmployee.CreateDate ,
            InternalEmployee.CreateByUserID ,
            Person.FirstName ,
            Person.LastName ,
            Person.MiddleInitial ,
            InternalEmployee.Nickname ,
            InternalEmployee.JobTitle ,
            InternalEmployee.Division ,
            InternalEmployee.Department ,
            InternalEmployee.[Group] ,
            InternalEmployee.[Status] ,
            InternalEmployee.ManagerID ,
            Person.WorkEmail AS EmailWork ,
            InternalEmployee.EmailAlt ,
            InternalEmployee.EmailIM ,
            InternalEmployee.SpouseName ,
            InternalEmployee.BirthDate ,
            Person.WorkPhoneNumber AS OfficePhoneNumber ,
            Person.HomePhoneNumber AS HomePhoneNumber ,
            Person.MobilePhoneNumber AS MobilePhoneNumber ,
            Person.FaxPhoneNumber AS FaxNumber ,
            InternalEmployee.Notes ,
            InternalEmployee.UpdateDate ,
            InternalEmployee.UpdateByUserID ,
            InternalEmployee.[Language] ,
            InternalEmployee.CommissionPercent ,
            InternalEmployee.Photo ,
            InternalEmployee.FaxUserID ,
            InternalEmployee.CompanyBranchID ,
            InternalEmployee.TerminationDate ,
            InternalEmployee.ApplyOverMarginBonus ,
            InternalEmployee.OverMarginBonusPercent ,
            InternalEmployee.HireDate ,
            InternalEmployee.OpenLoadLimit ,
			InternalEmployee.SalesLevel ,
			InternalEmployee.SalesStartDate,
			InternalEmployee.UltiproUserID,
			InternalEmployee.DeskID,
			InternalEmployee.Active,
			InternalEmployee.TimeZoneTypeId,
			InternalEmployee.MinimumCommission,
			InternalEmployee.DoNotDisturb,
			JobFunction.Code AS JobFunctionString,
			SystemUser.ActiveDirectoryDomainId 
    FROM    dbo.InternalEmployee
            INNER JOIN dbo.Person ON InternalEmployee.EmployeeID = Person.PersonID
            LEFT OUTER JOIN dbo.SystemUser ON InternalEmployee.EmployeeID = SystemUser.UserID
			LEFT OUTER JOIN dbo.JobTitle ON InternalEmployee.JobTitle = JobTitle.JobTitleID
			LEFT OUTER JOIN dbo.JobFunction ON JobTitle.JobFunctionID = JobFunction.JobFunctionID
GO
