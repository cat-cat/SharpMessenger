using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatClient.Core.Common.Interfaces;
using ChatClient.Core.Common.Models;
using ChatClient.Core.Common.Models.Base;

using SQLite;

using Xamarin.Forms;

namespace ChatClient.Core.DAL.Data.Base
{
    public static class DbProvider
    {
        static SQLiteAsyncConnection _database;
        public static async Task<SQLiteAsyncConnection> GetSqLiteAsyncConnection() {
            if (_database != null)
                return _database;
			string dbpath = await DependencyService.Get<IFileHelper>().GetLocalFilePath("ChatClientSQLite.db3");
			_database = new SQLiteAsyncConnection(dbpath);
           await _database.CreateTableAsync<GeoLocation>();
            await _database.CreateTableAsync<User>();
            await _database.CreateTableAsync<PushId>();
            await _database.CreateTableAsync<Parameters>();
            await _database.CreateTableAsync<CacheMessage>();
            return _database;
        }
    }
}
