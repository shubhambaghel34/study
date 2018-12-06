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
    using System.Configuration;
    using System.Data.SqlClient;
    using Coyote.Common.Extensions;

    public static class CustomerManagement
    {
        public static Customer CreateCustomer(string name, string code)
        {
            var cust = new Customer()
            {
                Name = name,
                Code = code
            };
            if (!CustomerExistsInBazooka(cust))
            {
                AddCustomerToBazooka(cust);
            }

            return cust;
        }

        private static void AddCustomerToBazooka(Customer customer)//, Credit credit, CustomerOutstandingBalance customerOutstandingBalance)
        {
            // This is a work around until a Add/Remove/Get customer service exists in CLAW
            ConnectionStringSettings bazookaConnectionString = ConfigurationManager.ConnectionStrings["Bazooka"];
            using (SqlConnection sqlConnection = new SqlConnection(bazookaConnectionString.ConnectionString))
            {
                sqlConnection.Open();

               
                string insertCredit = "INSERT INTO [dbo].[Credit] (Status,Limit, Available, TotalExposure, CreateDate,CreateByUserID,UpdateDate,UpdateByUserID,CallFrequencyType,CallDayType,TempCreditAmount)" +
                        "VALUES (@Status, @Limit, @Available, @TotalExposure, '1753-01-01 00:00:00.000', 0, '1753-01-01 00:00:00.000', 0, 0, 0, 0.00); select SCOPE_IDENTITY() as NewCreditId;";
                using (SqlCommand cmd = new SqlCommand(insertCredit, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@Status", 1);
                    cmd.Parameters.AddWithValue("@Limit", 25000);
                    cmd.Parameters.AddWithValue("@Available", 25000);
                    cmd.Parameters.AddWithValue("@TotalExposure", 0);

                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (!dataReader.HasRows)
                        {
                            throw new InvalidProgramException("Failed to execute SQL query to Bazooka");
                        }

                        dataReader.Read();
                        var creditId = Convert.ToInt32(dataReader["NewCreditId"]);
                        customer.CreditId = creditId;
                    }
                }
                

                string insertCustomer = "insert into [dbo].[Customer] (CreateDate, CreateByUserID, UpdateDate, UpdateByUserID, Code, Name, MarketingNotes, EdiId, SalesStatusType, CreditId, MileageBorderType, MileageRouteType, MileageSystemType, MileageVersion, MileageInTenths) " +
                      "values (@CreateDate, 0, @UpdateDate, 0, @Code, @Name, @MarketingNotes, @EdiId, @SalesStatusType, @CreditId, 2, 0, 1, 6, 0); select SCOPE_IDENTITY() as NewCustomerId";
                using (SqlCommand cmd = new SqlCommand(insertCustomer, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@MarketingNotes", "Integration Test Data");
                    cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Code", customer.Code);
                    cmd.Parameters.AddWithValue("@Name", customer.Name);
                    cmd.Parameters.AddWithValue("@EdiId", customer.Code);
                    cmd.Parameters.AddWithValue("@SalesStatusType", 1);
                    cmd.Parameters.AddWithValue("@CreditId", customer.CreditId);

                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (!dataReader.HasRows)
                        {
                            throw new InvalidProgramException("Failed to execute SQL query to Bazooka");
                        }

                        dataReader.Read();
                        int customerId = Convert.ToInt32(dataReader["NewCustomerId"]);
                        customer.Id = customerId;
                    }
                }

            }
        }

        private static bool CustomerExistsInBazooka(Customer customer)
        {
            // This is a work around until a Add/Remove/Get customer service exists in CLAW
            bool customerExists;

            ConnectionStringSettings bazookaConnectionString = ConfigurationManager.ConnectionStrings["Bazooka"];
            using (SqlConnection sqlConnection = new SqlConnection(bazookaConnectionString.ConnectionString))
            {
                sqlConnection.Open();

                string getCustomerId = @"select ID from [dbo].[Customer] where Code=@Code and Name=@Name";
                using (SqlCommand cmd = new SqlCommand(getCustomerId, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@Code", customer.Code);
                    cmd.Parameters.AddWithValue("@Name", customer.Name);

                    var customerId = cmd.ExecuteScalar();
                    customerExists = (customerId != null);

                    if (customerExists)
                    {
                        customer.Id = Convert.ToInt32(customerId);
                    }
                }
            }

            return customerExists;
        }

        public static void RemoveCustomer(Customer customer)
        {
            customer.ThrowIfNull("customer");
            // This is a work around until a Add/Remove/Get customer service exists in CLAW
            ConnectionStringSettings bazookaConnectionString = ConfigurationManager.ConnectionStrings["Bazooka"];
            using (SqlConnection sqlConnection = new SqlConnection(bazookaConnectionString.ConnectionString))
            {
                sqlConnection.Open();

                string deleteCustomer = @"delete from [dbo].[Customer] where Code=@Code and Name=@Name";
                using (SqlCommand cmd = new SqlCommand(deleteCustomer, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@Code", customer.Code);
                    cmd.Parameters.AddWithValue("@Name", customer.Name);

                    cmd.ExecuteNonQuery();
                }

                string deleteCustomerCredit = @"delete from [dbo].[Credit] where ID=@CreditId";
                using (SqlCommand cmd = new SqlCommand(deleteCustomerCredit, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@CreditId", customer.CreditId);

                    cmd.ExecuteNonQuery();
                }

            }
        }

    }
    public class Customer
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int CreditId { get; set; }
    }
}
