using LEARN_MVVM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LEARN_MVVM.Data
{
    // Configuration for Temperature Model
    internal class TemperatureConfiguration : IEntityTypeConfiguration<Temperature>
    {
        public void Configure(EntityTypeBuilder<Temperature> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasIndex(x => x.City).IsUnique();
        }
    }
}