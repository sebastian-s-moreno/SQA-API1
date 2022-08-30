using Yr.Facade.Models;

namespace Forte.Location.Services.Mappers
{
    public static class WeatherMapper
    {
        public static Models.Weather ToDomain(this Details? details)
        {
            return new Models.Weather
            {
                AirPressureAtSeaLevel = details?.Air_pressure_at_sea_level,
                AirTemperature = details?.Air_temperature,
                RelativeHumidity = details?.Relative_humidity,
                WindFromDirection = details?.Wind_from_direction,
                WindSpeed = details?.Wind_speed
            };
        }
    }
}
