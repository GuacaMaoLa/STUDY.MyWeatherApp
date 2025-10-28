using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LEARN_MVVM.DataAccess;
using Newtonsoft.Json;
using Refit;
using System.IO;
using System.Runtime.CompilerServices;

namespace LEARN_MVVM.Modules.NotifyAndreModule
{
    public partial class NotifyAndreViewModel : ObservableObject
    {
        private const string HOSTURL = "https://ntfy.androd.gleeze.com";

        [ObservableProperty]
        private string _massage = string.Empty;

        public IRelayCommand SendMessageCommand { get; }

        private async Task SendMessage ()
        {
            INotifyAndre notifyAndre = RestService.For<INotifyAndre>(HOSTURL);

            await notifyAndre.Collect(Massage);

            Massage = string.Empty;
        }

        public NotifyAndreViewModel() 
        {
            SendMessageCommand = new AsyncRelayCommand(SendMessage);
        }
    }
}
