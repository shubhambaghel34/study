/*
// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2015 - 2017
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

CREATE VIEW dbo.Contact
AS
SELECT  ExternalContact.ContactID AS ID ,
        ExternalContact.EntityType ,
        ExternalContact.EntityID ,
        ExternalContact.ContactType ,
        Person.FirstName ,
        Person.LastName ,
        ExternalContact.Nickname ,
        ExternalContact.BirthDate ,
        ExternalContact.AnniversaryDate ,
        ExternalContact.Notes ,
        Person.WorkEmail AS EmailAddressWork ,
        ExternalContact.EmaillAddressPersonal ,
        Person.WorkPhoneNumber AS OfficePhoneNumber ,
        Person.MobilePhoneNumber AS MobilePhoneNumber ,
        Person.HomePhoneNumber AS HomePhoneNumber ,
        Person.FaxPhoneNumber AS FaxNumber ,
        ExternalContact.SpouseName ,
        ExternalContact.CreateDate ,
        ExternalContact.CreateByUserID ,
        ExternalContact.JobTitle ,
        ExternalContact.Division ,
        ExternalContact.Department ,
        ExternalContact.[Group] ,
        ExternalContact.Company ,
        ExternalContact.Main ,
        ExternalContact.[Language] ,
        ExternalContact.RegionNotes ,
        CASE 
			 WHEN SystemUser.UserID IS NOT NULL THEN SystemUser.UserID
             ELSE 0
        END AS UserID,
        ExternalContact.UpdateDate ,
        ExternalContact.UpdateByUserID ,
        ExternalContact.LoadStatusNotify ,
        ExternalContact.JobTitleString ,
        ExternalContact.Documentation ,
        ExternalContact.Pricing ,
        ExternalContact.AfterHours ,
        ExternalContact.Active ,
        ExternalContact.ManagedCustomerID ,
        ExternalContact.ReceiveMarketingUpdates ,
        ExternalContact.ImageFileHost ,
        ExternalContact.ImageFilePath ,
        ExternalContact.SpotApprover ,
        ExternalContact.IsAccessorialApprover ,
        PromotedByUserInfo.FirstName + ' ' + PromotedByUserInfo.LastName AS PromotedByUser ,
        SystemUser.UpdateDate AS PromotedToClawDate ,
        UserApplication.AcceptedTermsDate AS AcceptedCoyoteGoDate ,
		ExternalContact.ReportsTo
FROM    dbo.ExternalContact
        INNER JOIN dbo.Person ON ExternalContact.ContactID = Person.PersonID
        LEFT  JOIN dbo.SystemUser ON SystemUser.UserID = Person.PersonID
        LEFT  JOIN dbo.Person PromotedByUserInfo ON SystemUser.UpdateByUserID = PromotedByUserInfo.PersonID
        LEFT  JOIN dbo.UserApplication ON ExternalContact.ContactID = UserApplication.UserID;
GO
