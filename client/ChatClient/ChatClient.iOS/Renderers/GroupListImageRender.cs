using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using ChatClient.iOS.Renderers;
using ChatClient.Core.UI.Controls;

using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(GroupListImage), typeof(GroupListImageRender))]
namespace ChatClient.iOS.Renderers
{
   public class GroupListImageRender:ImageRenderer {
       private UIImageView img;



		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Image> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
				// btn.SetBackgroundImage(UIImage.FromFile("create_group_button_normal.png"), UIControlState.Normal);
                img = (UIImageView)Control;
            }
        }

        //private void NewElementOnSizeChanged(object sender, EventArgs eventArgs)
        //{
        //    var blurredImage = sender as GroupListImage;
        //   if (blurredImage != null)
        //    {
        //        var width = blurredImage.Width;
        //       var height = blurredImage.Height;
        //      img.Frame = new RectangleF((float)img.Frame.Location.X, (float)img.Frame.Location.Y, (float)width, (float)height);
        //    }
        //}
    }
}
