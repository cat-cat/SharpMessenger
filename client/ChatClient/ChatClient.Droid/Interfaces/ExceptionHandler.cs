using System;
using System.Threading.Tasks;

using ChatClient.Core.BL.Session;
using ChatClient.Core.Common.Interfaces;
using ChatClient.Core.Common.Models;
using ChatClient.Core.UI;
using ChatClient.Droid.Interfaces;
using ChatClient.Core.Common.Resx;

using Xamarin.Forms;

[assembly: Dependency(typeof(ExceptionHandler))]
namespace ChatClient.Droid.Interfaces
{
   public class ExceptionHandler:IExceptionHandler
    {
       public Task<User> GreateNewUser() {

           return Authorization.CreateNewUser();
       }

       public void ShowException(string message) {
           throw new NotImplementedException();
       }

       public async void ShowMessage(string message) {
            Device.BeginInvokeOnMainThread(async () => {
				await App.Current.MainPage.DisplayAlert(AppResources.Notification, message, "OK");

            }
            );
          
        }

       public async Task<bool> YesNoQuestion(string message) {
            bool response = false;
            //Device.BeginInvokeOnMainThread(async () => {
			response = await App.Current.MainPage.DisplayAlert(AppResources.Notification, message, AppResources.Yes, AppResources.No);
            //}
            //);
            return response;
        }
   }
}
