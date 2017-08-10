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
  public  class UserRegistration:Request<bool> {
      private readonly string _target= "user/add/pushId";
      private Dictionary<string, string> _headers=new Dictionary<string, string>();
      private Dictionary<string, object> _bodyParameters;
      private Dictionary<string, object> _urlParameters=new Dictionary<string, object>();
      private object _content;
      private string _requestMethod="PUT";

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
#if DEBUG
                LogHelper.WriteLog(lException.Message, "RequestError", "GroupCreate");
                v.Add(k.OnExceptionMessage, lException.Message);
#endif
            }
            Dispose();
            return result;
        }

      public UserRegistration(string token,string pushId,string[] phones) {
            _headers.Add("Authorization", token);
			pushId = pushId.Replace(" ", string.Empty);
            _urlParameters.Add("push_id",pushId);
       ///     _urlParameters.Add("phones",string.Join(",",phones));
        }
  }
}
