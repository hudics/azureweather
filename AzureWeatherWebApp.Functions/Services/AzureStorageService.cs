using Azure.Storage.Queues;
using AzureWeatherWebApp.Core.Models;
using System.Text.Json;


namespace AzureWeather.Services
{
    public class AzureStorageService : IAzureStorageService
    {
        private readonly QueueClient _queueClient;

        public AzureStorageService(QueueClient queueClient)
        {
            _queueClient = queueClient;
        }

        public async Task QueueWeathers(IEnumerable<CityForecast> forecasts)
        {
            var message = JsonSerializer.Serialize(forecasts);
            await _queueClient.SendMessageAsync(message);
        }
    }
}
