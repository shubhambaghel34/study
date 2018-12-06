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
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using DM = Coyote.Execution.CheckCall.Domain.Models;
    using Coyote.Common.Extensions;
    public static class LoadManagement
    {
        public static void RemoveLoad(DM.Load load)
        {
            RemoveLoadCarriersFromBazooka(load);
            RemoveLoadFromBazooka(load);
        }

        public static DM.Load CreateLoad(DM.Load load, DM.Carrier carrier, Customer customer)
        {
            load.ThrowIfNull("load");
            ConnectionStringSettings bazookaConnectionString = ConfigurationManager.ConnectionStrings["Bazooka"];
            using (SqlConnection sqlConnection = new SqlConnection(bazookaConnectionString.ConnectionString))
            {
                sqlConnection.Open();

                string insertLoad = "insert into [dbo].[Load] (CreateDate, CreateByUserID, UpdateDate, UpdateByUserID, Mode, ProgressType, Type, StateType, Concealed, Team, " +
                                        "OriginCityID, OriginCityName, OriginStateCode, DestinationCityID, DestinationCityName, DestinationStateCode, Notes, LoadDate, " +
                                        "EquipmentType, MinTemp, MaxTemp, Seal, TMSPrivateCarrier, NumStops, GeneralOriginCityID, GeneralDestinationCityID,Private, " +
                                        "Division, Source,RoutingRankType) " +
                                        "values (@CreateDate, 0, @UpdateDate, 0, @Mode, @ProgressType, @Type, @StateType, @Concealed, @Team, @OriginCityID, @OriginCityName, @OriginStateCode, @DestinationCityID, @DestinationCityName, " +
                                        "@DestinationStateCode, @Notes, @LoadDate, @EquipmentType, @MinTemp, @MaxTemp, @Seal, @TMSPrivateCarrier, @NumStops," +
                                        "@GeneralOriginCityID, @GeneralDestinationCityID, @Private, @Division, @Source, @RoutingRankType); select SCOPE_IDENTITY() as NewLoadId";
                using (SqlCommand cmd = new SqlCommand(insertLoad, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@Notes", "Integration Testing");

                    cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Mode", (int) load.Mode);
                    cmd.Parameters.AddWithValue("@ProgressType", (int)load.ProgressType);
                    cmd.Parameters.AddWithValue("@Type", 1);
                    cmd.Parameters.AddWithValue("@StateType", (int)load.StateType);
                    cmd.Parameters.AddWithValue("@Concealed", 0);
                    cmd.Parameters.AddWithValue("@Team", 0);
                    cmd.Parameters.AddWithValue("@OriginCityID", 7111);
                    cmd.Parameters.AddWithValue("@OriginCityName", "Saint Charles");
                    cmd.Parameters.AddWithValue("@OriginStateCode", "IL");
                    cmd.Parameters.AddWithValue("@DestinationCityID", 24375);
                    cmd.Parameters.AddWithValue("@DestinationCityName", "Memphis");
                    cmd.Parameters.AddWithValue("@DestinationStateCode", "TN");
                    cmd.Parameters.AddWithValue("@LoadDate", load.LoadDate);
                    cmd.Parameters.AddWithValue("@EquipmentType", load.EquipmentType);
                    cmd.Parameters.AddWithValue("@MinTemp", (decimal)-9999.0);
                    cmd.Parameters.AddWithValue("@MaxTemp", (decimal)-9999.0);
                    cmd.Parameters.AddWithValue("@Seal",0);
                    cmd.Parameters.AddWithValue("@TMSPrivateCarrier",0);
                    cmd.Parameters.AddWithValue("@NumStops", 2);
                    cmd.Parameters.AddWithValue("@GeneralOriginCityID", 7111);
                    cmd.Parameters.AddWithValue("@GeneralDestinationCityID", 24375);
                    cmd.Parameters.AddWithValue("@Private", 0);
                    cmd.Parameters.AddWithValue("@Division", 1);
                    cmd.Parameters.AddWithValue("@Source", 1);
                    cmd.Parameters.AddWithValue("@RoutingRankType", 1);

                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (!dataReader.HasRows)
                        {
                            throw new InvalidProgramException("Failed to execute SQL query to Bazooka");
                        }

                        dataReader.Read();
                        int loadId = Convert.ToInt32(dataReader["NewLoadId"]);
                        load.Id = loadId;
                    }
                }

                AddLoadCustomersToBazooka(load, customer);
                AddLoadCarrierToBazooka(load, carrier);

                return load;
            }
        }
        private static void AddLoadCustomersToBazooka(DM.Load load, Customer customer)
        {
            // This is a work around until a Add/Remove/Get load service exists in CLAW
            ConnectionStringSettings bazookaConnectionString = ConfigurationManager.ConnectionStrings["Bazooka"];
            using (SqlConnection sqlConnection = new SqlConnection(bazookaConnectionString.ConnectionString))
            {
                sqlConnection.Open();
                
                string insertCustomer = "insert into [dbo].[LoadCustomer] (CreateDate, CreateByUserID, UpdateDate, UpdateByUserID, LoadID, CustomerID, Name, LoadNotes, Main, Type) " +
                                        "values (@CreateDate, 0, @UpdateDate, 0, @LoadID, @CustomerID, @Name, @LoadNotes, @Main, @Type);" +
                                        "select SCOPE_IDENTITY() as NewLoadCustomerId";
                using (SqlCommand cmd = new SqlCommand(insertCustomer, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@LoadNotes", "Integration Testing");

                    cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@LoadID", load.Id);
                    cmd.Parameters.AddWithValue("@CustomerID", customer.Id);
                    cmd.Parameters.AddWithValue("@Name", customer.Name);
                    cmd.Parameters.AddWithValue("@Main", 1);
                    cmd.Parameters.AddWithValue("@Type", 1); // Currently all LoadCustomers are Billing (needed for credit and invoicing)
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (!dataReader.HasRows)
                        {
                            throw new InvalidProgramException("Failed to execute SQL query to Bazooka");
                        }

                        dataReader.Read();
                        int loadCustomerID = Convert.ToInt32(dataReader["NewLoadCustomerId"]);
                    }
                }
                
            }
        }


        private static void AddLoadCarrierToBazooka(DM.Load load, DM.Carrier carrier)
        {
            // This is a work around until a Add/Remove/Get load service exists in CLAW
            ConnectionStringSettings bazookaConnectionString = ConfigurationManager.ConnectionStrings["Bazooka"];
            using (SqlConnection sqlConnection = new SqlConnection(bazookaConnectionString.ConnectionString))
            {
                sqlConnection.Open();
                
                string insertCarrier = "insert into [dbo].[LoadCarrier] (CreateDate, CreateByUserID, UpdateDate, UpdateByUserID, LoadID, CarrierID, Name, ConfirmationNotes, IsBounced, BookAsNonQualifiedCarrier, Main, ExpectedEquipmentType,EmptyDateTime) " +
                                        "values (@CreateDate, 0, @UpdateDate, 0, @LoadID, @CarrierID, @Name, @ConfirmationNotes, @IsBounced, @BookAsNonQualifiedCarrier, @Main, @ExpectedEquipmentType, @EmptyDateTime);" +
                                        "Select SCOPE_IDENTITY() as NewLoadCarrierId";
                using (SqlCommand cmd = new SqlCommand(insertCarrier, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@ConfirmationNotes", "Integration Testing");

                    cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@LoadID", load.Id);
                    cmd.Parameters.AddWithValue("@CarrierID", carrier.Id);
                    cmd.Parameters.AddWithValue("@Name", carrier.Name);
                    cmd.Parameters.AddWithValue("@IsBounced", 0);
                    cmd.Parameters.AddWithValue("@BookAsNonQualifiedCarrier", false);
                    cmd.Parameters.AddWithValue("@Main", 1);
                    cmd.Parameters.AddWithValue("@ExpectedEquipmentType",  "V");
                    cmd.Parameters.AddWithValue("@EmptyDateTime", DateTime.Now.AddDays(3));

                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (!dataReader.HasRows)
                        {
                            throw new InvalidProgramException("Failed to execute SQL query to Bazooka");
                        }

                        dataReader.Read();
                        int loadCarrierID = Convert.ToInt32(dataReader["NewLoadCarrierId"]);
                    }
                }
            }
        }

        
        private static void RemoveLoadCarriersFromBazooka(DM.Load load)
        {
            if (load == null)
            {
                return;
            }

            // This is a work around until a Add/Remove/Get load service exists in CLAW
            ConnectionStringSettings bazookaConnectionString = ConfigurationManager.ConnectionStrings["Bazooka"];
            using (SqlConnection sqlConnection = new SqlConnection(bazookaConnectionString.ConnectionString))
            {
                sqlConnection.Open();

                string deleteCarrier = " delete from [dbo].[LoadCarrierTrackingPreferenceDetail] where LoadCarrierId in (select Id from [dbo].[LoadCarrier] where LoadId =  @BazookaID);" +
                                        "delete from [dbo].[LoadCarrierAccounting] where LoadCarrierId in (select Id from [dbo].[LoadCarrier] where LoadId =  @BazookaID);" +
                                        "delete from [dbo].[LoadCarrierManagedRateDetail] where LoadId=@BazookaID; " +
                                        "delete from [dbo].[LoadCarrierManagedRate] where LoadId=@BazookaID; " +
                                        "delete from [dbo].[LoadCarrier] where LoadID=@BazookaID;";
                using (SqlCommand cmd = new SqlCommand(deleteCarrier, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@BazookaID", load.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void RemoveLoadFromBazooka(DM.Load load)
        {
            if (load == null)
            {
                return;
            }

            // This is a work around until a Add/Remove/Get load service exists in CLAW
            ConnectionStringSettings bazookaConnectionString = ConfigurationManager.ConnectionStrings["Bazooka"];
            using (SqlConnection sqlConnection = new SqlConnection(bazookaConnectionString.ConnectionString))
            {
                sqlConnection.Open();

                string deleteLoad = @"delete from [dbo].[LoadCustomer] where LoadID = @BazookaID;" +
                                    @"DELETE FROM [dbo].[LoadRoute] WHERE LoadId = @BazookaId; " +
                                    @"DELETE FROM [dbo].[LoadPayment] WHERE LoadId = @BazookaId; " +
                                    @"DELETE FROM [dbo].[LoadInvoice] WHERE LoadId = @BazookaId; " +
                                    @"DELETE FROM [dbo].[LoadInvoiceTemplateValidationXref] WHERE LoadId = @BazookaId; " +
                                    @"DELETE FROM [dbo].[LoadTracking] WHERE LoadId = @BazookaId; " +
                                    @"DELETE FROM [dbo].[LoadIncident] WHERE LoadId = @BazookaId; " +
                                    @"DELETE FROM [dbo].[LoadRate] WHERE LoadId = @BazookaId; " +
                                    @"DELETE FROM [dbo].[LoadRateDetail] WHERE LoadId = @BazookaId; " +
                                    @"DELETE FROM [dbo].[LoadBoard] WHERE LoadId = @BazookaId; " +
                                    @"DELETE FROM [dbo].[CarrierLoadPreference] WHERE LoadId = @BazookaId; " +
                                    @"DELETE FROM [dbo].[LoadCommission] WHERE LoadId = @BazookaId; " +
                                    @"DELETE FROM [dbo].[LoadSpotRate] WHERE LoadId = @BazookaId; " +
                                    @"DELETE FROM [dbo].[LoadRequiredDocs] WHERE LoadId = @BazookaId; " +
                                    @"DELETE FROM [dbo].[LoadNotificationList] WHERE LoadId = @BazookaId; " +
                                    @"DELETE FROM [dbo].[LoadCurrency] WHERE LoadId = @BazookaId; " +
                                    @"DELETE FROM [dbo].[LoadTMS] WHERE LoadId = @BazookaId; " +
                                    @"DELETE FROM [dbo].[Load] WHERE Id = @BazookaId; ";

                using (SqlCommand cmd = new SqlCommand(deleteLoad, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@BazookaId", load.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
