#region

using System;
using System.Threading.Tasks;

using ChatClient.Core.BL.Session;
using ChatClient.Core.Common;
using ChatClient.Core.Common.Interfaces;

using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions.Abstractions;

using Xamarin.Forms;

#endregion

namespace ChatClient.Core.BL.Media {
    public class Photo {
        #region Public Methods and Operators

        public static async Task<string> PickPhoto(string imageName) {
            IMedia lMedia = CrossMedia.Current;
            if (await Permissions.ChekPermission(Permission.Storage)) {
                MediaFile lMediaFile = await lMedia.PickPhotoAsync();
                if (lMediaFile != null) { 
                    string path= DependencyService.Get<IFileHelper>().CopyFile(lMediaFile.Path, imageName, true);
                    lMedia = null;
                    lMediaFile.Dispose();
                    lMediaFile = null;
                    return path;
                }
                ;
            }
            return null;
        }

        public static async Task<string> TakePhoto(string imageName) {
            IMedia lMedia = CrossMedia.Current;
            if (true) {
                if (await Permissions.ChekPermission(Permission.Camera)) {
                    MediaFile lMediaFile = await lMedia.TakePhotoAsync(
                        new StoreCameraMediaOptions {
                                                        Name = string.Format("{0}.{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), "jpg"),
                                                        Directory = "GroupsImages"
                                                    });
                    if (lMediaFile != null)
                    {
                        string path = DependencyService.Get<IFileHelper>().CopyFile(lMediaFile.Path, imageName, true);
                        lMedia = null;
                        lMediaFile.Dispose();
                        lMediaFile = null;
                        return path;
                    }
                }
            } else
                throw new NeedCamera();
            return null;
        }

        #endregion
    }
}