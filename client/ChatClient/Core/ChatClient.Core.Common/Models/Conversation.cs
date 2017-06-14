using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Xamarin.Forms;

namespace ChatClient.Core.Common.Models
{
    public class Conversation {
        private string _id;
        private DateTime _dlc;
        private DateTime _dc;
        private string _v;
        private ChatMessage _message;
        private User _opponent;

        private Color _color ;
    

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
        [JsonProperty(PropertyName = "updatedAt")]
        public DateTime DLC
        {
            get
            {
                return _dlc;
            }

            set
            {
                _dlc = value;
            }
        }
        [JsonProperty(PropertyName = "createdAt")]
        public DateTime DC
        {
            get
            {
                return _dc;
            }

            set
            {
                _dc = value;
            }
        }
        [JsonProperty(PropertyName = "__v")]
        public string V
        {
            get
            {
                return _v;
            }

            set
            {
                _v = value;
            }
        }
        [JsonProperty(PropertyName = "lastMessage")]
        public ChatMessage Message
        {
            get
            {
                return _message;
            }

            set
            {
                _message = value;
            }
        }

        public User Opponent {
            get {
                return _opponent;
            }
            set {
                _opponent = value;
            }
        }

        public Color Color
        {
            get
            {
                return _color;
            }

            set
            {
                _color = value;
            }
        }
    }
}
