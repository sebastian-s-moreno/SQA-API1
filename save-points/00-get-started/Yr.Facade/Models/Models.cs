namespace Yr.Facade.Models
{
    internal class YrApiResponse
    {
        public string? Type { get; set; }
        public Property Properties { get; set; } = new();
    }

    internal class Property
    {
        public List<TimeSerie> Timeseries { get; set; } = new();
    }

    internal class TimeSerie
    {
        public Data Data { get; set; } = new();
        public DateTimeOffset Time { get; set; }
    }
    internal class Data
    {
        public Instant Instant { get; set; } = new();
    }
    internal class Instant
    {
        public Details Details { get; set; } = new();
    }
    public class Details
    {
        public double Air_pressure_at_sea_level { get; set; }
        public double Air_temperature { get; set; }
        public double Relative_humidity { get; set; }
        public double Wind_from_direction { get; set; }
        public double Wind_speed { get; set; }
    }
}