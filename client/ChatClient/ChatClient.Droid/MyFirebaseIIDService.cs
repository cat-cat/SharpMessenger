using System;
using System.Diagnostics;

using Android.App;
using Android.Content;
using Android.Gms.Common.Apis;
using Android.OS;
using Android.Support.V4.Content;
using Android.Telephony;
using Android.Util;

using ChatClient.Core.SAL.Methods;
using ChatClient.Core.BL;
using Firebase.Iid;
using ChatClient.Core.Common.Models;
using ChatClient.Core.DAL.Data.Base;
using ChatClient.Core.SAL.Adapters;
using ChatClient.Core.UI;
using ChatClient.Core.UI.Pages;

using Firebase.Messaging;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ChatClient.Droid;

namespace ChatClient.Droid {
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIIDService : FirebaseInstanceIdService {
        private const string TAG = "MyFirebaseIIDService";

        public override void OnTokenRefresh() {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Log.Debug(TAG, "Refreshed token: " + refreshedToken);

            SendRegistrationToServer(refreshedToken);
        }

        private async void SendRegistrationToServer(string token) {

         //   TODO test with sim card
           //    TelephonyManager lTelephonyManager = (TelephonyManager)GetSystemService(TelephonyService);
          //  string lNumber = lTelephonyManager.Line1Number;
            await PersisataceService.GetPushIdPersistance().SaveItemAsync(new PushId() { Id = "droid:" + token, IsSended = false });
            try
            {
                User lUser = await Core.BL.Session.Authorization.GetUser();
              if(lUser==null)
                    return;
                if (await new UserRegistration(lUser.Token, "droid:" + token, new[] { "2123", "124435" }).Object())
                    await PersisataceService.GetPushIdPersistance().SaveItemAsync(new PushId() { Id = "droid:" + token, IsSended = true });

            }
            catch
            {
            }


        }



    }

    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService {


        private const string TAG = "MyFirebaseMessagingService";

        public override void OnMessageReceived(RemoteMessage message) {
            Device.BeginInvokeOnMainThread(
                () => {
                    if (message.Data.ContainsKey("Action") && message.Data["Action"] == "EndGroup")
                        App.SetPage(new pgFinishedGroup(message.Data["Ad_id"]));

                }
                );

        }


    }

    [BroadcastReceiver(Permission = "com.google.android.c2dm.permission.SEND")]
    [IntentFilter(new string[] { "com.google.android.c2dm.intent.RECEIVE" }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { "com.google.android.c2dm.intent.REGISTRATION" }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { "com.google.android.gcm.intent.RETRY" }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Intent.ActionBootCompleted })]
    public class BroadcastReceiver : WakefulBroadcastReceiver {

        public override void OnReceive(Context context, Intent intent) {
            StartWakefulService(context, intent);
           
        }
    }

    [Service]
    public class NotificationIntentService : IntentService {
        protected override void OnHandleIntent(Intent intent) {
            ShowLocalNotification(ApplicationContext, intent);

            WakefulBroadcastReceiver.CompleteWakefulIntent(intent);
        }


        private void ShowLocalNotification(Context context, Intent intent) {
            Notification.Builder builder = new Notification.Builder(context)
                .SetContentTitle("Title")
                .SetContentText("Message")
                .SetDefaults(NotificationDefaults.Sound)
                .SetSmallIcon(Resource.Drawable.icon);

            // Build the notification:
            Notification notification = builder.Build();

            // Get the notification manager:
            NotificationManager notificationManager = Forms.Context.GetSystemService(Context.NotificationService) as NotificationManager;

            notificationManager.Notify(0, notification);
        }
    }
}