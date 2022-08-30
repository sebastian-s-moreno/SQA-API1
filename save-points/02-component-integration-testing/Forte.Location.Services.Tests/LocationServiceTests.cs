using Forte.Location.DataAccess.Repository;
using Forte.Location.DataAccess.Schema;
using Forte.Location.Services.Implementation;
using Forte.Location.Services.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yr.Facade;
using Yr.Facade.Models;

namespace Forte.Location.Services.Tests
{
    [TestClass]
    public class LocationServiceTests
    {
        private LocationService _locationService;
        private Mock<ILocationRepository> _locationRepositoryMock;
        private Mock<IYrFacade> _yrFacadeMock;

        [TestInitialize]
        public void Initialize()
        {
            _locationRepositoryMock = new Mock<ILocationRepository>();
            _yrFacadeMock = new Mock<IYrFacade>();

            _locationService = new LocationService(_locationRepositoryMock.Object, _yrFacadeMock.Object);

            // Add the mocked locations to a list
            var locations = GetLocations();

            // Return all the locations when GetLocations() is called
            _locationRepositoryMock.Setup(mr => mr.GetLocations()).Returns(locations);

            // Return a specific location by ID when GetLocations(id) is called
            _locationRepositoryMock.Setup(mr => mr.GetLocation(
                It.IsAny<string>())).
                Returns((string i) => locations.
                Where(x => x.ID == i).
                Single());
        }

        private static List<LocationEntity> GetLocations()
        {
            return new List<LocationEntity>
            {
                new()
                {
                    ID= "040958d7-e085-4748-8518-8a23292c114b",
                    Name= "Oslo",
                    Latitude=59,
                    Longitude= 11,
                    AirPressureAtSeaLevel = 1001,
                    AirTemperature = 5.6,
                    RelativeHumidity = 95.6,
                    WindFromDirection = 216.6,
                    WindSpeed = 13},
                new()
                {
                    ID= "6db14abf-f819-4816-99c0-3f11592603aa",
                    Name= "Trondheim",
                    Latitude=63,
                    Longitude= 10,
                    AirPressureAtSeaLevel = 997.1,
                    AirTemperature = -1.6,
                    RelativeHumidity = 95.9,
                    WindFromDirection = 319.4,
                    WindSpeed = 9.3},
                new()
                {
                    ID= "5e525c01-d4c8-4c35-a7da-f4ad7b19dd59",
                    Name= "Bergen",
                    Latitude=60,
                    Longitude= 5,
                    AirPressureAtSeaLevel = 997.1,
                    AirTemperature = -1.6,
                    RelativeHumidity = 95.9,
                    WindFromDirection = 319.4,
                    WindSpeed= 9.3}
            };
        }

        [TestMethod]
        public void GetDetailsCorrectID()
        {
            // Act
            var result = _locationService.GetUpdatedDetails("this could be anything");

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetDetailsCorrectCoordinates()
        {
            // Act
            var result = _locationService.GetUpdatedDetails(5, 5);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetDetailsInvalidString()
        {
            // Act
            var result = _locationService.GetUpdatedDetails("123").Result;

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetDetailsInvalidCoordinates()
        {
            // Arrange
            _yrFacadeMock.Setup(x => x.GetYrResponse(It.IsAny<string>())).Returns(Task.FromResult<Details?>(null));

            // Act
            var result = _locationService.GetUpdatedDetails(100, 800).Result;

            // Assert
            Assert.IsNull(result.AirPressureAtSeaLevel);
            Assert.IsNull(result.AirTemperature);
            Assert.IsNull(result.RelativeHumidity);
            Assert.IsNull(result.WindFromDirection);
            Assert.IsNull(result.WindSpeed);
        }

        [TestMethod]
        public void GetDetailsNoInput()
        {
            // Act
            var result = _locationService.GetUpdatedDetails(null, null).Result;

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void CreateLocation_InputOK_ResultIsTrue()
        {
            // Arrange
            var location = new LocationM
            {
                Name = "Oslo",
                Latitude = 11,
                Longitude = 15
            };

            // Act
            var result = _locationService.AddLocation(location).Result;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CreateLocation_RepositoryThrowException_ResultIsFalse()
        {
            // Arrange
            _locationRepositoryMock.Setup(mr => mr.AddLocation(It.IsAny<LocationEntity>())).Throws(new Exception());
            var location = new LocationM
            {
                Name = "Oslo",
                Latitude = 11,
                Longitude = 15
            };

            // Act
            var result = _locationService.AddLocation(location).Result;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetLocations_Default_ListIsReturned()
        {
            // Arrange
            _locationRepositoryMock.Setup(mr => mr.GetLocations()).Returns(GetLocations());

            // Act
            var result = _locationService.GetLocations();

            // Assert
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetLocations_RepositoryThrowsException_EmptyList()
        {
            // Arrange
            _locationRepositoryMock.Setup(mr => mr.GetLocations()).Throws(new Exception());

            // Act
            var result = _locationService.GetLocations();

            // Assert
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void GetLocation_LocationExist_ReturnLocation()
        {
            // Arrange
            string id = "123";
            _locationRepositoryMock.Setup(mr => mr.GetLocation(id)).Returns(new LocationEntity { ID = "123" });

            // Act
            var result = _locationService.GetLocation(id);

            // Assert
            Assert.IsTrue(result?.Id == id);
        }

        [TestMethod]
        public void GetLocation_RepositoryThrowsException_ReturnNull()
        {
            // Arrange
            string id = "123";
            _locationRepositoryMock.Setup(mr => mr.GetLocation(id)).Throws(new Exception());

            // Act
            var result = _locationService.GetLocation(id);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void UpdateLocation_InputOK_ResultIsTrue()
        {
            // Arrange
            var location = new LocationM
            {
                Name = "Oslo",
                Latitude = 11,
                Longitude = 15
            };
            string id = "123";

            // Act
            var result = _locationService.UpdateLocation(id, location).Result;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UpdateLocation_RepositoryThrowException_ResultIsFalse()
        {
            // Arrange
            _locationRepositoryMock.Setup(mr => mr.UpdateLocation(It.IsAny<string>(), It.IsAny<LocationEntity>())).Throws(new Exception());
            var location = new LocationM
            {
                Name = "Oslo",
                Latitude = 11,
                Longitude = 15
            };
            string id = "123";

            // Act
            var result = _locationService.UpdateLocation(id, location).Result;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DeleteLocation_InputOK_ResultIsTrue()
        {
            // Arrange
            string id = "123";

            // Act
            var result = _locationService.DeleteLocation(id);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DeleteLocation_RepositoryThrowException_ResultIsFalse()
        {
            // Arrange
            _locationRepositoryMock.Setup(mr => mr.DeleteLocation(It.IsAny<string>())).Throws(new Exception());
            string id = "123";

            // Act
            var result = _locationService.DeleteLocation(id);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
