using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Core.Common.Helpers
{
   public static class ObjectHelper
    {
        public static bool IsPropertyExist(dynamic settings, string name)
        {
            bool isDefined = false;
            object axis = null;
            try
            {
                axis = settings[name];
                if(axis!=null)
                isDefined = true;
            }
            catch 
            { }
            return isDefined;
        }
    }
}
