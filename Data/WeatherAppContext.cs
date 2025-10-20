using LEARN_MVVM.Models;
using Microsoft.EntityFrameworkCore;

namespace LEARN_MVVM.Data
{
    public class WeatherAppContext : DbContext
    {
        public DbSet<Temperature> Temperatures { get; set; }

        public WeatherAppContext(DbContextOptions<WeatherAppContext> options) 
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite($"Data Source ={DbPath}");
        }
    }
}
