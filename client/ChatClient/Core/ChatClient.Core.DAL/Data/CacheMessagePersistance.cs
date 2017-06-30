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
        public override Task<List<CacheMessage>> GetItemsAsync() {
            return database.Table<CacheMessage>().ToListAsync();
        }

        public override Task<List<CacheMessage>> GetItemsNotDoneAsync() {
            return database.QueryAsync<CacheMessage>("SELECT * FROM [CacheMessages] WHERE [Done] = 0");
        }

        public override Task<CacheMessage> GetItemAsync(object id) {
			return database.Table<CacheMessage>().Where(i => i.guid == (string)id).FirstOrDefaultAsync();
        }

        public override async Task<int> SaveItemAsync(CacheMessage item)
        {
            if (await GetItemAsync(item.Id) != null)
            {
                return await database.UpdateAsync(item);
            }
            else
            {
                return await database.InsertAsync(item);
            }
        }
		public override Task<int> UpdateItemAsync(Dictionary<string, object> d)
		{
			ChatMessage.Status foo = (ChatMessage.Status)Enum.ToObject(typeof(ChatMessage.Status), d["status"]);
			var m = new CacheMessage()
			{
				guid = (string)d["guid"],
				status = foo
			};

			return database.UpdateAsync(m);
		}
		public override Task<int> DeleteItemAsync(CacheMessage item)
		{
			throw new NotImplementedException();
		}
    }
}
