using System;
using System.Collections.Generic;
using System.Linq;

using ChatClient.Core.Common;
using ChatClient.Core.Common.Helpers;
using ChatClient.Core.Common.Models;
using ChatClient.Core.DAL.Data.Base;
using ChatClient.Core.SAL.Adapters;
using ChatClient.Core.SAL.Methods;
using ChatClient.Core.UI;

using Firebase.RemoteConfig;

using Foundation;

using ImageCircle.Forms.Plugin.iOS;

using UIKit;

using HockeyApp.iOS;


namespace ChatClient.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

			var manager = BITHockeyManager.SharedHockeyManager;
			manager.Configure("1d6afb0735974c47b10a582d948742d3");
			manager.StartManager();
			//manager.Authenticator.AuthenticateInstallation(); // This line is obsolete in crash only builds


            Core.BL.Session.Configuration.UpdateConfiguration(
				new Parameters() { BaseUrl = MyConstants.baseURL, ReFillUrl = "/robo/nx" },
				true);
            Firebase.Analytics.App.Configure();
			// Enabling developer mode, allows for frequent refreshes of the cache
			// RemoteConfig.SharedInstance.ConfigSettings = new RemoteConfigSettings(true);
			FatchRemoteConfigs();
			ImageCircleRenderer.Init();


            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                                   UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                                   new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }


            return base.FinishedLaunching(app, options);
        }

        public async override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            // Get current device token

            var DeviceToken = deviceToken.Description;
            if (!string.IsNullOrWhiteSpace(DeviceToken))
            {
                DeviceToken = DeviceToken.Trim('<').Trim('>');
            }

            // Get previous device token
            var oldDeviceToken = NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");

            // Has the token changed?
            if (string.IsNullOrEmpty(oldDeviceToken) || !oldDeviceToken.Equals(DeviceToken))
            {
                await PersisataceService.GetPushIdPersistance().SaveItemAsync(new PushId() { Id = DeviceToken, IsSended = false });
                try
                {
                    User lUser = await Core.BL.Session.Authorization.GetUser();
                    if (lUser == null)
                        return;
                    if (await new UserRegistration(lUser.Token, DeviceToken, new[] { "2123", "124435" }).Object())
                        await PersisataceService.GetPushIdPersistance().SaveItemAsync(new PushId() { Id = DeviceToken, IsSended = true });

                }
                catch { }
            }

            // Save new device token 
            NSUserDefaults.StandardUserDefaults.SetString(DeviceToken, "PushDeviceToken");
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            new UIAlertView("Error registering push notifications", error.LocalizedDescription, null, "OK", null).Show();
        }

        public  void FatchRemoteConfigs()
        {
            RemoteConfig.SharedInstance.Fetch(5, (status, error) =>
            {
                switch (status)
                {
                    case RemoteConfigFetchStatus.Success:
                        try { 
                        RemoteConfig.SharedInstance.ActivateFetched();
                            string refill = RemoteConfig.SharedInstance["refill_url"].StringValue;
                            string baseurl = RemoteConfig.SharedInstance["BaseUrl"].StringValue;
                            if (!string.IsNullOrEmpty(refill) && !string.IsNullOrEmpty(baseurl))
                                Core.BL.Session.Configuration.UpdateConfiguration(new Parameters() { BaseUrl = baseurl, ReFillUrl = refill }, false);
                    }
                    catch
                {
							LogHelper.WriteLog("** Exception on firebase fetch in iOS", "AppDelegate.cs");	
                }
                break;

                    case RemoteConfigFetchStatus.Throttled:
                    case RemoteConfigFetchStatus.NoFetchYet:
                  break;
                }
            });
        }
        public override void WillTerminate(UIApplication application)
        {
            base.WillTerminate(application);
        }
        // backgrounded
        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            base.ReceivedRemoteNotification(application, userInfo);
        }
        //foreground
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            base.DidReceiveRemoteNotification(application, userInfo, completionHandler);
        }
    }
}
