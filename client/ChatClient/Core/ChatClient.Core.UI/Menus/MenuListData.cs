using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.UI.Pages;
using ChatClient.Core.Common.Resx;

namespace ChatClient.Core.UI.Menus
{
    public class MenuListData : List<MenuListItem>
    {
		public MenuListData()
		{
			this.Add(new MenuListItem()
			{
				Title = AppResources.ListOfGroups,
				IconSource = "menu_icon_list_of_groups.png",
				TargetType = typeof(pgGroups)
			});

			this.Add(new MenuListItem()
			{
				Title = AppResources.MyGroups,
				IconSource = "menu_my_groups.png",
				TargetType = typeof(pgMyGroups)
			});
			this.Add(new MenuListItem()
			{
				Title = AppResources.Dialogs,
				IconSource = "menu_dialogs.png",
				TargetType = typeof(pgDialogs)
			});
			this.Add(new MenuListItem()
			{
				Title = AppResources.About,
				IconSource = "menu_privacy_policies.png",
				TargetType = typeof(pgRules)
			});
		}
    }
}
