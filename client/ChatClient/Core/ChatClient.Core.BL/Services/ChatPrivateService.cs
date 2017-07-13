using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Plugin.DeviceInfo;
using ChatClient.Core.Common;
using ChatClient.Core.Common.Models;
using ChatClient.Core.Common.Services;
using ChatClient.Core.BL.Session;
using ChatClient.Core.Common.Interfaces;
using ChatClient.Core.Common.Models.Base;
using ChatClient.Core.SAL.Methods;
using ChatClient.iOS.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Quobject.SocketIoClientDotNet.Client;

using Xamarin.Forms;

[assembly: Dependency(typeof(ChatPrivateService))]
namespace ChatClient.iOS.Services
{
	public class ChatPrivateService : IChatServices
	{
		//public event EventHandler<ChatMessage> OnMessageReceived;
		Socket socket;
		private User _user;
		private string _lastConversation;

		void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				var newItem = (KeyValuePair<k, object>)e.NewItems[0];
				if (newItem.Key == k.JoinRoom)
				{
					// v.Consume(newItem.Key);
					JoinRoom((string)newItem.Value);
				}
				else if (newItem.Key == k.MessageSend)
				{
					// v.Consume(k.MessageSend);
					var m = (ChatMessage)newItem.Value;
					Send(m);
				}
				else if (newItem.Key == k.OnlineStatus)
				{
					// v.Consume(k.OnlineStatus);
					new OnlineStatusGet(_user.Token, (string)newItem.Value, _user.Id).Object();
				}
				else if (newItem.Key == k.IsTyping)
				{
					// v.Consume(k.IsTyping);
					var data = (Dictionary<string, object>)newItem.Value;
					var room = data.ContainsKey("room") ? (string)data["room"] : null;
					var participant = data.ContainsKey("participant") ? (string)data["participant"] : null;
					new TypingSetRequest(_user.Token, participant, room, (string)data["isTypingTimeStamp"]).Object();
				}
				else if (newItem.Key == k.MessageSendProgress)
				{
					var m = (ChatMessage)newItem.Value;
					new MessageStatusGet(_user.Token, (ChatMessage)newItem.Value).Object();
				}
			}

		}

		public ChatPrivateService()
		{
			//var options = new IO.Options() { IgnoreServerCertificateValidation = true, AutoConnect = true, ForceNew = true };
			//options.Transports = Quobject.Collections.Immutable.ImmutableList.Create<string>("websocket");
			//socket = IO.Socket("ws://10.1.15.209:9020", options);
			//socket.On(Socket.EVENT_CONNECT, () =>
			//{
			//	Debug.WriteLine("Connected");

			//	socket.Emit("hi");
			//});
			//socket.On(Socket.EVENT_DISCONNECT, () =>
			//{
			//	Debug.WriteLine("Disconnected");
			//});
			v.h(OnCollectionChanged);
		}

		~ChatPrivateService()
		{
			v.m(OnCollectionChanged);
			// v.Remove(v.OnMessageReceived);
			// v.Remove(k.OnUpdateUserOnlineStatus);
			// v.Remove(v.OnIsTyping);
		}

		//void SetRoomID(string _roomID)
		//{
		//	roomID = _roomID;
		//}

		public void Start()
		{
			socket.Emit("storage", false);
		}


		//	socket.emit('nickname', name);
		public void EmitNickname(string nickname)
		{
			socket.Emit("nickname", nickname);
		}



		private void OnMessageReceived(object sender, ChatMessage message)
		{
			v.Add(k.OnMessageReceived, message);
		}
		#region IChatServices implementation

		public async Task Connect()
		{

			//var options = new IO.Options() { IgnoreServerCertificateValidation = true, AutoConnect = true, ForceNew = true };
			//options.Transports = Quobject.Collections.Immutable.ImmutableList.Create<string>("websocket");
			_user = await Core.BL.Session.Authorization.GetUser();
			if (string.IsNullOrEmpty(_user.Nickname))
				_user.Nickname = "Phone";
#if DEBUG
			socket = IO.Socket(MyConstants.debugURL + ":" + MyConstants.chatPort);
#else
			socket = IO.Socket(MyConstants.baseURL + ":" + MyConstants.chatPort);
#endif
			socket.On(Socket.EVENT_CONNECT, () =>
		   {

				//socket.Emit("authenticate", "{ token:eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJfaWQiOiI1ODkxZTk3N2ZlM2Q5ZWEzYjI5NmVlY2IifQ.3xlogxTrkfZrtx79SXsfkvL5XvPtBPUpFgSqjrIvMoY}"); //send the jwt


				var jobj = new JObject();
				//jobj.Add("name", message.Name);
				//jobj.Add("message", message.Message);
				jobj.Add("token", _user.Token);
			   socket.Emit("authenticate", jobj);

				//socket.Emit("hi", "Hello server");
				//Start();

			});

			socket.On("authenticated", () =>
			{
				//do other things
				Debug.WriteLine("authenticated: ");

				//var array = new JArray();
				//array.Add("phone");
				//array.Add("iphone simulator");
				socket.Emit("joinserver", /*CrossDeviceInfo.Current.Id*/_user.Nickname, CrossDeviceInfo.Current.Id);
			});

			socket.On("messageDeleted", (data) =>
			{
				Debug.WriteLine("messageDeleted: " + data);

				var definition = new { guid = ""};
				var o = JsonConvert.DeserializeAnonymousType(data.ToString(), definition);

				v.Add(k.OnMessageSendProgress, new Dictionary<string, object>() { {"guid", o.guid }, {"status", ChatMessage.Status.Deleted } });
			});

			socket.On("unauthorized", (msg) =>
			{
				//     Debug.WriteLine("unauthorized: " + msg);
			});


			socket.On(Socket.EVENT_DISCONNECT,
				(data) =>
				{
					Debug.WriteLine("Disconnected: " + data);
				});

			socket.On(Quobject.EngineIoClientDotNet.Client.Socket.EVENT_ERROR,
				(data) =>
				{
					//socket.Emit("error");
					Debug.WriteLine("Socket error: " + data);
				});

			//io.sockets.emit("update-people", { people: people, count: sizePeople});
			socket.On("update-people", (data) =>
			{
				var definition = new { onlineStatus = "", id = "" };
				var o = JsonConvert.DeserializeAnonymousType(data.ToString(), definition);
				Dictionary<string, bool> d = new Dictionary<string, bool>() { { o.id, Convert.ToBoolean(o.onlineStatus) } };
				v.Add(k.OnUpdateUserOnlineStatus, d);

				Debug.WriteLine("on.update-people: " + data);

				//if(!string.IsNullOrEmpty(roomID)) // TODO: don't create room for whispering
				//	socket.Emit("createRoom", roomID);
			});

			socket.On("conversation", (data) =>
			{
				Debug.WriteLine("on.conversation: " + data);
				_lastConversation = data.ToString();
			});

			//socket.emit("roomList", { rooms: rooms, count: sizeRooms});
			//socket.On("roomList", (data) =>
			//{
			//	Debug.WriteLine("on.roomList: " + data);
			//});

			//socket.emit("joined"); //extra emit for GeoLocation
			socket.On("joined", (data) =>
			{
				Debug.WriteLine("on.joined: " + data);
			});

			//socket.emit("exists", {msg: "The username already exists, please pick another one.", proposedName: proposedName});
			socket.On("exists", (data) =>
			{
				Debug.WriteLine("on.exists: " + data);
			});

			socket.On("chat", async (data) =>
			{
				Debug.WriteLine("on.chat: " + data);

				// http://stackoverflow.com/questions/12674076/how-can-i-use-complex-property-names-in-anonymous-type
				var definition = new { userAvatarPrefix = "", socketID = new { id = "", name = "" }, msTime = "", msg = "", userImage = "", replyQuote = "", replyId = "", replyGuid = "", guid = ""};

				var o = JsonConvert.DeserializeAnonymousType(data.ToString(), definition);
				ChatMessage cm = new ChatMessage();
				cm.guid = o.guid;
				cm.ReplyId = o.replyId;
				cm.ReplyGuid = o.replyGuid;
				cm.ReplyQuote = o.replyQuote;
				cm.Message = o.msg;
				cm.Name = o.socketID.name;
				cm.Author = new User() { Id = o.socketID.id }; ;
				cm.IsMine = _user.Id == o.socketID.id;
				if (!string.IsNullOrEmpty(o.userImage) && o.userImage != "false")
					cm.Photo = await
							  DependencyService.Get<IFileHelper>()
								  .PhotoCache(o.userAvatarPrefix.ToString(), o.userImage, ImageType.Users);
				else
					cm.Photo = "profile_avatar.png";
				cm.Timestamp = Convert.ToDateTime(o.msTime);
				OnMessageReceived(this, cm);
			});

			socket.On("isTyping", (data) =>
			{
				var definition = new { person = "" };
				var o = JsonConvert.DeserializeAnonymousType(data.ToString(), definition);

				v.Add(k.OnIsTyping, o.person);

				Debug.WriteLine("on.isTyping: " + data);
			});

			socket.On("update", (data) =>
			{
				Debug.WriteLine("on.update: " + data);
			});

			socket.On("sendRoomID", (Object data) =>
			{
				Debug.WriteLine("on.sendRoomID: " + data);
			});

			socket.On("history", (data) =>
			{
				Debug.WriteLine("on.history: " + data);
			});

			socket.On("whisper", async (data) =>
			{
				Debug.WriteLine("on.whisper: " + data);

				// http://stackoverflow.com/questions/12674076/how-can-i-use-complex-property-names-in-anonymous-type
				var definition = new { userAvatarPrefix = "", socketID = new { id = "", name = "" }, msTime = "", msg = "", userImage = "", replyQuote = "", replyId = "", replyGuid = "", guid = ""};

				var o = JsonConvert.DeserializeAnonymousType(data.ToString(), definition);
				ChatMessage cm = new ChatMessage();
				cm.guid = o.guid;
				cm.ReplyId = o.replyId;
				cm.ReplyGuid = o.replyGuid;
				cm.ReplyQuote = o.replyQuote;
				cm.Message = o.msg;
				cm.Name = o.socketID.name;
				cm.Author = new User() { Id = o.socketID.id }; ;
				cm.IsMine = _user.Id == o.socketID.id;
				if (!string.IsNullOrEmpty(o.userImage) && o.userImage != "false")
					cm.Photo = await
							  DependencyService.Get<IFileHelper>()
								  .PhotoCache(o.userAvatarPrefix.ToString(), o.userImage, ImageType.Users);
				else
					cm.Photo = "profile_avatar.png";
				cm.Timestamp = Convert.ToDateTime(o.msTime);
				OnMessageReceived(this, cm);
			});

			socket.On("hi2back", (data) =>
			{
				//   Debug.WriteLine(data);
				socket.Emit("hi", "hi again data");
				//socket.Disconnect();
			});

			//==== receive socket nickname==//
			socket.On("nickname", (data) =>
			{
				Debug.WriteLine("on.nickname: " + data);
			});

			//socket.On("output-all", (data) =>
			//{
			//	//   Debug.WriteLine("on.output-all: " + data);
			//});


			//socket.On("output", (Object data) =>
			//{
			//	Debug.WriteLine("on.output: " + data);
			//	//ChatMessage cm = new ChatMessage();
			//	ChatMessage cm = JsonConvert.DeserializeObject<ChatMessage>(data.ToString());
			//	OnMessageReceived(this, cm);
			//});


			socket.On("status", (data) =>
			{
				//  Debug.WriteLine("on.status: " + data);
			});

			//socket.Connect();
		}

		public async Task Send(ChatMessage message)
		{
			// First - show message
			OnMessageReceived(this, message);

			// Second - send message to server
			var jobj = new JObject();
			jobj.Add("name", _user.Nickname);
			if (message.Opponent != null) // private message
				jobj.Add("opponent", message.Opponent.Id);
			jobj.Add("message", message.Message);
			jobj.Add("conversationId", _lastConversation);
			jobj.Add("date", message.Timestamp.ToString());
			jobj.Add("guid", message.guid);
			jobj.Add("avatar", "no avatar");
			jobj.Add("replyGuid", message.ReplyGuid);
			jobj.Add("replyQuote", message.ReplyQuote);
			jobj.Add("replyId", message.ReplyId);

			socket.Emit("send", jobj);
		}

		void JoinRoom(string roomName)
		{
			socket.Emit("joinRoom", roomName);
		}
		public void Disabled()
		{
			_lastConversation = null;
			socket.Disconnect();
			socket.Close();
		}
		#endregion


		//		});


	}
}
