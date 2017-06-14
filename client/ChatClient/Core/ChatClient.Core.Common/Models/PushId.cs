using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;

namespace ChatClient.Core.Common.Models
{
	public class PushId : IEquatable<PushId> {
       private bool _isSended;
       private string _id;

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
        [PrimaryKey]
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

		public bool Equals(PushId other)
		{
			return (_id == other._id) && (_isSended == other._isSended);
		}
	}
}
