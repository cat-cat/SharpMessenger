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
      
     
        public override Task<List<User>> GetItemsAsync()
        {
            return database.Table<User>().ToListAsync();
        }

        public override Task<List<User>> GetItemsNotDoneAsync()
        {
            return database.QueryAsync<User>("SELECT * FROM [User] WHERE [Done] = 0");
        }

       public override Task<User> GetItemAsync(object id) {
            return database.Table<User>().Where(i => i.Id == (string)id).FirstOrDefaultAsync();
        }

       

        public  override async Task<int> SaveItemAsync(User item) {
            if (await GetItemAsync(item.Id) != null)
                return await database.UpdateAsync(item);
            else
                return await database.InsertAsync(item);
        }

        public override Task<int> DeleteItemAsync(User item)
        {
            return database.DeleteAsync(item);
        }

		public override Task<int> UpdateItemAsync(Dictionary<string, object> d)
		{
			throw new NotImplementedException();
		}
      
   }
}
