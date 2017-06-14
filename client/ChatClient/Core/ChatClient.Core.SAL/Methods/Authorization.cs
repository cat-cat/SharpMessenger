using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.Common.Resx;
using ChatClient.Core.Common.Helpers;
using ChatClient.Core.Common.Interfaces;
using ChatClient.Core.SAL.Adapters;

using Xamarin.Forms;

namespace ChatClient.Core.SAL.Methods
{
   public class Authorization :Request<string> {
       private string _target= "user";
       private Dictionary<string, string> _headers=new Dictionary<string, string>();
       private object _content=new object();
       private string _requestMethod="POST";
		private Dictionary<string, object> _bodyParameters=new Dictionary<string, object>() { {"name", AppResources.NewMember} };
       private Dictionary<string, object> _urlParameters;

       public override string Target {
            get {
                return _target;
            }
            
        }

        public override Dictionary<string, string> Headers {
            get {
                return _headers;
            }
            set {
                _headers = value;
            }
        }

       public override Dictionary<string, object> BodyParameters {
           get {
               return _bodyParameters;
           }
           set {
               _bodyParameters = value;
           }
       }

       public override Dictionary<string, object> UrlParameters {
           get {
               return _urlParameters;
           }
           set {
               _urlParameters = value;
           }
       }

       public override object Content {
            get {
                return _content;
            }
            set {
                _content = value;
            }
        }

       public override Response Response { get; set; }

       public override string RequestMethod {
            get {
                return _requestMethod;
            }
            set {
                _requestMethod = value;
            }
        }

        public override async Task<string> Object()
        {
            Response =await Execute();
            if (Response.Error) {
                if(Response.ShowMessage)
					DependencyService.Get<IExceptionHandler>().ShowMessage(Response.ErrorMessage);
                else {
#if DEBUG
                    LogHelper.WriteLog(Response.ErrorMessage,"RequestError", "Authorization");
#endif
                }
                Dispose();
                return null;
            }
            if(Response.ShowMessage)
                DependencyService.Get<IExceptionHandler>().ShowMessage(Response.ErrorMessage);
            string lToken= Response.ResponseObject["token"].ToString();
            Dispose();
            return lToken;
        }
    }
}
