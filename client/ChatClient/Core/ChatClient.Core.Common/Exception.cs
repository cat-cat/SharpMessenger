using System;

namespace ChatClient.Core.Common
{
   public class NeedConnectionToNetwork:Exception {
       private string _message = "Need internet connection";
       public override string Message {
           get {
               return _message;
           }
       }
   }
    public class BadConnection : Exception
    {
        private string _message = "Bad internet connection";
        public override string Message
        {
            get
            {
                return _message;
            }
        }
    }

    public class NeedCamera : Exception {
        private string _message = "Camera is not available";
        public override string Message
        {
            get
            {
                return _message;
            }
        }
    }

    public class Unauthorized : Exception {
        private string _message = "Unauthorized";

        public override string Message
        {
            get
            {
                return _message;
            }
           
        }
    }
    public class InvalidPhotoSize : Exception
    {
        private string _message = "Invalid photo size";

        public override string Message
        {
            get
            {
                return _message;
            }

        }
    }
}
