using Ardalis.Result;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LEARN_MVVM.DataAccess;
using LEARN_MVVM.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Refit;
using System.IO;
using System.Runtime.CompilerServices;
using System.Web;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace LEARN_MVVM.Modules.NotifyAndreModule
{
    public partial class NotifyAndreViewModel : ObservableObject
    {
        private const string HOSTURL = "https://ntfy.androd.gleeze.com";

        [ObservableProperty]
        private string _massage = string.Empty;

        public IRelayCommand SendMessageCommand { get; }

        public ISnackbarService SnackbarService { get; }

        private async Task SendMessage ()
        {
            if (string.IsNullOrWhiteSpace(Massage))
            {
                ShowSnackbarErrorMsg("Pls type in a massage!");
                return;
            }
            
            INotifyAndre notifyAndre = RestService.For<INotifyAndre>(HOSTURL);

            try
            {
                await notifyAndre.Collect(Massage);
            }
            catch
            {
                ShowSnackbarErrorMsg("Something went wrong");
            }

            Massage = string.Empty;
        }
        private void ShowSnackbarErrorMsg(string errorMsg)
        {
            SnackbarService.Show("Something went wrong", errorMsg,
                    ControlAppearance.Danger, new SymbolIcon(SymbolRegular.Fluent24), TimeSpan.FromSeconds(2));
        }

        public NotifyAndreViewModel() 
        {
            SendMessageCommand = new AsyncRelayCommand(SendMessage);
            SnackbarService = App.ServiceProvider.GetRequiredService<ISnackbarService>();
        }
    }
}
