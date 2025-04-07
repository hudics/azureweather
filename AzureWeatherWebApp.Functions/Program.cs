using Azure.Storage.Queues;
using AzureWeather.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services => {
        services.AddSingleton<QueueClient>(serviceProvider =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var storageConnectionString = configuration["AzureWebJobsStorage"];
            var queueName = "weatherdataqueue";
            var queueClient = new QueueClient(storageConnectionString, queueName);
            queueClient.CreateIfNotExistsAsync().Wait();
            return queueClient;
        });
        services.AddHttpClient();       
        services.AddSingleton<IHttpClientFactoryWrapper, HttpClientFactoryWrapper>();
        services.AddSingleton<OpenWeatherApiClient>(serviceProvider =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var apiKey = configuration["OpenWeatherMap:ApiKey"]; 
            var httpClientFactoryWrapper = serviceProvider.GetRequiredService<IHttpClientFactoryWrapper>();
            return new OpenWeatherApiClient(httpClientFactoryWrapper, apiKey);
        });
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddSingleton<IWeatherApiService, WeatherApiService>();
        services.AddSingleton<IAzureStorageService, AzureStorageService>();
    })
    .Build();

host.Run();