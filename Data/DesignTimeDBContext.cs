using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LEARN_MVVM.Data
{
    internal class DesignTimeDBContext : IDesignTimeDbContextFactory<WeatherAppContext>
    {
        public WeatherAppContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WeatherAppContext>();
            optionsBuilder.UseSqlite($"Data Source={App.DbPath}");

            return new WeatherAppContext(optionsBuilder.Options);
        }
    }
}
