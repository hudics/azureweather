using AzureWeather.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureWeatherFunc
{
    public class GetPayloadFunction
    {
        private readonly IAzureStorageService _azureStorageService;
        private readonly ILogger<GetPayloadFunction> _logger;

        public GetPayloadFunction(IAzureStorageService azureStorageService, ILogger<GetPayloadFunction> logger)
        {
            _azureStorageService = azureStorageService;
            _logger = logger;
        }

        [Function(nameof(GetPayloadFunction))]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "payload/{logId}")] HttpRequest req, string logId)
        {
            _logger.LogInformation($"Downloading the payload for log ID: {logId}");

            if (string.IsNullOrEmpty(logId))
            {
                return new BadRequestObjectResult("The 'logId' parameter is required.");
            }

            var payload = await _azureStorageService.GetPayloadByLogId(logId);

            if (payload != null)
            {
                return new OkObjectResult(payload);
            }
            else
            {
                return new NotFoundResult();
            }
        }
    }
}
