using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.Common.Helpers;
using ChatClient.Core.Common;
using ChatClient.Core.Common.Models;
using ChatClient.Core.SAL.Adapters;

using Newtonsoft.Json;

using Xamarin.Forms;

namespace ChatClient.Core.SAL.Methods
{
  public  class GetMembers:Request<Dictionary<string, object>>
    {
      private readonly string _target= "transactions/group";
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
                        v.Add(k.OnExceptionMessage, Response.ErrorMessage);
                    else
                    {
#if DEBUG
                        LogHelper.WriteLog(Response.ErrorMessage, "RequestError", "Authorization");
                        v.Add(k.OnExceptionMessage, Response.ErrorMessage);
#endif
                    }
                    Dispose();
                    return null;
                }
                lDictionary = new Dictionary<string, object>() {
                                                                                      {"users", new List<Member>() },
                                                                                      {"ImagePrefix",Response.ResponseObject["userAvatarPrefix"].ToString()}
                                                                                  };
                foreach (var lUserString in Response.ResponseObject["transactions"]) {
                    Member _user = JsonConvert.DeserializeObject<Member>(lUserString["_id"]["_creator"][0].ToString());
                    _user.TransactionsSum = Convert.ToInt32(lUserString["sum"].ToString());
                  (lDictionary["users"] as List<Member>).Add(_user);
                }
              
            }
            catch (Exception lException)
            {
#if DEBUG
                LogHelper.WriteLog(lException.Message, "RequestError", "GroupCreate");
                v.Add(k.OnExceptionMessage, lException.Message);
#endif
            }
            Dispose();
            return lDictionary;
        }

      public GetMembers(string token,string groupId) {
            _headers.Add("Authorization", token);
            _urlParameters.Add("_id",groupId);
        }
  }
}
