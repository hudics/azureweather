public class OpenWeatherApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public OpenWeatherApiClient(IHttpClientFactoryWrapper httpClientFactoryWrapper, string apiKey)
    {
        _httpClient = httpClientFactoryWrapper.CreateClient();
        _apiKey = apiKey;
    }

    public async Task<string> GetWeatherJsonAsync(string city, string country)
    {
        var url = $"http://api.openweathermap.org/data/2.5/weather?q={city},{country}&appid={_apiKey}&units=metric";
        var response = await _httpClient.GetStringAsync(url);
        return response;
    }
}
