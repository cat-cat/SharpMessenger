using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;

namespace ChatClient.Core.Common.Models
{
	// TODO: remove this class, replace with ChatMessage
  public  class CacheMessage {
		private ChatMessage.Status _status;
      private string _id;
      private string _message;
      private bool _isSended;
		string _guid;

		public ChatMessage.Status status
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

		[PrimaryKey]
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
        public string Id {
            get {
                return _id;
            }
            set {
                _id = value;
            } 
		}
        public string Message
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
        public bool IsSended
        {
            get
            {
                return _isSended;
            }
            set
            {
                _isSended = value;
            }
        }
    }
}
