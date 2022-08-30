using System.Collections.Generic;
using Forte.Location.Services;
using Forte.Location.Services.Implementation;
using Forte.Location.Services.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Forte.Location.Services.Tests
{
    [TestClass]
    public class RecommendationServiceTests
    {
        private IRecommendationService _recommendationService;
        private Mock<ILocationService> _locationServiceMock;

        [TestInitialize]
        public void Initialize()
        {
            _locationServiceMock = new Mock<ILocationService>();
            _locationServiceMock.Setup(x => x.GetLocations()).Returns(GetLocations());

            _recommendationService = new RecommendationService(_locationServiceMock.Object);
        }

        [TestMethod]
        public void GetRecommendedLocation_Swimming_ReturnsWarmestLocation()
        {
            // Arrange

            // Act
            var location = _recommendationService.GetRecommendedLocation(Activity.Swimming);

            // Assert
            Assert.IsNotNull(location);
            Assert.AreEqual("1", location.Id);
        }

        [TestMethod]
        public void GetRecommendedLocation_Sailing_ReturnsMostWindyLocation()
        {
            // Arrange

            // Act
            var location = _recommendationService.GetRecommendedLocation(Activity.Sailing);

            // Assert
            Assert.IsNotNull(location);
            Assert.AreEqual("3", location.Id);
        }

        [TestMethod]
        public void GetRecommendedLocation_Skiing_ReturnsColdestLocation()
        {
            // Arrange

            // Act
            var location = _recommendationService.GetRecommendedLocation(Activity.Skiing);

            // Assert
            Assert.IsNotNull(location);
            Assert.AreEqual("2", location.Id);
        }

        [TestMethod]
        public void GetRecommendedLocation_Sightseeing_ReturnsHighestAirPressureLocation()
        {
            // Arrange

            // Act
            var location = _recommendationService.GetRecommendedLocation(Activity.Sightseeing);

            // Assert
            Assert.IsNotNull(location);
            Assert.AreEqual("4", location.Id);
        }

        [TestMethod]
        public void GetRecommendedLocation_Unspecified_ReturnsRandomLocationNotNull()
        {
            // Arrange

            // Act
            var location = _recommendationService.GetRecommendedLocation(Activity.Unspecified);

            // Assert
            Assert.IsNotNull(location);
        }

        [TestMethod]
        public void GetRecommendedLocations_NoneLocationsExists_ReturnsNull()
        {
            //Arrange
            _locationServiceMock.Setup(mr => mr.GetLocations()).Returns(new List<LocationM>());
            //Act
            var result = _recommendationService.GetRecommendedLocation(Activity.Swimming);
            //Assert
            Assert.IsNull(result);
        }

        private static List<LocationM> GetLocations()
        {
            return new List<LocationM>
            {
                new()
                {
                    Id = "1",
                    Latitude = 10,
                    Longitude = 20,
                    Name = "Oslo",
                    WeatherData = new Location.Services.Models.Weather
                    {
                        AirPressureAtSeaLevel = 30,
                        AirTemperature = 25,
                        RelativeHumidity = 30,
                        WindFromDirection = 180,
                        WindSpeed = 5
                    }
                },
                new()
                {
                    Id = "2",
                    Latitude = 11,
                    Longitude = 21,
                    Name = "Tromsø",
                    WeatherData = new Location.Services.Models.Weather
                    {
                        AirPressureAtSeaLevel = 30,
                        AirTemperature = 10,
                        RelativeHumidity = 30,
                        WindFromDirection = 180,
                        WindSpeed = 5
                    }
                },
                new()
                {
                    Id = "3",
                    Latitude = 13,
                    Longitude = 15,
                    Name = "Bergen",
                    WeatherData = new Location.Services.Models.Weather
                    {
                        AirPressureAtSeaLevel = 30,
                        AirTemperature = 12,
                        RelativeHumidity = 30,
                        WindFromDirection = 180,
                        WindSpeed = 12
                    }
                },
                new()
                {
                    Id = "4",
                    Latitude = 17,
                    Longitude = 53,
                    Name = "Trondheim",
                    WeatherData = new Location.Services.Models.Weather
                    {
                        AirPressureAtSeaLevel = 40,
                        AirTemperature = 15,
                        RelativeHumidity = 30,
                        WindFromDirection = 180,
                        WindSpeed = 5
                    }
                }
            };
        }
    }
}