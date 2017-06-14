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
			return await database.Table<Parameters>().ToListAsync();
        }

		public async override Task<List<Parameters>> GetItemsNotDoneAsync() {
			return await database.QueryAsync<Parameters>("SELECT * FROM [Parameters] WHERE [Done] = 0");
        }

		public async  override Task<Parameters> GetItemAsync(object id) {
           // int lId = Convert.ToInt32(id);
			return await database.Table<Parameters>().Where(i => i.Id == (int)id).FirstOrDefaultAsync();

        }

        public async override Task<int> SaveItemAsync(Parameters item) {
            if (await GetItemAsync(item.Id) != null)
            {
                return await database.UpdateAsync(item);

            }
            else
            {
                return await database.InsertAsync(item);
            }
        }

        public override Task<int> DeleteItemAsync(Parameters item) {
            throw new NotImplementedException();
        }
    }
}
