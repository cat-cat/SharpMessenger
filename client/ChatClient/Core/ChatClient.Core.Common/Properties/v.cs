using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ChatClient.Core.Common
{
	public class v
	{
		//static int init = 0;
		public enum k {MessageSend, JoinRoom, SetRoomID, OnMessageReceived, OnlineStatus, OnUpdateUserOnlineStatus }

		static ObservableCollection<KeyValuePair<k, object> > s = new ObservableCollection<KeyValuePair<k, object> >();

		public static void h(System.Collections.Specialized.NotifyCollectionChangedEventHandler handler)
		{
			s.CollectionChanged += handler;
		}

		public static void m(System.Collections.Specialized.NotifyCollectionChangedEventHandler handler)
		{
			s.CollectionChanged -= handler;
		}

		public static void Add(k key, object o)
		{
			//if (init == 0)
			//{
			//	initialize();
			//	init = 1;
			//}

			var loc = new KeyValuePair<k, object>(key, o);
			s.Add(loc);
		}

		public static void Remove(k key)
		{
			KeyValuePair<k, object> item;
			foreach (var d in s)
			{
				if (d.Key == key)
				{
					item = d;
					break;
				}
			}

			if (item.Value != null)
				s.Remove(item);
		}

		public static void Consume(k key)
		{
			Remove(key);
		}

		public static bool ContainsKey(k key)
		{
			foreach (var d in s)
			{
				if (d.Key == key)
				   return true;
			}

			return false;
		}

		//private static void initialize()
		//{
		//	s.CollectionChanged += (sender, e) =>
		//	{ 
		//		int i = 4;
		//		i++;
		//	};

		//	s.CollectionChanged += (sender, e) =>
		//	{ 
		//		int i = 5;
		//		i++;
		//	};
		//}

		private v()
		{
		}
	}
}
