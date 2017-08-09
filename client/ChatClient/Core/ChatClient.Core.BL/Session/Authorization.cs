#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ChatClient.Core.Common;
using ChatClient.Core.Common.Helpers;
using ChatClient.Core.Common.Interfaces;
using ChatClient.Core.Common.Models;
using ChatClient.Core.Common.Models.Base;
using ChatClient.Core.DAL.Data.Base;
using ChatClient.Core.SAL.Adapters;
using ChatClient.Core.SAL.Methods;

using Newtonsoft.Json;

using Xamarin.Forms;

#endregion

namespace ChatClient.Core.BL.Session {
    public class Authorization {
        #region Static & Const

        private static User _user;
        private static bool IsBusy;

        #endregion

        #region Public Methods and Operators

        public static async void Save(User user) {
            _user = user;
            await PersisataceService.GetUserPersistanse().SaveItemAsync(user);
        }

        private static int _attemps = 0;
        public static async Task<User> CreateNewUser() {
            _attemps++;
            if (_attemps > 5)
                _attemps = 0;
            if (_attemps > 1)
                return null;
            string ltoken = "";
         //   Response lResponse =
            //if (lResponse.Error) {
            //    LogHelper.WriteLog("Can't create new user", "CreateNewUse", lResponse.ErrorMessage);
            //    return null;
            //} else {
                ltoken = await new SAL.Methods.Authorization().Object();

            if (string.IsNullOrEmpty(ltoken))
                return null;
            foreach (PushId lPushId in await PersisataceService.GetPushIdPersistance().GetItemsAsync()) {
                lPushId.IsSended = false;
              await  PersisataceService.GetPushIdPersistance().SaveItemAsync(lPushId);
            }
          //  lResponse.Dispose();
           //     lResponse = null;
                return await UpdateFromServer(ltoken);
            
        }

        public static async Task<User> GetUser(bool fromServer = false) {
            if (_user != null) {
                if (fromServer)
                    _user = await UpdateFromServer(_user.Token);
                return _user;
            }
            while (IsBusy)
                await Task.Delay(300);
            if (!IsBusy) {
                IsBusy = true;
                var up = PersisataceService.GetUserPersistanse();
                List<User> lUsers = await up.GetItemsAsync();
                if (lUsers.Count == 0)
                    _user = await CreateNewUser();
                else {
                   // _user = lUsers[lUsers.Count - 1];

                    try {
                        _user = await UpdateFromServer(lUsers[lUsers.Count - 1].Token);
                    } catch (Unauthorized) {
                        _user = await CreateNewUser();
                    }
                   
                    lUsers = null;
					if(_user != null) // probably was unavailable server
	                    if (string.IsNullOrEmpty(_user.Token))
	                        _user = await CreateNewUser();
                }
                IsBusy = false;
            }
            return _user;
        }

        #endregion

        #region Private Methods and Operators

        private static async Task<User> UpdateFromServer(string token) {
          //  Response lResponse;
            User lUser= await new UserGet(token).Object(); ;
           // UserGet lUserGet = new UserGet(token);
         //   lResponse = await lUserGet.Execute();
          //  if (lResponse.Error) {
         ///       LogHelper.WriteLog("Can't create new user", "CreateNewUse", lResponse.ErrorMessage);
        //        return null;
        //    } else {
            if (lUser == null)
                return null;
                lUser.Token = token;
                    // lUser.Photo = lResponse.ResponseObject["userAvatarPrefix"].ToString()+lUser.Photo;
         //   }
            await PersisataceService.GetUserPersistanse().SaveItemAsync(lUser);
         //   lUserGet.Dispose();
          //  lUserGet = null;
         //   lResponse?.Dispose();
         //   lResponse = null;
            return lUser;
        }

        #endregion
    }
}