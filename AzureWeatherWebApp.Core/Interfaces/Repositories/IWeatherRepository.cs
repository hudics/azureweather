using AzureWeatherWebApp.Core.Models;

namespace Core.Interfaces.Repository
{
    public interface IWeatherRepository
    {
        Task<bool> SaveWeatherData(IEnumerable<CityForecast> forecasts);
    }
}
