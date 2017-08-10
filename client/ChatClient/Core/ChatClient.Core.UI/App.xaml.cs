using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using ChatClient.Core.Common.Models;
using ChatClient.Core.DAL.Data;
using ChatClient.Core.DAL.Data.Base;
using ChatClient.Core.SAL.Adapters;
using ChatClient.Core.SAL.Methods;
using ChatClient.Core.UI.ViewModels;
using ChatClient.Core.UI.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ChatClient.Core.Common.Resx;
using ChatClient.Core.Common;
using ChatClient.Core.Common.Services;

namespace ChatClient.Core.UI
{
    public partial class App : Application
    {
        public static INavigation Navigation { get; set; }

		//  public static InAppViewModel ViewModel;
		IChatServices _chatServices;
		UIMessages _uiMessages;

        public async static void SetPage(Page page) {
            var a = await App.Current.MainPage.DisplayAlert(
									AppResources.Notification,
									AppResources.EndingGroupQuestion,
									"OK", AppResources.Cancel);
            if (a == true) {
               await Navigation.PushAsync(page);
            }
            }
        public async static void ShowMessage(string message)
        {
             await App.Current.MainPage.DisplayAlert(
				AppResources.Notification,
                message,
				"OK", AppResources.Cancel);
            
        }
        public App()
        {
            InitializeComponent();
            //      ViewModel = new InAppViewModel();
            //    ViewModel.RestoreState(Current.Properties);
            MainPage = new RootPage();
            Navigation = MainPage.Navigation;
            UpdatePushIds();
			_uiMessages = new UIMessages();

			//System.Diagnostics.Debug.WriteLine("====== resource debug info =========");
			//var assembly = typeof(App).GetTypeInfo().Assembly;
			//foreach (var res in assembly.GetManifestResourceNames())
			//	System.Diagnostics.Debug.WriteLine("found resource: " + res);
			//System.Diagnostics.Debug.WriteLine("====================================");

			// This lookup NOT required for Windows platforms - the Culture will be automatically set
			if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
			{
				// determine the correct, supported .NET culture
				var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
				AppResources.Culture = ci; // set the RESX for resource localization
				DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
			}

			// setup socket connection

			_chatServices = DependencyService.Get<IChatServices>();
			//_chatServices.SetRoomID(_roomName);
			//_chatServices = new ChatPrivateService();
			// _chatMessage = new ChatMessageViewModel();

			//    _messages = new ObservableCollection<ChatMessageViewModel>();
			//_receiver = user;
			//v.h(OnCollectionChanged);
			//v.Add(k.OnlineStatus, new Dictionary<string, object>() { { "userid", _receiver.Id } });
			//if (string.IsNullOrEmpty(_receiver.Nickname))
			//	_receiver.Nickname = _receiver.Id;
			_chatServices.Connect();
			//_chatServices.OnMessageReceived += _chatServices_OnMessageReceived;
			//_isPrivatChat = true;
			//GetMessages();

        }

        private async void UpdatePushIds() {
            try {
                List<PushId> lPushIds = await PersisataceService.GetPushIdPersistance().GetItemsAsync();
                User lUser = await Core.BL.Session.Authorization.GetUser();
                if(lUser==null)
                    return;
                foreach (PushId lPush in lPushIds.Where(o=>!o.IsSended)) {
                    if (await new UserRegistration(lUser.Token, lPush.Id, new[] { "2123", "124435" }).Object()) {
                        lPush.IsSended = true;
                        await PersisataceService.GetPushIdPersistance().SaveItemAsync(lPush);
                    }
                }
                lPushIds = null;
                lUser = null;
            }
            catch { }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
