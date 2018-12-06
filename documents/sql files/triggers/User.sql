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

CREATE TRIGGER dbo.TR_User_I ON [User]
    INSTEAD OF INSERT
AS
    BEGIN
        DECLARE @RowCount INT

        BEGIN TRY
            SELECT  @RowCount = COUNT(*)
            FROM    INSERTED
	
            IF ( @RowCount > 1 ) 
                BEGIN
                    RAISERROR('Unable to Insert more than one User at a time', 16, 1)
                END
	
            INSERT  SystemUser
                    ( UserID ,
                      CreateByUserID ,
                      UpdateByUserID ,
                      Code ,
                      WebOnly ,
                      WebPassword ,
                      WebLoginEnabled ,
                      WebLastLoginDate ,
                      [Password] ,
                      PasswordReset ,
                      ClawUserName,
					  ActiveDirectoryDomainId
                    )
                SELECT  EntityID ,
                        CreateByUserID ,
                        CreateByUserID ,
                        Code ,
                        WebOnly ,
                        WebPassword ,
                        WebLoginEnabled ,
                        WebLastLoginDate ,
                        [Password] ,
                        PasswordReset ,
                        ClawUserName,
						ActiveDirectoryDomainId
                FROM    INSERTED
                
                IF UPDATE(Active)
                    BEGIN
                        UPDATE InternalEmployee
                        SET    Active = INSERTED.Active
                        FROM   INSERTED
                               INNER JOIN InternalEmployee on INSERTED.EntityID = InternalEmployee.EmployeeID
                    END

                IF UPDATE(FirstName)
                    OR UPDATE(LastName)
                    OR UPDATE(MI) 
                    BEGIN
                        UPDATE  Person
                        SET     FirstName = INSERTED.FirstName ,
                                LastName = INSERTED.LastName ,
                                MiddleInitial = INSERTED.MI
                        FROM    INSERTED
                                INNER JOIN Person ON INSERTED.EntityID = Person.PersonID
        			  
                        IF ( @@error != 0 ) 
                            BEGIN
                                RAISERROR('Unable to upate Person.Name', 16, 1)
                            END
                    END
            SELECT EntityID
            FROM   INSERTED
        END TRY         
        BEGIN CATCH        
            EXEC dbo.spClawRethrowError
        END CATCH                
    END
GO	

CREATE TRIGGER dbo.TR_User_U ON [User]
    INSTEAD OF UPDATE
AS
    BEGIN
        BEGIN TRY
            UPDATE  SystemUser
            SET     UpdateByUserID = INSERTED.UpdateByUserID ,
                    Code = INSERTED.Code ,
                    WebOnly = INSERTED.WebOnly ,
                    WebPassword = INSERTED.WebPassword ,
                    WebLoginEnabled = INSERTED.WebLoginEnabled ,
                    WebLastLoginDate = INSERTED.WebLastLoginDate ,
                    [Password] = INSERTED.[Password] ,
                    PasswordReset = INSERTED.PasswordReset ,
                    ClawUserName = INSERTED.ClawUserName
            FROM    dbo.SystemUser
                    INNER JOIN INSERTED ON SystemUser.UserID = INSERTED.EntityID

            IF UPDATE(Active)
                BEGIN
                    UPDATE InternalEmployee
                    SET    Active = INSERTED.Active
                    FROM   INSERTED
                           INNER JOIN InternalEmployee on INSERTED.EntityID = InternalEmployee.EmployeeID
                END

            IF UPDATE(FirstName)
                OR UPDATE(LastName)
                OR UPDATE(MI) 
                BEGIN
                    UPDATE  Person
                    SET     UpdateByUserID = INSERTED.UpdateByUserID ,
                            FirstName = INSERTED.FirstName ,
                            LastName = INSERTED.LastName ,
                            MiddleInitial = INSERTED.MI
                    FROM    INSERTED
                            INNER JOIN Person ON INSERTED.EntityID = Person.PersonID
    			  
                    IF ( @@error != 0 ) 
                        BEGIN
                            RAISERROR('Unable to upate Person.Name', 16, 1)
                        END
                END
        END TRY         
        BEGIN CATCH        
            EXEC dbo.spClawRethrowError
        END CATCH                
    END
GO	

CREATE TRIGGER dbo.TR_User_D ON [User]
    INSTEAD OF DELETE
AS
    BEGIN
        BEGIN TRY
            DELETE  SystemUser
            FROM    dbo.SystemUser
                    INNER JOIN DELETED ON SystemUser.UserID = DELETED.EntityID
        END TRY         
        BEGIN CATCH        
            EXEC dbo.spClawRethrowError
        END CATCH                
    END
GO	
