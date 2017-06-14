using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.Common;
using ChatClient.Core.Common.Interfaces;
using ChatClient.Core.Common.Models;
using ChatClient.Core.DAL.Data.Base;
using ChatClient.Core.SAL.Adapters;
using ChatClient.Core.SAL.Methods;
using ChatClient.Core.UI.PopupPages;
using ChatClient.Core.Common.Resx;

using Newtonsoft.Json;

using Rg.Plugins.Popup.Extensions;

using Xamarin.Forms;

namespace ChatClient.Core.UI.ViewModels
{
	public class UserViewModel : BaseViewModel
	{
		private User _account;
		private string _verificationCode;
		private Command _verificationCommand;
		private Command _cancelVerificationCommand;
		private Command _restoreAccountCommand;
		private Command _startRestoreAccountCommand;
		private string _oldEmail;
		public User Account
		{
			get
			{
				return _account;
			}
			set
			{
				SetProperty(ref _account, value, "Account");

			}
		}

		public string PageTitle
		{
			get
			{
				return AppResources.MyAccount;
			}
		}

		public string VerificationCode
		{
			get
			{
				return _verificationCode;
			}

			set
			{
				SetProperty(ref _verificationCode, value, "VerificationCode");
			}
		}

		public Command VerificationCommand
		{
			get
			{
				return _verificationCommand ?? (_verificationCommand = new Command(() => Verification()));
			}


		}

		public Command CancelVerificationCommand
		{
			get
			{
				return _cancelVerificationCommand ?? (_cancelVerificationCommand = new Command(() => CancelVerification()));
			}


		}

		public Command RestoreAccountCommand
		{
			get
			{
				return _restoreAccountCommand ?? (_restoreAccountCommand = new Command(() => RestoreAccount()));
			}


		}

		public string OldEmail
		{
			get
			{
				return _oldEmail;
			}

			set
			{
				SetProperty(ref _oldEmail, value, "OldEmail");
			}
		}

		public Command StartRestoreAccountCommand
		{
			get
			{
				return _startRestoreAccountCommand ?? (_startRestoreAccountCommand = new Command(() => StartRestore()));
			}


		}

		public UserViewModel()
		{
			Load();
		}

       private async void StartRestore() {
            await App.Navigation.PushPopupAsync(new pgRestoreAccount() { BindingContext = this });
        }
       private async void RestoreAccount() {
            if (IsBusy)
                return;
            IsBusy = true;
            if (await new RestoreAccount(Account.Token, OldEmail).Object()) {
                Account.Email = OldEmail;
                await App.Navigation.PushPopupAsync(new pgVerificationEmailCode() { BindingContext = this });
              
            }
            IsBusy = false;
        }
       private async void Verification() {
            if(IsBusy)
                return;
           IsBusy = true;
           User lUser = await new CheckEmail(_account.Token, _account.Email, _verificationCode).Object();
           if (lUser !=null) {
                   JsonConvert.PopulateObject(JsonConvert.SerializeObject(lUser), Account);
                   BL.Session.Authorization.Save(_account);
                   CloseAllPopPage();
           }
           IsBusy = false;
       }

       private async void CloseAllPopPage() {
            await App.Navigation.PopAllPopupAsync();
        }

       private async void CancelVerification() {
           CloseAllPopPage();
       }
       private async void Load() {
            Account = await BL.Session.Authorization.GetUser(true);
        }
   }
}
