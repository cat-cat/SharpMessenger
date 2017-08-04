using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;

namespace ChatClient.Core.Common
{
	public enum k {OnMessageEdit, MessageEdit, MessageReply, Unused, MessageSendProgress, OnMessageSendProgress, OnIsTyping, IsTyping, MessageSend, JoinRoom, OnMessageReceived, OnlineStatus, OnUpdateUserOnlineStatus }

	public class v
	{
		static Dictionary<k, HashSet<NotifyCollectionChangedEventHandler>> handlersMap = new Dictionary<k, HashSet<NotifyCollectionChangedEventHandler>>();

		public static void h(k[] keys, NotifyCollectionChangedEventHandler handler)
		{
			foreach (var key in keys)
				lock(handlersMap[key])
					handlersMap[key].Add(handler);
		}

		public static void m(NotifyCollectionChangedEventHandler handler)
		{
			foreach (k key in Enum.GetValues(typeof(k)))
				lock(handlersMap[key])
					handlersMap[key].Remove(handler);
		}

		public static void Add(k key, object o)
		{
			Monitor.Enter(handlersMap[key]);
			foreach (var handlr in new List<NotifyCollectionChangedEventHandler>(handlersMap[key]))
			{
				if (Monitor.IsEntered(handlersMap[key]))
				{
					Monitor.PulseAll(handlersMap[key]);
					Monitor.Exit(handlersMap[key]);
				}

				lock (handlr)
					try
					{
						handlr.Invoke(key, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<KeyValuePair<k, object>>(){ new KeyValuePair<k, object>(key, o) }));

#if __tests__
						/* check modification of global collection of handlers for a key while iteration through its copy */
						handlersMap[key].Add((object sender, NotifyCollectionChangedEventArgs e) => { });
#endif
					}
					catch (Exception e)
					{
						if (e is NullReferenceException)
							// handler invalid, remove it
							m(handlr);
					}
			}

			if (Monitor.IsEntered(handlersMap[key]))
			{
				Monitor.PulseAll(handlersMap[key]);
				Monitor.Exit(handlersMap[key]);
			}

		}

		static v()
		{
			foreach (k e in Enum.GetValues(typeof(k)))
				handlersMap[e] = new HashSet<NotifyCollectionChangedEventHandler>();

			new Tests().run();
		}
	}
}
