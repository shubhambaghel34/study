// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2016 - 2018
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////

namespace Coyote.Execution.CheckCall.Tests.Unit.Email
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Coyote.Execution.CheckCall.Contracts;
    using Coyote.Execution.CheckCall.Contracts.Commands;
    using Coyote.Execution.CheckCall.Domain.Models;
    using Coyote.Execution.CheckCall.Email.Endpoint.Email;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    [TestClass]
    public class DailyCheckCallTests
    {
        Carrier _carrier;

        SendDailyCheckCallEmail _command;

        [TestInitialize]
        public void Init()
        {
            _carrier = new Carrier()
            {
                Id = 1,
                Reps = new Collection<Rep>()
                {
                    new Rep()
                    {
                        Main = true,
                        RepType = RepType.Sales,
                        EmployeeId = 400,
                        EmailWork = "workMain@fake.dat",
                        Addresses = new Collection<Address>()
                            {
                                new Address()
                                {
                                    Address1 = "1234 Street",
                                    AddressType = 1,
                                    ZipCode = "54321",
                                    CityName = "Chicago"

                                },
                                new Address()
                                {
                                    Address1 = "78978 Street",
                                    AddressType = 2,
                                    ZipCode = "09877",
                                    CityName = "Boston"

                                }
                            }
                    },
                    new Rep()
                    {
                        Main = false,
                        RepType = RepType.CarrierOperations,
                        EmployeeId = 500,
                        EmailWork = "workNotMain@fake.dat"
                    },
                    new Rep()
                    {
                        Main = false,
                        RepType = RepType.CarrierOperations,
                        EmployeeId = 600,
                        EmailWork = "workNotMain2@fake.dat"
                    }
                },
                CarrierTrackingPreference = new CarrierTrackingPreference()
                {
                    OutboundDefaultCommunicationTypeId = 1,
                    Email = " ; one@fake.dat;two@fake.date;;"
                }
            };


            _command = new SendDailyCheckCallEmail(new CheckCallNotificationRecord()
            {
                CarrierId = 1236,
                Email = " ; one@fake.dat;two@fake.date;;",
                EmailName = string.Empty,
                LoadId = 1234,
                OriginCityName = "One",
                OriginState = "IL",
                DestCityName = "One",
                DestState = "IL",
                LoadDate = DateTime.Now.Date
            },new Collection<LoadInfo>()
            {
                new LoadInfo
                {
                    LoadId = 1234,
                    OriginCityName = "One",
                    OriginState = "IL",
                    DestinationCityName = "One",
                    DestinationState = "IL"
                },
                new LoadInfo
                {
                    LoadId = 43212,
                    OriginCityName = "Two",
                    OriginState = "CA",
                    DestinationCityName = "Two",
                    DestinationState = "IL"
                },

            });

        }

        [TestMethod, TestCategory("Unit")]
        public void DailyCheckCallEmail_CorrectFromAddress()
        {
            var email = CreateEmail();

            Assert.AreEqual(_carrier.Reps.ElementAt(0).EmailWork, email.From);
        }

        [TestMethod, TestCategory("Unit")]
        public void DailyCheckCallEmail_ToContainsAllPreferenceAddresses()
        {
            var email = CreateEmail();

            Assert.AreEqual(2, email.To.Count());
        }

        [TestMethod, TestCategory("Unit")]
        public void DailyCheckCallEmail_CorrectSubject()
        {
            var email = CreateEmail();

            Assert.AreEqual($"Dispatch and Confirm Coyote Loads for {DateTime.Now.ToString("MM/dd/yy")}", email.Subject);
        }

        [TestMethod, TestCategory("Unit")]
        public void DailyCheckCallEmail_CorrectCCEmailsIncluded()
        {
            var email = CreateEmail();

            Assert.AreEqual(3, email.CC.Count());
        }

        [TestMethod, TestCategory("Unit")]
        public void DailyCheckCallEmail_CorrectLoadInfoIncluded()
        {
            var email = CreateEmail();

            Assert.AreEqual(2, email.TemplateData.card.loads.collection.Count, "Expected different count of loads");
            Assert.AreEqual(43212, email.TemplateData.card.loads.collection[1].LoadId, "Expected different LoadId");
        }

        [TestMethod, TestCategory("Unit")]
        public void DailyCheckCallEmail_CorrectLinkBuilt()
        {
            
            var email = CreateEmail();

            Assert.AreEqual("http://localhost/Coyote.Client.WebClient/my-tasks#tracking/?pickupToday=true&showOnlyCoveredLoads=true", email.TemplateData.card.links.confirm.url, "Incorrect confirmation link");
        }

        [TestMethod, TestCategory("Unit")]
        public void DailyPriorDayCheckCallEmail_CorrectLinkBuilt()
        {
            _command.Date = DateTime.Now.AddDays(1).Date;
            var email = CreateEmail();

            Assert.AreEqual("http://localhost/Coyote.Client.WebClient/my-tasks#tracking/?pickupToday=false&showOnlyCoveredLoads=true", email.TemplateData.card.links.confirm.url, "Incorrect confirmation link");
        }

        [TestMethod, TestCategory("Unit")]
        public void DailyCheckCallEmail_CorrectTemplateName()
        {
            var email = CreateEmail();

            Assert.AreEqual("DailyCheckCallEmailTemplate", email.TemplateName, "Expected Formatted Work Phone Number not found");
        }

        #region Private Helper

        private DailyCheckCall CreateEmail()
        {
            return new DailyCheckCall(_carrier, _command);
        }

        #endregion
    }
}
