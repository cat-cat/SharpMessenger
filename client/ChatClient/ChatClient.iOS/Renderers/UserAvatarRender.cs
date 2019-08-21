using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using ChatClient.Core.UI.Controls;
using ChatClient.iOS.Renderers;

using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(UserAvatar), typeof(UserAvatarRender))]
namespace ChatClient.iOS.Renderers
{
 public   class UserAvatarRender:ImageRenderer
    {
        private UIImageView img;



		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Image> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                img = (UIImageView)Control;

                img.SizeToFit();
             
               e.NewElement.SizeChanged += NewElementOnSizeChanged;

                // btn.SetBackgroundImage(UIImage.FromFile("create_group_button_normal.png"), UIControlState.Normal);
            }
        }
        private void NewElementOnSizeChanged(object sender, EventArgs eventArgs)
        {
            var blurredImage = sender as GroupListImage;
            if (blurredImage != null)
            {
                var width = blurredImage.Width;
                var height = blurredImage.Height;
                img.Frame = new RectangleF((float)img.Frame.Location.X, (float)img.Frame.Location.Y, (float)100, (float)100);
            }
        }
    }
}
