using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ChatClient.Core.Common
{
	public enum k { OnIsTyping, IsTyping, MessageSend, JoinRoom, OnMessageReceived, OnlineStatus, OnUpdateUserOnlineStatus }

	public class v : ObservableCollection<KeyValuePair<k, object> >
	{
		static v i;


		static v sharedInstance()
		{
			if (i == null)
				i = new v();

			return i;
		}

		public static void h(System.Collections.Specialized.NotifyCollectionChangedEventHandler handler)
		{
            sharedInstance().CollectionChanged += handler;
		}

		public static void m(System.Collections.Specialized.NotifyCollectionChangedEventHandler handler)
		{
            sharedInstance().CollectionChanged -= handler;
		}

		public static void Add(k key, object o)
		{
			//while (keysToRemove.Count > 0)
			//	Remove(keysToRemove.Pop());

			//var loc = new KeyValuePair<k, object>(key, o);
			//s.Add(loc);

			sharedInstance().OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Add,
			                                                                                             new List<KeyValuePair<k, object>>(new KeyValuePair<k, object>[] { new KeyValuePair<k, object>(key, o) })));
		}

		private v()
		{
		}
	}
}
