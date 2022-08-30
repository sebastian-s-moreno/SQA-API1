using Forte.Location.DataAccess.Infrastructure;
using Forte.Location.Services.Implementation;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using Yr.Facade;

namespace Forte.Location.Services
{
    public static class ServiceInstaller
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddDataAccess();
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<IYrFacade, YrFacade>();
        }
    }
}