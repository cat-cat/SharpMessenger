using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ChatClient.Core.UI.Controls
{
   public class ChatListView:ListView
    {
		void Handle_ItemAppearing(object sender, ItemVisibilityEventArgs e)
		{
			int i = 4;
		}

		public ChatListView()
		{
			this.ItemAppearing += Handle_ItemAppearing;
		}
    }
}
