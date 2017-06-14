using ChatClient.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ChatClient.Core.Common.Resx;

namespace ChatClient.Core.SAL.Adapters
{
 public   class Response:IDisposable
    {
     private bool _error;
     private int _httpStatus;
     private string _errorMessage;
     private dynamic _responseObject;
     private object _errorCode;
     private bool _showMessage;


     public Response(Exception exception) {
         _error = true;
         _errorMessage = exception.Message;
			_showMessage = true;
         if (exception is WebException) {
             _httpStatus = (int)(exception as WebException).Status;
             if (_httpStatus == 2) {
                 _error = true;
                 _errorMessage = AppResources.NeedInternetConnection;
             } else {
                    _error = true;
					_errorMessage = AppResources.RemoteServerUnavailable;
                }
         }
         if (exception is Unauthorized) {
                _error = true;
				_errorMessage = AppResources.AuthorizationError;
             _errorCode = "0x00001";
         }
            if (exception is InvalidPhotoSize)
            {
                _error = true;
				_errorMessage = AppResources.PhotoSizeUnavailable;
            }
        }

     public Response() {
         
     }

     public bool Error {
         get {
             return _error;
         }
         set {
             _error = value;
         }
           
     }

     public int HttpStatus {
         get {
             return _httpStatus;
         }
         set {
             _httpStatus = value;
         }
     }

     public object ErrorCode {
         get {
             return _errorCode;
         }
         set {
             _errorCode = value;
         }
     }

     public string ErrorMessage {
         get {
             return _errorMessage;
         }
         set {
             _errorMessage = value;
         }
     }

     public dynamic ResponseObject {
         get {
             return _responseObject;
         }
         set {
             _responseObject = value;
         }
     }

        public bool ShowMessage
        {
            get
            {
                return _showMessage;
            }

            set
            {
                _showMessage = value;
            }
        }

        public void Dispose() {
         ResponseObject = null;
     }
 }
}
