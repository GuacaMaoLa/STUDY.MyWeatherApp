using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LEARN_MVVM.HistoryModule;
using LEARN_MVVM.WeatherAppModule;

namespace LEARN_MVVM
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public IRelayCommand HomeViewCommand { get; set; }

        public IRelayCommand HistoryViewCommand { get; set; }

        private void SetWeatherAppView()
        {
            CurrentView = WeatherAppVM;
        }

        private void SetHistoryView()
        {
            CurrentView = HistoryVM;
        }

        public HistoryViewModel HistoryVM { get; set; }

        public WeatherAppViewModel WeatherAppVM { get; set; }

        [ObservableProperty]
        private object _currentView;

        public MainWindowViewModel()
        {
            HistoryVM = new HistoryViewModel();
            WeatherAppVM = new WeatherAppViewModel();
            HomeViewCommand = new RelayCommand(SetWeatherAppView);
            HistoryViewCommand = new RelayCommand(SetHistoryView);
            CurrentView = WeatherAppVM;
        }
    }
}