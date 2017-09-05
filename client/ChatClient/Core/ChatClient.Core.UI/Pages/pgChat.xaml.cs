using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ChatClient.Core.Common.Models;
using ChatClient.Core.UI.ViewModels;

namespace ChatClient.Core.UI.Pages
{
    public class bc
    {
        public ObservableCollection<ChatMessage> Messages { get; set; }
        public bc()
        {
            Messages = new ObservableCollection<ChatMessage>
            {
                new ChatMessage { Name = "James Smith", Message = "404 Nowhere Street" },
                new ChatMessage { Name = "John Doe", Message = "404 Nowhere Ave" }
            };
        }
    }

    public class Person
    {
        public string FullName
        {
            get;
            set;
        }

        public string Address
        {
            get;
            set;
        }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pgChat : ContentPage
    {
        BaseViewModel ctx;
        public pgChat(string group_id)
        {
            BindingContext = new GroupChatViewModel(group_id);

            InitializeComponent();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            //await DisplayAlert("Item Tapped", ((Person)e.Item).FullName, "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}