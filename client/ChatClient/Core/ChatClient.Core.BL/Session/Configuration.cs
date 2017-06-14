using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.Common.Models;
using ChatClient.Core.DAL.Data.Base;

namespace ChatClient.Core.BL.Session
{
  public  class Configuration
    {
		public static async Task<int> UpdateConfiguration(Parameters parameters, bool isDefault = false) {
            if (isDefault) {
				var currentConfig = await PersisataceService.GetParameterPersistance().GetItemAsync(1);
				if (currentConfig == null)
					return await PersisataceService.GetParameterPersistance().SaveItemAsync(parameters);
				else
					return 1; // if startup and settings already saved, don't touch it
            } else {
				return await PersisataceService.GetParameterPersistance().SaveItemAsync(parameters);
            }
        }
    }
}
