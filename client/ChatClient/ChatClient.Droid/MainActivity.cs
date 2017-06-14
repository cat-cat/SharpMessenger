#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.OS;
using Android.Util;

using ChatClient.Core.UI;
using ChatClient.Core.UI.Pages;
using ChatClient.Core.DAL.Data.Base;
using ChatClient.Core.Common.Models;
using ChatClient.Core.Common;
using ChatClient.Core.SAL.Methods;

using Firebase.Iid;
using Firebase.RemoteConfig;

using ImageCircle.Forms.Plugin.Droid;

using Plugin.Permissions;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Support.Compat;
using Android.Widget;

using HockeyApp.Android;
using HockeyApp.Android.Metrics;

#endregion

namespace ChatClient.Droid {
    [Activity(Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : FormsAppCompatActivity {
        #region Static & Const

        public static MainActivity Instance;

        #endregion

        #region Public Methods and Operators

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults) {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public bool IsPlayServicesAvailable() {
            GoogleApiAvailability api = GoogleApiAvailability.Instance;
            int resultCode = api.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success) {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode)) {
                    string error = GoogleApiAvailability.Instance.GetErrorString(resultCode);

                    //msgText.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                    Console.WriteLine(GoogleApiAvailability.Instance.GetErrorString(resultCode));
                } else {
                    Console.WriteLine("This device is not supported");
                    Finish();
                }
                return false;
            } else {
                Console.WriteLine("Google Play Services is available.");
                return true;
            }
        }

		#endregion


		async void checkTokens(string token)
		{			
			List<PushId> l = await PersisataceService.GetPushIdPersistance().GetItemsAsync();
			PushId pd = new PushId();
			pd.Id = "droid:" + token;
			pd.IsSended = true;

			if (!l.Contains(pd))
			{
				await PersisataceService.GetPushIdPersistance().SaveItemAsync(new PushId() { Id = "droid:" + token, IsSended = false });
				try
				{
					User lUser = await Core.BL.Session.Authorization.GetUser();
					if (lUser == null)
						return;
					if (await new UserRegistration(lUser.Token, "droid:" + token, new[] { "2123", "124435" }).Object())
						await PersisataceService.GetPushIdPersistance().SaveItemAsync(new PushId() { Id = "droid:" + token, IsSended = true });

					List<PushId> l2 = await PersisataceService.GetPushIdPersistance().GetItemsAsync();
					int i = l2.Count;
				}
				catch
				{
				}

			}
				
		}


		#region Protected Methods and Operators

        protected override void OnCreate(Bundle bundle) {
           	MetricsManager.Register(Application, "5e44345f59ba491c83bc268532e6d793");

            Log.Debug("TAG_FIREBASE_GOOGLE_APP_ID", "google app id: " + Resource.String.google_app_id);
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(bundle);
            Instance = this;
          
            Forms.Init(this, bundle);
            Core.BL.Session.Configuration.UpdateConfiguration(
                new Parameters() { BaseUrl = MyConstants.baseURL, ReFillUrl = "/robo/nx" },
                true);
            ImageCircleRenderer.Init();
           
            LoadApplication(new App());
            IsPlayServicesAvailable();
            var fbRemoteConfig = FirebaseRemoteConfig.Instance;
            SetRemoteConfig(fbRemoteConfig);
            string a = FirebaseInstanceId.Instance.Token;
			if (!string.IsNullOrEmpty(a))
			{
				Log.Debug("TAG_FIREBASE_TOKEN", "Current: {0}", a);
				checkTokens(a);
			}
            if (Intent.Extras != null)
                foreach (string key in Intent.Extras.KeySet()) {
                    string value = Intent.Extras.GetString(key);
                    Log.Debug("TAG_FIREBASE_TOKEN", "Key: {0} Value: {1}", key, value);
                }
        }

        protected async  void SetRemoteConfig(FirebaseRemoteConfig fbRemoteConfig) {
            try {
            await  fbRemoteConfig.FetchAsync(5);
            fbRemoteConfig.ActivateFetched();
            string refill = FirebaseRemoteConfig.Instance.GetString("refill_url");
            string baseurl = FirebaseRemoteConfig.Instance.GetString("BaseUrl");
             if(!string.IsNullOrEmpty(refill)&& !string.IsNullOrEmpty(baseurl))
                Core.BL.Session.Configuration.UpdateConfiguration(new Parameters() { BaseUrl = baseurl, ReFillUrl = refill }, false);
            }
            catch 
            {
            }
         
        }

        protected override void OnDestroy() {
            //   InAppService inAppService = App.ViewModel._inAppService as InAppService;
            //  inAppService.OnDestroy();

            base.OnDestroy();
        }

		protected override void OnResume()
		{
			base.OnResume();
			CrashManager.Register(this, "5e44345f59ba491c83bc268532e6d793");
		}

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data) {
            // InAppService inAppService = App.ViewModel._inAppService as InAppService;
            // inAppService.HandleActivityResult(requestCode, resultCode, data);
        }

        protected override async void OnNewIntent(Intent intent) {
            if (intent.HasExtra("Action")) {
                RootPage root = new RootPage();

                // root.Master = new NavigationPage(new pgGroups() {Title = "Игры"});

                string lAction = intent.GetStringExtra("Action");
                switch (lAction) {
                    case "EndGroup":
                        root.Detail = new NavigationPage(new pgFinishedGroup(intent.GetStringExtra("Ad_id")));
                        App.Current.MainPage = root;
                        App.Navigation = App.Current.MainPage.Navigation;
                        break;
                }
            }
        }

        #endregion
    }
}