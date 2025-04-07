
using AzureWeatherWebApp.Core.Models;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;

namespace Core.Services
{
    public class WeatherCityForecastService : IWeatherCityForecastService
    {
      
        private readonly IWeatherRepository _weatherRepository;

        public WeatherCityForecastService(IWeatherRepository weatherRepository)
        {
            _weatherRepository = weatherRepository;
        }
       
        public Task<IEnumerable<CityForecast>> GetCityForecasts()
        {
            // GET forecasts from db repo
            return default;
        }
    }
}
