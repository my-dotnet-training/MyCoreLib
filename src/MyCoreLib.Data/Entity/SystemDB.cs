using System;
using System.Collections.Generic;

namespace MyCoreLib.Data.Entity
{
    /// <summary>
    /// all db conneciton infomation in the system_db table
    /// </summary>
    public class SystemDB : IDbEntity, IDBInstance
    {
        public string Name { get; private set; }
        public DBInstance DbType { get; private set; }
        public string Server { get; private set; }
        public string DatabaseName { get; private set; }
        public bool Enabled { get; private set; }
        public DateTime TimeCreated { get; private set; }
        public DateTime TimeUpdated { get; private set; }

        // [CLSCompliant(false)] // MySqlDataReader isn't CLS compliant.
        public void ReadFromDataReader(System.Data.IDataReader dr)
        {
            //Name = (string)dr["name"];
            //DbType = (DBType)((int)dr["db_type"]);
            //Server = (string)dr["server"];
            //DatabaseName = (string)dr["db_name"];
            //Enabled = (bool)dr["enabled"];
            //TimeCreated = (DateTime)dr["time_created"];
            //TimeUpdated = (DateTime)dr["time_updated"];
        }

        internal static IList<SystemDB> GetDatabaseInstances(DBSqlConnection db)
        {
            //using (var dr = WeipanDB.sproc_sys_db_get_all.ExecuteReader(db))
            //{
            //    return dr.ReadEntities<SystemDB>();
            //}
            return null;
        }
    }
}
