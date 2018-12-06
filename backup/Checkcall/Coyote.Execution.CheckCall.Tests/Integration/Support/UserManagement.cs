// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2016 - 2017
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////

namespace Coyote.Execution.CheckCall.Tests.Integration.Support
{
    using System;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using DM = Coyote.Execution.CheckCall.Domain.Models;
    using Coyote.Common.Extensions;

    public static class UserManagement
    {
        private class Rep
        {
            public int Id { get; set; }
            public bool Main { get; set; }
            public EmployeeUser Employee { get; set; }
        }

        private class EmployeeUser
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string EmailAddress { get; set; }
            public string WorkPhoneNumber { get; set; }
        }

        public static DM.Rep AddCarrierRep(DM.Carrier carrier, bool isMain)
        {
            var randoms = new Randoms();

            var emp = new EmployeeUser()
            {
                FirstName = randoms.FirstName,
                LastName = randoms.LastName,
                EmailAddress = randoms.EmailAddress
            };
            AddEmployeeToBazooka(emp);

            var rep = new Rep()
            {
                Main = isMain,
                Employee = emp
            };

            CreateNewCarrierRep(rep, carrier, isMain);

            return new DM.Rep()
            {
                Id = rep.Id,
                Main = rep.Main,
                EmployeeId = emp.Id
            };
        }

        public static void RemoveCarrierRep(DM.Rep rep)
        {
            rep.ThrowIfNull("rep");
                
            DeleteAllConnections(rep.EmployeeId);

            ConnectionStringSettings bazookaConnectionString = ConfigurationManager.ConnectionStrings["Bazooka"];
            using (var sqlConnection = new SqlConnection(bazookaConnectionString.ConnectionString))
            {
                sqlConnection.Open();

                string removeEmployeeSqls =
                        "delete from [dbo].[Rep] where EmployeeID = @ID;" +
                        "delete from [dbo].[InternalEmployee] where EmployeeID = @ID; ";// + 
                       // "delete from [dbo].[SystemUser] where UserID = @ID; " + 
                       // "delete from [dbo].[Person] where PersonID = @ID;";
                       //these deletes take to long so they are removed for performance reason, but wish they were still being run

                using (var cmd = new SqlCommand(removeEmployeeSqls, sqlConnection))
                {
                    cmd.CommandTimeout = 150;
                    cmd.Parameters.AddWithValue("@ID", rep.EmployeeId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void DeleteAllConnections(int userID)
        {
            ConnectionStringSettings bazookaConnectionString = ConfigurationManager.ConnectionStrings["Bazooka"];
            using (var sqlConnection = new SqlConnection(bazookaConnectionString.ConnectionString))
            {
                sqlConnection.Open();

                string connection = @"DELETE FROM [UserCarrier] WHERE [UserId] = @UserId;";
                connection += @"DELETE FROM [UserCustomer] WHERE [UserId] = @UserId;";
                connection += @"DELETE FROM [UserFacility] WHERE [UserId] = @UserId;";

                using (var cmd = new SqlCommand(connection, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@UserId", userID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void AddEmployeeToBazooka(EmployeeUser employee)
        {

            var randoms = new Randoms();

            // This is a work around until a Add/Remove/Get externalUser service exists in CLAW
            ConnectionStringSettings bazookaConnectionString = ConfigurationManager.ConnectionStrings["Bazooka"];
            using (var sqlConnection = new SqlConnection(bazookaConnectionString.ConnectionString))
            {
                sqlConnection.Open();

                employee.WorkPhoneNumber = "|1|3125551212||";

                string insertEmployee = "declare @UserID int; " +
                                        "insert [dbo].[Person] ( CreateByUserID, UpdateByUserID, FirstName, LastName, WorkEmail, WorkPhoneNumber ) " +
                                        "values ( 0, 0, @FirstName, @LastName, @WorkEmail, @WorkPhoneNumber ); select @UserId = SCOPE_IDENTITY(); " +
                                        "insert [dbo].[SystemUser] ( UserID, CreateByUserID, UpdateByUserID, Code, ClawuserName) " +
                                        "values ( @UserID, 0, 0, @Code, @ClawUserName); " +
                                        "insert [dbo].[InternalEmployee]( EmployeeID, CreateByUserID, UpdateByUserID, TerminationDate, Active ) " +
                                        "values ( @UserID, 0, 0 , @TerminationDate, 1 ); " +
                                        "select @UserID as NewUserId;";

                using (var cmd = new SqlCommand(insertEmployee, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                    cmd.Parameters.AddWithValue("@WorkEmail", employee.EmailAddress);
                    cmd.Parameters.AddWithValue("@WorkPhoneNumber", employee.WorkPhoneNumber);
                    cmd.Parameters.AddWithValue("@ClawUserName", randoms.ClawLogOnUserName);
                    cmd.Parameters.AddWithValue("@Code", randoms.ClawLogOnUserName);
                    cmd.Parameters.AddWithValue("@TerminationDate", new DateTime(1753, 1, 1, 0, 0, 0));

                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (!dataReader.HasRows)
                        {
                            throw new InvalidProgramException("Failed to execute SQL query to Bazooka");
                        }

                        if (dataReader.Read())
                        {
                            employee.Id = Convert.ToInt32(dataReader["NewUserId"]);
                        }
                    }
                }
            }
        }

        private static void CreateNewCarrierRep(Rep rep, DM.Carrier carrier, bool isMainContact)
        {
            ConnectionStringSettings bazookaConnectionString = ConfigurationManager.ConnectionStrings["Bazooka"];
            using (var sqlConnection = new SqlConnection(bazookaConnectionString.ConnectionString))
            {
                sqlConnection.Open();

                const string sql = "declare @RepID int; " +
                             "insert into [dbo].[Rep] (CreateDate, CreateByUserID, UpdateDate, UpdateByUserID,EntityType,EntityID,Main,RepType, EmployeeID )" +
                                  "values (@CreateDate, 0, @UpdateDate,0, @EntityType,@EntityID, @Main, 1, @EmployeeID);" +
                             "select @RepID = SCOPE_IDENTITY();" +
                             "select @RepID as NewRepId;";

                using (var cmd = new SqlCommand(sql, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@EntityType", 1);
                    cmd.Parameters.AddWithValue("@EntityID", carrier.Id);
                    cmd.Parameters.AddWithValue("@EmployeeID", rep.Employee.Id);
                    cmd.Parameters.AddWithValue("@Main", isMainContact);

                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (!dataReader.HasRows)
                        {
                            throw new InvalidProgramException("Failed to execute SQL query to Bazooka");
                        }

                        if (dataReader.Read())
                        {
                            rep.Id = Convert.ToInt32(dataReader["NewRepId"]);
                        }
                    }

                }
            }
        }

    }

    internal class Randoms
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string EmailAddress { get; private set; }
        public string ClawLogOnUserName { get; private set; }
        public string Password { get; private set; }

        public Randoms()
        {
            FirstName = RandomDataGeneration.String(4);
            LastName = RandomDataGeneration.String(8);
            EmailAddress = string.Format("{0}.{1}@company.com", FirstName, LastName);
            ClawLogOnUserName = string.Format("{0}.{1}", FirstName, LastName).ToLower();
            Password = string.Format("!{0}1", RandomDataGeneration.String(6));
        }
    }

}
