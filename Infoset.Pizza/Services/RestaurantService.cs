using System.Collections.Generic;
using System.Linq;
using Infoset.Pizza.Models;
using Microsoft.EntityFrameworkCore;

namespace Infoset.Pizza.Services
{
    public class RestaurantService
    {
        private readonly DbContextOptions _dbContextOptions;

        public RestaurantService(DbContextOptions dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }

        public List<Restaurant> GetNearestRestaurants(double latitude, double longitude, int maxDistance, int maxResults)
        {
            using (var context = new InfosetPizzaDbContext(_dbContextOptions))
            {
                var restaurants = context.Restaurants
                    .Where(r => CalculateDistance(latitude, longitude, r.Latitude, r.Longitude) <= maxDistance)
                    .OrderBy(r => CalculateDistance(latitude, longitude, r.Latitude, r.Longitude))
                    .Take(maxResults)
                    .ToList();

                return restaurants;
            }
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double r = 6371e3;
            var phi1 = lat1 * Math.PI / 180;
            var phi2 = lat2 * Math.PI / 180;
            var deltaPhi = (lat2 - lat1) * Math.PI / 180;
            var deltaLambda = (lon2 - lon1) * Math.PI / 180;

            var a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                    Math.Cos(phi1) * Math.Cos(phi2) *
                    Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var d = r * c;

            return d;
        }
    }
}
