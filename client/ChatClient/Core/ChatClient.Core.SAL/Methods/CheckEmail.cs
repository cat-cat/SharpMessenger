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
   public class CheckEmail:Request<User>
    {
       private readonly string _target="mail";
       private Dictionary<string, string> _headers=new Dictionary<string, string>();
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

       public async override Task<User> Object() {
           User lUser = null;
            try {
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
                return null;
            }
                string lresp = Response.ResponseObject["success"].ToString();
                if (Convert.ToBoolean(lresp)) {
                    if (ObjectHelper.IsPropertyExist(Response.ResponseObject, "user"))
                        lUser = JsonConvert.DeserializeObject<User>(Response.ResponseObject["user"].ToString());
                } else {
                    v.Add(k.OnExceptionMessage, Response.ResponseObject["error"].ToString());
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
            return lUser;
        }

       public CheckEmail(string token,string email,string checkCode) {
            _headers.Add("Authorization", token);
            _urlParameters.Add("email", email);
            _urlParameters.Add("check_code", checkCode);
        }
   }
}
