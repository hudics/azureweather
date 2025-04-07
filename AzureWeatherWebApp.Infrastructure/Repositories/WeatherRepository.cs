
using AzureWeatherWebApp.Core.Models;
using Core.Interfaces.Repository;
using Infrastructure.Data;


namespace Infrastructure.Repositories    
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly AppDbContext _context;

        public WeatherRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveWeatherData(IEnumerable<CityForecast> forecasts)
        {
            return true; 
        }
    }
}
