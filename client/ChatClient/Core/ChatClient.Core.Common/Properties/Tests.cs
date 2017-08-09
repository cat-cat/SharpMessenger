using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;

namespace ChatClient.Core.Common
{
	class DeadObject
	{
		void OnEvent(object sender, NotifyCollectionChangedEventArgs e)
		{
			var newItem = (KeyValuePair<k, object>)e.NewItems[0];
			Debug.WriteLine(String.Format("~ OnEvent() of dead object: key: {0} value: {1}", newItem.Key.ToString(), newItem.Value));
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
            Debug.WriteLine(String.Format("~ OnEvent(): key: {0} value: {1}", newItem.Key.ToString(), newItem.Value));

            if (newItem.Key == k.Unused)
            {
                // v.Add(k.Unused, "stack overflow crash"); // reentrant call in current thread causes stack overflow crash. Deadlock doesn't happen, because lock mechanism allows reentrancy for a thread that already has a lock on a particular object
                // Task.Run(() => v.Add(k.Unused, "deadlock")); // the same call in a separate thread don't overflow, but causes infinite recursive loop
            }
        }

        void OnEvent2(object sender, NotifyCollectionChangedEventArgs e)
        {
            var newItem = (KeyValuePair<k, object>)e.NewItems[0];
            Debug.WriteLine(String.Format("~ OnEvent2(): key: {0} value: {1}", newItem.Key.ToString(), newItem.Value));
        }

        void foreachTest(string[] a)
        {
            for (int i = 0; i < a.Length; i++)
            {
                Debug.WriteLine(String.Format("~ : {0}{1}", a[i], i));
            }
        }

        async void HandlersLockTester1(object sender, NotifyCollectionChangedEventArgs e)
        {
            var newItem = (KeyValuePair<k, object>)e.NewItems[0];
            Debug.WriteLine(String.Format("~ HandlersLockTester1(): key: {0} value: {1}", newItem.Key.ToString(), newItem.Value));
            await Task.Delay(300);
        }

        async void HandlersLockTester2(object sender, NotifyCollectionChangedEventArgs e)
        {
            var newItem = (KeyValuePair<k, object>)e.NewItems[0];
            Debug.WriteLine(String.Format("~ HandlersLockTester2(): key: {0} value: {1}", newItem.Key.ToString(), newItem.Value));
        }

        async Task<bool> rb()
        {
            await Task.Delay(10);
            return true;
        }

		public async void run()
		{
			// Direct call for garbage collector - should be called for testing purposes only, not recommended for a business logic of an application
			GC.Collect();

            /*
			 * == test v.Add()::foreach (var handlr in new List<NotifyCollectionChangedEventHandler>(handlersMap[key]))
			 * for two threads entering the foreach loop at the same time and iterating handlers only of its key
			 */
            var tbres = await rb();
			Task t1 = Task.Run(() => { v.Add(k.OnMessageReceived, "this key"); });
			Task t2 = Task.Run(() => { v.Add(k.MessageEdit, "that key"); });

			// wait for both threads to complete before executing next test
			await Task.WhenAll(new Task[] { t1, t2 });



			/* For now DeadObject may be already destroyed, so we may test catch block in v class */
			v.Add(k.OnlineStatus, "for dead object");


			/* test reentrant calls - causes stack overflow or infinite loop, depending on code at OnEvent::if(newItem.Key == k.Unused) clause */
			v.Add(k.Unused, 'a');


			/* testing foreach loop entering multiple threads */
			var s = Enumerable.Repeat("string", 200).ToArray();
			var n = Enumerable.Repeat("astring", 200).ToArray();
			t1 = Task.Run(() => { foreachTest(s); });
			t2 = Task.Run(() => { foreachTest(n); });

			// wait for both threads to complete before executing next test
			await Task.WhenAll(new Task[] { t1, t2 });


			/* testing lock(handlr) in Add() method of class v */
			v.h(new k[] { k.IsTyping }, HandlersLockTester1);
			v.h(new k[] { k.JoinRoom }, HandlersLockTester2);

			// line 1
			Task.Run(() => { v.Add(k.IsTyping, "first thread for the same handler"); });
			// line 2
			Task.Run(() => { v.Add(k.IsTyping, "second thread for the same handler"); });
			// line below will MOST OF TIMES complete executing before the line 2 above, because line 2 will wait completion of line 1
			// as both previous lines 1 and 2 are calling the same handler, access to which is synchronized by lock(handlr) in Add() method of class v
			Task.Run(() => { v.Add(k.JoinRoom, "third thread for other handler"); });
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
