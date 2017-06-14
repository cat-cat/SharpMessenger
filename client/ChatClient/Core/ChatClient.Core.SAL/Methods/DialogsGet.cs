using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.Common.Helpers;
using ChatClient.Core.Common.Interfaces;
using ChatClient.Core.Common.Models;
using ChatClient.Core.SAL.Adapters;

using Newtonsoft.Json;

using Xamarin.Forms;

namespace ChatClient.Core.SAL.Methods
{
  public  class DialogsGet:Request<Dictionary<string, object>>
    {
      private readonly string _target="chat";
      private Dictionary<string, string> _headers=new Dictionary<string, string>();
      private Dictionary<string, object> _bodyParameters;
      private Dictionary<string, object> _urlParameters=new Dictionary<string, object>();
      private string _requestMethod="GET";

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

      public override async Task<Dictionary<string, object>> Object() {
            Dictionary<string, object> lDictionary = null;
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
                        DependencyService.Get<IExceptionHandler>().ShowMessage(Response.ErrorMessage);
#endif
                    }
                    Dispose();
                    return null;
                }
                lDictionary = new Dictionary<string, object>() {
                                                                                      {"dialogs", JsonConvert.DeserializeObject<List<Conversation>>((string)Response.ResponseObject["conversations"]["docs"].ToString()) },
                                                                                      {"pageCount",Convert.ToInt32(Response.ResponseObject["conversations"]["pages"].ToString())},
                                                                                      {"ImagePrefix",Response.ResponseObject["userAvatarPrefix"].ToString()}
                                                                                  };
            }
            catch (Exception lException)
            {				
                LogHelper.WriteLog(lException.StackTrace, "RequestError", "DialogsGet");
                DependencyService.Get<IExceptionHandler>().ShowMessage(lException.Message);
            }
            Dispose();
            return lDictionary;
        }

      public DialogsGet(string token,int page,int count) {
            _headers.Add("Authorization", token);
            _urlParameters.Add("page",page);
            _urlParameters.Add("limit",count);

        }
  }
}
