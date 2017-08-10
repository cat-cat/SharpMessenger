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
	public class OnlineStatusGet : Request<bool>
	{
		private readonly string _target = "onlineStatus";
		private Dictionary<string, string> _headers = new Dictionary<string, string>();
		private Dictionary<string, object> _urlParameters = new Dictionary<string, object>();
		private string _requestMethod = "GET";

		~OnlineStatusGet()
		{
			// v.Remove(k.OnUpdateUserOnlineStatus);
		}

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

		public override Dictionary<string, object> BodyParameters { get; set; }

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

		public async override Task<bool> Object()
		{
			bool success = false;
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
					return false;
				}
				string s = Response.ResponseObject["success"].ToString();
				success = Convert.ToBoolean(s);
				if (success)
				{
					Dictionary<string, bool> lResponse = JsonConvert.DeserializeObject<Dictionary<string, bool>>(Response.ResponseObject["data"].ToString());
					v.Add(k.OnUpdateUserOnlineStatus, lResponse);
				}
				return success;

			}
			catch (Exception lException)
			{
#if DEBUG
				LogHelper.WriteLog(lException.Message, "RequestError", "OnlineStatusGet");
				v.Add(k.OnExceptionMessage, lException.Message);
#endif
			}
			Dispose();
			return success;
		}

		public OnlineStatusGet(string token, string ids, string thisUserId)
		{
			_headers.Add("Authorization", token);  
			_urlParameters.Add("ids", ids);
			_urlParameters.Add("client", thisUserId);
        }
	}
}

