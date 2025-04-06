using Microsoft.Extensions.Logging;


namespace AzureWeather.Services
{
    public class WeatherApiService : IWeatherApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WeatherApiService> _logger;

        public WeatherApiService(HttpClient httpClient, ILogger<WeatherApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<(bool IsSuccess, string Payload)> FetchWeatherData(string apiUrl)
        {
            try
            {
                var response = await _httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var payload = await response.Content.ReadAsStringAsync();
                    return (true, payload);
                }
                else
                {
                    _logger.LogError($"Error HTTP: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                    return (false, string.Empty);
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error while executing API: {ex.Message}");
                return (false, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error: {ex.Message}");
                return (false, string.Empty);
            }
        }

    }
}
