using Forte.Location.DataAccess.Schema;
using Forte.Location.Services.Models;

namespace Forte.Location.Services.Mappers
{
    public static class LocationMapper
    {
        public static List<LocationM> ToModel(this List<LocationEntity> entities)
        {
            return entities.Select(entity => entity.ToModel()).ToList();
        }

        public static LocationM ToModel(this LocationEntity entity)
        {
            return new LocationM
            {
                Id = entity.ID,
                Name = entity.Name,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                WeatherData = new Models.Weather
                {
                    AirPressureAtSeaLevel = entity.AirPressureAtSeaLevel,
                    AirTemperature = entity.AirTemperature,
                    RelativeHumidity = entity.RelativeHumidity,
                    WindFromDirection = entity.WindFromDirection,
                    WindSpeed = entity.WindSpeed
                }
            };
        }

        public static LocationEntity FromModel(this LocationM model)
        {
            return new LocationEntity
            {
                ID = model.Id,
                Name = model.Name,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                AirPressureAtSeaLevel = model.WeatherData?.AirPressureAtSeaLevel,
                AirTemperature = model.WeatherData?.AirTemperature,
                RelativeHumidity = model.WeatherData?.RelativeHumidity,
                WindFromDirection = model.WeatherData?.WindFromDirection,
                WindSpeed = model.WeatherData?.WindSpeed
            };
        }
    }
}
