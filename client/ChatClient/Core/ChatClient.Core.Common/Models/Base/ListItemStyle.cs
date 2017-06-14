using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ChatClient.Core.Common.Models.Base
{
   public class ListItemStyle:BaseModel {
       private string _clockImage;
       private string _parImage;
       private string _fundImage;
       private string _celenderImage;
       private Color _textColor;
       private Color _timerBackGroumdColor;

       public ListItemStyle(bool isEnded) {
           if (isEnded) {
                _clockImage = "clock_inactive.png";
                _parImage = "par_inactive.png";
                _fundImage = "fund_inactive.png";
                _celenderImage = "celendar_inactive.png";
                _textColor = Color.FromHex("#9e9e9e");
                _timerBackGroumdColor = Color.FromHex("#bdbdbd");
            } else {
               _clockImage = "clock.png";
               _parImage = "par.png";
               _fundImage = "fund.png";
               _celenderImage = "celendar.png";
               _textColor = Color.FromHex("#388e3c");
               _timerBackGroumdColor = Color.FromHex("#fbc02d");
           }
       }
        public string ClockImage
        {
            get
            {
                return _clockImage;
            }

            set
            {
                if (Equals(_clockImage, value))
                    return;
                _clockImage = value;
                OnPropertyChanged("ClockImage");
            }
        }

        public string ParImage
        {
            get
            {
                return _parImage;
            }

            set
            {
                if (Equals(_parImage, value))
                    return;
                _parImage = value;
                OnPropertyChanged("ParImage");
            }
        }

        public string FundImage
        {
            get
            {
                return _fundImage;
            }

            set
            {

                if (Equals(_fundImage, value))
                    return;
                _fundImage = value;
                OnPropertyChanged("FundImage");
            }
        }

        public string CelenderImage
        {
            get
            {
                return _celenderImage;
            }

            set
            {
                if (Equals(_celenderImage, value))
                    return;
                _celenderImage = value;
                OnPropertyChanged("CelenderImage");
            }
        }

        public Color TextColor
        {
            get
            {
                return _textColor;
            }

            set
            {
                if (Equals(_textColor, value))
                    return;
                _textColor = value;
                OnPropertyChanged("TextColor");
            }
        }

        public Color TimerBackGroumdColor
        {
            get
            {
                return _timerBackGroumdColor;
            }

            set
            {
                if (Equals(_timerBackGroumdColor, value))
                    return;
                _timerBackGroumdColor = value;
                OnPropertyChanged("TimerBackGroumdColor");
            }
        }
    }
}
