using Forte.Location.Services.Models;

namespace Forte.Location.Services
{
    public interface ILocationService
    {
        public Task<bool> AddLocation(LocationM location);
        public List<LocationM> GetLocations();

        public LocationM? GetLocation(string id);

        public bool DeleteLocation(string id);

        public Task<bool> UpdateLocation(string id, LocationM location);

        public Task<Models.Weather?> GetUpdatedDetails(string id);

        public Task<Models.Weather?> GetUpdatedDetails(double? longitude, double? latitude);
    }
}
