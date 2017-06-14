using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.Common;
using ChatClient.Core.Common.Helpers;
using ChatClient.Core.Common.Interfaces;
using ChatClient.Core.Common.Models;
using ChatClient.Core.Common.Models.Base;
using ChatClient.Core.SAL.Adapters;

using Newtonsoft.Json;

using Xamarin.Forms;

namespace ChatClient.Core.SAL.Methods
{
  public  class UserGet:Request<User> {
      private readonly string _target="user";
      private Dictionary<string, string> _headers=new Dictionary<string, string>();
      private object _content=new object();
      private string _requestMethod="GET";
      private Dictionary<string, object> _bodyParameters;
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

      public async override Task<User> Object() {
          User lUser = null;
          try {

              Response = await Execute();
              if (Response.Error) {
                  if (Response.ErrorCode == "0x00001")
                      throw new Unauthorized();
                  if (Response.ShowMessage)
                      DependencyService.Get<IExceptionHandler>().ShowMessage(Response.ErrorMessage);
                  else {
#if DEBUG
                      LogHelper.WriteLog(Response.ErrorMessage, "RequestError", "Authorization");
                      DependencyService.Get<IExceptionHandler>().ShowMessage(Response.ErrorMessage);
#endif
                  }
                  Dispose();
                  return null;
              }
              if (Response.ShowMessage)
                  DependencyService.Get<IExceptionHandler>().ShowMessage(Response.ErrorMessage);
              lUser = JsonConvert.DeserializeObject<User>(Response.ResponseObject["user"].ToString());
              if (lUser.Photo != "profile_avatar.png")
                  lUser.Photo = await
                      DependencyService.Get<IFileHelper>()
                          .PhotoCache(Response.ResponseObject["userAvatarPrefix"].ToString(), lUser.Photo, ImageType.Users);

          }
            catch (Unauthorized) { throw new Unauthorized(); }
            catch (Exception lException) {
#if DEBUG
                LogHelper.WriteLog(lException.Message, "RequestError", "UserGet");
                DependencyService.Get<IExceptionHandler>().ShowMessage(lException.Message);
#endif
            }
            Dispose();
            return lUser;
        }

      public UserGet(string token) {
            _headers.Add("Authorization", token);
      }
  }
}
