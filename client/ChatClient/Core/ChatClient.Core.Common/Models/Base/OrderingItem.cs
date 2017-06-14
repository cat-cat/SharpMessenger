using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Core.Common.Models.Base
{
   public class OrderingItem {
       private string _text;
       private string _value;
       
        public string Text
        {
            get
            {
                return _text;
            }

            set
            {
                _text = value;
            }
        }

        public string Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }

        
    }
}
