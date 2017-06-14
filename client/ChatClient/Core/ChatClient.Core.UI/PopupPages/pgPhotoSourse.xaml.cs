using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rg.Plugins.Popup.Pages;

using Xamarin.Forms;
using ChatClient.Core.Common.Resx;

namespace ChatClient.Core.UI.PopupPages
{
    public partial class pgPhotoSourse : PopupPage
    {
        public pgPhotoSourse()
        {
			BindingContext = this;
            InitializeComponent();
        }

		public string PageTitle
		{
			get
			{
				return AppResources.SelectPhotoLocation;
			}
		}
    }
}
