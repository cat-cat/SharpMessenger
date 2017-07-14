using System.Collections.Generic;
using System.Collections.Specialized;
using ChatClient.Core.Common.Models;
using ChatClient.Core.Common;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace ChatClient.Core.UI.Controls
{
 public   class MessageViewCell:ViewCell
    {

		public MessageViewCell()
		{
			//string id = chatMessage.Id;
			//id =  id + "sf";
		}

		//void OnEvent(object sener, NotifyCollectionChangedEventArgs e)
		//{
		//	if (e.Action == NotifyCollectionChangedAction.Add)
		//	{
		//		var newItem = (KeyValuePair<k, object>)e.NewItems[0];
		//		if (newItem.Key == k.OnMessageSendProgress)
		//		{
		//			var d = (Dictionary<string, object>)newItem.Value;
		//			ChatMessage m = this.BindingContext as ChatMessage;
		//			if ((string)d["guid"] == m.guid && (ChatMessage.Status)d["status"] == ChatMessage.Status.Deleted)
		//			{
		//				// display status
		//				m.Message = "<deleted>";
		//				this.BindingContext = m;
		//			}
		//		}
		//	}
		//}

		protected override void OnAppearing()
		{
			//v.h(OnEvent);

			ChatMessage m = this.BindingContext as ChatMessage;
			//if (m.JustSent)
			//{
				v.Add(k.MessageSendProgress, m);
			//}
		}

		protected override void OnDisappearing()
		{
			//v.m(OnEvent);
		}
    }
}
