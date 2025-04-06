using Moq;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using AzureWeather.Models;

namespace AzureWeather.Services.Tests
{
    public class AzureStorageServiceTests
    {
        [Fact]
        public async Task LogAttempt_Success()
        {
            var mockTableClient = new Mock<TableClient>();
            var mockBlobContainerClient = new Mock<BlobContainerClient>();

            var azureStorageService = new AzureStorageService(mockTableClient.Object, mockBlobContainerClient.Object);

            var logEntry = new LogEntry { RowKey = "123", PartitionKey = "log", IsSuccess = true };

            await azureStorageService.LogAttempt(logEntry);

            mockTableClient.Verify(m => m.AddEntityAsync(logEntry, default), Times.Once);
        }
    }
}