using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.Common;
using ChatClient.Core.Common.Models;
using ChatClient.Core.UI.ViewModels;

using Xamarin.Forms;

namespace ChatClient.Core.UI.Pages
{
    public partial class pgMembers : ContentPage
    {
        public pgMembers()
        {
            InitializeComponent();
            try
            {
                lsvMembers.ItemTapped += async (sender, args) =>
                {
                    User item = args.Item as Member;
                    if (item == null) return;
					await App.Navigation.PushAsync(new pgChat { BindingContext = new PrivateChatViewModel(item) });
                    lsvMembers.SelectedItem = null;
                };
            }
            catch (Exception lException)
            {
                if (lException is NeedConnectionToNetwork)
                    DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                else if (lException is BadConnection)
                    DisplayAlert("Bad connection", String.Format("{0, {1}}", lException.Message, "try again."), "OK");
            }
        }
    }
}
