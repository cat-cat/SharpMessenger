using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.Common.Models;
using ChatClient.Core.SAL.Adapters;

namespace ChatClient.Core.SAL.Methods
{
    public class UsersGet:Request<List<User>>
    {
        private readonly string _target="user/all";
        private Dictionary<string, string> _headers=new Dictionary<string, string>();
        private Dictionary<string, object> _bodyParameters;
        private Dictionary<string, object> _urlParameters;
        private string _requestMethod="GET";
        private object _content;


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

        public override async Task<List<User>> Object() {
            Response lResponse = await Execute();
            //TODO GetALL USers using only for testing
            return new List<User>();
        }

        public UsersGet(string token) {
            _headers.Add("Authorization", token);
        }
    }
}
