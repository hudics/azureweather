using AzureWeather.Models;

namespace AzureWeather.Services
{
    public interface IAzureStorageService
    {
        Task LogAttempt(LogEntry logEntry);
        Task SavePayload(string blobName, string payload);
        IEnumerable<LogEntry> GetLogsByTimePeriod(DateTimeOffset from, DateTimeOffset to);
        Task<string> GetPayloadByLogId(string logId);
    }
}
