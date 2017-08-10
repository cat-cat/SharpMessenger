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
   public class GroupCreate:Request<string>
    {
       private readonly string _target="group";
       private Dictionary<string, string> _headers=new Dictionary<string, string>();
       private object _content=new object();
       private string _requestMethod="POST";
       private Dictionary<string, object> _bodyParameters=new Dictionary<string, object>();
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

       public async override Task<string> Object() {
            string lResponse = "";
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
                    return "";
                }
                string lresp = Response.ResponseObject["success"].ToString();
                bool success = Convert.ToBoolean(lresp);
                if (success)
                    lResponse = Response.ResponseObject["_id"].ToString();
                else 
                    v.Add(k.OnExceptionMessage, Response.ResponseObject["error"]["errors"]["_creator"]["message"].ToString());
            }
            catch (Exception lException)
            {
#if DEBUG
                LogHelper.WriteLog(lException.Message, "RequestError", "GroupCreate");
                v.Add(k.OnExceptionMessage, lException.Message);
#endif
            }
            Dispose();
            return lResponse;
        }

       public GroupCreate(string token,Group group) {
            _headers.Add("Authorization", token);
          _bodyParameters.Add("name", group.Name);
            _bodyParameters.Add("cost",group.Cost);
            _bodyParameters.Add("nominal", "999"); // TODO:999
            _bodyParameters.Add("statusMessage", group.OwnerStatus);
            if(!string.IsNullOrEmpty(group.Image))
                _bodyParameters.Add("adimage",group.Image);
        }
   }
}
