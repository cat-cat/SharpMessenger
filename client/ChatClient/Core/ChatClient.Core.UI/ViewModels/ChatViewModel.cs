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
	public class ChatViewModel : BaseViewModel
	{
		#region Fields

		private ChatMessageViewModel _chatMessage = new ChatMessageViewModel();
		//private IChatServices _chatServices;
		private ObservableCollection<ChatMessageViewModel> _messages = new ObservableCollection<ChatMessageViewModel>();
		private string _roomName;
		private bool _isPrivatChat { get; set; }

		private CacheMessage _cacheMessage;

		private User _receiver { get; set; }

		#endregion

		#region Constractors and Destructors
		~ChatViewModel()
		{
			v.m(OnCollectionChanged);
		}

		public ChatViewModel(string roomName)
		{
			// subscribe for events
			v.h(OnCollectionChanged);

			_roomName = roomName;
			ExecuteJoinRoomCommand();
			//_chatServices = DependencyService.Get<IChatServices>();
			//_chatServices.SetRoomID(roomName);
			//_chatServices.Connect();
			//_chatServices.OnMessageReceived += _chatServices_OnMessageReceived;
			GetMessages();
			_messages = new ObservableCollection<ChatMessageViewModel>();
			///  _chatServices.JoinRoom(roomName);

		}



		public async void SocketOff()
		{
			if (_cacheMessage != null)
				await PersisataceService.GetCacheMessagePersistance().SaveItemAsync(_cacheMessage);
			//_chatServices.Disabled();
		}

		void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				var newItem = (KeyValuePair<v.k, object>)e.NewItems[0];
				if (newItem.Key == v.k.OnUpdateUserOnlineStatus)
				{
					var data = (string)newItem.Value;
					if (data == _receiver.Id)
					{
						v.Consume(newItem.Key);
						// set status: data["status"] = "online" ? On : Off;
					}
				}
				else if (newItem.Key == v.k.OnMessageReceived)
				{
					ChatMessage message = (ChatMessage)newItem.Value;
					_chatServices_OnMessageReceived(sender, message);
				}

			}
		}

		public ChatViewModel(User user)
		{
			_messages = new ObservableCollection<ChatMessageViewModel>();
			//_chatServices = DependencyService.Get<IChatServices>();
			//_chatServices.SetRoomID(_roomName);
			//_chatServices = new ChatPrivateService();
			// _chatMessage = new ChatMessageViewModel();

			//    _messages = new ObservableCollection<ChatMessageViewModel>();
			_receiver = user;

			// subscribe for events
			v.h(OnCollectionChanged);

			// request from server online status for interlocutor
			v.Add(v.k.OnlineStatus, _receiver.Id);

			//v.Add(v.k.MessageReceived, _receiver.Id);

			if (string.IsNullOrEmpty(_receiver.Nickname))
				_receiver.Nickname = _receiver.Id;

			//_chatServices.Connect();
			//_chatServices.OnMessageReceived += _chatServices_OnMessageReceived;

			_isPrivatChat = true;
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
			if (_isPrivatChat)
			{
				response = await new GetMessages(lUser.Token, 1, 100, "", String.Format("{0},{1}", lUser.Id, _receiver.Id)).Object();
				_cacheMessage = await PersisataceService.GetCacheMessagePersistance()
								   .GetItemAsync(String.Format("{0},{1}", lUser.Id, _receiver.Id)) ?? new CacheMessage() { Id = String.Format("{0},{1}", lUser.Id, _receiver.Id), IsSended = true };
			}
			else if (!_isPrivatChat)
			{
				response = await new GetMessages(lUser.Token, 1, 100, _roomName).Object();
				_cacheMessage = await PersisataceService.GetCacheMessagePersistance()
									.GetItemAsync(_roomName) ?? new CacheMessage() { Id = _roomName, IsSended = true };
			}
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
			if (_isPrivatChat)
				lMessage = String.Format("w:{0}:{1}", _receiver.Id, _chatMessage.Message);

			v.Add(v.k.MessageSend, new Dictionary<string, object>() { { "message", new ChatMessage { Name = _chatMessage.Name, Message = lMessage } }, { "roomName", _roomName} });
            //await _chatServices.Send(new ChatMessage { Name = _chatMessage.Name, Message = lMessage }, _roomName);
            IsBusy = false;
        }

        #endregion

        #region Join Room Command

        private Command joinRoomCommand;

        /// <summary>
        ///     Command to Send Message
        /// </summary>
        public Command JoinRoomCommand
        {
            get
            {
                return joinRoomCommand ??
                       (joinRoomCommand = new Command(ExecuteJoinRoomCommand));
            }
        }

        private async void ExecuteJoinRoomCommand()
        {
            IsBusy = true;
			v.Add(v.k.JoinRoom, _roomName);
            //await _chatServices.JoinRoom(_roomName);
            IsBusy = false;
        }
		#endregion
    }
}

