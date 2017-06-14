namespace ChatClient.Core.Common.Models
{
	public class OAuthSettings
	{
		public OAuthSettings(
			string clientId,
			string scope,
			string authorizeUrl,
			string redirectUrl, 
            OAuthType oAuthType)
		{
			ClientId = clientId;
			Scope = scope;
			AuthorizeUrl = authorizeUrl;
			RedirectUrl = redirectUrl;
		    this.OAuthType = oAuthType;
		}

	    public OAuthSettings(string consumerKey,string secretKey,string requestTokenUrl,string authorizeUrl,string accessTokenUrl,string redirectUrl,OAuthType oAuthType) {
	        ConsumerKey = consumerKey;
	        RedirectUrl = redirectUrl;
	        SecretKey = secretKey;
	        RequestTokenUrl = requestTokenUrl;
	        AuthorizeUrl = authorizeUrl;
	        AccessTokenUrl = accessTokenUrl;
            this.OAuthType = oAuthType;

        }


	    public OAuthType OAuthType { get; set; }
        public string ConsumerKey { get; private set; }

        public string ClientId {get; private set;}

	    public string SecretKey { get; set; }

	    public string RequestTokenUrl { get; set; }

	    public string AccessTokenUrl { get; set; }


	    public string Scope {get; private set;}
		public string AuthorizeUrl {get; private set;}
		public string RedirectUrl {get; private set;}
	}
}

