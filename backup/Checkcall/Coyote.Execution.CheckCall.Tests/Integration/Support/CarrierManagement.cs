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
    using Coyote.Common.Extensions;
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using DM = Coyote.Execution.CheckCall.Domain.Models;

    public static class CarrierManagement
    {
        private class Carrier
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public int BazookaCarrierId { get; set; }
        }
        
        public static DM.Carrier AddNewCarrier(string name, string code)
        {
            var carrier = new Carrier()
            {
                Code = code,
                Name = name
            };

            if (!CarrierExistsInBazooka(carrier))
            {

                // This is a work around until a Add/Remove/Get carrier service exists in CLAW
                ConnectionStringSettings bazookaConnectionString = ConfigurationManager.ConnectionStrings["Bazooka"];
                using (SqlConnection sqlConnection = new SqlConnection(bazookaConnectionString.ConnectionString))
                {
                    sqlConnection.Open();

                    if (CarrierExistsInBazooka(carrier)) return null;

                    string insertRegularCarrier = "insert into [dbo].[Carrier] (CreateDate, CreateByUserID, UpdateDate, UpdateByUserID, Code, Name, TruckNotes," +
                                           " ContractApprovalStatus,SalesStatus,CargoLimit,CargoExpirationDate,WorkmansCompLimit,GeneralLimit," +
                                           " SafetyRating,LiabilityLimit,LiabilityExpirationDate,AceDocExpDt,LoadOfferEmail) " +
                                        "values (@CreateDate, 0, @UpdateDate, 0, @Code, @Name, @TruckNotes," +
                                         " @ContractApprovalStatus,@SalesStatus,@CargoLimit,@CargoExpirationDate,@WorkmansCompLimit,@GeneralLimit," +
                                         " @SafetyRating,@LiabilityLimit,@LiabilityExpirationDate,@AceDocExpDt,@LoadOfferEmail); " +
                                        "select SCOPE_IDENTITY() as NewCarrierId";

                    using (SqlCommand cmd = new SqlCommand(insertRegularCarrier, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@TruckNotes", "Integration Testing");

                        var yearAway = DateTime.Now.AddYears(1);
                        cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Code", carrier.Code);
                        cmd.Parameters.AddWithValue("@Name", carrier.Name);

                        cmd.Parameters.AddWithValue("@ContractApprovalStatus", 1); //Approved
                        cmd.Parameters.AddWithValue("@SalesStatus", 1);
                        cmd.Parameters.AddWithValue("@CargoLimit", 1000000);
                        cmd.Parameters.AddWithValue("@CargoExpirationDate", yearAway);
                        cmd.Parameters.AddWithValue("@WorkmansCompLimit", 1000000);
                        cmd.Parameters.AddWithValue("@GeneralLimit", 1000000);
                        cmd.Parameters.AddWithValue("@SafetyRating", 1); //Satasfactory
                        cmd.Parameters.AddWithValue("@LiabilityLimit", 10000000);
                        cmd.Parameters.AddWithValue("@LiabilityExpirationDate", yearAway);
                        cmd.Parameters.AddWithValue("@AceDocExpDt", yearAway);
                        cmd.Parameters.AddWithValue("@LoadOfferEmail", "fakeloadoffer.email@test.com");


                        using (SqlDataReader dataReader = cmd.ExecuteReader())
                        {
                            if (!dataReader.HasRows)
                            {
                                throw new InvalidProgramException("Failed to execute SQL query to Bazooka");
                            }

                            dataReader.Read();
                            int carrierId = Convert.ToInt32(dataReader["NewCarrierId"]);
                            carrier.BazookaCarrierId = carrierId;
                        }


                    }
                }
            }

            return new Domain.Models.Carrier()
            {
                Name = carrier.Name,
                Id = carrier.BazookaCarrierId,

            };

        }

        private static bool CarrierExistsInBazooka(Carrier carrier)
        {
            // This is a work around until a Add/Remove/Get carrier service exists in CLAW
            bool carrierExists = false;

            ConnectionStringSettings bazookaConnectionString = ConfigurationManager.ConnectionStrings["Bazooka"];
            using (SqlConnection sqlConnection = new SqlConnection(bazookaConnectionString.ConnectionString))
            {
                sqlConnection.Open();

                string getCarrierId = @"select ID from [dbo].[Carrier] where Code=@Code and Name=@Name";
                using (SqlCommand cmd = new SqlCommand(getCarrierId, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@Code", carrier.Code);
                    cmd.Parameters.AddWithValue("@Name", carrier.Name);

                    var carrierId = cmd.ExecuteScalar();
                    carrierExists = (carrierId != null);

                    if (carrierExists)
                    {
                        carrier.BazookaCarrierId = Convert.ToInt32(carrierId);
                    }
                }
            }

            return carrierExists;
        }

        public static void RemoveCarrier(DM.Carrier carrier)
        {
            carrier.ThrowIfNull("carrier");
            // This is a work around until a Add/Remove/Get carrier service exists in CLAW
            ConnectionStringSettings bazookaConnectionString = ConfigurationManager.ConnectionStrings["Bazooka"];
            using (SqlConnection sqlConnection = new SqlConnection(bazookaConnectionString.ConnectionString))
            {
                sqlConnection.Open();

                string deleteUserCarrier = @"delete from [dbo].[UserCarrier] where CarrierID=@CarrierID";
                using (SqlCommand cmd = new SqlCommand(deleteUserCarrier, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@CarrierID", carrier.Id);
                    cmd.ExecuteNonQuery();
                }

                string deleteTextProgram = @"delete t from [dbo].[TextProgramXRef] t " +
                                           @"INNER JOIN dbo.LoadCarrier lc ON lc.Id = t.EntityId AND t.EntityType=13 " +
                                           "where lc.CarrierId=@CarrierId; ";
                using (SqlCommand cmd = new SqlCommand(deleteTextProgram, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@CarrierID", carrier.Id);
                    cmd.ExecuteNonQuery();
                }
                
                RemoveCarrierTrackingPreferences(sqlConnection, carrier.Id);
                string deleteCarrier = @"delete from [dbo].[Carrier] where ID=@CarrierID";
                using (SqlCommand cmd = new SqlCommand(deleteCarrier, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@CarrierID", carrier.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="carrierId"></param>
        /// <param name="communicationPreferenceType">1 for email</param>
        /// <param name="email"></param>
        /// <param name="dailyCheckCallEmail"></param>
        /// <param name="autoTrackEmail"></param>
        public static void InsertCarrierTrackingPreference(int carrierId, int communicationPreferenceType, string email, bool dailyCheckCallEmail, bool autoTrackEmail)
        {
            ConnectionStringSettings bazookaConnectionString = ConfigurationManager.ConnectionStrings["Bazooka"];
            using (SqlConnection sqlConnection = new SqlConnection(bazookaConnectionString.ConnectionString))
            {
                sqlConnection.Open();
                string query = @"INSERT INTO[dbo].[CarrierTrackingPreference](CarrierID, CreateDate, CreateDateUTC, CreateByUserID, UpdateDate, UpdateDateUTC, UpdateByUserID, OutboundDefaultCommunicationTypeId, Email, EmailName, DailyCheckCallEmail, AutoTrackEmail)
                    VALUES(@carrierId, GETDATE(), GETUTCDATE(), 0, GETDATE(), GETUTCDATE(), 0, @outboundDefaultCommunicationTypeId, @email, @emailName, @dailyCheckCallEmail, @autoTrackEmail)";

                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@carrierId", carrierId);
                    cmd.Parameters.AddWithValue("@outboundDefaultCommunicationTypeId", communicationPreferenceType);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@emailName", email);
                    cmd.Parameters.AddWithValue("@dailyCheckCallEmail", dailyCheckCallEmail);
                    cmd.Parameters.AddWithValue("@autoTrackEmail", autoTrackEmail);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        private static void RemoveCarrierTrackingPreferences(SqlConnection connection, int carrierId)
        {
            const string sql = @"delete from [dbo].[CarrierTrackingPreference] where CarrierId = @CarrierId";
            using (var cmd = new SqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@CarrierId", carrierId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}

