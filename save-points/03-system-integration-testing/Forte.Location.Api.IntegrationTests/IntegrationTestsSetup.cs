using Forte.Location.DataAccess.Schema;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Forte.Location.Services.Models;
using System.Text.Json;
using System.Collections.Generic;
using WireMock.Server;
using Microsoft.Extensions.Configuration;

namespace Forte.Location.Api.IntegrationTests
{
    public class IntegrationTestsSetup
    {
        protected readonly HttpClient TestClient;
        protected readonly string BasePath = "/api/weather/locations";
        protected readonly WireMockServer WireMockServer;

        protected IntegrationTestsSetup()
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

                        services.AddDbContext<LocationDbContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDb");
                        });

                        services.AddSingleton(wireMockServer);
                    });
                });

            TestClient = appFactory.CreateClient();
            WireMockServer = appFactory.Services.GetRequiredService<WireMockServer>();
        }

        protected async Task<string> CreateLocationAsync(string uri, LocationM location)
        {
            var json = JsonSerializer.Serialize(location);

            var request = new HttpRequestMessage();
            request.Content = new StringContent(json);
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await TestClient.PostAsync(uri, request.Content);
            var contentString = await response.Content.ReadAsStringAsync();

            return contentString;
        }
    }
}
