using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.Common.Helpers;
using ChatClient.Core.Common.Interfaces;
using ChatClient.Core.Common.Models;
using ChatClient.Core.Common.Models.Base;
using ChatClient.Core.SAL.Adapters;

using Newtonsoft.Json;

using Xamarin.Forms;

namespace ChatClient.Core.SAL.Methods
{
   public class GroupsGet:Request<Dictionary<string, object>>
    {
        
       private readonly string _target="group";
       private Dictionary<string, string> _headers=new Dictionary<string, string>();
       private object _content=new object();
       private string _requestMethod="GET";
       private Dictionary<string, object> _bodyParameters;
       private Dictionary<string, object> _urlParameters=new Dictionary<string, object>();

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

      

       public async override Task<Dictionary<string, object>> Object() {
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
                 lDictionary=new Dictionary<string, object>() {
                                                                                      {"groups", JsonConvert.DeserializeObject<List<Group>>((string)Response.ResponseObject["groups"]["docs"].ToString()) },
                                                                                      {"pageCount",Convert.ToInt32(Response.ResponseObject["groups"]["pages"].ToString())},
                                                                                      {"ImagePrefix",Response.ResponseObject["filePrefix"].ToString()}
                                                                                  };
            }
            catch (Exception lException)
            {
#if DEBUG
                LogHelper.WriteLog(lException.Message, "RequestError", "GroupCreate");
                DependencyService.Get<IExceptionHandler>().ShowMessage(lException.Message);
#endif
            }
            Dispose();
           return lDictionary;
       }

       public GroupsGet(string token,int pageNumber,int pageSize,string orderBy) {
            _headers.Add("Authorization", token);
            _urlParameters.Add("page",pageNumber);
            _urlParameters.Add("limit",pageSize);
           if (!string.IsNullOrEmpty(orderBy)) {
               _urlParameters.Add(orderBy,1);
           }

        }
   }
}
