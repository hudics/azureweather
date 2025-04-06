using Azure;
using Azure.Data.Tables;

namespace AzureWeather.Models
{
    public class LogEntry : ITableEntity
    {
        public string PartitionKey { get; set; } = DateTime.UtcNow.ToString("yyyyMMdd");
        public string RowKey { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset? Timestamp { get; set; } = DateTimeOffset.MinValue;
        public ETag ETag { get; set; } = ETag.All;
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
