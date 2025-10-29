using Ardalis.Result;
using LEARN_MVVM.DataAccess;
using LEARN_MVVM.Models;

namespace LEARN_MVVM.Repository
{
    internal sealed class ApiWeatherRepository : IWeatherRepository
    {
        
        private const short CACHETIME= 10;
        private readonly IWeatherService _service;
        private readonly IWeatherRepository _other;

        public ApiWeatherRepository(IWeatherService service, IWeatherRepository other)
        {
            _service = service;
            _other = other;
        }

        public Task SaveWeatherAsync(Temperature entry)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<Temperature>> ReadWeatherAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return Result.Error("Please type in a city name");
            }
            else if (city.Any(char.IsDigit))
            {
                return Result.Error("Invalid search input");
            }

            var dbEntry = await _other.ReadWeatherAsync(city);

            DateTimeOffset now = DateTimeOffset.UtcNow;

            if (dbEntry.IsSuccess)
            {
                TimeSpan interval = now - dbEntry.Value!.TimeStamp;
                if (interval.TotalMinutes <= CACHETIME)
                {
                    return dbEntry;
                }
            }
            
            try
            {
                Root response = await _service.GetWeather(city);

                if (dbEntry.IsSuccess)
                {
                    dbEntry.Value.TimeStamp = now;
                    dbEntry.Value.Temp = response.Main.Temp;
                    await _other.UpdateWeatherAsync();
                    return Result.Success(dbEntry);
                }

                Temperature temperature = new()
                {
                    TimeStamp = now,
                    City = city,
                    Temp = response.Main.Temp
                };
                await _other.SaveWeatherAsync(temperature);

                return Result.Success(temperature);
            }
            catch (Exception ex)
            {
                // TODO: Exception to Result mapping

                return Result.Error(ex.Message);
            }
        }

        public Task UpdateWeatherAsync()
        {
            throw new NotImplementedException();
        }

        public Task DeleteWeatherAsync(Temperature entry)
        {
            throw new NotImplementedException();
        }
    }
}
