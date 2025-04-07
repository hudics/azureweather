using AzureWeather.Services;
using AzureWeatherWebApp.Core.Models;
using Newtonsoft.Json;

namespace AzureWeather.Services
{
    public class WeatherApiService : IWeatherApiService
    {
        private readonly OpenWeatherApiClient _openWeatherApiClient;

        public WeatherApiService(OpenWeatherApiClient openWeatherApiClient)
        {
            _openWeatherApiClient = openWeatherApiClient;
        }

        public async Task<CityForecast> GetWeatherAsync(string city, string country)
        {
            var weatherJson = await _openWeatherApiClient.GetWeatherJsonAsync(city, country);
            dynamic weather = JsonConvert.DeserializeObject(weatherJson);
            return new CityForecast
            {
                City = weather.name,
                Country = weather.sys.country,
                Temperature = weather.main.temp
            };
        }
    }
}

