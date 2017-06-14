using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.Common.Models;

using SQLite;

namespace ChatClient.Core.DAL.Data.Base {
    public abstract class DatabasePersistance<TTYPE> {
        public SQLiteAsyncConnection database
        {
            get {
                return DbProvider.GetSqLiteAsyncConnection();
            }
        }

        public abstract Task<List<TTYPE>> GetItemsAsync();


        public abstract Task<List<TTYPE>> GetItemsNotDoneAsync();


        public abstract Task<TTYPE> GetItemAsync(object id);

        public abstract Task<int> SaveItemAsync(TTYPE item);


        public abstract Task<int> DeleteItemAsync(TTYPE item);
        
    }
}

