using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rg.Plugins.Popup.Extensions;

using Xamarin.Forms;

namespace ChatClient.Core.UI.ViewModels
{
   public class PhotoSourseViewModel:BaseViewModel {
       private string _fileName;
       public PhotoSourseViewModel(string fileName) {
           _fileName = fileName;
       }
       private string _photo;
       private Command _takePhotoCommand;
       private Command _pickPhotoCommand;
       private Command _cancelCommand;
        public string Photo
        {
            get
            {
                return _photo;
            }

            set
            {
               SetProperty(ref  _photo,value,"Photo");
            }
        }

        public Command TakePhotoCommand
        {
            get
            {
                return _takePhotoCommand??(_takePhotoCommand=new Command(()=>TakePhoto()));
            }
        }

        public Command PickPhotoCommand
        {
            get
            {
                return _pickPhotoCommand??(_pickPhotoCommand=new Command(PickPhoto));
            }
        }

        public Command CancelCommand
        {
            get
            {
                return _cancelCommand??(_cancelCommand=new Command(Cancel));
            }
        }

       private async void PickPhoto() {
           Photo = await BL.Media.Photo.PickPhoto(_fileName);
            ClosePopPages();
        }
        private async void TakePhoto()
        {
            Photo = await BL.Media.Photo.TakePhoto(_fileName);
            ClosePopPages();
        }

       private async void ClosePopPages() {
            await App.Navigation.PopAllPopupAsync();
        }
        private async void Cancel()
        {
            ClosePopPages();
        }
    }
}
