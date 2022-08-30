using Forte.Location.DataAccess.Schema;

namespace Forte.Location.DataAccess.Repository
{
    public interface ILocationRepository
    {
        public List<LocationEntity> GetLocations();

        public LocationEntity? GetLocation(string id);

        public void AddLocation(LocationEntity location);

        public void DeleteLocation(string id);

        public void UpdateLocation(string id, LocationEntity location);
    }
}
