using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.Common.Models;
using ChatClient.Core.DAL.Data.Base;

using SQLite;

namespace ChatClient.Core.DAL.Data
{
   public class UserPersistanse:DatabasePersistance<User> {
      
     
        public override async Task<List<User>> GetItemsAsync()
        {
            var db = await database();
            return await db.Table<User>().ToListAsync();
        }

        public override async Task<List<User>> GetItemsNotDoneAsync()
        {
            var db = await database();
            return await db.QueryAsync<User>("SELECT * FROM [User] WHERE [Done] = 0");
        }

       public override async Task<User> GetItemAsync(object id) {
            var db = await database();
            return await db.Table<User>().Where(i => i.Id == (string)id).FirstOrDefaultAsync();
        }

       

        public  override async Task<int> SaveItemAsync(User item) {
            var db = await database();
            if (await GetItemAsync(item.Id) != null)
                return await db.UpdateAsync(item);
            else
                return await db.InsertAsync(item);
        }

        public override async Task<int> DeleteItemAsync(User item)
        {
            var db = await database();
            return await db.DeleteAsync(item);
        }

		public override Task<int> UpdateItemAsync(Dictionary<string, object> d)
		{
			throw new NotImplementedException();
		}
      
   }
}
