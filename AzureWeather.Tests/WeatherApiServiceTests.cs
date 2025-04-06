using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;

namespace AzureWeather.Services.Tests
{
    public class WeatherApiServiceTests
    {
        [Fact]
        public async Task FetchWeatherData_SuccessfulResponse_ReturnsSuccessAndPayload()
        {            
            var expectedPayload = "{\"key\":\"value\"}";
            var apiUrl = "http://example.com/api";

          
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expectedPayload)
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var mockLogger = new Mock<ILogger<WeatherApiService>>();
            var weatherApiService = new WeatherApiService(httpClient, mockLogger.Object);
           
            var (isSuccess, payload) = await weatherApiService.FetchWeatherData(apiUrl);
           
            Assert.True(isSuccess);
            Assert.Equal(expectedPayload, payload);
        }

        [Fact]
        public async Task FetchWeatherData_UnsuccessfulResponse_ReturnsFailureAndEmptyPayload()
        {           
            var apiUrl = "http://example.com/api";
           
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("Error message")
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var mockLogger = new Mock<ILogger<WeatherApiService>>();
            var weatherApiService = new WeatherApiService(httpClient, mockLogger.Object);
           
            var (isSuccess, payload) = await weatherApiService.FetchWeatherData(apiUrl);
          
            Assert.False(isSuccess);
            Assert.Equal(string.Empty, payload);
        }

        [Fact]
        public async Task FetchWeatherData_HttpRequestException_ReturnsFailureAndEmptyPayloadAndLogsError()
        {           
            var apiUrl = "http://example.com/api";
           
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Simulated exception"));

            var httpClient = new HttpClient(mockHandler.Object);
            var mockLogger = new Mock<ILogger<WeatherApiService>>();
            var weatherApiService = new WeatherApiService(httpClient, mockLogger.Object);
            
            var (isSuccess, payload) = await weatherApiService.FetchWeatherData(apiUrl);
           
            Assert.False(isSuccess);
            Assert.Equal(string.Empty, payload);
        }

        [Fact]
        public async Task FetchWeatherData_GeneralException_ReturnsFailureAndEmptyPayloadAndLogsError()
        {        
            var apiUrl = "http://example.com/api";
           
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new Exception("Simulated exception"));

            var httpClient = new HttpClient(mockHandler.Object);
            var mockLogger = new Mock<ILogger<WeatherApiService>>();
            var weatherApiService = new WeatherApiService(httpClient, mockLogger.Object);
            
            var (isSuccess, payload) = await weatherApiService.FetchWeatherData(apiUrl);
          
            Assert.False(isSuccess);
            Assert.Equal(string.Empty, payload);
        }
    }
}
