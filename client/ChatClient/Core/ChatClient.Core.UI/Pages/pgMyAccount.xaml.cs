using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.Common;
using ChatClient.Core.Common.Helpers;
using ChatClient.Core.Common.Models;
using ChatClient.Core.SAL.Adapters;
using ChatClient.Core.SAL.Methods;
using ChatClient.Core.UI.PopupPages;
using ChatClient.Core.UI.ViewModels;
using ChatClient.Core.Common.Resx;

using Rg.Plugins.Popup.Extensions;

using Xamarin.Forms;

namespace ChatClient.Core.UI.Pages
{
    public partial class pgMyAccount : ContentPage
    {
        private UserViewModel _userViewModel;
        private PhotoSourseViewModel _photoSourseViewModel;
        public pgMyAccount()
        {
            InitializeComponent();
            _userViewModel = new UserViewModel();
            BindingContext = _userViewModel;
            lblUserName.GestureRecognizers.Add(item: new TapGestureRecognizer((view) => ShowEditElemet(lblUserName, txbUserName)));
            lblUserEmail.GestureRecognizers.Add(item: new TapGestureRecognizer((view) => ShowEditElemet(lblUserEmail, txbUserEmail)));
            imgAvatar.GestureRecognizers.Add(item: new TapGestureRecognizer((view) => SelectImage()));


        }

        private bool Timer()
        {

            if (!string.IsNullOrEmpty(_photoSourseViewModel?.Photo) && _userViewModel.Account?.Photo != _photoSourseViewModel.Photo)
            {
                _userViewModel.Account.Photo = _photoSourseViewModel.Photo;
                UpdateUser();
                _photoSourseViewModel.Photo = "";
                return false;
            }
            return true;
        }

        private async void UpdateUser()
        {
          
                await new UserUpdate(_userViewModel.Account.Token, photo: _photoSourseViewModel.Photo).Object();
          

        }
        private async void txb_OnUnfocused(object sender, FocusEventArgs e)
        {

            if (sender == txbUserName)
            {
                User lUser = await BL.Session.Authorization.GetUser();
                if(lUser==null)
                    return;
             if(! await new UserUpdate(lUser.Token, null, txbUserName.Text).Object())
                    return;
                lUser = await BL.Session.Authorization.GetUser(true);

                BindingContext = new UserViewModel();
                lblUserName.IsVisible = true;
                txbUserName.IsVisible = false;
            }
            else if (sender == txbUserEmail)
            {
                if (!txbUserEmail.Text.Contains("@"))
                    return;
                IsBusy = true;
                if(await new AddEmail(_userViewModel.Account.Token, _userViewModel.Account.Email).Object())
                { 
                    await App.Navigation.PushPopupAsync(new pgVerificationEmailCode() { BindingContext = _userViewModel });
                    txbUserEmail.IsVisible = false;
                    lblUserEmail.IsVisible = true;
                }
                IsBusy = false;

            }
        }

        private async void ShowEditElemet(Label lable, Entry entry)
        {

            lable.IsVisible = false;
            entry.IsVisible = true;
            if (entry == txbUserEmail && _userViewModel?.Account != null && _userViewModel.Account.Email == AppResources.EnterEmail)
                entry.Text = "";
        }

        private async void SelectImage()
        {
            Device.StartTimer(new TimeSpan(0, 0, 0, 0, 100), Timer);
            _photoSourseViewModel = new PhotoSourseViewModel("userimage.png");
            pgPhotoSourse lPgPhotoSourse = new pgPhotoSourse() { BindingContext = _photoSourseViewModel };
            await App.Navigation.PushPopupAsync(lPgPhotoSourse);

        }
    }
}
