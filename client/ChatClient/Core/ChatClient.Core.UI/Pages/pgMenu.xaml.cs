using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.BL.Session;
using ChatClient.Core.Common;
using ChatClient.Core.Common.Models;
using ChatClient.Core.UI.Menus;
using ChatClient.Core.Common.Resx;

using Xamarin.Forms;

namespace ChatClient.Core.UI.Pages
{
    public partial class pgMenu : ContentPage {
        private Command _userCommand;

        public Command UserCommand {
            get {
                return _userCommand;
            }set {
                _userCommand = value;
            }
        }

        public ListView Menu { get; private set; }
        public pgMenu() {
            
            InitializeComponent();
			BindingContext = this;
            List<MenuListItem> lMenuListItems= new MenuListData();
           lstMenu.ItemsSource = lMenuListItems;
            Menu = lstMenu;
            lblUserName.GestureRecognizers.Add(item: new TapGestureRecognizer((view)=>UserCommand.Execute(null)));
            imgUser.GestureRecognizers.Add(item: new TapGestureRecognizer((view) => UserCommand.Execute(null)));
        }

		public string PageTitle
		{
			get
			{
				return AppResources.Menu;
			}
		}

        public async void Update() {
            try {

       
            User lUser = await Authorization.GetUser();
            lblUserName.Text = lUser.Nickname;
           imgUser.Source = lUser.Photo;
            }
            catch (Exception lException)
            {
                if (lException is NeedConnectionToNetwork)
                    await DisplayAlert("Connection Denied", "Can not continue, try again.", "OK");
                else if (lException is BadConnection)
                    await DisplayAlert("Bad connection", String.Format("{0, {1}}", lException.Message, "try again."), "OK");
            }

        }
        private void ShowUserDetail_OnClicked(object sender, EventArgs e) {
           _userCommand.Execute(null);
        }
    }
}
