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

CREATE TRIGGER TR_Contact_I ON Contact
    INSTEAD OF INSERT
AS
    BEGIN
        DECLARE @RowCount INT
        DECLARE @PersonID INT

        BEGIN TRY
            SELECT  @RowCount = COUNT(*)
            FROM    INSERTED
	
            IF ( @RowCount > 1 ) 
                BEGIN
                    RAISERROR('Unable to Insert more than one Contact at a time', 16, 1)
                END
	
            INSERT  Person
                    ( CreateByUserID ,
                      UpdateByUserID ,
                      FirstName ,
                      LastName ,
					  WorkEmail ,
					  WorkPhoneNumber ,
					  HomePhoneNumber ,
					  MobilePhoneNumber ,
					  FaxPhoneNumber
                    )
            SELECT  CreateByUserID ,
                    CreateByUserID ,
                    FirstName ,
                    LastName ,
                    EmailAddressWork ,
                    OfficePhoneNumber ,
                    HomePhoneNumber ,
                    MobilePhoneNumber ,
                    FaxNumber
            FROM    INSERTED

            IF ( @@error != 0 ) 
                BEGIN
                    RAISERROR('Unable to Insert Contact (Person)', 16, 1)
                END
 	
            SELECT  @PersonID = SCOPE_IDENTITY()
            
            INSERT  ExternalContact
                    ( ContactID ,
                      EntityType ,
                      EntityID ,
                      ContactType ,
                      Nickname ,
                      BirthDate ,
                      AnniversaryDate ,
                      Notes ,
                      EmaillAddressPersonal ,
                      SpouseName ,
                      CreateByUserID ,
                      JobTitle ,
                      Division ,
                      Department ,
                      [Group] ,
                      Company ,
                      Main ,
                      [Language] ,
                      RegionNotes ,
                      UpdateByUserID ,
                      LoadStatusNotify ,
                      JobTitleString ,
                      Documentation ,
                      Pricing ,
                      AfterHours ,
                      Active ,
                      ManagedCustomerID ,
                      ReceiveMarketingUpdates,
					  ImageFileHost,
					  ImageFilePath,
					  SpotApprover,
					  ReportsTo
                    )
            SELECT  @PersonID ,
                    EntityType ,
                    EntityID ,
                    ContactType ,
                    Nickname ,
                    BirthDate ,
                    AnniversaryDate ,
                    Notes ,
                    EmaillAddressPersonal ,
                    SpouseName ,
                    CreateByUserID ,
                    JobTitle ,
                    Division ,
                    Department ,
                    [Group] ,
                    Company ,
                    Main ,
                    [Language] ,
                    RegionNotes ,
                    UpdateByUserID ,
                    LoadStatusNotify ,
                    JobTitleString ,
                    Documentation ,
                    Pricing ,
                    AfterHours ,
                    Active ,
                    ManagedCustomerID ,
                    ReceiveMarketingUpdates,
					ImageFileHost,
					ImageFilePath,
					SpotApprover,
					ReportsTo
            FROM    INSERTED

            IF ( @@error != 0 ) 
                BEGIN
                    RAISERROR('Unable to Insert Contact', 16, 1)
                END
            
            SELECT @PersonID
        END TRY         
        BEGIN CATCH        
            EXEC dbo.spClawRethrowError
        END CATCH                
    END
GO

CREATE TRIGGER TR_Contact_U ON Contact
    INSTEAD OF UPDATE
AS
    BEGIN
        DECLARE @CurrentDatetime DATETIME
        SELECT  @CurrentDatetime = GETDATE()

        BEGIN TRY
            IF UPDATE(FirstName)
                OR UPDATE(LastName) 
                BEGIN
                    UPDATE  Person
                    SET     UpdateByUserID = INSERTED.UpdateByUserID ,
                            FirstName = INSERTED.FirstName ,
                            LastName = INSERTED.LastName
                    FROM    INSERTED
                            INNER JOIN Person ON INSERTED.ID = Person.PersonID
			  
                    IF ( @@error != 0 ) 
                        BEGIN
                            RAISERROR('Unable to upate Person.Name', 16, 1)
                        END
                END

			IF UPDATE(EmailAddressWork)
				BEGIN
					UPDATE	Person
					SET		WorkEmail = INSERTED.EmailAddressWork
					FROM	Person inner join INSERTED on Person.PersonID = INSERTED.ID
				END
	
			IF UPDATE(OfficePhoneNumber)
				BEGIN
					UPDATE	Person
					SET		WorkPhoneNumber = INSERTED.OfficePhoneNumber
					FROM	Person inner join INSERTED on Person.PersonID = INSERTED.ID
				END
	
				IF UPDATE(HomePhoneNumber)
				BEGIN
					UPDATE	Person
					SET		HomePhoneNumber = INSERTED.HomePhoneNumber
					FROM	Person inner join INSERTED on Person.PersonID = INSERTED.ID
				END

			IF UPDATE(MobilePhoneNumber)
				BEGIN
					UPDATE	Person
					SET		MobilePhoneNumber = INSERTED.MobilePhoneNumber
					FROM	Person inner join INSERTED on Person.PersonID = INSERTED.ID
				END

			IF UPDATE(FaxNumber)
				BEGIN
					UPDATE	Person
					SET		FaxPhoneNumber = INSERTED.FaxNumber
					FROM	Person inner join INSERTED on Person.PersonID = INSERTED.ID
				END

			IF UPDATE(Active)
				BEGIN
					UPDATE	EC
					SET		ReportsTo = 0
					FROM	dbo.[ExternalContact] AS EC
							INNER JOIN INSERTED ON EC.EntityType = INSERTED.EntityType 
							AND EC.EntityID = INSERTED.EntityID
					WHERE	INSERTED.Active = 0
							AND EC.ReportsTo = INSERTED.ID;
				END

            UPDATE  ExternalContact
            SET     EntityType = INSERTED.EntityType ,
                    EntityID = INSERTED.EntityID ,
                    ContactType = INSERTED.ContactType ,
                    Nickname = INSERTED.Nickname ,
                    BirthDate = INSERTED.BirthDate ,
                    AnniversaryDate = INSERTED.AnniversaryDate ,
                    Notes = INSERTED.Notes ,
                    EmaillAddressPersonal = INSERTED.EmaillAddressPersonal ,
                    SpouseName = INSERTED.SpouseName ,
                    JobTitle = INSERTED.JobTitle ,
                    Division = INSERTED.Division ,
                    Department = INSERTED.Department ,
                    [Group] = INSERTED.[Group] ,
                    Company = INSERTED.Company ,
                    Main = INSERTED.Main ,
                    [Language] = INSERTED.[Language] ,
                    RegionNotes = INSERTED.RegionNotes ,
                    UpdateByUserID = INSERTED.UpdateByUserID ,
                    LoadStatusNotify = INSERTED.LoadStatusNotify ,
                    JobTitleString = INSERTED.JobTitleString ,
                    Documentation = INSERTED.Documentation ,
                    Pricing = INSERTED.Pricing ,
                    AfterHours = INSERTED.AfterHours ,
                    Active = INSERTED.Active ,
                    ManagedCustomerID = INSERTED.ManagedCustomerID ,
                    ReceiveMarketingUpdates = INSERTED.ReceiveMarketingUpdates,
					ImageFileHost = INSERTED.ImageFileHost,
					ImageFilePath = INSERTED.ImageFilePath,
					SpotApprover = INSERTED.SpotApprover,
					ReportsTo = INSERTED.ReportsTo
            FROM    ExternalContact
                    INNER JOIN INSERTED ON ExternalContact.ContactID = Inserted.ID
            IF ( @@error != 0 ) 
                BEGIN
                    RAISERROR('Unable to Update Contact', 16, 1)
                END
        END TRY         
        BEGIN CATCH        
            EXEC dbo.spClawRethrowError
        END CATCH                
    END
GO

CREATE TRIGGER TR_Contact_D ON Contact
    INSTEAD OF DELETE
AS
    BEGIN
        BEGIN TRY

			UPDATE	EC
			SET		ReportsTo = 0
			FROM	dbo.[ExternalContact] AS EC
					INNER JOIN DELETED ON EC.EntityType = DELETED.EntityType 
					AND EC.EntityID = DELETED.EntityID
			WHERE	EC.ReportsTo = DELETED.ID;

            DELETE  ExternalContact
            FROM    ExternalContact
                    INNER JOIN DELETED ON ExternalContact.ContactID = DELETED.ID;
    		
            DELETE  Person
            FROM    Person
                    INNER JOIN DELETED ON Person.PersonID = DELETED.ID;
        END TRY         
        BEGIN CATCH        
            EXEC dbo.spClawRethrowError
        END CATCH                
    END
GO
