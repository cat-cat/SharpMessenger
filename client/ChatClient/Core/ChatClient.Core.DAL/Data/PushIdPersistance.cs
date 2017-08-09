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
       public override async Task<List<PushId>> GetItemsAsync() {
            var db = await database();
            return await db.Table<PushId>().ToListAsync();
        }

       public override async Task<List<PushId>> GetItemsNotDoneAsync() {
            var db = await database();
            return await db.QueryAsync<PushId>("SELECT * FROM [PushId] WHERE [Done] = 0");
        }

       public override async Task<PushId> GetItemAsync(object id) {
            var db = await database();
            return await db.Table<PushId>().FirstOrDefaultAsync();
        }

       public async override Task<int> SaveItemAsync(PushId item) {
            var db = await database();
            if (await GetItemAsync(item.Id) != null)
                return await db.UpdateAsync(item);
            else
                return await db.InsertAsync(item);
        }

       public override Task<int> DeleteItemAsync(PushId item) {
           throw new NotImplementedException();
       }

		public override Task<int> UpdateItemAsync(Dictionary<string, object> d)
		{
			throw new NotImplementedException();
		}
   }
}
