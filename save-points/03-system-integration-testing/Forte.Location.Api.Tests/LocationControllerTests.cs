using Forte.Location.Api.Controllers;
using Forte.Location.Services;
using Forte.Location.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forte.Location.Api.Tests
{
    [TestClass]
    public class LocationControllerTests
    {
        private LocationController _controller;
        private Mock<ILocationService> _locationServiceMock;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            _locationServiceMock = new Mock<ILocationService>();
            _controller = new LocationController(_locationServiceMock.Object);
        }

        [TestMethod]
        public async Task Create_InputIsNull_Return400()
        {
            //Act
            var result = await _controller.Post(null);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task Create_CorrectInput_ReturnOk()
        {
            //Arrange 
            _locationServiceMock.Setup(mr => mr.AddLocation(It.IsAny<LocationM>())).Returns(Task.FromResult(true));

            LocationM location = new LocationM();

            //Act
            var result = await _controller.Post(location);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task Create_CorrectInput_Return500()
        {
            //Arrange 
            _locationServiceMock.Setup(mr => mr.AddLocation(It.IsAny<LocationM>())).Returns(Task.FromResult(false));
            LocationM location = new LocationM();

            //Act
            var result = await _controller.Post(location);

            //Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            Assert.IsTrue(((ObjectResult)result).StatusCode == 500);
        }

        [TestMethod]
        public void Read_ReturnOk()
        {
            //Arrange 
            _locationServiceMock.Setup(mr => mr.GetLocations()).Returns(GetLocations());

            //Act
            var result = _controller.Get();

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
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
                    WeatherData = new Services.Models.Weather
                    {
                        AirPressureAtSeaLevel = 40,
                        AirTemperature = 30,
                        RelativeHumidity = 30,
                        WindFromDirection = 180,
                        WindSpeed = 5
                    }
                }
            };
        }

        [TestMethod]
        public async Task Update_InputIsNull_Return400()
        {
            //Act
            var result = await _controller.Update(null, null);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task Update_CorrectInput_ReturnOk()
        {
            //Arrange 
            _locationServiceMock.Setup(mr => mr.UpdateLocation(It.IsAny<string>(), It.IsAny<LocationM>())).Returns(Task.FromResult(true));
            LocationM location = new LocationM();
            string id = "";

            //Act
            var result = await _controller.Update(id, location);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task Update_CorrectInput_Return500()
        {
            //Arrange 
            _locationServiceMock.Setup(mr => mr.UpdateLocation(It.IsAny<string>(), It.IsAny<LocationM>())).Returns(Task.FromResult(false));
            LocationM location = new LocationM();
            string id = "";

            //Act
            var result = await _controller.Update(id, location);

            //Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            Assert.IsTrue(((ObjectResult)result).StatusCode == 500);
        }

        [TestMethod]
        public void Delete_CorrectInput_ReturnOk()
        {
            //Arrange 
            _locationServiceMock.Setup(mr => mr.DeleteLocation(It.IsAny<string>())).Returns(true);
            string id = "123";

            //Act
            var result = _controller.Delete(id);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void Delete_WrongInput_Return500()
        {
            //Arrange 
            _locationServiceMock.Setup(mr => mr.DeleteLocation(It.IsAny<string>())).Returns(false);
            string id = "123";

            //Act
            var result = _controller.Delete(id);

            //Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            Assert.IsTrue(((ObjectResult)result).StatusCode == 500);
        }


    }
}