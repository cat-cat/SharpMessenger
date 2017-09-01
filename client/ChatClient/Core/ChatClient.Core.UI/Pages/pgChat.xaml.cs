using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChatClient.Core.UI.Pages
{
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
        public ObservableCollection<Person> Items { get; set; }

        public pgChat()
        {
            InitializeComponent();

            Items = new ObservableCollection<Person>
            {
                new Person { FullName = "James Smith", Address = "404 Nowhere Street" },
                new Person { FullName = "John Doe", Address = "404 Nowhere Ave" }
            };

            BindingContext = this;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", ((Person)e.Item).FullName, "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}