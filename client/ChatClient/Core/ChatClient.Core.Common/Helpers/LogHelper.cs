using System;

namespace ChatClient.Core.Common.Helpers
{
 public   class LogHelper
    {
     public static async void WriteException(Exception exception) {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(String.Format("StackTrace:{0}\n Error:{1}", exception.StackTrace,exception.Message));
#endif
            //TODO Create log for release version
        }

        public static async void WriteLog(string message, params object[] args) {
#if DEBUG
			System.Diagnostics.Debug.WriteLine("{0}", string.Format(message, args));
#endif
            //TODO Create log for release version
        }

    }
}
