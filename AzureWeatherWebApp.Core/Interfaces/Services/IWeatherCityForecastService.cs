using AzureWeatherWebApp.Core.Models;

namespace Core.Interfaces.Services
{
    public interface IWeatherCityForecastService
    {
        Task<IEnumerable<CityForecast>> GetCityForecasts();        
    }
}