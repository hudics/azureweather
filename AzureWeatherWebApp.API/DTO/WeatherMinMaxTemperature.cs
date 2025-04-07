using AzureWeatherWebApp.Core.Models;

namespace AzureWeatherWebApp.API.DTO
{
    public class WeatherMinMaxTemperature
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public string City { get;set; }
    }
}
