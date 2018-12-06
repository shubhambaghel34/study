/*
// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2018 - 2018
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
CREATE VIEW [dbo].[Contact_CarrierSearch]
AS
SELECT  ExternalContact.EntityType,
        ExternalContact.EntityID ,      
        Person.FirstName ,
        Person.LastName ,
        ExternalContact.Nickname ,      
        Person.WorkEmail AS EmailAddressWork ,
        ExternalContact.EmaillAddressPersonal ,
        Person.WorkPhoneNumber AS OfficePhoneNumber ,
        Person.MobilePhoneNumber AS MobilePhoneNumber ,
        Person.HomePhoneNumber AS HomePhoneNumber ,
        ExternalContact.Main ,     
        ExternalContact.Active     
		
FROM    dbo.ExternalContact
        INNER JOIN dbo.Person ON ExternalContact.ContactID = Person.PersonID

GO