using Forte.Location.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forte.Location.Services
{
    public interface IRecommendationService
    {
        public LocationM? GetRecommendedLocation(Activity activity);
    }
}
