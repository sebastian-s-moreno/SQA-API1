using Yr.Facade.Models;

namespace Yr.Facade
{
    public interface IYrFacade
    {
        public Task<Details?> GetYrResponse(string elements);
    }
}
