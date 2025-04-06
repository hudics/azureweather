using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AzureWeather.Services;

namespace AzureWeather
{
    public class GetLogsFunction
    {
        private readonly IAzureStorageService _azureStorageService;
        private readonly ILogger<GetLogsFunction> _logger;

        public GetLogsFunction(IAzureStorageService azureStorageService, ILogger<GetLogsFunction> logger)
        {
            _azureStorageService = azureStorageService;
            _logger = logger;
        }

        [Function(nameof(GetLogsFunction))]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "logs")] HttpRequest req)
        {           
            string fromParam = req.Query["from"];
            string toParam = req.Query["to"];

            if (!DateTimeOffset.TryParse(fromParam, out var fromTime) || !DateTimeOffset.TryParse(toParam, out var toTime))
            {
                return new BadRequestObjectResult("The 'from' and 'to' parameters are required and must be in valid date format.");
            }

            var logs = _azureStorageService.GetLogsByTimePeriod(fromTime, toTime);

            return new OkObjectResult(logs.OrderByDescending(l => l.Timestamp));
        }
    }
}
