using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ChatClient.Core.Common.Models;
using ChatClient.Core.UI.ViewModels;
using ChatClient.Core.Common;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ChatClient.Core.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pgChat : ContentPage
    {
        DateTime isTypingDateTime = DateTimeOffset.UtcNow.DateTime - new TimeSpan(0, 0, 6);
        ChatMessage _messageReplyTo;

        public pgChat()
        {
            InitializeComponent();
        }
						
		async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            string action = await App.Current.MainPage.DisplayActionSheet("Actions", "Cancel", "Delete", new string[2] { "Reply", "Edit" });
            User lUser = await Core.BL.Session.Authorization.GetUser();

            var selectedChatMessage = (ChatMessage)e.Item;
            if (action == "Delete" && lUser.Id == selectedChatMessage.Author.Id) // delete only message of the current user
            {
                selectedChatMessage.status = ChatMessage.Status.PendingDelete;
                v.Add(k.MessageSendProgress, selectedChatMessage);
            }
            else if (action == "Reply") // reply
            {
                v.Add(k.MessageReply, selectedChatMessage);
            }
            else if (action == "Edit" && lUser.Id == selectedChatMessage.Author.Id) // edit only message of the current user
            {
                v.Add(k.MessageEdit, selectedChatMessage);
            }

            ((ListView)sender).SelectedItem = null;
        }

        async void OnEvent(object sender, NotifyCollectionChangedEventArgs e)
        {
            var newItem = (KeyValuePair<k, object>)e.NewItems[0];
			var bc = (BaseViewModel)BindingContext;

            if (newItem.Key == k.MessageReply)
            {
                _messageReplyTo = (ChatMessage)newItem.Value;
                Message_Entry.Text = "rep: ";

                bc.ChatMessage.ReplyQuote = _messageReplyTo.Message;
                bc.ChatMessage.ReplyGuid = _messageReplyTo.guid;
                bc.ChatMessage.ReplyId = _messageReplyTo.Id;
            }
            else if (newItem.Key == k.MessageEdit)
            {
                var m = (ChatMessage)newItem.Value;
                bc.StartEditMessage(m);
            }
			else if (newItem.Key == k.OnIsTyping)
			{
				Device.BeginInvokeOnMainThread(() => { Title = newItem.Value + " is typing..."; });

				Device.StartTimer(TimeSpan.FromSeconds(5), () => {
					try // this page can already be destroyied for the time this handler being called
					{
						Device.BeginInvokeOnMainThread(() => { Title = ""; });
					}
					catch // this page can already be destroyied for the time this handler being called
					{
					}
					return false;
				});
			}
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
			var bc = (BaseViewModel)BindingContext;

            if (e.NewTextValue.Length > 0)
                sendMessageButton.Image = "send_message_normal.png";
            else
            {
                sendMessageButton.Image = "send_message_inactive.png";

                // reset replyTo message if wipe all the text in message entry
                _messageReplyTo = null;
                bc.ChatMessage.ReplyQuote = null;
                bc.ChatMessage.ReplyGuid = null;
                bc.ChatMessage.ReplyId = null;
            }

            // handle Typing... logic
            if ((DateTimeOffset.UtcNow.DateTime - isTypingDateTime) > new TimeSpan(0, 0, 5))
            {
                isTypingDateTime = DateTimeOffset.UtcNow.DateTime;
                bc.TypingBroadcast(isTypingDateTime);
            }
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
			v.h(new k[] { k.MessageEdit, k.MessageReply, k.OnIsTyping }, OnEvent);
        }

        private void ContentPage_Disappearing(object sender, EventArgs e)
        {
            v.m(OnEvent);
        }
    }
}