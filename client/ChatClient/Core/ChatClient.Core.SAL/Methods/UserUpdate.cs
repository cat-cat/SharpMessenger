using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.Common.Helpers;
using ChatClient.Core.Common;
using ChatClient.Core.SAL.Adapters;

using Xamarin.Forms;

namespace ChatClient.Core.SAL.Methods
{
  public  class UserUpdate:Request<bool>
    {
      private readonly string _target="user";
      private Dictionary<string, string> _headers=new Dictionary<string, string>();
      private Dictionary<string, object> _bodyParameters=new Dictionary<string, object>();
      private Dictionary<string, object> _urlParameters=new Dictionary<string, object>();
      private object _content;
      private string _requestMethod = "PUT";

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

      public override async Task<bool> Object() {
            bool result = false;
            try
            {
                Response = await Execute();
                if (Response.Error)
                {
                    if (Response.ShowMessage)
                        v.Add(k.OnExceptionMessage, Response.ErrorMessage);
                    else
                    {
#if DEBUG
                        LogHelper.WriteLog(Response.ErrorMessage, "RequestError", "Authorization");
#endif
                    }
                    Dispose();
                    return false;
                }
                string lresp = Response.ResponseObject["success"].ToString();
                result = Convert.ToBoolean(lresp);
            }
            catch (Exception lException)
            {
                LogHelper.WriteLog(lException.Message, "RequestError", "UserUpdate");
                v.Add(k.OnExceptionMessage, lException.Message);
            }
            Dispose();
            return result;
        }

      public UserUpdate(string token, int? balance = null, string name = "",string photo="") {
            _headers.Add("Authorization", token);
            if(balance!=null)
                _bodyParameters.Add("balance",balance);
            if(!string.IsNullOrEmpty(name))
                _bodyParameters.Add("name",name);
            if (!string.IsNullOrEmpty(photo))
                _bodyParameters.Add("userimage", photo);
        }
  }
}
