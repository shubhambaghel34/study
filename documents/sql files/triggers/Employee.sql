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

CREATE TRIGGER dbo.TR_Employee_I ON Employee
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
                    RAISERROR('Unable to Insert more than one Employee at a time', 16, 1)
                END
	
            INSERT  Person
                    ( CreateByUserID ,
                      UpdateByUserID ,
                      FirstName ,
                      MiddleInitial ,
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
                    MiddleInitial ,
                    LastName ,
					EmailWork ,
					OfficePhoneNumber ,
				    HomePhoneNumber ,
					MobilePhoneNumber ,
					FaxNumber
            FROM    INSERTED

            IF ( @@error != 0 ) 
                BEGIN
                    RAISERROR('Unable to Insert Employee (Person)', 16, 1)
                END
 	
            SELECT  @PersonID = SCOPE_IDENTITY()

            INSERT  InternalEmployee
                    ( EmployeeID ,
                      CreateByUserID ,
                      UpdateByUserID ,
                      Nickname ,
                      JobTitle ,
                      Division ,
                      Department ,
                      [Group] ,
                      [Status] ,
                      ManagerID ,
                      EmailAlt ,
                      EmailIM ,
                      SpouseName ,
                      BirthDate ,
                      Notes ,
                      [Language] ,
                      CommissionPercent ,
                      Photo ,
                      FaxUserID ,
                      CompanyBranchID ,
                      TerminationDate ,
                      ApplyOverMarginBonus ,
                      OverMarginBonusPercent ,
                      HireDate ,
                      OpenLoadLimit ,
					  SalesLevel ,
					  SalesStartDate ,
					  UltiproUserID,					  
					  DeskID,
					  MinimumCommission,
					  TimeZoneTypeId,
					  DoNotDisturb
                    )
            SELECT  @PersonID ,
                    CreateByUserID ,
                    UpdateByUserID ,
                    Nickname ,
                    JobTitle ,
                    Division ,
                    Department ,
                    [Group] ,
                    [Status] ,
                    ManagerID ,
                    EmailAlt ,
                    EmailIM ,
                    SpouseName ,
                    BirthDate ,
                    Notes ,
                    [Language] ,
                    CommissionPercent ,
                    Photo ,
                    FaxUserID ,
                    CompanyBranchID ,
                    TerminationDate ,
                    ApplyOverMarginBonus ,
                    OverMarginBonusPercent ,
                    HireDate ,
                    OpenLoadLimit ,
					SalesLevel ,
					SalesStartDate,
					UltiproUserID,
					DeskID,
					MinimumCommission,
					TimeZoneTypeId,
					DoNotDisturb
            FROM    INSERTED

            IF ( @@error != 0 ) 
                BEGIN
                    RAISERROR('Unable to Insert Employee (InternalEmployee)', 16, 1)
                END

            SELECT @PersonID
        END TRY         
        BEGIN CATCH        
            EXEC dbo.spClawRethrowError
        END CATCH                
    END	
GO

CREATE TRIGGER dbo.TR_Employee_U ON Employee
    INSTEAD OF UPDATE
AS
    BEGIN
        DECLARE @CurrentDatetime DATETIME
        SELECT  @CurrentDatetime = GETDATE()

        BEGIN TRY

            IF UPDATE(FirstName)
                OR UPDATE(LastName)
                OR UPDATE(MiddleInitial) 
                BEGIN
                    UPDATE  Person
                    SET     UpdateByUserID = INSERTED.UpdateByUserID ,
                            FirstName = INSERTED.FirstName ,
                            LastName = INSERTED.LastName ,
                            MiddleInitial = INSERTED.MiddleInitial
                    FROM    INSERTED
                            INNER JOIN Person ON INSERTED.ID = Person.PersonID
			  
                    IF ( @@error != 0 ) 
                        BEGIN
                            RAISERROR('Unable to upate Person.Name', 16, 1)
                        END
                END
		    
            IF UPDATE(Code) 
                BEGIN
                    UPDATE  SystemUser
                    SET     UpdateByUserID = INSERTED.UpdateByUserID ,
                            Code = INSERTED.CODE
                    FROM    INSERTED
                            INNER JOIN SystemUser ON INSERTED.ID = SystemUser.UserID
                END

			IF UPDATE(EmailWork)
				BEGIN
					UPDATE	Person
					SET		WorkEmail = INSERTED.EmailWork
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

	        UPDATE  InternalEmployee
            SET     UpdateByUserID = INSERTED.UpdateByUserID ,
                    Nickname = INSERTED.Nickname ,
                    JobTitle = INSERTED.JobTitle ,
                    Division = INSERTED.Division ,
                    Department = INSERTED.Department ,
                    [Group] = INSERTED.[Group] ,
                    [Status] = INSERTED.[Status] ,
                    ManagerID = INSERTED.ManagerID ,
                    EmailAlt = INSERTED.EmailAlt ,
                    EmailIM = INSERTED.EmailIM ,
                    SpouseName = INSERTED.SpouseName ,
                    BirthDate = INSERTED.BirthDate ,
                    Notes = INSERTED.Notes ,
                    [Language] = INSERTED.[Language] ,
                    CommissionPercent = INSERTED.CommissionPercent ,
                    Photo = INSERTED.Photo ,
                    FaxUserID = INSERTED.FaxUserID ,
                    CompanyBranchID = INSERTED.CompanyBranchID ,
                    TerminationDate = INSERTED.TerminationDate ,
                    ApplyOverMarginBonus = INSERTED.ApplyOverMarginBonus ,
                    OverMarginBonusPercent = INSERTED.OverMarginBonusPercent ,
                    HireDate = INSERTED.HireDate ,
                    OpenLoadLimit = INSERTED.OpenLoadLimit ,
					SalesLevel = INSERTED.SalesLevel ,
					SalesStartDate = INSERTED.SalesStartDate ,
					UltiproUserID = INSERTED.UltiproUserID ,
					MinimumCommission= INSERTED.MinimumCommission,
					DeskID = INSERTED.DeskID,
					TimeZoneTypeId = INSERTED.TimeZoneTypeId,
					DoNotDisturb = INSERTED.DoNotDisturb
            FROM    INSERTED
                    INNER JOIN InternalEmployee ON INSERTED.ID = InternalEmployee.EmployeeID

            IF ( @@error != 0 ) 
                BEGIN
                    RAISERROR('Unable to Update Employee (InternalEmployee)', 16, 1)
                END
        END TRY         
        BEGIN CATCH        
            EXEC dbo.spClawRethrowError
        END CATCH                
    END	
GO

CREATE TRIGGER dbo.TR_Employee_D ON Employee
    INSTEAD OF DELETE
AS
    BEGIN
        BEGIN TRY
            DELETE  InternalEmployee
            FROM    dbo.InternalEmployee
                    INNER JOIN DELETED ON InternalEmployee.EmployeeID = DELETED.ID

            DELETE  Person
            FROM    Person
                    INNER JOIN DELETED ON Person.PersonID = DELETED.ID
        END TRY         
        BEGIN CATCH        
            EXEC dbo.spClawRethrowError
        END CATCH                
    END
GO