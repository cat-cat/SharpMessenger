#region

using System.Collections.Generic;
using System.Threading.Tasks;

using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

#endregion

namespace ChatClient.Core.BL.Session {
    public class Permissions {
        #region Public Methods and Operators

        public static async Task<bool> ChekPermission(Permission permission) {
            PermissionStatus lPermissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
            if (lPermissionStatus != PermissionStatus.Granted) {
                Dictionary<Permission, PermissionStatus> results =
                    await CrossPermissions.Current.RequestPermissionsAsync(permission);

                lPermissionStatus = results[permission];
            }
            if (lPermissionStatus == PermissionStatus.Granted)
                return true;
            else
                return false;
        }

        #endregion
    }
}