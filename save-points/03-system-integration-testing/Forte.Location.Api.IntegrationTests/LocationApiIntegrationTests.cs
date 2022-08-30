using Forte.Location.DataAccess.Schema;
using Forte.Location.Services.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WireMock.Server;
using Microsoft.Extensions.DependencyInjection;
using System;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Yr.Facade.Models;
using System.Net.Http.Json;

namespace Forte.Location.Api.IntegrationTests
{
    [TestClass]
    public class LocationApiIntegrationTests
    {
        private HttpClient _testClient;
        private readonly string BasePath = "/api/weather/locations";
        private WireMockServer _wireMockServer;

        [TestInitialize]
        public void Setup()
        {
            var appFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    var wireMockServer = WireMockServer.Start();

                    builder.ConfigureAppConfiguration((hostContext, app) =>
                    {
                        app.AddInMemoryCollection(new KeyValuePair<string, string>[]
                        {
                            new KeyValuePair<string, string>("YrApi:BaseAddress", wireMockServer.Url)
                        });
                    });

                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<LocationDbContext>));

                        services.Remove(descriptor);

                        services.AddDbContext<LocationDbContext>(
                            options => options.UseInMemoryDatabase("TestDb"));

                        var context = services.BuildServiceProvider().GetRequiredService<LocationDbContext>();

                        context.Database.EnsureDeleted();

                        services.AddSingleton(wireMockServer);
                    });
                });

            _testClient = appFactory.CreateClient();
            _wireMockServer = appFactory.Services.GetRequiredService<WireMockServer>();
        }

        [TestMethod]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType()
        {
            // Act
            var response = await _testClient.GetAsync(BasePath);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [TestMethod]
        public async Task Get_UnknownEndpointReturnNotFound()
        {
            // Act
            var response = await _testClient.GetAsync("/should/not/exist");

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Get_EmptyArrayInResponseWhenNoLocationsInDb()
        {
            // Act
            var response = await _testClient.GetAsync(BasePath);
            var jsonContent = await response.Content.ReadFromJsonAsync<List<LocationM>>();

            // Assert
            Assert.AreEqual(0, jsonContent.Count);
        }

        [TestMethod]
        public async Task Post_MessageSayingLocationAddedInResponseAfterAddNewLocation()
        {
            // Arrange
            var testLocation = new LocationM() { Id = "testid", Name = "test name", Latitude = 55, Longitude = 65 };

            // Act
            var responseString = await CreateLocationAsync(BasePath, testLocation);
            var contentDict = JsonSerializer.Deserialize<Dictionary<string, string>>(responseString);

            // Assert
            Assert.AreEqual("Location added", contentDict["message"]);
        }

        [TestMethod]
        public async Task Post_ExactlyOneItemInResponseAfterAddNewLocation()
        {
            // Arrange
            var testLocation = new LocationM() { Id = "testid", Name = "test name", Latitude = 55, Longitude = 65 };

            // Act
            await CreateLocationAsync(BasePath, testLocation);

            var response = await _testClient.GetAsync(BasePath);
            var contentString = await response.Content.ReadAsStringAsync();
            var jsonContent = JsonSerializer.Deserialize<List<LocationM>>(contentString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assert
            Assert.AreEqual(1, jsonContent.Count);
        }

        [TestMethod]
        public async Task Post_ExactlyTwoItemsInResponseAfterTwoNewLocationsAdded()
        {
            // Arrange
            var testLocation1 = new LocationM() { Id = "testId1", Name = "testName1", Latitude = 55, Longitude = 66 };
            var testLocation2 = new LocationM() { Id = "testId2", Name = "testName2", Latitude = 77, Longitude = 88 };

            // Act
            await CreateLocationAsync(BasePath, testLocation1);
            await CreateLocationAsync(BasePath, testLocation2);

            //var jsonContent = JsonSerializer.Deserialize<List<LocationM>>(responseString);
            var response = await _testClient.GetAsync(BasePath);
            var contentString = await response.Content.ReadAsStringAsync();
            var jsonContent = JsonSerializer.Deserialize<List<LocationM>>(contentString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var firstElement = jsonContent.ElementAt(0);

            // Assert
            Assert.AreEqual(2, jsonContent.Count);
        }

        [TestMethod]
        public async Task Post_YrMock()
        {
            var loc = new LocationM() { Id = "testid", Name = "test name", Latitude = 55, Longitude = 66 };
            //var yrResponseMock = "{\"properties\":{\"timeseries\":[{\"time\":\"2022-08-28T16:00:00Z\",\"data\":{\"instant\":{\"details\":{\"air_pressure_at_sea_level\":1201,\"air_temperature\":14.2,\"cloud_area_fraction\":40.3,\"relative_humidity\":47.5,\"wind_from_direction\":24.7,\"wind_speed\":3}}}}]}}";
            var yrResponseMock = new YrApiResponse
            {
                Type = "Feature",
                Properties = new Property
                {
                    Timeseries = new List<TimeSerie>
                    {
                        new TimeSerie()
                        {
                            Time = new DateTimeOffset(DateTime.Now),
                            Data = new Data
                            {
                                Instant = new Instant
                                {
                                    Details = new Details
                                    {
                                        Air_pressure_at_sea_level = 1019.0,
                                        Air_temperature = 19.7,
                                        Relative_humidity = 47.5,
                                        Wind_from_direction = 24.7,
                                        Wind_speed = 3.0
                                    }
                                }
                            }
                        }
                    }
                }
            };

            _wireMockServer
                .Given(
                    Request.Create()
                        .WithPath("/weatherapi/locationforecast/2.0/compact")
                        .WithParam("lat", loc.Latitude.ToString())
                        .WithParam("lon", loc.Longitude.ToString())
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        //.WithBody(yrResponseMock)
                        .WithBodyAsJson(yrResponseMock)
                        .WithStatusCode(System.Net.HttpStatusCode.OK)
                );

            await CreateLocationAsync(BasePath, loc);

            var response = await _testClient.GetAsync(BasePath);
            var contentString = await response.Content.ReadAsStringAsync();
            var jsonContent = JsonSerializer.Deserialize<List<LocationM>>(contentString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var firstElement = jsonContent.ElementAt(0);

            Assert.AreEqual(1019.0, firstElement.WeatherData.AirPressureAtSeaLevel);
        }

        private async Task<string> CreateLocationAsync(string uri, LocationM location)
        {
            var json = JsonSerializer.Serialize(location);

            var request = new HttpRequestMessage();
            request.Content = new StringContent(json);
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await _testClient.PostAsync(uri, request.Content);
            var contentString = await response.Content.ReadAsStringAsync();

            return contentString;
        }
    }
}