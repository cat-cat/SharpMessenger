using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using ChatClient.Core.Common.Helpers;

namespace ChatClient.Core.Common
{
	class DeadObject
	{
		void OnEvent(object sender, NotifyCollectionChangedEventArgs e)
		{
			var newItem = (KeyValuePair<k, object>)e.NewItems[0];
			LogHelper.WriteLog("~ OnEvent() of dead object: key {0} value {1}", newItem.Key.ToString(), newItem.Value);
		}

		public DeadObject()
		{
			v.h(new k[] { k.OnlineStatus }, OnEvent);
		}

		~DeadObject()
		{
			// Accidentally we forgot to call v.m(OnEvent) here, and now v.handlersMap contains reference to "dead" handler
		}
	}

	public class Tests
	{

		void OnEvent(object sender, NotifyCollectionChangedEventArgs e)
		{
			var newItem = (KeyValuePair<k, object>)e.NewItems[0];
			LogHelper.WriteLog("~ OnEvent(): key {0} value {1}", newItem.Key.ToString(), newItem.Value);

			if (newItem.Key == k.Unused)
			{
				// v.Add(k.Unused, "stack overflow crash"); // reentrant call in current thread causes stack overflow crash. Deadlock doesn't happen, because lock mechanism allows reentrancy for a thread that already has a lock on a particular object
				// Task.Run(() => v.Add(k.Unused, "deadlock")); // the same call in a separate thread don't overflow, but causes infinite recursive loop
			}
		}

		void OnEvent2(object sender, NotifyCollectionChangedEventArgs e)
		{
			var newItem = (KeyValuePair<k, object>)e.NewItems[0];
			LogHelper.WriteLog("~ OnEvent2(): key {0} value {1}", newItem.Key.ToString(), newItem.Value);
		}

		void foreachTest(string[] s)
		{
			foreach (string i in s)
			{
				LogHelper.WriteLog("~ : {0}", i);
			}
		}

		public async void run()
		{
			// Direct call for garbage collector - should be called for testing purposes only, not recommended for a business logic of an application
			GC.Collect();

			/*
			 * == test v.Add()::foreach (var handlr in new List<NotifyCollectionChangedEventHandler>(handlersMap[key]))
			 * for two threads entering the foreach loop at the same time and iterating handlers only of its key
			 */
			Task t1 = Task.Run(() => { v.Add(k.OnMessageReceived, "this key"); });
			Task t2 = Task.Run(() => { v.Add(k.MessageEdit, "that key"); });

			/* wait for both threads to complete before executing next test */
			await Task.WhenAll(new Task[] { t1, t2 });



			// For now DeadObject may be already destroyed, so we may get into catch block in v class
			v.Add(k.OnlineStatus, "for dead object");


			/* test reentrant calls - causes stack overflow or infinite loop, depending on code at OnEvent::if(newItem.Key == k.Unused) clause */
			v.Add(k.Unused, 'a');


			/* testing foreach loop entering multiple threads */
			string[] s = new string[1000];
			string[] n = new string[1000];
			int i = 0;
			while (i < 1000) {
				s[i] = "string" + i++;
			}
			i = 0;
			while (i < 1000) {
				n[i] = "astring" + i++;
			}
			Task.Run(() => { foreachTest(s); });
			Task.Run(() => { foreachTest(n); });
		}


		public Tests()
		{
			// add OnEvent for each key
			v.h(new k[] { k.OnMessageReceived, k.MessageEdit, k.Unused }, OnEvent);

			// add OnEvent2 for each key
			v.h(new k[] { k.Unused, k.OnMessageReceived, k.MessageEdit }, OnEvent2);

			/* == test try catch blocks in v class, when handler is destroyed before handlr.Invoke() called */
			var ddo = new DeadObject();
			// then try to delete object, setting its value to null. We are in a managed environment, so we can't directly manage life cicle of an object.
			ddo = null;
		}
	}
}
