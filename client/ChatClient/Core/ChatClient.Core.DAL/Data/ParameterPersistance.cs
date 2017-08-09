using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.DAL.Data.Base;
using ChatClient.Core.Common.Models;

namespace ChatClient.Core.DAL.Data
{
  public  class ParameterPersistance:DatabasePersistance<Parameters> {
		public async override Task<List<Parameters>> GetItemsAsync() {
            var db = await database();
            return await db.Table<Parameters>().ToListAsync();
        }

		public async override Task<List<Parameters>> GetItemsNotDoneAsync() {
            var db = await database();
            return await db.QueryAsync<Parameters>("SELECT * FROM [Parameters] WHERE [Done] = 0");
        }

		public async  override Task<Parameters> GetItemAsync(object id) {
            // int lId = Convert.ToInt32(id);
            var db = await database();
            return await db.Table<Parameters>().Where(i => i.Id == (int)id).FirstOrDefaultAsync();

        }

        public async override Task<int> SaveItemAsync(Parameters item) {
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

        public override Task<int> DeleteItemAsync(Parameters item) {
            throw new NotImplementedException();
        }

		public override Task<int> UpdateItemAsync(Dictionary<string, object> d)
		{
            throw new NotImplementedException();
		}
    }
}
