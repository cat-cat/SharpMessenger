using ChatClient.Core.Common.Resx;
using ChatClient.Core.Common;
using ChatClient.Core.Common.Models;
using System.Collections.Generic;

namespace ChatClient.Core.UI
{
	public class UIMessages
	{
		//public async Task<User> GreateNewUser()
		//{

		//	return await Authorization.CreateNewUser();
		//}

		//public void ShowException(string message)
		//{
		//	throw new NotImplementedException();
		//}
		async void OnEvent(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			var newItem = (KeyValuePair<k, object>)e.NewItems[0];
			if (newItem.Key == k.OnExceptionMessage)
			{
				await App.Current.MainPage.DisplayAlert(AppResources.Notification, (string)newItem.Value, "OK");
			}
			else if (newItem.Key == k.ConfirmGroupRemoval)
			{
				var d = (Dictionary<string, string>) newItem.Value;
				bool delete = await App.Current.MainPage.DisplayAlert(AppResources.Notification, d["message"], AppResources.Yes, AppResources.No);
				if (delete)
				{
					User lUser = await BL.Session.Authorization.GetUser();
					d.Add("token", lUser.Token);
					v.Add(k.OnConfirmGroupRemoval, d);
				}
			}
		}

		public UIMessages()
		{
			v.h(new k[] { k.OnExceptionMessage, k.ConfirmGroupRemoval }, OnEvent);
		}
	}
}
