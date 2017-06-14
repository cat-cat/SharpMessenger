#region

using System;
using System.Collections.Generic;

using ChatClient.Core.Common.Models.Base;
using ChatClient.Core.UI.ViewModels;
using ChatClient.Core.Common.Resx;

using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;

using Xamarin.Forms;

#endregion

namespace ChatClient.Core.UI.PopupPages {
    public partial class pgGroupOrdering : PopupPage {
        #region Fields

        private readonly List<OrderingItem> _orderingItems = new List<OrderingItem>() {
                                                                                          new OrderingItem() {
																												Text = AppResources.ByLastMember,
                                                                                                                 Value = "lastMember"
                                                                                                             },
                                                                                          new OrderingItem() {
                                                                                                                 Text = AppResources.ByEndTime,
                                                                                                                 Value = "endAt"
                                                                                                             }
                                                                                      };

        private OrderingItem _orderItem = new OrderingItem();
        private Button _perviusOrderItem;

        #endregion

        #region Constractors and Destructors

        public pgGroupOrdering() {
            InitializeComponent();
            lstOrderItems.ItemsSource = _orderingItems;
          lblOrdering.GestureRecognizers.Add(item: new TapGestureRecognizer(async (view) => await App.Navigation.PopAllPopupAsync()));
        }

        #endregion

        #region Properties

        public OrderingItem OrderItem {
            get {
                return _orderItem;
            }
            set {
                _orderItem = value;
            }
        }

        public List<OrderingItem> OrderingItems {
            get {
                return _orderingItems;
            }
        }

        #endregion

        #region Protected Methods and Operators

        protected override void OnDisappearing() {
            (((this.Parent as MasterDetailPage).Detail as NavigationPage).CurrentPage.BindingContext as GroupsViewModel).DoOrdering();
            base.OnDisappearing();
        }

        #endregion

        #region Private Methods and Operators

        private async void selectOrdering_OnClicked(object sender, EventArgs e) {
            if (_perviusOrderItem != null)
                _perviusOrderItem.Image = "radio_button_normal.png";
            (sender as Button).Image = "radio_button_pressed.png";
            _orderItem = (sender as Button).Parent.BindingContext as OrderingItem;
            _perviusOrderItem = sender as Button;
            await App.Navigation.PopAllPopupAsync();
        }

        #endregion

        private async void btnOrdering_OnClicked(object sender, EventArgs e) {
            await App.Navigation.PopAllPopupAsync();
        }
    }
}