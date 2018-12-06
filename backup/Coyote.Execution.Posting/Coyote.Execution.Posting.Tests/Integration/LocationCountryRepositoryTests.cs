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
namespace Coyote.Execution.Posting.Tests.Integration
{
    using Coyote.Execution.Posting.Contracts.Models;
    using Coyote.Execution.Posting.Contracts.Storage;
    using Coyote.Execution.Posting.Storage.Repositories;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Configuration;

    [TestClass]
    public class LocationCountryRepositoryTests
    {
        #region " Private Fields "
        private ILocationCountryRepository _locationCountryRepository { get; set; }
        #endregion

        #region " Setup "
        [TestInitialize()]
        public void LocationCountryRepositoryTestInitialize()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Integrated.BazookaDbContext"]?.ConnectionString;
            _locationCountryRepository = new LocationCountryRepository(connectionString);
        }
        #endregion

        #region " Tests "
        [TestMethod, TestCategory("Integration")]
        public void GetLocationCountryByCityId_IsValid()
        {
            var cityId = 6302;                  //CityId 6302 is for Chicago
            var locationCoutryId = 230;         //countryId 230 is for United States

            var locationcountry = _locationCountryRepository.GetLocationCountryByCityId(cityId);

            Assert.IsNotNull(locationcountry, "Location country should not be null.");
            Assert.IsTrue(locationcountry.ISOCodeAlpha2.Equals("US"), "ISOCodeAlpha2 should be US");
            Assert.IsTrue(locationcountry.ISOCodeAlpha3.Equals("USA"), "ISOCodeAlpha3 should be USA");
            Assert.IsTrue(locationcountry.Name.Equals("United States"), "Name should be United States");
            Assert.IsTrue(locationcountry.LocationCountryId.Equals(locationCoutryId), "Retrieved locationCoutryId should match the one for United States i.e. 230");
        }

        [TestMethod, TestCategory("Integration")]
        public void GetLocationCountryByCityId_IsInvalid()
        {
            var locationcountry = _locationCountryRepository.GetLocationCountryByCityId(-1);

            Assert.IsNull(locationcountry, "Location country should be null.");
        }

        [TestMethod, TestCategory("Integration")]
        public void GetCityDetailsByCityId_IsValid()
        {
            int cityId = 6302;                  //CityId 6302 is for Chicago

            City city = _locationCountryRepository.GetCityDetailsByCityId(cityId);

            Assert.IsNotNull(city, "City should not be null.");
            Assert.IsTrue(city.Name.Equals("Chicago"), "City Name should be Chicago.");
            Assert.IsTrue(city.MainZipCode.Equals("60607"), "City MainZipCode should be 60607.");
            Assert.IsTrue(city.StateCode.Equals("IL"), "StateCode should be IL.");
        }

        [TestMethod, TestCategory("Integration")]
        public void GetCityDetailsByCityId_IsInvalid()
        {
            City city = _locationCountryRepository.GetCityDetailsByCityId(-1);

            Assert.IsNull(city, "City should be null.");
        }
        #endregion
    }
}