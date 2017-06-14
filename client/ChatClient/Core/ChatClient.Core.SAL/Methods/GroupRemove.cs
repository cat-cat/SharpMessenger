using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.Common.Helpers;
using ChatClient.Core.Common.Interfaces;
using ChatClient.Core.SAL.Adapters;

using Xamarin.Forms;

namespace ChatClient.Core.SAL.Methods
{
    public class GroupRemove : Request<bool>
    {
        private readonly string _target = "group/byId";
        private Dictionary<string, string> _headers = new Dictionary<string, string>();
        private Dictionary<string, object> _bodyParameters;
        private Dictionary<string, object> _urlParameters = new Dictionary<string, object>();
        private string _requestMethod = "PUT";

        public override string Target
        {
            get
            {
                return _target;
            }
        }

        public override Dictionary<string, string> Headers
        {
            get
            {
                return _headers;
            }
            set
            {
                _headers = value;
            }
        }

        public override Dictionary<string, object> BodyParameters
        {
            get
            {
                return _bodyParameters;
            }
            set
            {
                _bodyParameters = value;
            }
        }

        public override Dictionary<string, object> UrlParameters
        {
            get
            {
                return _urlParameters;
            }
            set
            {
                _urlParameters = value;
            }
        }

        public override object Content { get; set; }

        public override Response Response { get; set; }

        public override string RequestMethod
        {
            get
            {
                return _requestMethod;
            }
            set
            {
                _requestMethod = value;
            }
        }

        public override async Task<bool> Object()
        {
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
                    return response;
                }
                string lresp = Response.ResponseObject["success"].ToString();
                response = Convert.ToBoolean(lresp);
                if (response) {
                    DependencyService.Get<IExceptionHandler>().ShowMessage("Group removed");
                }
                else {
                    if (Response.ResponseObject["message"].ToString() == "group is ending") {
                        DependencyService.Get<IExceptionHandler>().ShowMessage(Response.ResponseObject["message"].ToString());
                    }
                    else if (Response.ResponseObject["message"].ToString() == "remove this group?" && !_urlParameters.ContainsKey("delete")) {
                        if (await DependencyService.Get<IExceptionHandler>().YesNoQuestion(Response.ResponseObject["message"].ToString()))
                        {
                            _urlParameters.Add("delete", 1);
                            return await Object();
                        }
                        else response = true; ;
                    }
                    else if(Response.ResponseObject["message"].ToString() == "bets available")
                        DependencyService.Get<IExceptionHandler>().ShowMessage(Response.ResponseObject["message"].ToString());
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
            return response;
        }

        public GroupRemove(string token, string groupId)
        {
            _headers.Add("Authorization", token);
            _urlParameters.Add("id", groupId);
        }
    }
}
