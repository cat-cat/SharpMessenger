using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using ChatClient.Core.UI.PopupPages;
using ChatClient.Core.Common;
using ChatClient.Core.Common.Models;
using ChatClient.Core.UI.Controls;
using ChatClient.Core.UI.ViewModels;

using ImageCircle.Forms.Plugin.Abstractions;

using Xamarin.Forms;

using Rg.Plugins.Popup.Extensions;

namespace ChatClient.Core.UI.Pages
{
    public class ChatPage : BasePage<BaseViewModel>
    {
		DateTime isTypingDateTime = DateTimeOffset.UtcNow.DateTime - new TimeSpan(0,0,6);
        //private Entry nameEntry;
        private Editor messageEntry;
        private Button sendMessageButton;
        //private ListView messagesListView;
        //private Button joinButton;
		public static ChatListView messageList;
		ChatMessage _messageReplyTo;

        public ChatPage()
        {
            //Join Room Button
            //joinButton = new Button
            //{
            //    Text = "Join Group"
            //};
            //joinButton.SetBinding(Button.CommandProperty, "JoinRoomCommand");

            ////Init Name Entry
            //nameEntry = new Entry
            //{
            //    TextColor = Color.Black
            //};
            //nameEntry.SetBinding(Entry.TextProperty, "ChatMessage.Name", BindingMode.TwoWay);

            //Init Message Entry
            SetBinding(IsBusyProperty,new Binding("IsBusy"));
            messageEntry = new Editor
            {
                TextColor = Color.Black,
              //  Placeholder = "Введите сообщения",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor =Color.FromHex("#f5f5f5"),
            };
            messageEntry.TextChanged += MessageEntry_TextChanged;
            messageEntry.SetBinding(Editor.TextProperty, "ChatMessage.Message");
           
            sendMessageButton = new Button
            {
                Image = "send_message_inactive.png",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.End,
                Margin = new Thickness(0,5,15,5),
                WidthRequest = 30,
                BorderWidth = 0,
                BackgroundColor =Color.Transparent
            };
            sendMessageButton.SetBinding(Button.CommandProperty, "SendMessageCommand");

            //Init List
            //			messagesListView = new ListView ();
            //			messagesListView.SetBinding (ListView.ItemsSourceProperty, "Messages");

            messageList = new ChatListView();
            messageList.VerticalOptions = LayoutOptions.FillAndExpand;
            messageList.SetBinding(ChatListView.ItemsSourceProperty, new Binding("Messages"));
            messageList.ItemTemplate = new DataTemplate(CreateMessageCell);
           messageList.SetBinding(ChatListView.IsRefreshingProperty,new Binding("IsBusy", BindingMode.OneWay));
            var stackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.FromHex("#f5f5f5"),
              
                Children = {
                    messageEntry,
                    sendMessageButton
                }
            };
            Content=new ScrollView() { 
                Orientation = ScrollOrientation.Vertical,
                InputTransparent = false,
            Content = new StackLayout
            {
               Orientation = StackOrientation.Vertical,
                Children =
                {
                    messageList,
                    stackLayout
                    
                }
            }
            };
    //        this.Disappearing +=  (sender,e) =>
    //        {
				//ViewModel.SocketOff();
    //        };
        }

		protected override void OnAppearing()
		{
			base.OnAppearing();

			v.h(new k[] {k.MessageEdit, k.MessageReply}, OnEvent);
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			v.m(OnEvent);
		}

		async void OnEvent(object sender, NotifyCollectionChangedEventArgs e)
		{
			var newItem = (KeyValuePair<k, object>)e.NewItems[0];
			if (newItem.Key == k.MessageReply)
			{
				_messageReplyTo = (ChatMessage)newItem.Value;
				messageEntry.Text = "rep: ";
				ViewModel.ChatMessage.ReplyQuote = _messageReplyTo.Message;
				ViewModel.ChatMessage.ReplyGuid = _messageReplyTo.guid;
				ViewModel.ChatMessage.ReplyId = _messageReplyTo.Id;
			}
			else if (newItem.Key == k.MessageEdit)
			{
				var m = (ChatMessage)newItem.Value;
				ViewModel.StartEditMessage(m);
			}
		}     

        private void MessageEntry_TextChanged(object sender, TextChangedEventArgs e) {
			if (e.NewTextValue.Length > 0)
				sendMessageButton.Image = "send_message_normal.png";
			else
			{
				sendMessageButton.Image = "send_message_inactive.png";

				// reset replyTo message if wipe all the text in message entry
				_messageReplyTo = null;
				ViewModel.ChatMessage.ReplyQuote = null;
				ViewModel.ChatMessage.ReplyGuid = null;
				ViewModel.ChatMessage.ReplyId = null;
			}

			// handle Typing... logic
			if ((DateTimeOffset.UtcNow.DateTime - isTypingDateTime) > new TimeSpan(0, 0, 5))
			{
				isTypingDateTime = DateTimeOffset.UtcNow.DateTime;
				ViewModel.TypingBroadcast(isTypingDateTime);
			}
        }

        private Cell CreateMessageCell()
        {

            CircleImage lImage = new CircleImage() {HeightRequest = 50,WidthRequest = 50, Aspect = Aspect.AspectFill};
            lImage.SetBinding(Image.SourceProperty, new Binding("ChatMessage.Photo"));

            var timestampLabel = new Label();
            timestampLabel.SetBinding(Label.TextProperty, new Binding("Timestamp", stringFormat: "[{0:HH:mm}]"));
            timestampLabel.TextColor = Color.Silver;
            timestampLabel.Font = Font.SystemFontOfSize(14);
            
          

            var authorLabel = new Label();
            authorLabel.SetBinding(Label.TextProperty, new Binding("ChatMessage.Name", stringFormat: "{0}: "));
            authorLabel.TextColor = Color.Black;
            authorLabel.Font = Font.SystemFontOfSize(14);

            var messageLabel = new Label();
            messageLabel.SetBinding(Label.TextProperty, new Binding("ChatMessage.Message"));
            messageLabel.Font = Font.SystemFontOfSize(14);

            var stack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children = { authorLabel, messageLabel ,timestampLabel}
            };
            StackLayout lLayout = new StackLayout {
                Orientation = StackOrientation.Horizontal,
                Children = { lImage,stack}
                                                  };

			//if (Device.Idiom == TargetIdiom.Tablet)
			//{
			//    stack.Children.Insert(0, timestampLabel);
			//}
			var view = new MessageViewCell
			{
                View = lLayout
            };
            return view;
        }
    }
}
