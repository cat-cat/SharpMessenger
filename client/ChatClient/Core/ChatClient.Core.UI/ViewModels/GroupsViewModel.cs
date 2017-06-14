#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using ChatClient.Core.Common;
using ChatClient.Core.Common.Helpers;
using ChatClient.Core.Common.Interfaces;
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

#endregion

namespace ChatClient.Core.UI.ViewModels
{
    public class GroupsViewModel : BaseViewModel, IDisposable
    {
        #region Fields

        private ObservableCollection<Group> _collection = new ObservableCollection<Group>();
        private int _currentPage = 1;
		private string _mainTitle = AppResources.ListOfGroups;
        private int _pageSize = 50;
        private string _title;
        private int _totalPages;
        private Command _uploadCommand;
        private Command _createGroupCommand;
        private Command _setOrderingCommand;
        private bool _isLoading;
        private bool _isDisposed = false;
        private pgGroupOrdering _pageOrdering;
        private bool _myGroups;

        private OrderingItem _currentOrder = new OrderingItem()
        {
			Text = AppResources.ByEndingTime,
            Value = "endAt"
        };
        #endregion

        #region Constractors and Destructors

        public GroupsViewModel(bool myGroups=false) {
            _myGroups = myGroups;
            _pageOrdering = new pgGroupOrdering();
            // UpdateViewModel();
            Title = _mainTitle;
            _collection = new ObservableCollection<Group>();
            MessagingCenter.Subscribe<pgGroups, Group>(this, "LoadItems", (sender, e) => { LoadMoreItems(e); });
            MessagingCenter.Subscribe<pgMyGroups, Group>(this, "LoadItems", (sender, e) => { LoadMoreItems(e); });
            UpdateViewModel(_currentPage);
         Device.StartTimer(new TimeSpan(0, 0, 0, 1), Timer);
        }

        #endregion

        #region Properties



        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                SetProperty(ref _isLoading, value, "IsLoading");
            }
        }

        public string Title
        {
            get
            {
				return _mainTitle;
            }
            set
            {
                SetProperty(ref _mainTitle, value, "Title");
            }
        }

        public Command UploadCommand
        {
            get
            {
                return _uploadCommand ?? (_uploadCommand = new Command(() => { UpdateViewModel(1, true); }));
            }
        }

        public int CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                SetProperty(ref _currentPage, value, "CurrentPage");
            }
        }

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }

        public ObservableCollection<Group> Collection
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

        public OrderingItem CurrentOrder
        {
            get
            {
                return _currentOrder;
            }

            set
            {
                _currentOrder = value;
            }
        }

        public Command SetOrderingCommand
        {
            get
            {
                return _setOrderingCommand ?? (_setOrderingCommand = new Command(() => { SetOrdering(); }));
            }
        }

        public pgMyGroup GreationPage { get; set; }

        public Command CreateGroupCommand
        {
            get
            {
                return _createGroupCommand ?? (_createGroupCommand = new Command(async () => { GreationPage=new pgMyGroup(); await App.Navigation.PushAsync(GreationPage) ; }));
            }
        }
        #endregion

        #region Private Methods and Operators

        private bool Timer()
        {
            if (_isDisposed)
                return false;
            foreach (Group lGroup in _collection)
            {
                lGroup.OnPropertyChanged("EndDate");
            }

            return true;
        }

        private async Task LoadMoreItems(Group e)
        {
            if (e == Collection[Collection.Count - 1] && IsLoading == false && _currentPage < _totalPages)
            {
                _currentPage++;
                await UpdateViewModel(_currentPage);
            }
        }

        private async void SetOrdering()
        {
            await App.Navigation.PushPopupAsync(_pageOrdering);
        }

        public async void DoOrdering()
        {
            if (_pageOrdering != null && _currentOrder.Value != _pageOrdering.OrderItem.Value)
            {
                _currentOrder = _pageOrdering.OrderItem;
                _collection = new ObservableCollection<Group>();
                _currentPage = 1;
                await UpdateViewModel(1);
            }
        }
        private async Task UpdateViewModel(int page, bool insert = false)
        {
            IsLoading = true;
            try
            {
                User lUser = await BL.Session.Authorization.GetUser();
                if (lUser == null) {
                    IsLoading = false;
                    return;
                }
                Dictionary<string, object> lResponseObjects = null;
                if(_myGroups)
                    lResponseObjects = await new GetFavorites(lUser.Token, page, PageSize, _currentOrder.Value).Object();
                else
                    lResponseObjects = await new GroupsGet(lUser.Token, page, PageSize, _currentOrder.Value).Object();
              
                if (lResponseObjects == null)
                {
                    IsLoading = false;
                    return;
                }
                if (insert)
                    _collection=new ObservableCollection<Group>();
                IFileHelper lFileHelper = DependencyService.Get<IFileHelper>();
                foreach (Group lGroup in lResponseObjects["groups"] as List<Group>) {
                   

                    if (_collection.Any(adv => adv.Id == lGroup.Id))
                        continue;
                    if (!string.IsNullOrEmpty(lGroup.Image))
                    {
                        lGroup.Image =
                            await lFileHelper.PhotoCache(lResponseObjects["ImagePrefix"].ToString(), lGroup.Image, ImageType.Groups);
                    }
                        _collection.Add(lGroup);
                }
                lFileHelper = null;
                    _totalPages = (int)lResponseObjects["pageCount"] ;
                lResponseObjects = null;
                    Title = string.Format("{0} ({1})", _mainTitle, _collection.Count);
                    OnPropertyChanged("Collection");
                
            }
            catch (Exception lException)
            {

                LogHelper.WriteException(lException);
            }
            IsLoading = false;
        }

        #endregion

        public void Dispose()
        {
            _isDisposed = true;
            Collection.Select(c => { c.Image = null; return c; }).ToList();
            Collection = null;


        }
    }
}