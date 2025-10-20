using Ardalis.Result;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LEARN_MVVM.DataAccess;
using LEARN_MVVM.Models;
using LEARN_MVVM.Repository;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace LEARN_MVVM.WeatherAppModule
{
    public partial class WeatherAppViewModel : ObservableObject
    {
        private const short COOLDOWN = 10;
        private const double KELVIN = 273.15;
        private const string WEATHERAPI = "https://api.openweathermap.org/data/2.5";
        
        /// <summary>
        /// Clear the content of every textbox
        /// </summary>
        public IRelayCommand ClearBoxCommand { get; }
        /// <summary>
        /// Search for current temperature using openweather api
        /// </summary>
        public IRelayCommand SearchTempCommand { get; }
        /// <summary>
        /// SnackbarService to display error state
        /// </summary>
        public ISnackbarService SnackbarService { get; }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ClearBoxCommand))]
        private string _city = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ClearBoxCommand))]
        private string _temp = string.Empty;

        private bool CanExecuteClearBox()
        {
            return !string.IsNullOrEmpty(City) || !string.IsNullOrEmpty(Temp);
        }

        private void OnExecuteClearBox()
        {
            City = string.Empty;
            Temp = string.Empty;
        }

        private async Task OnExecuteSearchTemp()
        {
            
            // check if Database already have an entry for the city
            bool hasEntry = await HasEntryTemperatureTable(City);
            if (hasEntry) return;
            
            // else get new api response
            Result<Root> weatherApiResponse = await GetApiRequestAsync(City);

            if (!weatherApiResponse.IsSuccess)
            {
                // show snackbar with error message
                SnackbarService.Show("Something went wrong", string.Concat(weatherApiResponse.Errors),
                    ControlAppearance.Danger, new SymbolIcon(SymbolRegular.Fluent24), TimeSpan.FromSeconds(3));

                return;
            }
            
            double temp_K = weatherApiResponse.Value!.Main.Temp;
            double temp_C = temp_K - KELVIN;
            string temp_text = temp_C.ToString("0.##");

            Temp = $"It is currently {temp_text}°C";

            using var scope = App.ServiceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<WeatherRepository>();
            using var transaction = repo.BeginTransaction();

            try
            {
                // Creates a new entry in Temperature Table
                await repo.SaveWeatherAsync(new Temperature
                {
                    City = City,
                    Temp = temp_K,
                    TimeStamp = DateTimeOffset.UtcNow
                });
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
        }

        private async Task<bool> HasEntryTemperatureTable(string _city)
        {
            using var scope = App.ServiceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<WeatherRepository>();
            
            Temperature? read = await repo.ReadWeatherAsync(_city);
            
            if (read is null) return false;

            string temp_text = string.Empty;
            DateTimeOffset now = DateTimeOffset.UtcNow;
            TimeSpan interval = now - read.TimeStamp;

            if (interval.TotalMinutes < COOLDOWN) return false;

            Result<Root> update = await GetApiRequestAsync(_city);
            
            if (!update.IsSuccess)
            {
                SnackbarService.Show("Fehler", string.Concat(update.Errors),
                    ControlAppearance.Danger, new SymbolIcon(SymbolRegular.Fluent24), TimeSpan.FromSeconds(3));

                return true;
            }
            
            using var transaction = repo.BeginTransaction();

            try
            {
                read.TimeStamp = now;
                
                read.Temp = update.Value!.Main.Temp;
                // Update time stamp for entry
                await repo.UpdateWeatherAsync();

                double _temp_C = read.Temp - KELVIN;
                
                temp_text = _temp_C.ToString("0.##");
                
                Temp = $"It is currently {temp_text}°C";

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }

            return true;
        }

        private static async Task<Result<Root>> GetApiRequestAsync(string _city)
        {
            if (string.IsNullOrEmpty(_city))
            {
                return Result.Error("Please type in a city name");
            }

            IWeatherService weatherApi = RestService.For<IWeatherService>(WEATHERAPI);

            try
            {
                Root response = await weatherApi.GetWeather(_city);

                return Result.Success(response);
            }
            catch (Exception ex)
            {
                // TODO: Exception to Result mapping

                return Result.Error(ex.Message);
            }
        }

        public WeatherAppViewModel()
        {
            ClearBoxCommand = new RelayCommand(OnExecuteClearBox, CanExecuteClearBox);
            SearchTempCommand = new AsyncRelayCommand(OnExecuteSearchTemp);
            SnackbarService = App.ServiceProvider.GetRequiredService<ISnackbarService>();
        }
    } 
}