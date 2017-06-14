using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.Common.Models;
using ChatClient.Core.UI.PopupPages;
using ChatClient.Core.UI.ViewModels;

using Rg.Plugins.Popup.Extensions;

using Xamarin.Forms;

namespace ChatClient.Core.UI.Pages
{
    public partial class pgMyGroup : ContentPage
    {
        private PhotoSourseViewModel _photoSourseViewModel;
        private bool _onPhotoSelect;
        public pgMyGroup(Group group)
        {
            _groupViewModel = new GroupViewModel(group);
            InitializeComponent();
            BindingContext = _groupViewModel;
            lblStatus.GestureRecognizers.Add(item: new TapGestureRecognizer((view) => SetControlEditable(lblStatus)));
           // btnSave.Command = _groupViewModel.UpdateGroupCommand;
        }

        private GroupViewModel _groupViewModel ;
        public pgMyGroup() {
            _groupViewModel= new GroupViewModel(new Group() { EndDate = DateTime.Now });
            BindingContext = _groupViewModel;
            InitializeComponent();
         //   btnSave.Command = _groupViewModel.SaveGroupCommand;
            btnCloseGroup.IsVisible = false;
			btnChat.IsVisible = false;
            // imgGroup.GestureRecognizers.Add(item: new TapGestureRecognizer((view) => _groupCreateViewModel.SelectImageCommand.Execute(null)));
            imgGroup.GestureRecognizers.Add(item: new TapGestureRecognizer((view) => SelectImage()));
            lblStatus.GestureRecognizers.Add(item: new TapGestureRecognizer((view) => SetControlEditable(lblStatus)));
            lblName.GestureRecognizers.Add(item: new TapGestureRecognizer((view) => SetControlEditable(lblName)));
          
        }

        public bool AddedNewGroup { get; set; }

        public Group NewGroup { get; set; }

        private void OnValueChanged(object sender, ValueChangedEventArgs e) {

            int lNominal = (int)e.NewValue;
            _groupViewModel.CurrentGroup.Nominal = lNominal;
        }

        private async void SelectImage() {
            _onPhotoSelect = true;
            Device.StartTimer(new TimeSpan(0, 0, 0, 0, 100), Timer);
            _photoSourseViewModel = new PhotoSourseViewModel("groupimg.jpg");
            pgPhotoSourse lPgPhotoSourse = new pgPhotoSourse() { BindingContext = _photoSourseViewModel };
            await App.Navigation.PushPopupAsync(lPgPhotoSourse);
            //string image=  await     _groupViewModel.SelectImage();
            //if (!string.IsNullOrEmpty(image)) {
            //    _groupViewModel.CurrentGroup.Image = image;
         //  imgGroup.Source = _groupViewModel.CurrentGroup.Image;
             //   rwImag.Height = imgGroup.Height;

            
           
        }

        private bool Timer() {
            if (!string.IsNullOrEmpty(_photoSourseViewModel?.Photo) && _groupViewModel.CurrentGroup.Image != _photoSourseViewModel.Photo) {
                _onPhotoSelect = false;
_groupViewModel.CurrentGroup.Image= _photoSourseViewModel.Photo;
              //  rwImag.Height = imgGroup.Height;
                return false;
            }
            return true;
        }

        private void txbNominal_OnTextChanged(object sender, TextChangedEventArgs e) {
            try {
                int lNominal = Convert.ToInt32(e.NewTextValue);
               _groupViewModel.CurrentGroup.Nominal = lNominal;
            } catch {
                (sender as Entry).Text = e.OldTextValue;

            }
        }

        private void SetControlEditable(Object control) {
            if (control is Label) {
                var lableId= (control as Label).Id;
                if (lblStatus.Id == lableId) {
                    lblStatus.IsVisible = false;
                    txbStatus.IsVisible = true;
                }
                else if (lblName.Id == lableId) {
                    lblName.IsVisible = false;
                    txbName.IsVisible = true;
                }
              
               
            }
        }

        private void txb_OnUnfocused(object sender, FocusEventArgs e) {
            if (sender == txbName) {
                lblName.Text= _groupViewModel.CurrentGroup.Name ;
                lblName.IsVisible = true;
                txbName.IsVisible = false;
            }
            else if (sender == txbStatus) {
                lblStatus.Text = _groupViewModel.CurrentGroup.OwnerStatus;
                lblStatus.IsVisible =true;
                txbStatus.IsVisible = false;
            }
        }

        private async void btnSave_OnClicked(object sender, EventArgs e) {
            if (_groupViewModel.CurrentGroup.Id != null && _groupViewModel.CurrentGroup.Id.Length > 5) {
                if (await _groupViewModel.UpdateGroup())
                    ;
            } else {
                if (await _groupViewModel.SaveGroup()) {
                    btnCloseGroup.IsVisible = true;
					btnChat.IsVisible = true;
                    // imgGroup.GestureRecognizers.Add(item: new TapGestureRecognizer((view) => _groupCreateViewModel.SelectImageCommand.Execute(null)));
                    imgGroup.GestureRecognizers.RemoveAt(imgGroup.GestureRecognizers.Count-1);
                 
                    lblName.GestureRecognizers.RemoveAt(lblName.GestureRecognizers.Count - 1);
                   AddedNewGroup = true;
                   NewGroup = _groupViewModel.CurrentGroup;
                }
            }
        }

        private async void btnCloseGroup_OnClicked(object sender, EventArgs e) {
            if (await _groupViewModel.RemoveGroup()) {
             //   btnCloseGroup.IsEnabled = false;
            //    btnMakeBid.IsEnabled = false;
            }
        }

        private void PgMyGroup_OnDisappearing(object sender, EventArgs e) {
            if (!_onPhotoSelect) {
                MessagingCenter.Send<pgMyGroup,bool>(this, "EndAdding",true);
            }
        }
    }
}
