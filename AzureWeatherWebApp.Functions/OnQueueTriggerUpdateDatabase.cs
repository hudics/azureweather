using System;
using System.Text.Json;
using Azure.Storage.Queues.Models;
using AzureWeatherWebApp.Core.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureWeatherWebApp.Functions
{
    public class OnQueueTriggerUpdateDatabase
    {
        private readonly ILogger<OnQueueTriggerUpdateDatabase> _logger;

        public OnQueueTriggerUpdateDatabase(ILogger<OnQueueTriggerUpdateDatabase> logger)
        {
            _logger = logger;
        }

        [Function(nameof(OnQueueTriggerUpdateDatabase))]
        public void Run([QueueTrigger("weatherdataqueue", Connection = "AzureWebJobsStorage")] string queueItem)
        {
            var forecasts = JsonSerializer.Deserialize<List<CityForecast>>(queueItem);
            _logger.LogInformation("Saving to database");
        }
    }
}
