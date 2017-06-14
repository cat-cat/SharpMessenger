using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.BL.Session;
using ChatClient.Core.Common.Models;
using ChatClient.Core.UI.ViewModels;

using Xamarin.Forms;

namespace ChatClient.Core.UI.Pages
{
    public partial class pgDialogs : ContentPage
    {
        public pgDialogs()
        {
            InitializeComponent();
           lstDialogs.ItemAppearing += (sender, e) =>
            {
                MessagingCenter.Send<pgDialogs, Conversation>(this, "LoadItems", e.Item as Conversation);

            };
            BindingContext =new DialogsViewModel();
           lstDialogs.ItemTapped += async (sender, args) =>
            {
               Conversation item = args.Item as Conversation;
                if (item == null) return;
                    await Navigation.PushAsync(new ChatPage() {BindingContext = new ChatViewModel(item.Message.Opponent.Id!=Authorization.GetUser().Result.Id?item.Message.Opponent:item.Message.OwnerId)});
                lstDialogs.SelectedItem = null;
            };
        }
    }
}
