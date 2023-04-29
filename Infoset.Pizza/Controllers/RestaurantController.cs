using Infoset.Pizza.Services;
using Microsoft.AspNetCore.Mvc;

namespace Infoset.Pizza.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RestaurantController : ControllerBase
    {
        private readonly RestaurantService _restaurantService;

        public RestaurantController(RestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public IActionResult GetNearestRestaurants(double lat, double lng)
        {
            var maxDistance = 10000; // 10 km in meters
            var maxResults = 5;

            var restaurants = _restaurantService.GetNearestRestaurants(lat, lng, maxDistance, maxResults);

            return Ok(restaurants);
        }
    }
}
