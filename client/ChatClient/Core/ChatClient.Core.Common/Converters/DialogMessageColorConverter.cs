using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ChatClient.Core.Common.Converters
{
  public  class DialogMessageColorConverter
    {
        public object Convert(object o, Type type, object parameter, CultureInfo culture) {
            bool isMine = (bool)o;
            if (isMine)
                return Color.Gray;
            else
                return Color.Black;
           


        }
        public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
