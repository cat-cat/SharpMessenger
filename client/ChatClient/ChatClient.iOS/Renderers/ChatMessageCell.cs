using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Text;
using ChatClient.Core.Common.Helpers;
using ChatClient.Core.Common;
using ChatClient.Core.Common.Models;
using CoreGraphics;

using Foundation;

using UIKit;

namespace ChatClient.iOS.Renderers
{
    public class ChatMessageCell : UITableViewCell
    {
		ChatMessage _chatMessage;
        public static NSString KeyLeft = new NSString("BubbleElementLeft");
        public static NSString KeyRight = new NSString("BubbleElementRight");
        public static UIImage bleft, bright, left, right;
        public static UIImage Avatar;
        public static UIFont font = UIFont.SystemFontOfSize(12);
        UIView view;
        UIView vwLine;
        UIView imageView;
        UILabel lblMessage;
        private UILabel lblNickname;
        private UILabel lblTimestamp;
        private UIImageView lAvatarView;
        bool isLeft;
		UILongPressGestureRecognizer longPressGesture;

        static ChatMessageCell()
        {
            bright = UIImage.FromFile("my_message.png");
            bleft = UIImage.FromFile("opponent_message.png");
            Avatar = ResizeImage(UIImage.FromFile("profile_avatar.png"), 50, 50);
            // buggy, see https://bugzilla.xamarin.com/show_bug.cgi?id=6177
            //left = bleft.CreateResizableImage (new UIEdgeInsets (10, 16, 18, 26));
            //right = bright.CreateResizableImage (new UIEdgeInsets (11, 11, 17, 18));
            left = bleft.StretchableImage(11, 11);
            right = bright.StretchableImage(11, 11);
			///	Avatar = Avatar.StretchableImage(11, 11);
        }

		void LongPressMethod(UILongPressGestureRecognizer gestureRecognizer)
		{
			if (gestureRecognizer.State == UIGestureRecognizerState.Began)
			{
				var actionSheet = new UIActionSheet("ActionSheet", null, "Cancel", "Delete", new string[1] {"Reply"});
			    actionSheet.Clicked += delegate(object a, UIButtonEventArgs b) {
					if (b.ButtonIndex == 0) // delete
					{
						_chatMessage.status = ChatMessage.Status.Deleted;
						v.Add(k.MessageSendProgress, _chatMessage);
					}
					else if (b.ButtonIndex == 1) // reply
					{
						v.Add(k.MessageReply, _chatMessage);
					}
			    };
				actionSheet.ShowInView(this);

			}
		}

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
						_chatMessage.Message = "<deleted>";
						lblMessage.Text = _chatMessage.Message;
					}
				}
			}
		}

		public void OnAppear()
		{
			v.h(OnEvent);
			v.Add(k.MessageSendProgress, _chatMessage);

			longPressGesture = new UILongPressGestureRecognizer(LongPressMethod);
			AddGestureRecognizer(longPressGesture);

			if (_chatMessage.ReplyGuid != null)
				// TODO: make this good
				lblMessage.Text = "REPLIED: " + _chatMessage.Message + " TO: " + _chatMessage.ReplyQuote;
		}

		public void OnDisappear()
		{
			v.m(OnEvent);
			RemoveGestureRecognizer(longPressGesture);
		}

        static UIImage FromUrl(string uri)
        {
            using (var url = new NSUrl(uri))
            using (var data = NSData.FromUrl(url))
                return UIImage.LoadFromData(data);
        }
        public ChatMessageCell(bool isLeft)
            : base(UITableViewCellStyle.Default, isLeft ? KeyLeft : KeyRight)
        {
            var rect = new RectangleF(0, 0, 1, 10);

            this.isLeft = isLeft;
            view = new UIView(rect);
            //UIView lCellView = new UIView();
            imageView = new UIImageView(isLeft ? left : right);
            lAvatarView = new UIImageView(Avatar);

          
            lblMessage = new UILabel(rect)
            {
                LineBreakMode = UILineBreakMode.WordWrap,
                Lines = 0,
                Font = font,
                BackgroundColor = UIColor.Clear
            };
            lblNickname = new UILabel(rect)
            {
                LineBreakMode = UILineBreakMode.WordWrap,
                Lines =0,
                Font = UIFont.SystemFontOfSize(10),
                BackgroundColor = UIColor.Clear,
                Text = "TestNickName"
            };
            lblTimestamp = new UILabel(rect) {
                                                 LineBreakMode = UILineBreakMode.WordWrap,
                                                 Lines = 0,
                                                 Font = UIFont.SystemFontOfSize(8),
                                                 BackgroundColor = UIColor.Clear,
                                                 Text = "14:30"

                                             };
            vwLine = new UIView(rect) {
                BackgroundColor= UIColor.Black
            };
            // lCellView.AddSubview(imageView);
            //lCellView.AddSubview(vwLine);
            //lCellView.AddSubview(lblMessage);
            //lCellView.AddSubview(lblTimestamp);
            //lCellView.AddSubview(lblNickname);

            view.AddSubview(imageView);
            view.AddSubview(vwLine);
            view.AddSubview(lblMessage);
            view.AddSubview(lblTimestamp);
            view.AddSubview(lblNickname);
            view.AddSubview(lAvatarView);
           // view.AddSubview(lCellView);


            ContentView.Add(view);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            var frame = ContentView.Frame;
            
            var MessageSize = GetSizeForText(this, lblMessage.Text) + BubblePadding;
          
            var nickNameSize= GetSizeForText(this, lblNickname.Text) + BubblePadding;
            var AvatarSize = Avatar.Size;
          //  lblNickname.Frame= new CGRect(isLeft ? 65 : frame.Width - 65 - (MessageSize.Width > nickNameSize.Width ? MessageSize.Width : nickNameSize.Width) , frame.Y + 10, nickNameSize.Width, nickNameSize.Height);
            imageView.Frame = new CGRect(isLeft ? 65 : frame.Width - 65 - (MessageSize.Width > nickNameSize.Width ? MessageSize.Width : nickNameSize.Width), frame.Y+10, MessageSize.Width> nickNameSize.Width? MessageSize.Width: nickNameSize.Width, MessageSize.Height+ nickNameSize.Height);
            lAvatarView.Frame = new CGRect(isLeft ? 10 : frame.Width - 60, frame.Y + 5, AvatarSize.Width, AvatarSize.Height);
            lAvatarView.Layer.CornerRadius = lAvatarView.Frame.Size.Height / 2;
            lAvatarView.Layer.MasksToBounds = true;
            //			imageView.Frame = new RectangleF(new PointF(isLeft ? 10 : frame.Width - size.Width - 10, frame.Y), size);
            view.SetNeedsDisplay();
            frame = imageView.Frame;
            var noBubbleSize = MessageSize - BubblePadding;
            lblNickname.Frame = new CGRect(frame.X + (isLeft ? 12 : 8), frame.Y , noBubbleSize.Width, nickNameSize.Height);
            vwLine.Frame = new CGRect(frame.X + (isLeft ? 12 : 8), frame.Y + nickNameSize.Height +1, noBubbleSize.Width, 1);
            lblMessage.Frame = new CGRect(frame.X + (isLeft ? 12 : 8), frame.Y + nickNameSize.Height+3, noBubbleSize.Width, MessageSize.Height);
            lblTimestamp.Frame = new CGRect(frame.X + (isLeft ? 12 : 8), frame.Y + frame.Height+1, noBubbleSize.Width, 10);
            
            //			label.Frame = new RectangleF(new PointF(frame.X + (isLeft ? 12 : 8), frame.Y + 6), size - BubblePadding);
        }

        static internal SizeF BubblePadding = new SizeF(22, 12);

        static internal SizeF GetSizeForText(UIView tv, string text)
        {
            //return tv.StringSize(text, font, new SizeF(tv.Bounds.Width * .7f - 10 - 22, 99999));

            NSString s = new NSString(text);
            var size = s.StringSize(font, new CGSize(tv.Bounds.Width * .7f - 10 - 22, 99999));
            return new SizeF((float)size.Width, (float)size.Height);
        }

        public void Update(ChatMessage m)
        {
			_chatMessage = m;
			lblMessage.Text = m.Message;
			lblTimestamp.Text = m.Timestamp.ToString("HH:mm");
			lblNickname.Text = m.Name;

			try
			{
				if (m.Photo.Contains("http"))
				{
					Avatar = ResizeImage(FromUrl(m.Photo), 50, 50);
				}
				else if (!string.IsNullOrEmpty(m.Photo) && m.Photo != "profile_avatar.png")
				{
					Avatar = ResizeImage(UIImage.FromFile(m.Photo), 50, 50);
				}
			}
			catch
			{
				LogHelper.WriteLog("ios error getting avatar", "ios error getting avatar", "BubbleCell");
				Avatar = ResizeImage(UIImage.FromFile("profile_avatar.png"), 50, 50);
			}


            SetNeedsLayout();
        }

        public float GetHeight(UIView tv)
        {
            return GetSizeForText(tv, lblMessage.Text).Height + GetSizeForText(this, lblNickname.Text).Height+ GetSizeForText(this, lblTimestamp.Text).Height+10 + BubblePadding.Height;
        }
        public static UIImage ResizeImage(UIImage sourceImage, float width, float height)
        {
            UIGraphics.BeginImageContext(new SizeF(width, height));
            sourceImage.Draw(new RectangleF(0, 0, width, height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return resultImage;
        }

        // crop the image, without resizing

    }
}