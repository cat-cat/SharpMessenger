#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ChatClient.Core.BL.Media;
using ChatClient.Core.Common;
using ChatClient.Core.Common.Annotations;
using ChatClient.Core.Common.Helpers;
using ChatClient.Core.Common.Models;
using ChatClient.Core.Common.Models.Base;
using ChatClient.Core.SAL.Adapters;
using ChatClient.Core.SAL.Methods;
using ChatClient.Core.UI.Pages;
using ChatClient.Core.UI.PopupPages;
using ChatClient.Core.Common.Resx;

using Newtonsoft.Json;

using Rg.Plugins.Popup.Extensions;

using Xamarin.Forms;
using ChatClient.Core.Common.Interfaces;

#endregion

namespace ChatClient.Core.UI.ViewModels
{
    public class GroupViewModel : BaseViewModel
    {
        #region Fields

        private int _contribution;
        private User _creator;
        private User _winner;
        private Group _group;
        private Command _startChatCommand;
        private Command _membersCommand;
        private Command _contributionCommand;
        private Command _cancelContributionCommand;
        private Command _makeContributionCommand;
        private Command _saveGroupCommand;
        private Command _updateGroupCommand;
        private Command _startPrivatChat;
        private MembersViewModel _membersViewModel;
        #endregion

        #region Constractors and Destructors

        public GroupViewModel(Group group,bool endedGroup=false)
        {
            CurrentGroup = group;
            if (!string.IsNullOrEmpty(group.Id))
                MembersViewModel = new MembersViewModel(_group.Id);
            if (endedGroup) {
              UpdateViewModel(null,CurrentGroup.Id);
            }
            // Device.StartTimer(new TimeSpan(0, 0, 0, 1), Timer);
        }
        public GroupViewModel(string groupId)
        {
            CurrentGroup = new Group();
            UpdateViewModel(null,groupId);
          
               
            // Device.StartTimer(new TimeSpan(0, 0, 0, 1), Timer);
        }
       

        #endregion

        #region Properties

		public string PageTitle
		{
			get
			{
				return AppResources.Group;
			}
		}

       //public async Task<string>  SelectImage() {
        //    try {
        //        string lFilePath = await Photo.PickPhoto("newgroup.jpg");
        //        if (string.IsNullOrEmpty(lFilePath))
        //            await App.Current.MainPage.DisplayAlert("", "Изображение не выбранно", "Ok");
        //        else
        //            return  lFilePath;
        //    } catch (Exception lException) {
        //        if (lException is NeedCamera)
        //            await App.Current.MainPage.DisplayAlert("", lException.Message, "Ok");
        //        else
        //            LogHelper.WriteException(lException);
        //    }
        //    return "";
        //}

        public Command MembersCommand
        {
            get
            {
                return _membersCommand ?? (_membersCommand = new Command(() => { ShowMembers(); }));
            }
        }

        public Command StartChatCommand
        {
            get
            {
                return _startChatCommand ?? (_startChatCommand = new Command(
                () => { StartChat(); }));
            }

        }


        public Group CurrentGroup
        {
            get
            {
                return _group;
            }
            set
            {
                SetProperty(ref _group, value, "CurrentGroup");
            }
        }

        public int Contribution
        {
            get
            {
                return _contribution;
            }
            set
            {
                SetProperty(ref _contribution, value, "Contribution");
            }
        }

        public Command SaveGroupCommand
        {
            get
            {
                return _saveGroupCommand ?? (_saveGroupCommand = new Command(() => { SaveGroup(); }));
            }
        }

        public Command UpdateGroupCommand
        {
            get
            {
                return _updateGroupCommand ?? (_updateGroupCommand = new Command(() => { UpdateGroup(); }));
            }
        }

        public MembersViewModel MembersViewModel
        {
            get
            {
                return _membersViewModel;
            }

            set
            {
                SetProperty(ref _membersViewModel, value, "MembersViewModel");
            }
        }

        public User Creator
        {
            get
            {
                return _creator;
            }

            set
            {
                _creator = value;
            }
        }

        public User Winner
        {
            get
            {
                return _winner;
            }

            set
            {
                _winner = value;
            }
        }

        public Command StartPrivatChatCommand
        {
            get
            {
                return _startPrivatChat ?? (_startPrivatChat = new Command(( user) => { StartPrivatChat((User)user); })); ;
            }

           
        }

        #endregion

        #region Private Methods and Operators

        private async void UpdateViewModel(User user=null, string groupId = null,bool updateMembers=false) {
            if (user == null)
                user = await BL.Session.Authorization.GetUser(true);
            Dictionary<string,object> lResponse = await new GroupGet(user.Token, groupId ?? (groupId = _group.Id)).Object();
            if(lResponse==null)
                return;
            string response = JsonConvert.SerializeObject(lResponse["Group"] as Group);
            JsonConvert.PopulateObject(response, CurrentGroup);
            IFileHelper lFileHelper = DependencyService.Get<IFileHelper>();
            if (!string.IsNullOrEmpty(CurrentGroup.Image))
                CurrentGroup.Image =
                    await lFileHelper.PhotoCache(lResponse["groupFilePerifix"].ToString(), CurrentGroup.Image, ImageType.Groups);

            if (CurrentGroup.Creator!=null &&!string.IsNullOrEmpty(CurrentGroup.Creator.Photo) && CurrentGroup.Creator.Photo != "profile_avatar.png")
                CurrentGroup.Creator.Photo = await
                  lFileHelper.PhotoCache(lResponse["userFilePerifix"].ToString(), CurrentGroup.Creator.Photo, ImageType.Users);
            if (CurrentGroup.Winner != null && !string.IsNullOrEmpty(CurrentGroup.Winner.Photo) && CurrentGroup.Winner.Photo != "profile_avatar.png")
                CurrentGroup.Winner.Photo = await
                  lFileHelper.PhotoCache(lResponse["userFilePerifix"].ToString(), CurrentGroup.Winner.Photo, ImageType.Users);
            if (updateMembers)
                MembersViewModel = new MembersViewModel(_group.Id);
           
        }
        public async Task<bool> RemoveGroup()
        {
            User lUser = await BL.Session.Authorization.GetUser();
            if (lUser == null)
            {
                return false;
            }

            if (
                await
                    DependencyService.Get<IExceptionHandler>()
				.YesNoQuestion(AppResources.RemoveWarning)) {
                if (await new GroupRemove(lUser.Token, _group.Id).Object()) {
                    UpdateViewModel(lUser);
                    return true;
                } else {
                    return false;
                }
            } else
                return false;

        }
        public async Task<bool> UpdateGroup()
        {

            User lUser = await BL.Session.Authorization.GetUser();
            if (lUser == null)
            {
                return false;
            }
                if (await new GroupUpdate(lUser.Token, _group).Object())
                {
                    UpdateViewModel(lUser);
                    return true;
                }
                else {
                    return false;
                }

           
        }
        public async Task<bool> SaveGroup()
        {

            if (_group.Id != null && _group.Id.Length > 5)
            {
                return await UpdateGroup();
            }
			if (string.IsNullOrEmpty(_group.Name))
            {
				await App.Current.MainPage.DisplayAlert("", AppResources.EnterGroupName, "Ok");
                return false;
            }
            if (string.IsNullOrEmpty(_group.Image) || (_group.Image.Contains("winning_sum"))) {
				if (!await App.Current.MainPage.DisplayAlert("", AppResources.YouDidNotSelectImageForGroup, AppResources.Continue, AppResources.Cancel))
                    return false;
                _group.Image = "";
            }
            User lUser = await BL.Session.Authorization.GetUser();
            if (lUser == null)
                return false;
            try {
                string lGroupId = await new GroupCreate(lUser.Token, _group).Object();
                if (!string.IsNullOrEmpty(lGroupId)) {
                    UpdateViewModel(lUser, lGroupId);
                    return true;
                } else
                    return false;
            }
            catch (Exception lException)
            {
                LogHelper.WriteException(lException);
                return false;
            }
        }

        private async Task StartChat()
        {
            await App.Navigation.PushAsync(new ChatPage { BindingContext = new ChatViewModel(_group.Id) });
        }

        private async Task StartPrivatChat(User user) {
            User lUser = BL.Session.Authorization.GetUser().Result;
            if (user !=null && user.Id!=lUser.Id)
            await App.Navigation.PushAsync(new ChatPage { BindingContext = new ChatViewModel(user) });
        }
        private async void ShowMembers()
        {
            await App.Navigation.PushAsync(new pgMembers() { BindingContext = MembersViewModel });

        }
		        
        #endregion
    }
}
