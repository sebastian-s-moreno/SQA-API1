using Forte.Location.DataAccess.Repository;
using Forte.Location.Services.Models;
using Yr.Facade;

namespace Forte.Location.Services.Implementation
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _repository;
        private readonly IYrFacade _yrFacade;

        public LocationService(ILocationRepository repository, IYrFacade yrFacade)
        {
            _repository = repository;
            _yrFacade = yrFacade;
        }

        public Task<bool> AddLocation(LocationM location)
        {
            throw new NotImplementedException();
        }

        public List<LocationM> GetLocations()
        {
            throw new NotImplementedException();
        }

        public LocationM? GetLocation(string id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteLocation(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateLocation(string id, LocationM location)
        {
            throw new NotImplementedException();
        }

        public Task<Models.Weather?> GetUpdatedDetails(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Models.Weather?> GetUpdatedDetails(double? longitude, double? latitude)
        {
            throw new NotImplementedException();
        }
    }
}
