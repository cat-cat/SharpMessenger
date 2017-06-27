using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ChatClient.Core.Common.Models;
using ChatClient.Core.Common.Helpers;
using ChatClient.Core.UI.Controls;
using ChatClient.Core.UI.ViewModels;
using ChatClient.Droid.Renderers;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MessageViewCell), typeof(MessageRenderer))]
namespace ChatClient.Droid.Renderers
{
    public class MessageRenderer : ViewCellRenderer
    {
        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {
            var inflatorservice = (LayoutInflater)Forms.Context.GetSystemService(Android.Content.Context.LayoutInflaterService);
            var dataContext = item.BindingContext as ChatMessage;



            if (convertView == null)
            {
                convertView = (context as Activity).LayoutInflater.Inflate(Resource.Layout.image_item_owner, null);
                return this.GetCellCore(item, convertView, parent, context);
            }

            var textMsgVm = dataContext as ChatMessage;
            if (textMsgVm != null)
            {
                LinearLayout template = (LinearLayout)inflatorservice.Inflate(textMsgVm.IsMine ? Resource.Layout.image_item_owner : Resource.Layout.image_item_opponent, null, false);
                template.FindViewById<TextView>(Resource.Id.timestamp).Text = textMsgVm.Timestamp.ToString("HH:mm");
                if (!textMsgVm.IsMine)
                    template.FindViewById<TextView>(Resource.Id.nick).Text = textMsgVm.Name;

				try
				{
					if (textMsgVm.Photo.Contains("http"))
						template.FindViewById<ImageView>(Resource.Id.image).SetImageBitmap(getRoundedShape(GetImageBitmapFromUrl(textMsgVm.Photo)));
					else if (!string.IsNullOrEmpty(textMsgVm.Photo) && textMsgVm.Photo != "profile_avatar.png")
						template.FindViewById<ImageView>(Resource.Id.image).SetImageBitmap(getRoundedShape(BitmapFactory.DecodeFile(textMsgVm.Photo)));
				}
				catch
				{
					LogHelper.WriteLog("android error getting avatar", "android error getting avatar", "MessageRenderer");
					//FileHelper fh = new FileHelper();
					//string avatarPath = fh.GetImageSrc("profile_avatar.png");
					//Bitmap bitmap = BitmapFactory.DecodeResource(GetResources(), Resource.Drawable.profile_avatar); 
					//template.FindViewById<ImageView>(Resource.Id.image).SetImageBitmap(bitmap);
				}

                template.FindViewById<TextView>(Resource.Id.message).Text = textMsgVm.Message;
                return template;

            }

            return base.GetCellCore(item, convertView, parent, context);
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;
            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }
            return imageBitmap;
        }
        public Bitmap getRoundedShape(Bitmap scaleBitmapImage) {
            int targetWidth =75;
            int targetHeight =75;
            //  int targetWidth = scaleBitmapImage.Height ;
          //   int targetHeight = scaleBitmapImage.Height;
            Bitmap targetBitmap = Bitmap.CreateBitmap(targetWidth,
                targetHeight, Bitmap.Config.Argb8888);

            Canvas canvas = new Canvas(targetBitmap);
            Android.Graphics.Path path = new Android.Graphics.Path();
            path.AddCircle(((float)targetWidth - 1) / 2,
                ((float)targetHeight - 1) / 2,
                (Math.Min(((float)targetWidth),
                    ((float)targetHeight)) / 2),
                Android.Graphics.Path.Direction.Ccw);
            canvas.ClipPath(path);
            Bitmap sourceBitmap = scaleBitmapImage;
            canvas.DrawBitmap(sourceBitmap,
                new Rect(0, 0, sourceBitmap.Width,
                    sourceBitmap.Height),
                new Rect(0, 0, targetWidth, targetHeight), null);
            sourceBitmap.Dispose();
            sourceBitmap = null;
            canvas.Dispose();
            canvas = null;
            scaleBitmapImage.Dispose();
            scaleBitmapImage = null;
            path.Dispose();
            path = null;
            return targetBitmap;
        }
        //protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    base.OnCellPropertyChanged(sender, e);
        //}
    }
}