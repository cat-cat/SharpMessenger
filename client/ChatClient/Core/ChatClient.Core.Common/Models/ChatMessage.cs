using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace ChatClient.Core.Common.Models
{
    public class ChatMessage {
        private string _id;
        private string _name;
        private string _message;
        private string _photo;
        private User _ownerId;
        private bool _isMine;
        private DateTime _timestamp;
        private User _opponent;

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
        public User OwnerId {
            get {
                return _ownerId;
            }
            set {
                _ownerId = value;
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
