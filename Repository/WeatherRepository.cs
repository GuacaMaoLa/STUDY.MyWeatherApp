using System.Data;
using Ardalis.Result;
using LEARN_MVVM.Data;
using LEARN_MVVM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace LEARN_MVVM.Repository
{
    internal sealed class WeatherRepository : IWeatherRepository
    {
        private readonly WeatherAppContext _db;

        public WeatherRepository(WeatherAppContext db)
        {
            _db = db;
        }

        public IDbTransaction BeginTransaction()
        {
            var transaction = _db.Database.BeginTransaction();

            return transaction.GetDbTransaction();
        }

        // Create a entry
        public async Task SaveWeatherAsync(Temperature entry)
        {
            _db.Temperatures.Add(entry);
            await _db.SaveChangesAsync();
        }

        // Read a entry
        public async Task<Result<Temperature>> ReadWeatherAsync(string city)
        {
            var entry = await _db.Temperatures.FirstOrDefaultAsync(t => t.City == city);
            
            if (entry is not null)
            {
                return entry;
            }

            return Result.NotFound();
        }
        //=> await _db
        //    .Set<Temperature>()
        //    .SingleOrDefaultAsync(temperature => temperature.City == city);

        // Update a entry
        public async Task UpdateWeatherAsync()
        {
            bool hasChanges= _db.ChangeTracker.HasChanges();
            if (!hasChanges) return;
            await _db.SaveChangesAsync();
        }

        // Delete a entry
        public async Task DeleteWeatherAsync(Temperature entry)
        {
            _db.Temperatures.Remove(entry);
            await _db.SaveChangesAsync();
        }
    }
}
