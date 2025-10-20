using System.Data;
using LEARN_MVVM.Data;
using LEARN_MVVM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace LEARN_MVVM.Repository
{
    public class WeatherRepository(WeatherAppContext db) : IWeatherRepository
    {
        public IDbTransaction BeginTransaction()
        {
            var transaction = db.Database.BeginTransaction();

            return transaction.GetDbTransaction();
        }

        // Create a entry
        public async Task SaveWeatherAsync(Temperature entry)
        {
            db.Temperatures.Add(entry);
            await db.SaveChangesAsync();
        }

        // Read a entry
        public async Task<Temperature?> ReadWeatherAsync(string city)
        {
            //var entry = from temperatures in db.Temperature
            //            select temperatures;

            var entry = await db.Temperatures.SingleOrDefaultAsync(t => t.City == city);

            return entry;
        }
        
        // Update a entry
        public async Task UpdateWeatherAsync()
        {
            bool hasChanges= db.ChangeTracker.HasChanges();
            if (!hasChanges) return;
            await db.SaveChangesAsync();
        }

        // Delete a entry
        public async Task RemoveWeatherAsync(Temperature entry)
        {
            db.Temperatures.Remove(entry);
            await db.SaveChangesAsync();
        }
    }
}
