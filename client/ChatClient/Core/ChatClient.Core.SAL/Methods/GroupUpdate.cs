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
  public  class GroupUpdate:Request<bool>
        {
      private readonly string _target= "group";
      private Dictionary<string, string> _headers=new Dictionary<string, string>();
      private Dictionary<string, object> _bodyParameters=new Dictionary<string, object>();
      private Dictionary<string, object> _urlParameters;
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
          bool response = false;
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
                if (Response.ResponseObject["ok"].ToString() == "1")
                    response = true;
            }
            catch (Exception lException)
            {
#if DEBUG
                LogHelper.WriteLog(lException.Message, "RequestError", "GroupCreate");
                DependencyService.Get<IExceptionHandler>().ShowMessage(lException.Message);
#endif
            }
            Dispose();
          return response;
      }

      public GroupUpdate(string token, Group group) {
            _headers.Add("Authorization", token);
            _bodyParameters.Add("id", group.Id);
            _bodyParameters.Add("statusMessage", group.OwnerStatus);
        }
  }
}
