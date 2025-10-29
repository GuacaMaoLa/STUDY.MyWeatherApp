using Ardalis.Result;
using LEARN_MVVM.Models;

namespace LEARN_MVVM.Repository
{
    public interface IWeatherRepository
    {
        Task SaveWeatherAsync(Temperature entry);
        Task<Result<Temperature>> ReadWeatherAsync(string city);
        Task UpdateWeatherAsync();
        Task DeleteWeatherAsync(Temperature entry);
    }
}
