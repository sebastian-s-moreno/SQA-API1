using Forte.Location.DataAccess.Repository;
using Forte.Location.DataAccess.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Forte.Location.DataAccess.Infrastructure
{
    public static class DbInstaller
    {
        public static void AddDataAccess(this IServiceCollection services)
        {
            services.AddDbContext<LocationDbContext>(options => options.UseSqlite(GetConnectionString()));

            services.AddTransient<ILocationRepository, LocationRepository>();
        }

        private static string GetConnectionString()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            return builder.GetConnectionString("WeatherDb");
        }
    }
}
