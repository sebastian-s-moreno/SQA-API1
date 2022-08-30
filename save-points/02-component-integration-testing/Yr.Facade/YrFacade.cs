using System.Net.Http.Headers;
using System.Net.Http.Json;
using Yr.Facade.Models;
using Microsoft.Extensions.Configuration;

namespace Yr.Facade
{
    public class YrFacade : IYrFacade
    {
        private readonly IConfiguration Configuration;

        public YrFacade(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<Details?> GetYrResponse(string elements)
        {
            var yrApiBaseUrl = Configuration["YrApi:BaseAddress"];
            var url = $"{yrApiBaseUrl}/weatherapi/locationforecast/2.0/compact?" + elements;
            using var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var productValue = new ProductInfoHeaderValue("ForteWeatherApp", "1.0");
            request.Headers.UserAgent.Add(productValue);
            var httpResponse = await client.SendAsync(request);
            if (httpResponse.IsSuccessStatusCode)
            {
                var response = await httpResponse.Content.ReadFromJsonAsync<YrApiResponse>();
                return response?.Properties.Timeseries.FirstOrDefault()?.Data.Instant.Details;
            }
            else
            {
                return null;
            }
        }
    }
}