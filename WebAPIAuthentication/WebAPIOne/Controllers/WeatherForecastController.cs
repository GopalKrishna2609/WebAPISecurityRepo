using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIOne.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "RequireMultipleRoles")]
        public List<Weather> GetWeather()
        {
            List<Weather> weatherList = new List<Weather>();

            weatherList.Add(new Weather { CityName = "Delhi", Temperature = Random.Shared.Next(-20, 55) });
            weatherList.Add(new Weather { CityName = "Shimla", Temperature = Random.Shared.Next(-20, 55) });
            weatherList.Add(new Weather { CityName = "Gurgaon", Temperature = Random.Shared.Next(-20, 55) });
            weatherList.Add(new Weather { CityName = "Mumbai", Temperature = Random.Shared.Next(-20, 55) });
            weatherList.Add(new Weather { CityName = "Calcutta", Temperature = Random.Shared.Next(-20, 55) });

            return weatherList;
        }

        [HttpGet]
        [Authorize(Roles = "Viewer")]
        public ActionResult Hello()
        {
            return Ok("Hello Gopal");
        }
        [HttpGet]
        [Authorize(Roles = "Viewer,Admin")]
        public ActionResult CheckMultipleRoles()
        {
            return Ok("Hi From Multiple Roles");
        }
        [HttpGet]
        [Authorize(Policy = "AgeOver18")]
        public ActionResult CheckAgeQual()
        {
            return Ok("Hi from Custom Policies");
        }

    }
}