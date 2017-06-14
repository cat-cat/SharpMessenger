using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.Common.Models;
using ChatClient.Core.DAL.Data.Base;

namespace ChatClient.Core.DAL.Data
{
   public class PushIdPersistance:DatabasePersistance<PushId> {
       public override Task<List<PushId>> GetItemsAsync() {
            return database.Table<PushId>().ToListAsync();
        }

       public override Task<List<PushId>> GetItemsNotDoneAsync() {
            return database.QueryAsync<PushId>("SELECT * FROM [PushId] WHERE [Done] = 0");
        }

       public override Task<PushId> GetItemAsync(object id) {
            return database.Table<PushId>().FirstOrDefaultAsync();
        }

       public async override Task<int> SaveItemAsync(PushId item) {
            if (await GetItemAsync(item.Id) != null)
                return await database.UpdateAsync(item);
            else
                return await database.InsertAsync(item);
        }

       public override Task<int> DeleteItemAsync(PushId item) {
           throw new NotImplementedException();
       }
   }
}
