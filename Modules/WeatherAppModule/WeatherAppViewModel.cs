using Ardalis.Result;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LEARN_MVVM.DataAccess;
using LEARN_MVVM.Models;
using LEARN_MVVM.Repository;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Refit;
using System.IO;
using Wpf.Ui;
using Wpf.Ui.Controls;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LEARN_MVVM.Modules.WeatherAppModule
{
    public partial class WeatherAppViewModel : ObservableObject
    {
        private const short COOLDOWN = 10;
        private const double KELVIN = 273.15;
        
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
            // trim any leading or trailing whitespaces
            City = City.Trim();
            
            using var scope = App.ServiceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IWeatherRepository>();
            // check if Database already have an entry for the city
            var temperatureEntry = await repo.ReadWeatherAsync(City);

            if (!temperatureEntry.IsSuccess)
            {
                // show snackbar with error message
                ShowSnackbarErrorMsg(temperatureEntry);

                return;
            }

            double temp_K = temperatureEntry.Value!.Temp;
            
            ShowTemperature(temp_K);
        }

        private void ShowSnackbarErrorMsg(Result<Temperature> apiResponse)
        {
            SnackbarService.Show("Something went wrong", string.Concat(apiResponse.Errors),
                    ControlAppearance.Danger, new SymbolIcon(SymbolRegular.Fluent24), TimeSpan.FromSeconds(3));
        }

        private void ShowTemperature(double temp_K)
        {
            double _temp_C = temp_K - KELVIN;

            string _temp_text = _temp_C.ToString("0.##");

            Temp = $"It is currently {_temp_text}°C";
        }

        public WeatherAppViewModel()
        {
            ClearBoxCommand = new RelayCommand(OnExecuteClearBox, CanExecuteClearBox);
            SearchTempCommand = new AsyncRelayCommand(OnExecuteSearchTemp);
            SnackbarService = App.ServiceProvider.GetRequiredService<ISnackbarService>();
        }
    } 
}

//string json = JsonConvert.SerializeObject(_weatherApiResponse, Formatting.Indented);

//using (StreamWriter file = File.CreateText($"{Directory.GetCurrentDirectory()}SQLite"))
//{
//    JsonSerializer serializer = new();
//    //serialize object directly into file stream
//    serializer.Serialize(file, _weatherApiResponse);
//}