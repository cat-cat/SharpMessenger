using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.ComponentModel;

namespace ChatClient.Core.Common.Models
{
    public class ChatMessage : INotifyPropertyChanged
    {
        public enum Status { Pending /* no info whether message saved on server or not */, Delivered /* message saved on server */, Read, Deleted, PendingDelete }
        string _conversationId;
        string _room;
        bool _messageEdited;
        string _replyId;
        Status _status;
        bool _justSent;
        string _guid;
        string _replyGuid;
        string _id;
        string _name;
        string _message;
        string _replyQuote;
        string _photo;
        User _author;
        bool _isMine;
        DateTime _timestamp;
        User _opponent;

        public event PropertyChangedEventHandler PropertyChanged;

        public Status status
		{
			get
			{
				return _status;
			}
			set
			{
				_status = value;
			}
		}

		public bool JustSent
		{
			get
			{
				return _justSent;
			}
			set
			{
				_justSent = value;
			}
		}

		[JsonProperty(PropertyName = "messageEdited")]
		public bool messageEdited
		{
			get
			{
				return _messageEdited;
			}
			set
			{
				_messageEdited = value;
			}
		}

		[JsonProperty(PropertyName = "conversationId")]
		public string conversationId
		{
			get
			{
				return _conversationId;
			}
			set
			{
				_conversationId = value;
			}
		}

		[JsonProperty(PropertyName = "Room")]
		public string Room
		{
			get
			{
				return _room;
			}
			set
			{
				_room = value;
			}
		}

		[JsonProperty(PropertyName = "ReplyId")]
		public string ReplyId
		{
			get
			{
				return _replyId;
			}
			set
			{
				_replyId = value;
			}
		}

		[JsonProperty(PropertyName = "ReplyGuid")]
		public string ReplyGuid
		{
			get
			{
				return _replyGuid;
			}
			set
			{
				_replyGuid = value;
			}
		}

		[JsonProperty(PropertyName = "guid")]
		public string guid
		{
			get
			{
				return _guid;
			}
			set
			{
				_guid = value;
			}
		}

        [JsonProperty(PropertyName = "Name")]
        public string Name {
            get {
                return _name;
            }
            set {
                _name = value;
            }
        }

		[JsonProperty(PropertyName = "Message")]
		public string Message
		{
			get
			{
				return _message;
			}
			set
			{
				_message = value;
                if(PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Message"));
			}
		}

		[JsonProperty(PropertyName = "ReplyQuote")]
		public string ReplyQuote
		{
			get
			{
				return _replyQuote;
			}
			set
			{
				_replyQuote = value;
			}
		}

        public string Photo {
            get {
                return _photo;
            }
            set {
                _photo = value;
            }
        }
        
        [JsonProperty(PropertyName = "_to")]
        public User Opponent
        {
            get
            {
                return _opponent;
            }

            set
            {
                _opponent = value;
            }
        }
        [JsonProperty(PropertyName = "_creator")]
        public User Author {
            get {
				return _author;
            }
            set {
                _author = value;
            }
        }

        public bool IsMine {
            get {
                return _isMine;
            }
            set {
                _isMine = value;
            }
        }
        [JsonProperty(PropertyName = "createdAt")]
        public DateTime Timestamp {
            get {
                return _timestamp;
            }
            set {
                _timestamp = value;
            }
        }
        [JsonProperty(PropertyName = "_id")]
        public string Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

    }
}
