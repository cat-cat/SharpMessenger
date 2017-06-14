using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatClient.Core.Common.Resx;

using Xamarin.Forms;

namespace ChatClient.Core.Common.Helpers
{
    public class MyConverter : IValueConverter
    {
		public static string ByteArrayToHex(byte[] hash)
		{
			var hex = new StringBuilder(hash.Length * 2);
			foreach (byte b in hash)
				hex.AppendFormat("{0:x2}", b);

			return hex.ToString();
		}
		
        public object Convert(object o, Type type, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)o;
            return string.Format("{0}:{1}", (int)(date-DateTime.UtcNow ).TotalMinutes, (DateTime.UtcNow - date).Seconds);


        }
        public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class DateConverter : IValueConverter
    {
        public object Convert(object o, Type type, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)o;
            if (date < DateTime.Now.AddYears(-10))
                return "---";
            return date.ToString("HH:mm:ss");

        }
        public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class FullTimeConverter : IValueConverter
    {
        public object Convert(object o, Type type, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)o;
            if (date < DateTime.UtcNow)
                return "00:00";
            return string.Format("{0}:{1}:{2}", ((int)(date-DateTime.UtcNow).TotalHours).ToString("00"), ((int)(date - DateTime.UtcNow).Minutes).ToString("00"), (date - DateTime.UtcNow).Seconds.ToString("00"));

        }
        public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class EndDateConverter : IValueConverter
    {
        public object Convert(object o, Type type, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)o;
            int dateSubstring = DateTime.Now.Subtract(DateTime.UtcNow).Hours;
           date= date.AddHours(dateSubstring);
			return string.Format(AppResources.GroupWillEnd + " {0} {1}",date.ToString("dd.MM.yyyy"),date.ToString("HH:mm"));

        }
        public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class FinishedDateConverter : IValueConverter
    {
        public object Convert(object o, Type type, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)o;
            int dateSubstring = DateTime.Now.Subtract(DateTime.UtcNow).Hours;
            date = date.AddHours(dateSubstring);
			return string.Format(AppResources.GroupEnded + " {0} {1}", date.ToString("dd.MM.yyyy"), date.ToString("HH:mm"));

        }
        public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class DialogDateConverter : IValueConverter
    {
        public object Convert(object o, Type type, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)o;
            if(date.Year!=DateTime.Now.Year)
            return  date.ToString("dd MMM yyyy");
            else if (date.Day == DateTime.Now.Day && date.Month == DateTime.Now.Month)
                return date.ToString("HH:mm");
            return date.ToString("dd MMM");

        }
        public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
