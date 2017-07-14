using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Collections.Specialized;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ChatClient.Core.Common;
using ChatClient.Core.Common.Models;
using ChatClient.Core.Common.Helpers;
using ChatClient.Core.UI;
using ChatClient.Core.UI.Controls;
using ChatClient.Core.UI.ViewModels;
using ChatClient.Droid.Renderers;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MessageViewCell), typeof(MessageRenderer))]
namespace ChatClient.Droid.Renderers
{
	class NotifyView : LinearLayout
	{
		public ChatMessage _chatMessage;

		void OnEvent(object sener, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				var newItem = (KeyValuePair<k, object>)e.NewItems[0];
				if (newItem.Key == k.OnMessageSendProgress)
				{
					var d = (Dictionary<string, object>)newItem.Value;
					if ((string)d["guid"] == _chatMessage.guid && (ChatMessage.Status)d["status"] == ChatMessage.Status.Deleted)
					{
						Device.BeginInvokeOnMainThread(() =>
						{
							FindViewById<TextView>(Resource.Id.message).Text = "<deleted>";
						});
						_chatMessage.Message = "<deleted>";
					}
				}
				else if (newItem.Key == k.OnMessageEdit)
				{
					var d = (Dictionary<string, object>)newItem.Value;
					if ((string)d["guid"] == _chatMessage.guid)
					{
						Device.BeginInvokeOnMainThread(() =>
						{
							FindViewById<TextView>(Resource.Id.message).Text = (string)d["message"];
						});
						_chatMessage.Message = (string)d["message"];
					}
				}
			}
		}

		public NotifyView(Context c) : base(c)
		{
			Id = 2525;

			v.h(OnEvent);
		}
	}

    public class MessageRenderer : ViewCellRenderer
    {
		async void EventHandler<TEventArgs>(Object sender, TEventArgs e) 
		{
			string action = await App.Current.MainPage.DisplayActionSheet("Actions", "Cancel", "Delete", new string[2] {"Reply", "Edit"});
			User lUser = await Core.BL.Session.Authorization.GetUser();

			var nv = sender as NotifyView;
			if (action == "Delete" && lUser.Id == nv._chatMessage.Author.Id) // delete only message of the current user
			{
				nv._chatMessage.status = ChatMessage.Status.PendingDelete;
				v.Add(k.MessageSendProgress, nv._chatMessage);
			}
			else if (action == "Reply") // reply
			{
				v.Add(k.MessageReply, nv._chatMessage);
			}
			else if (action == "Edit" && lUser.Id == nv._chatMessage.Author.Id) // edit only message of the current user
			{
				v.Add(k.MessageEdit, nv._chatMessage);
			}
		}

        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {
            //if (convertView == null)
            //{
            //    convertView = (context as Activity).LayoutInflater.Inflate(Resource.Layout.image_item_owner, null);
            //    return this.GetCellCore(item, convertView, parent, context);
            //}

            var textMsgVm = item.BindingContext as ChatMessage;
			//if (textMsgVm != null)
			//{
			LinearLayout template = (LinearLayout) convertView;
			if (convertView == null)
			{
				var inflatorservice = (LayoutInflater)Forms.Context.GetSystemService(Android.Content.Context.LayoutInflaterService);
				var nv = new NotifyView(Forms.Context);
				template = (LinearLayout)inflatorservice.Inflate(textMsgVm.IsMine ? Resource.Layout.image_item_owner : Resource.Layout.image_item_opponent, nv);
				template.LongClick += EventHandler;
			}


			var rv = template.FindViewById(2525) as NotifyView;

			if (rv != null)
			{
				rv._chatMessage = textMsgVm;
			}

            template.FindViewById<TextView>(Resource.Id.timestamp).Text = textMsgVm.Timestamp.ToString("HH:mm");
			if (!textMsgVm.IsMine)
			{
				TextView v = template.FindViewById<TextView>(Resource.Id.nick);
				if (v != null)
					template.FindViewById<TextView>(Resource.Id.nick).Text = textMsgVm.Name;
				else
				{
					int i = 4;
				}
			}

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

			if (textMsgVm.status == ChatMessage.Status.Deleted)
				template.FindViewById<TextView>(Resource.Id.message).Text = "<deleted>";
			else
			{
				if (textMsgVm.ReplyGuid != null)
					// TODO: make this good
					template.FindViewById<TextView>(Resource.Id.message).Text = "REPLIED: " + textMsgVm.Message + " TO: " + textMsgVm.ReplyQuote;
				else
					template.FindViewById<TextView>(Resource.Id.message).Text = textMsgVm.Message;
			}
			//template.SetOnSystemUiVisibilityChangeListener(
			return template;

            //}

            //return base.GetCellCore(item, convertView, parent, context);
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