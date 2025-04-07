using System;
using AzureWeather.Models;
using AzureWeather.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AzureWeather
{
    public class FetchWeatherFunction
    {
        private readonly IWeatherApiService _weatherApiService;
        private readonly IAzureStorageService _azureStorageService;
        private readonly ILogger<FetchWeatherFunction> _logger;
        private readonly string _apiKey;

        public FetchWeatherFunction(IWeatherApiService weatherApiService, IAzureStorageService azureStorageService, IConfiguration configuration, ILogger<FetchWeatherFunction> logger)
        {
            _weatherApiService = weatherApiService;
            _azureStorageService = azureStorageService;
            _logger = logger;
            _apiKey = configuration["OpenWeatherMap:ApiKey"] ?? throw new InvalidOperationException("Brak klucza API OpenWeatherMap w konfiguracji.");
        }

        [Function(nameof(FetchWeatherFunction))]
        public async Task Run([TimerTrigger("0 * * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# FetchWeatherFunction function executed at: {DateTime.Now}");
            try
            {
                var apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q=London&appid={_apiKey}";

                var (isSuccess, payload) = await _weatherApiService.FetchWeatherData(apiUrl);
                var logEntry = new LogEntry
                {
                    IsSuccess = isSuccess,
                    ErrorMessage = isSuccess ? null : "Error while retrieving data from API."
                };       

                await _azureStorageService.LogAttempt(logEntry);

                if (isSuccess && !string.IsNullOrEmpty(payload))
                {
                    await _azureStorageService.SavePayload(logEntry.RowKey, payload);
                    _logger.LogInformation($"Successfully downloaded and saved log data: {logEntry.RowKey}");
                }
                else
                {
                    _logger.LogError($"Failed to fetch data from API. Log ID: {logEntry.RowKey}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                var logEntry = new LogEntry
                {
                    IsSuccess = false,
                    ErrorMessage = $"Unexpected error: {ex.Message}"
                };                
                await _azureStorageService.LogAttempt(logEntry);
            }
        }
    }
}
