using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.Common.Models;
using ChatClient.Core.DAL.Data.Base;

namespace ChatClient.Core.DAL.Data
{
   public class CacheMessagePersistance:DatabasePersistance<CacheMessage>
    {
        public override async Task<List<CacheMessage>> GetItemsAsync() {
            var db = await database();
            return await db.Table<CacheMessage>().ToListAsync();
        }

        public override async Task<List<CacheMessage>> GetItemsNotDoneAsync() {
            var db = await database();
            return await db.QueryAsync<CacheMessage>("SELECT * FROM [CacheMessages] WHERE [Done] = 0");
        }

        public override async Task<CacheMessage> GetItemAsync(object id) {
            var db = await database();
            return await db.Table<CacheMessage>().Where(i => i.guid == (string)id).FirstOrDefaultAsync();
        }

        public override async Task<int> SaveItemAsync(CacheMessage item)
        {
            var db = await database();
            if (await GetItemAsync(item.Id) != null)
            {
                return await db.UpdateAsync(item);
            }
            else
            {
                return await db.InsertAsync(item);
            }
        }
		public override async Task<int> UpdateItemAsync(Dictionary<string, object> d)
		{
			var m = new CacheMessage()
			{
				guid = (string)d["guid"],
				status = (ChatMessage.Status) d["status"]
			};
            var db = await database();
            return await db.UpdateAsync(m);
		}
		public override Task<int> DeleteItemAsync(CacheMessage item)
		{
			throw new NotImplementedException();
		}
    }
}
