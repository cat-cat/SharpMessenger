#region

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Linq;

using ChatClient.Core.Common;
using ChatClient.Core.Common.Helpers;
using ChatClient.Core.Common.Interfaces;
using ChatClient.Core.Common.Models;
using ChatClient.Core.Common.Models.Base;
using ChatClient.Core.Common.Services;
using ChatClient.Core.DAL.Data.Base;
using ChatClient.Core.SAL.Adapters;
using ChatClient.Core.SAL.Methods;
using ChatClient.Core.UI.Pages;
using ChatClient.Core.Common.Resx;

using Newtonsoft.Json;

using Xamarin.Forms;

#endregion

namespace ChatClient.Core.UI.ViewModels
{
	public class PrivateChatViewModel : BaseViewModel
	{
		#region Fields

		private ChatMessageViewModel _chatMessage = new ChatMessageViewModel();
		//private IChatServices _chatServices;
		private ObservableCollection<ChatMessageViewModel> _messages = new ObservableCollection<ChatMessageViewModel>();

		private CacheMessage _cacheMessage;

		private User _receiver { get; set; }

		#endregion

		#region Constractors and Destructors
		~PrivateChatViewModel()
		{
			v.m(OnCollectionChanged);
			// v.Remove(k.IsTyping);
			// v.Remove(k.OnlineStatus);
			// v.Remove(k.MessageSend);
		}

		public override void TypingBroadcast(DateTime d)
		{
			
			v.Add(k.IsTyping, new Dictionary<string, object>() { { "isTypingTimeStamp", d.ToString() }, { "participant", _receiver == null ? null: _receiver.Id }, { "room", null } });
		}

		public override async void SocketOff()
		{
			if (_cacheMessage != null)
				await PersisataceService.GetCacheMessagePersistance().SaveItemAsync(_cacheMessage);
			//_chatServices.Disabled();
		}

	void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				var newItem = (KeyValuePair<k, object>)e.NewItems[0];
				if (newItem.Key == k.OnUpdateUserOnlineStatus)
				{
					var data = (Dictionary<string, bool>)newItem.Value;
					if (data.ContainsKey(_receiver.Id))
					{
						// only consume data for this _receiver
						// v.Consume(k.OnUpdateUserOnlineStatus);
						bool status = data[_receiver.Id];
						// TODO: display status
					}
				}
				else if (newItem.Key == k.OnMessageReceived)
				{
					// v.Consume(k.OnMessageReceived);
					ChatMessage message = (ChatMessage)newItem.Value;
					_chatServices_OnMessageReceived(sender, message);
				}
				else if (newItem.Key == k.OnIsTyping)
				{
						// v.Consume(k.OnIsTyping);
						// show newItem.Value isTyping...
				}
			}
		}

		public PrivateChatViewModel(User user)
		{
			_messages = new ObservableCollection<ChatMessageViewModel>();

			_receiver = user;

			// subscribe for events
			v.h(OnCollectionChanged);

			// request from server online status for interlocutor
			v.Add(k.OnlineStatus, _receiver.Id);

			if (string.IsNullOrEmpty(_receiver.Nickname))
				_receiver.Nickname = _receiver.Id;

			GetMessages();
		}

		#endregion

		#region Properties

		public string Title
		{
			get
			{
				return AppResources.Chat;
			}
		}

		public ObservableCollection<ChatMessageViewModel> Messages
		{
			get
			{
				return _messages;
			}
			set
			{
				_messages = value;
				OnPropertyChanged("Messages");

			}
		}

		public ChatMessageViewModel ChatMessage
		{
			get
			{
				return _chatMessage;
			}
			set
			{
				_chatMessage = value;
				OnPropertyChanged("ChatMessage");
			}
		}

		#endregion

		#region Private Methods and Operators
		private async void GetMessages()
		{
			IsBusy = true;
			User lUser = await BL.Session.Authorization.GetUser();
			if (lUser == null)
			{
				IsBusy = false;
				return;
			}

			Dictionary<string, object> response = null;

			response = await new GetMessages(lUser.Token, 1, 100, "", String.Format("{0},{1}", lUser.Id, _receiver.Id)).Object();
			_cacheMessage = await PersisataceService.GetCacheMessagePersistance()
							   .GetItemAsync(String.Format("{0},{1}", lUser.Id, _receiver.Id)) ?? new CacheMessage() { Id = String.Format("{0},{1}", lUser.Id, _receiver.Id), IsSended = true };
			
			if (_cacheMessage != null && !_cacheMessage.IsSended)
				ChatMessage.Message = _cacheMessage.Message;
			if (response == null)
			{
				IsBusy = false;
				return;
			}
			foreach (
				ChatMessage lMessage in
				   response["messages"] as List<ChatMessage>)
			{
				if (_messages.Any(mess => mess.Id == lMessage.Id))
					continue;
				_messages.Add(new ChatMessageViewModel()
				{
					Id = lMessage.Id,
					Image = string.IsNullOrEmpty(lMessage.OwnerId.Photo) || lMessage.OwnerId.Photo.Contains("profile_avatar") ? "profile_avatar.png" : await
						 DependencyService.Get<IFileHelper>()
							 .PhotoCache(response["ImagePrefix"].ToString(), lMessage.OwnerId.Photo, ImageType.Users),
					Name = string.IsNullOrEmpty(lMessage.OwnerId.Nickname) ? AppResources.NewMember : lMessage.OwnerId.Nickname,
					Message = lMessage.Message,
					IsMine = lMessage.OwnerId.Id == lUser.Id,
					Timestamp = lMessage.Timestamp
				});
			}
			if (_messages.Count > 0)
				ChatPage.messageList.ScrollTo(_messages[_messages.Count - 1], ScrollToPosition.End, true);

			IsBusy = false;
		}
		#endregion

		#region Private Methods and Operators
		private void _chatServices_OnMessageReceived(object sender, ChatMessage e)
		{
			try
			{
				ChatMessageViewModel lMessage = new ChatMessageViewModel
				{
					Name = e.Name,
					Message = e.Message,
					IsMine = e.IsMine,
					Image = e.Photo,
					Timestamp = e.Timestamp
				};
				if (lMessage.IsMine && lMessage.Message == _cacheMessage.Message)
					_cacheMessage.IsSended = true;
				_messages.Add(lMessage);
			}
			catch (Exception error)
			{
				Debug.WriteLine("crash _chatServices_OnMessageReceived: " + error.ToString());

#if DEBUG
				Debug.Assert(false);
#endif
			}

		}
		#endregion

		#region Send Message Command

		private Command sendMessageCommand;

		/// <summary>
		///     Command to Send Message
		/// </summary>
		public Command SendMessageCommand
		{
			get
			{
				return sendMessageCommand ??
					   (sendMessageCommand = new Command(ExecuteSendMessageCommand));
			}
		}

		private async void ExecuteSendMessageCommand()
		{
			//if(string.IsNullOrEmpty(_cacheMessage.Message))
			//    return;
			//if(_cacheMessage!=null && !_cacheMessage.IsSended)
			//    return;
			IsBusy = true;
			string lMessage = _chatMessage.Message;
			if (_cacheMessage != null)
			{
				_cacheMessage.IsSended = false;
				_cacheMessage.Message = _chatMessage.Message;
			}

			lMessage = String.Format("w:{0}:{1}", _receiver.Id, _chatMessage.Message);

			v.Add(k.MessageSend, new Dictionary<string, object>() { { "message", new ChatMessage { Name = _chatMessage.Name, Message = lMessage } }, { "roomName", null} });
            IsBusy = false;
        }

        #endregion

    }
}

