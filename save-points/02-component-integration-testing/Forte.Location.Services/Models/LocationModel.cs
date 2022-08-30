using System.ComponentModel.DataAnnotations;

namespace Forte.Location.Services.Models
{
    public class LocationM
    {
        public string? Id { get; set; }
        [Required]
        public string Name { get; set; } = "";
        [Required]
        [Range(-90, 90)]
        public double? Latitude { get; set; }
        [Required]
        [Range(-180, 180)]
        public double? Longitude { get; set; }
        public Weather? WeatherData { get; set; }
    }

    public class Weather
    {
        public double? AirPressureAtSeaLevel { get; set; }
        public double? AirTemperature { get; set; }
        public double? RelativeHumidity { get; set; }
        public double? WindFromDirection { get; set; }
        public double? WindSpeed { get; set; }
    }
}