using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatClient.Core.Common;
using ChatClient.Core.Common.Helpers;
using ChatClient.Core.Common.Interfaces;
using ChatClient.Core.DAL.Data.Base;
using ChatClient.Core.Common.Models;
using ChatClient.Core.SAL.Adapters;

using Newtonsoft.Json;

using Xamarin.Forms;

namespace ChatClient.Core.SAL.Methods
{
	public class MessageStatusGet : Request<bool>
	{
		private readonly string _target = "messages/messageStatus";
		private Dictionary<string, string> _headers = new Dictionary<string, string>();
		private Dictionary<string, object> _urlParameters = new Dictionary<string, object>();
		private string _requestMethod = "GET";

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

		async Task<List<object> > findLocal()
		{
			CacheMessage c = await PersisataceService.GetCacheMessagePersistance().GetItemAsync(_urlParameters["guids"]);
			if (c != null)
				return new List<object>() { new Dictionary<string, object>() { { "guid", c.guid }, { "status", c.status } } };
				
			return new List<object>();
		}

		public async override Task<bool> Object()
		{
			bool success = true;

			// TODO: First search locally
			//List<object> locList = await findLocal();
			//if (locList.Count > 0)
			//{
			//	var d = (Dictionary<string, object>)locList[0];
			//	var status = (ChatMessage.Status) d["status"];

			//	if (status == ChatMessage.Status.Read)
			//	{
			//		v.Add(k.OnMessageSendProgress, d);
			//		Dispose();
			//		return success;
			//	}
			//}

			// Second try to get message from server
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
					success = false;
				}
				else
				{
					string s = Response.ResponseObject["success"].ToString();
					success = Convert.ToBoolean(s);
					if (success)
					{
						var r = Response.ResponseObject["data"].ToString();
						if(r.Contains("status")) // response not empty
						{
							Dictionary<string, object> retVal = JsonConvert.DeserializeObject<Dictionary<string, object>>(r);

							// convert status from web to internal type
							retVal["status"] = (ChatMessage.Status)Enum.ToObject(typeof(ChatMessage.Status), retVal["status"]);

							await PersisataceService.GetCacheMessagePersistance().UpdateItemAsync(retVal);
							v.Add(k.OnMessageSendProgress, retVal);
						}
					}
				}
			}
			catch (Exception lException)
			{
#if DEBUG
				LogHelper.WriteLog(lException.Message, "RequestError", "MessageStatusGet");
				DependencyService.Get<IExceptionHandler>().ShowMessage(lException.Message);
#endif
			}
			Dispose();
			return success;
		}

		public MessageStatusGet(string token, ChatMessage m)
		{
			_headers.Add("Authorization", token);

			if (m.status == ChatMessage.Status.PendingDelete || m.messageEdited)
			{
				if (!string.IsNullOrEmpty(m.Room)) // group message
					_urlParameters.Add("room", m.Room);
				else // private message
					_urlParameters.Add("client", m.Opponent.Id);
			}

			if (m.status == ChatMessage.Status.PendingDelete)
			{
				_urlParameters.Add("deleted", 1);
			}
			else if (m.Id != m.Author.Id)
				_urlParameters.Add("read", 1);

			if (m.messageEdited)
			{
				_urlParameters.Add("edited", 1);
				_urlParameters.Add("message", m.Message);
			}
			
			_urlParameters.Add("guids", m.guid);
        }
	}
}

