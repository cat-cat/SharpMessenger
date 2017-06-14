using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Core.DAL.Data.Base
{
  public  static class PersisataceService {
      private static UserPersistanse _UserPersistanse;
      private static PushIdPersistance _pushIdPersistance;
      private static ParameterPersistance _parameterPersistance;
      private static CacheMessagePersistance _cacheMessagePersistance;
      public static UserPersistanse GetUserPersistanse() {
          return _UserPersistanse ?? (_UserPersistanse = new UserPersistanse());
      }

      public static PushIdPersistance GetPushIdPersistance() {
          return _pushIdPersistance ?? (_pushIdPersistance = new PushIdPersistance());
      }

      public static ParameterPersistance GetParameterPersistance() {
          return _parameterPersistance ?? (_parameterPersistance = new ParameterPersistance());
      }

      public static CacheMessagePersistance GetCacheMessagePersistance() {
          return _cacheMessagePersistance ?? (_cacheMessagePersistance = new CacheMessagePersistance());
      }
  }
}
