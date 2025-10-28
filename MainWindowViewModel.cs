using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LEARN_MVVM.Modules.HistoryModule;
using LEARN_MVVM.Modules.NotifyAndreModule;
using LEARN_MVVM.Modules.WeatherAppModule;

namespace LEARN_MVVM
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public IRelayCommand HomeViewCommand { get; set; }

        public IRelayCommand HistoryViewCommand { get; set; }

        public IRelayCommand NotifyAndreCommand { get; set; }

        private void SetWeatherAppView()
        {
            CurrentView = WeatherAppVM;
        }

        private void SetHistoryView()
        {
            CurrentView = HistoryVM;
        }

        private void SetNotifyAndreView()
        {
            CurrentView = NotifyAndreVM;
        }

        public HistoryViewModel HistoryVM { get; set; }

        public WeatherAppViewModel WeatherAppVM { get; set; }

        public NotifyAndreViewModel NotifyAndreVM { get; set; }

        [ObservableProperty]
        private object _currentView;

        public MainWindowViewModel()
        {
            HistoryVM = new HistoryViewModel();
            WeatherAppVM = new WeatherAppViewModel();
            NotifyAndreVM = new NotifyAndreViewModel();
            HomeViewCommand = new RelayCommand(SetWeatherAppView);
            HistoryViewCommand = new RelayCommand(SetHistoryView);
            NotifyAndreCommand = new RelayCommand(SetNotifyAndreView);
            CurrentView = WeatherAppVM;
        }
    }
}