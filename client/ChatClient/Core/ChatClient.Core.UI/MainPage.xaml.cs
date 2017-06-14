
using Xamarin.Forms;
using ChatClient.Core.Common.Resx;

namespace ChatClient.Core.UI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
			BindingContext = this;
            InitializeComponent();
            OnLoad();
        }


		public string PageTitle
		{
			get
			{
				return AppResources.MainPage;
			}
		}
      

        private async void OnLoad()
        {
            //try
            //{
            //    User lUser = await BL.Session.Authorization.GetUser();
            //    UserGet lUserGet = new UserGet(lUser.Token);
            //    Response lResponse = await lUserGet.Execute();
            //    if (lResponse.Error)
            //    {
            //        LogHelper.WriteLog("Can't create new user", "CreateNewUse", lResponse.ErrorMessage);
            //    }
            //}
            //catch (Exception lException)
            //{
            //    if (lException is NeedConnectionToNetwork)
            //        await DisplayAlert("Connection Denied", "Can not continue, try again.", "OK");
            //    else if (lException is BadConnection)
            //        await DisplayAlert("Bad connection", String.Format("{0, {1}}", lException.Message, "try again."), "OK");
            //}

          //  Position lPosition = await new GeoLocation().GetLocation();
         //   if (lPosition == null)
          //      await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");

    //        LabelGeolocation.Text = "Lat: " + lPosition.Latitude + " Long: " + lPosition.Longitude;

        }

    }
}