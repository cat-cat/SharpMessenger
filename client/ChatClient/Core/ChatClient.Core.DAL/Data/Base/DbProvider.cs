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
        public static SQLiteAsyncConnection GetSqLiteAsyncConnection() {
            if (_database != null)
                return _database;
			string dbpath = DependencyService.Get<IFileHelper>().GetLocalFilePath("ChatClientSQLite.db3");
			_database = new SQLiteAsyncConnection(dbpath);
            _database.CreateTableAsync<GeoLocation>().Wait();
            _database.CreateTableAsync<User>().Wait();
            _database.CreateTableAsync<PushId>().Wait();
            _database.CreateTableAsync<Parameters>().Wait();
            _database.CreateTableAsync<CacheMessage>().Wait();
            return _database;
        }
    }
}
