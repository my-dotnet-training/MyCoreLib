
using MyCoreLib.Common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace MyCoreLib.BaseData
{
    /// <summary>
    /// The default implementation of <see cref="IDBFactoryImpl"/> inteface.
    /// </summary>
    internal class DefaultFactoryImpl
    {
        private ConcurrentDictionary<string, string> s_connectionStrings = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private string _defaultAppName;
        private static IDBContext s_dbContext;

        private string DefaultAppName
        {
            get
            {
                if (_defaultAppName == null)
                    Interlocked.CompareExchange(ref _defaultAppName, AppSettings.AppName, null);

                return _defaultAppName;
            }
        }

        public string GetConnectionString(DBInstance type)
        {
            return GetConnectionString(type, DefaultAppName);
        }
        public string GetConnectionString(DBInstance type, string appName)
        {
            string cs;
            string key = string.Concat(appName, "/", type.ToString());
            if (!s_connectionStrings.TryGetValue(key, out cs))
            {
                cs = AppSettings.ConnectionString;
                if (string.IsNullOrWhiteSpace(cs))
                    throw new Exception("The required app setting is missing for the key 'ConnectionString'.");

                if (s_dbContext == null)
                {
                    // create a db context using the default connection string
                    using (var db = new DBSqlConnection(cs))
                    {
                        db.Open();
                        s_dbContext = DBContext.GetInstance(db);
                    }
                }

                MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder(cs);

                IDBInstance instance = s_dbContext.GetDatabaseInstance(type);
                sb.Server = instance.Server;
                sb.Database = instance.DatabaseName;

                // We will always use 4-byte unicode character set to support emoji, e.g. 🌺anfuture🌺
                sb.CharacterSet = "utf8mb4";

                cs = sb.ToString();
                s_connectionStrings.TryAdd(key, cs);
            }

            return cs;
        }

        public DBSqlConnection GetSQLConnection(DBInstance type)
        {
            return GetSQLConnection(type, DefaultAppName);
        }
        public DBSqlConnection GetSQLConnection(DBInstance type, string appName)
        {
            string conString = GetConnectionString(type, appName);
            DBSqlConnection connection = new DBSqlConnection(conString);
            connection.Open();
            return connection;
        }
    }
}
