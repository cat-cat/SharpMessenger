using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace ChatClient.Core.Common.Models
{
   public class Member:User {
       private int _transactionsSum;
        [JsonProperty(PropertyName = "sum")]
        public int TransactionsSum
        {
            get
            {
                return _transactionsSum;
            }

            set
            {
                _transactionsSum = value;
            }
        }
    }
}
