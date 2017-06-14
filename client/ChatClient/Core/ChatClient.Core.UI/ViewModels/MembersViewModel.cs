using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.Common;
using ChatClient.Core.Common.Helpers;
using ChatClient.Core.Common.Interfaces;
using ChatClient.Core.Common.Models;
using ChatClient.Core.Common.Models.Base;
using ChatClient.Core.SAL.Adapters;
using ChatClient.Core.SAL.Methods;
using ChatClient.Core.Common.Resx;

using Newtonsoft.Json;
using Xamarin.Forms;

namespace ChatClient.Core.UI.ViewModels
{
   public class MembersViewModel:BaseViewModel
    {
        private ObservableCollection<Member> _collection = new ObservableCollection<Member>();
		private string _mainTitle = AppResources.Members;
        private string _title;
        private Command _uploadCommand;
       private string _groupId;
       private int _biddersCount;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                SetProperty(ref _title, value, "Title");
            }
        }
        public ObservableCollection<Member> Collection
        {
            get
            {
                return _collection;
            }
            set
            {
                SetProperty(ref _collection, value, "Collection");
            }
        }

       public MembersViewModel(string groupId) {
            Title = _mainTitle;
           _groupId = groupId;
            _collection=new ObservableCollection<Member>();
           UpdateViewModel();

       }
        public Command UploadCommand
        {
            get
            {
                return _uploadCommand ?? (_uploadCommand = new Command(() => { UpdateViewModel(); }));
            }
        }

        public int BiddersCount
        {
            get
            {
                return _biddersCount;
            }

            set
            {
                SetProperty(ref _biddersCount, value, "BiddersCount");
            }
        }

		private async Task UpdateViewModel()
		{
			IsBusy = true;

			User lUser = await BL.Session.Authorization.GetUser();
			if (lUser == null)
			{
				IsBusy = false;
				return;
			}

			Dictionary<string, object> response = await new GetBidders(lUser.Token, _groupId).Object();
			if (response == null)
			{
				IsBusy = false;
				return;
			}
                    foreach (Member _user in response["users"] as List<Member>) {
                        if (!string.IsNullOrEmpty(_user.Photo) && _user.Photo != "profile_avatar.png") {
                            _user.Photo = await
                              DependencyService.Get<IFileHelper>()
                                  .PhotoCache((string)response["ImagePrefix"], _user.Photo, ImageType.Users);
                        }
                    
                    if (_collection.Any(usr => usr.Id == _user.Id))
                            continue;
                            _collection.Insert(0,_user);
                      
                    }
                    Title = string.Format("{0} ({1})", _mainTitle, _collection.Count);
                    BiddersCount = _collection.Count;
                    OnPropertyChanged("Collection");
                
           
            IsBusy = false;
        }


    }

}
