using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace ChatClient.Core.Common.Models
{
    public class ChatMessage {
		public enum Status {Pending /* no info whether message saved on server or not */, Delivered /* message saved on server */, Read }
		private Status _status;
		private bool _justSent;
		private string _guid;
        private string _id;
        private string _name;
        private string _message;
        private string _photo;
        private User _author;
        private bool _isMine;
        private DateTime _timestamp;
        private User _opponent;


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
        public string Message {
            get {
                return _message;
            }
            set {
                _message = value;
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
