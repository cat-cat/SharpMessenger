using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using ChatClient.Core.Common.Interfaces;
using ChatClient.Core.Common.Helpers;
using ChatClient.Core.Common.Resx;

using Xamarin.Forms;

namespace ChatClient.Core.UI.Pages
{
    public class pgRules : ContentPage
    {
        public pgRules()
        {
			var readHelper = new FileReadHelper();
			//string htmlContent = readHelper.ReadEmbeddedFile("PCL_g_for_rules.html");
			var browser = new WebView();
			var htmlSource = new HtmlWebViewSource();
			htmlSource.Html = AppResources.RulesText;
			htmlSource.BaseUrl = DependencyService.Get<IBaseUrl>().Get();
			//htmlSource.Html = "<html>" +
			//                  "<head>" +
			//                  "<title>Правила</title>" +
			//                  "<head>" +
			//                  "<body>" +
			//                 "<h1>Xamarin.Forms</h1>" +
			//                 "<img src= '" + DependencyService.Get<IFileHelper>().GetImageSrc("Default.png") + "'/>" +
			//                 "<p>Welcome to WebView.</p>" +
			//                  "<a href='https://www.google.com.ua/'>testLink</a>" +
			//                 "</body>" +
			//                  "</html>";
			browser.Source = htmlSource;


			Content = browser;

        }
    }
}
