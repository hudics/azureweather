using AzureWeather.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;


namespace AzureWeatherWebApp.Functions
{
    public class FetchWeatherData
    {

        private readonly ILogger _logger;
        private readonly IWeatherApiService _weatherApiService;
        private readonly IAzureStorageService _azureStorageService;

        public FetchWeatherData(IWeatherApiService weatherApiService, IAzureStorageService azureStorageService, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FetchWeatherData>();
            _weatherApiService = weatherApiService;
            _azureStorageService = azureStorageService;
        }

        [Function(nameof(FetchWeatherData))]
        public async Task Run([TimerTrigger("0 * * * * *")] TimerInfo myTimer)
        {
            var cities = new (string City, string Country)[]
            {
            ("New York", "US"),
            ("Warsaw", "PL"),
            ("Tokyo", "JP"),
            ("Berlin", "DE")
            };

            var tasks = cities.Select(async city =>
            {
                try
                {
                    var weather = await _weatherApiService.GetWeatherAsync(city.City, city.Country);
                    _logger.LogInformation($"Fetched weather for {weather.City}, {weather.Country}: {weather.Temperature}°C");
                    return weather;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to fetch weather for {city.City}, {city.Country}");
                    return null;
                }
            });            

            var forecasts = (await Task.WhenAll(tasks))
                    .Where(f => f != null)
                    .ToList();

            if (forecasts.Any())
            {               
                await _azureStorageService.QueueWeathers(forecasts);
            }

            _logger.LogInformation($"C# Timer trigger function processed at: {DateTime.UtcNow}");
        }
    }
}
