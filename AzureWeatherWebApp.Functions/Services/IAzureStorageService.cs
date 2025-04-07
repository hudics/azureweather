using AzureWeatherWebApp.Core.Models;

namespace AzureWeather.Services
{
    public interface IAzureStorageService
    {
        Task QueueWeathers(IEnumerable<CityForecast> forecasts);
    }
}
