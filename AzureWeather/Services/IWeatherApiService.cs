
namespace AzureWeather.Services
{
    public interface IWeatherApiService
    {
        Task<(bool IsSuccess, string Payload)> FetchWeatherData(string apiUrl);       
    }
}
