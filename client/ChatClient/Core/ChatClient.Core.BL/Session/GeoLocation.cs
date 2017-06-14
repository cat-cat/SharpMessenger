#region

using System;
using System.Threading.Tasks;

using ChatClient.Core.Common.Helpers;

using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions.Abstractions;

#endregion

namespace ChatClient.Core.BL.Session {
    public class GeoLocation {
        #region Public Methods and Operators

        public async Task<Position> GetLocation() {
            try {
                if (await Permissions.ChekPermission(Permission.Location)) {
                    Position lPosition = await CrossGeolocator.Current.GetPositionAsync(10000);
                    return lPosition;
                } else
                    return null;
            } catch (Exception lException) {
                LogHelper.WriteException(lException);
            }
            return null;
        }

        #endregion
    }
}