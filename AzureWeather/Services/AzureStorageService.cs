using Azure.Data.Tables;
using Azure.Storage.Blobs;
using AzureWeather.Models;
using Microsoft.Extensions.Configuration;
using System.Text;


namespace AzureWeather.Services
{
    public class AzureStorageService : IAzureStorageService
    {
        private readonly TableClient _tableClient;
        private readonly BlobContainerClient _blobContainerClient;

        public AzureStorageService(TableClient tableClient, BlobContainerClient blobContainerClient)
        {
            _tableClient = tableClient;
            _blobContainerClient = blobContainerClient;
        }

        public IEnumerable<LogEntry> GetLogsByTimePeriod(DateTimeOffset from, DateTimeOffset to)
        {
            var fromUtc = from.ToUniversalTime().ToString("o");
            var toUtc = to.ToUniversalTime().ToString("o");
            var filter = $"Timestamp ge datetime'{fromUtc}' and Timestamp le datetime'{toUtc}'";
            var logs = _tableClient.Query<LogEntry>(filter);
            return logs;
        }

        public async Task<string> GetPayloadByLogId(string logId)
        {            
            var blobClient = _blobContainerClient.GetBlobClient($"{logId}.json");
            if (await blobClient.ExistsAsync())
            {
                var response = await blobClient.DownloadAsync();
                using var streamReader = new StreamReader(response.Value.Content);
                return await streamReader.ReadToEndAsync();
            }
            return null;
        }

        public async Task LogAttempt(LogEntry logEntry)
        {
            await _tableClient.AddEntityAsync(logEntry);
        }

        public async Task SavePayload(string blobName, string payload)
        {
            var blobClient = _blobContainerClient.GetBlobClient($"{blobName}.json");
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(payload));
            await blobClient.UploadAsync(stream, overwrite: true);
        }
    }
}
