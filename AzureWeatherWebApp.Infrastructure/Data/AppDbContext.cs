using AzureWeatherWebApp.Core.Models;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {       
        public DbSet<CityForecast> Forecasts { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CityForecast>()
                .HasIndex(cf => new { cf.City });
        }
    }
}
