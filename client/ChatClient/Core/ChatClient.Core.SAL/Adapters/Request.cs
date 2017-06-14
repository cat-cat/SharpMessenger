using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;

using Newtonsoft.Json;
using System.Threading.Tasks;

using ChatClient.Core.Common;
using ChatClient.Core.Common.Interfaces;
using ChatClient.Core.Common.Models;

using Xamarin.Forms;
using ChatClient.Core.DAL.Data.Base;

namespace ChatClient.Core.SAL.Adapters
{
	public abstract class Request<TTYPE> : IDisposable
	{

		public abstract string Target { get; }

		public abstract Dictionary<string, string> Headers { get; set; }

		public abstract Dictionary<string, object> BodyParameters { get; set; }
		public abstract Dictionary<string, object> UrlParameters { get; set; }


		public abstract object Content { get; set; }

		public abstract Response Response { get; set; }

		public abstract string RequestMethod { get; set; }

		private string BuildUrl(string fullBaseUrl)
		{
			string lUrl = string.Format("{0}/{1}/",fullBaseUrl, Target);
            if (UrlParameters != null && UrlParameters.Count > 0)
                lUrl += "?" +  String.Join("&", UrlParameters.Select(o=>o.Key+ "="+  o.Value));

			Debug.WriteLine("\n ~~~~~~~~~~~~~~~~ Request url: " + lUrl + '\n');

            return lUrl;
        }

        protected async Task<Response> Execute()
        {
            try
            {
                HttpClient lHttpClient = new HttpClient();
                MultipartFormDataContent lContent=null;//= new MultipartFormDataContent();
                if (Headers != null && Headers.Count > 0)
                {
                    foreach (KeyValuePair<string, string> lHeader in Headers) {
                        switch (lHeader.Key) {
                            case "Authorization":
                                lHttpClient.DefaultRequestHeaders.Add(lHeader.Key,String.Format("JWT {0}", lHeader.Value)); ;
                                continue;    
                        }
                    }
                }
                if (BodyParameters != null && BodyParameters.Count > 0) {
                    lContent=new MultipartFormDataContent();
                    byte[] Bytes;
                    ByteArrayContent lFileContent;
                    foreach (KeyValuePair<string, object> lParameter in BodyParameters) {
                        switch (lParameter.Key) {
                            case "adimage":
                                if (string.IsNullOrEmpty((string)lParameter.Value))
                                    continue;
                                Bytes = DependencyService.Get<IFileHelper>().ReadAllBytes(lParameter.Value.ToString());
                                if(Bytes.Count()>= 10000000)
                                    throw new InvalidPhotoSize();
                               lFileContent = new ByteArrayContent(Bytes);
                                lFileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                                {
                                    FileName = DependencyService.Get<IFileHelper>().FileName(lParameter.Value.ToString()),
                                    Name = lParameter.Key
                                };
                                lFileContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                                lContent.Add(lFileContent);
                                continue;
                            case "userimage":
                                if (string.IsNullOrEmpty((string)lParameter.Value))
                                    continue;
                               Bytes = DependencyService.Get<IFileHelper>().ReadAllBytes(lParameter.Value.ToString());
                                if (Bytes.Count() >= 10000000)
                                    throw new InvalidPhotoSize();
                                lFileContent = new ByteArrayContent(Bytes);
                                lFileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                                {
                                    FileName = DependencyService.Get<IFileHelper>().FileName(lParameter.Value.ToString()),
                                    Name = lParameter.Key
                                };
                                lFileContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                                lContent.Add(lFileContent);
                                string a =await lContent.ReadAsStringAsync();
                                continue;
                        }
                        StringContent lStringContent = new StringContent(lParameter.Value.ToString());
                        lContent.Add(lStringContent, lParameter.Key);
                    }
                }

                Response lResponse;
                lHttpClient.Timeout = new TimeSpan(0,0,0,15);
                HttpResponseMessage lRequestResponse=null;

#if DEBUG
				string fullBaseUrl = MyConstants.debugURL + ":" + MyConstants.debugPort + "/api";
#else
				Parameters savedParams = await PersisataceService.GetParameterPersistance().GetItemAsync(1);
				string fullBaseUrl;
				if(savedParams == null)
					fullBaseUrl = MyConstants.baseURL + ":" + MyConstants.port + "/api";
				else
					fullBaseUrl = savedParams.BaseUrl + ":" + MyConstants.port + "/api";
#endif 

				if (RequestMethod == "GET")
                {
					lRequestResponse = await lHttpClient.GetAsync(BuildUrl(fullBaseUrl));
                }
                else if(RequestMethod=="POST") {
                 //   string lcontent = await lContent.ReadAsStringAsync();
					lRequestResponse = await lHttpClient.PostAsync(BuildUrl(fullBaseUrl), lContent);
                   
                } else if(RequestMethod=="PUT") {
					lRequestResponse =  await lHttpClient.PutAsync(BuildUrl(fullBaseUrl),lContent);
                }
                string lResponseString = lRequestResponse.Content.ReadAsStringAsync().Result;
                if (lResponseString == "Unauthorized") {
                    throw new Unauthorized();

                    //   User lUser=  DependencyService.Get<IExceptionHandler>().GreateNewUser().Result;
                    //    if(Headers != null && Headers.ContainsKey("Authorization"))
                    //     Headers["Authorization"]= lUser.Token;
                    //   return await Execute();
                }
                lResponse = new Response() { HttpStatus = (int)lRequestResponse.StatusCode, Error = false, ErrorCode = null, ErrorMessage = null, ResponseObject = JsonConvert.DeserializeObject(lResponseString) };
                lContent?.Dispose();
                lContent = null;
               lHttpClient.Dispose();
                lHttpClient = null;
                lRequestResponse.Dispose();
                lRequestResponse = null;
                return lResponse;

            }
            catch (WebException lWebException)
            {
                return new Response(lWebException);
            }
            catch (Exception lException)
            {
                return new Response(lException);
            }
        }

        public abstract Task<TTYPE> Object();  
        public void Dispose() {
           Headers=null;
            BodyParameters=null;
            UrlParameters = null;
            Response = null;
        }
    }


}
