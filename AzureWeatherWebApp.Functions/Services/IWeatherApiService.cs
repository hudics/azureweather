
using AzureWeatherWebApp.Core.Models;

namespace AzureWeather.Services
{
    public interface IWeatherApiService
    {
        Task<CityForecast> GetWeatherAsync(string city, string country);
    }
}
