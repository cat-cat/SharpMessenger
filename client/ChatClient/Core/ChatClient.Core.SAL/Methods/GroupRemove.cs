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
                        v.Add(k.OnExceptionMessage, Response.ErrorMessage);
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
                    v.Add(k.OnExceptionMessage, "Group removed");
                }
                else {
                    if (Response.ResponseObject["message"].ToString() == "group is ending") {
                        v.Add(k.OnExceptionMessage, Response.ResponseObject["message"].ToString());
                    }
                    else if (Response.ResponseObject["message"].ToString() == "remove this group?" && !_urlParameters.ContainsKey("delete")) {
						v.Add(k.ConfirmGroupRemoval, new Dictionary<string, string>() { { "message", Response.ResponseObject["message"].ToString() }, {"groupid", _urlParameters["id"].ToString() } });
                    }
                    else if(Response.ResponseObject["message"].ToString() == "bets available")
                        v.Add(k.OnExceptionMessage, Response.ResponseObject["message"].ToString());
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
            return response;
        }

		static void OnEvent(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			var newItem = (KeyValuePair<k, object>)e.NewItems[0];
			if (newItem.Key == k.OnConfirmGroupRemoval)
			{
				var d = (Dictionary<string, string>) newItem.Value;
				new GroupRemove(d["token"], d["groupid"], true /* delete group */).Object();
			}
		}

		public GroupRemove(string token, string groupId, bool delete = false)
        {
			_headers.Add("Authorization", token);
            _urlParameters.Add("id", groupId);
			if (delete)
				_urlParameters.Add("delete", true);
        }

		static GroupRemove()
		{
			v.h(new k[] { k.OnConfirmGroupRemoval }, OnEvent);
		}
    }
}
