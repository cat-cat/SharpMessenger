using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.Common.Helpers;
using ChatClient.Core.Common.Interfaces;
using ChatClient.Core.Common.Models;
using ChatClient.Core.SAL.Adapters;

using Xamarin.Forms;

namespace ChatClient.Core.SAL.Methods
{
  public  class RestoreAccount:Request<bool>
    {
      private readonly string _target="mail";
      private Dictionary<string, string> _headers=new Dictionary<string, string>();
      private Dictionary<string, object> _urlParameters=new Dictionary<string, object>();
      private string _requestMethod="POST";

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

      public override Dictionary<string, object> BodyParameters { get; set; }

      public override Dictionary<string, object> UrlParameters {
          get {
              return _urlParameters;
          }
          set {
              _urlParameters = value;
          }
      }

      public override object Content { get; set; }

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
                        DependencyService.Get<IExceptionHandler>().ShowMessage(Response.ErrorMessage);
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

				if (!Convert.ToBoolean(result)) {
                    DependencyService.Get<IExceptionHandler>().ShowMessage(Response.ResponseObject["error"].ToString());
                }

            }
            catch (Exception lException)
            {
#if DEBUG
                LogHelper.WriteLog(lException.Message, "RequestError", "GroupCreate");
                DependencyService.Get<IExceptionHandler>().ShowMessage(lException.Message);
#endif
            }
            Dispose();
            return result;
        }

      public RestoreAccount(string token,string email) {
            _headers.Add("Authorization", token);
            _urlParameters.Add("email", email);
        }
  }
}
