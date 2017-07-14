using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.Common.Models;
using ChatClient.Core.UI.ViewModels;

using Rg.Plugins.Popup.Pages;

using Xamarin.Forms;

namespace  ChatClient.Core.UI.PopupPages
{
    public partial class pgEditMessage : PopupPage
    {
		public pgEditMessage(BaseViewModel vm)
        {
            InitializeComponent();
            BindingContext=vm;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

    }
}
