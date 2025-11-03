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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Temperature>(builder =>
            {
                builder.HasKey(x => x.Id);

                builder.Property(x => x.TimeStamp)
                    .IsRequired();

                builder.Property(x => x.City)
                    .IsRequired()
                    .HasMaxLength(200);
                builder.HasIndex(x => x.City)
                    .IsUnique();

                builder.Property(x => x.Temp)
                    .IsRequired()
                    .HasPrecision(5, 2);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
