using System.ComponentModel.DataAnnotations;

namespace Forte.Location.DataAccess.Schema
{
    public class LocationEntity
    {
        [Key]
        public string ID { get; set; } = "";
        public string Name { get; set; } = "";
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? AirPressureAtSeaLevel { get; set; }
        public double? AirTemperature { get; set; }
        public double? RelativeHumidity { get; set; }
        public double? WindFromDirection { get; set; }
        public double? WindSpeed { get; set; }
    }
}