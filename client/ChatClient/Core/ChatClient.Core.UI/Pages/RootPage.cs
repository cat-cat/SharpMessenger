#region

using System;
using System.Linq;

using ChatClient.Core.UI.Menus;
using ChatClient.Core.Common.Resx;
                         
using Xamarin.Forms;

#endregion

namespace ChatClient.Core.UI.Pages
{
    public class RootPage : MasterDetailPage
    {
        #region Fields

        private pgMenu _menuPage;
        private Page _currentPage;
        private NavigationPage _currentNavigationPage;
        #endregion

        #region Constractors and Destructors

        public RootPage()
        {
            MasterBehavior = MasterBehavior.Popover;
            _menuPage = new pgMenu();
            _menuPage.UserCommand = new Command(
                () =>
                {
                    Page lUserPage = new NavigationPage(new pgMyAccount());
                    Detail = lUserPage;
                    App.Navigation = lUserPage.Navigation;
                    _menuPage.Menu.SelectedItem = null;
                    IsPresented = false;
                });
            _menuPage.Menu.ItemSelected += (sender, e) => NavigateTo(e.SelectedItem as MenuListItem);
            _menuPage.Update();
            Master = _menuPage;
            _currentPage = new MainPage();
            _currentNavigationPage = new NavigationPage(_currentPage) { BarBackgroundColor = Color.FromHex("#ffc107"), BarTextColor = Color.Black, Icon = "list_of_the_groups_icon.png" };
            Detail = _currentNavigationPage;
            App.Navigation = _currentNavigationPage.Navigation;
            Device.BeginInvokeOnMainThread(
                () =>
                {
                    NavigateTo(
                        new MenuListItem()
                        {
							Title = AppResources.ListOfGroups,
                            IconSource = "menu_icon_list_of_groups.png",
                            TargetType = typeof(pgGroups)
                        });
                }
                );
        }

        #endregion

        #region Private Methods and Operators

        private async void NavigateTo(MenuListItem menu)
        {
            Device.BeginInvokeOnMainThread(async () =>
                {
                    Page lPage;
                    if (menu == null)
                        return;
                    lPage = (Page)Activator.CreateInstance(menu.TargetType);
                                                     if (Device.OS != TargetPlatform.iOS) {
                                                         _currentNavigationPage.Navigation.InsertPageBefore(lPage, _currentPage);
                                                         await _currentNavigationPage.PopAsync();
                                                     }
                                                     if(Device.OS== TargetPlatform.iOS)
                   _currentNavigationPage=new NavigationPage(lPage) { BarBackgroundColor = Color.FromHex("#ffc107"), BarTextColor = Color.Black, Icon = "list_of_the_groups_icon.png" };
                    if (_currentPage != null)
                    {
                        if (_currentPage is pgGroups)
                        {
                            (_currentPage as pgGroups).Dispose();

                            App.Navigation = null;
                        }
                        _currentPage = null;
                    }
                    _currentPage = lPage;
                    Detail = _currentNavigationPage;
                    App.Navigation = _currentNavigationPage.Navigation;
                    _menuPage.Update();
                    _menuPage.Menu.SelectedItem = null;
                    IsPresented = false;
                });
        }
        #endregion
    }
}