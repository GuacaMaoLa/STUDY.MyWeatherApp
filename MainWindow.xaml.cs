using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Wpf.Ui;

namespace LEARN_MVVM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            App.ServiceProvider.GetRequiredService<ISnackbarService>().SetSnackbarPresenter(SnackbarPresenter);
        }
    }
}