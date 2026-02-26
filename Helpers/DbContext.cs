using System;
using GoIoFish.Entities;
using NLog;
using SqlSugar;

namespace GoIoFish.Helpers
{
    public class DbContext
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        
        public SqlSugarClient Db { get; private set; }

        public DbContext(string dbPath)
        {
            Db = new SqlSugarClient(new ConnectionConfig
            {
                DbType = DbType.Sqlite, 
                ConnectionString = dbPath,
                IsAutoCloseConnection = true,   
                InitKeyType = InitKeyType.Attribute  
            });
        
            Db.Aop.OnLogExecuting = (sql, parameters) =>
            {
                Log.Info($"SQL: {sql}");
            };
            
            Db.CodeFirst.InitTables(typeof(GooFishProductEntity));
        }
    }
}