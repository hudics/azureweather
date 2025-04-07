using AzureWeatherWebApp.API.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AzureWeatherWebApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
       
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<WeatherMinMaxTemperature>), 200)]
        public ActionResult List()
        {
            //should get the forecast from service and map to dto
            return Ok();
        }
    }
}
