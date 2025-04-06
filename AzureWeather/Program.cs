using Azure.Data.Tables;
using Azure.Storage.Blobs;
using AzureWeather.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services => {
        services.AddSingleton<TableClient>(serviceProvider =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var storageConnectionString = configuration["AzureWebJobsStorage"];
            var logTableName = "WeatherFetchLogs";
            var tableClient = new TableClient(storageConnectionString, logTableName);
            tableClient.CreateIfNotExists();
            return tableClient;
        });
        services.AddSingleton<BlobContainerClient>(serviceProvider =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var storageConnectionString = configuration["AzureWebJobsStorage"];
            var payloadContainerName = "weather-payloads";
            var blobServiceClient = new BlobServiceClient(storageConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(payloadContainerName);
            containerClient.CreateIfNotExistsAsync().Wait();
            return containerClient;
        });
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddHttpClient<IWeatherApiService, WeatherApiService>();
        services.AddSingleton<IAzureStorageService, AzureStorageService>();
    })
    .Build();

host.Run();