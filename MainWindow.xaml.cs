using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui;
using System.Runtime.InteropServices;
using System.Windows.Automation;
using System.Windows.Interop;

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

            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

            App.ServiceProvider.GetRequiredService<ISnackbarService>().SetSnackbarPresenter(SnackbarPresenter);
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private void plnControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, 2, 0);
        }
    }
}