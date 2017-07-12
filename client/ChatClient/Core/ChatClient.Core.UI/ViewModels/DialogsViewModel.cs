using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.Common;
using ChatClient.Core.Common.Resx;
using ChatClient.Core.Common.Helpers;
using ChatClient.Core.Common.Interfaces;
using ChatClient.Core.Common.Models;
using ChatClient.Core.Common.Models.Base;
using ChatClient.Core.SAL.Adapters;
using ChatClient.Core.SAL.Methods;
using ChatClient.Core.UI.Pages;

using Newtonsoft.Json;

using Xamarin.Forms;

namespace ChatClient.Core.UI.ViewModels
{
   public class DialogsViewModel:BaseViewModel
    {
        private ObservableCollection<Conversation> _collection = new ObservableCollection<Conversation>();
        private int _currentPage = 1;
        private int _pageSize = 50;
        private int _totalPages;
        private Command _uploadCommand;

		public string PageTitle
		{
			get
			{
				return AppResources.Dialogs;
			}
		}

		public ObservableCollection<Conversation> Collection
        {
            get
            {
                return _collection;
            }

            set
            {
                _collection = value;
            }
        }
        public Command UploadCommand
        {
            get
            {
                return _uploadCommand ?? (_uploadCommand = new Command(() => { LoadDialogs(1); }));
            }
        }
        public DialogsViewModel() {
            _collection=new ObservableCollection<Conversation>();
            MessagingCenter.Subscribe<pgDialogs, Conversation>(this, "LoadItems", (sender, e) => { LoadMoreItems(e); });
            LoadDialogs(_currentPage);
       }
        private async Task LoadMoreItems(Conversation e)
        {
            if (e == Collection[Collection.Count - 1] && IsBusy == false && _currentPage < _totalPages)
            {
                _currentPage++;
               LoadDialogs(_currentPage);
            }
        }
        private async void LoadDialogs(int page) {
            IsBusy = true;
            try
            {
                User lUser = await BL.Session.Authorization.GetUser();
                if (lUser == null)
                {
					IsBusy = false;
                    return;
                }

                Dictionary<string, object> lResponseObjects = await new DialogsGet(lUser.Token, page,_pageSize).Object();
				if (lResponseObjects == null)
				{
					IsBusy = false;
					return;
				}
                IFileHelper lFileHelper = DependencyService.Get<IFileHelper>();
                foreach (Conversation lConversation in lResponseObjects["dialogs"] as List<Conversation>)
                {
                    if (_collection.Any(dlg => dlg.Id == lConversation.Id))
                        continue;
                    if (lConversation.Message == null)
                        continue;
                    if (lConversation.Message.Opponent.Id == lUser.Id)
                    {
                        lConversation.Message.IsMine = true;
                        lConversation.Color = Color.Gray;
                        lConversation.Opponent = lConversation.Message.Author;
                    }
                    else
                    {
                        lConversation.Message.IsMine = false;
                        lConversation.Color = Color.Black;
                        lConversation.Opponent = lConversation.Message.Opponent;
                    }
                    if (!string.IsNullOrEmpty(lConversation.Opponent.Photo) && lConversation.Opponent.Photo!= "profile_avatar.png")
                    {
                        lConversation.Opponent.Photo =
                            await lFileHelper.PhotoCache(lResponseObjects["ImagePrefix"].ToString(), lConversation.Opponent.Photo, ImageType.Users);
                    }
                        _collection.Add(lConversation);
                }
                lFileHelper = null;
                _totalPages = (int)lResponseObjects["pageCount"];
                lResponseObjects = null;
                OnPropertyChanged("Collection");

            }
            catch (Exception lException)
            {
                LogHelper.WriteException(lException);
            }
           IsBusy = false;
        }
    }
}
