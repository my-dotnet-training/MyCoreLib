using MyCoreLib.BaseData.Exceptions;
using MyCoreLib.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MyCoreLib.BaseData
{
    /// <summary>
    /// all connection DB in dictionary for read and write separation
    /// problem : how to use connection pool to control
    /// </summary>
    public class DBContext : IDBContext
    {
        private IDictionary<DBInstance, IDBInstance> _map;
        private DBContext(IList<SystemDB> list)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            var map = new Dictionary<DBInstance, IDBInstance>();
            foreach (IDBInstance item in list)
            {
                map[item.DbType] = item;
            }

            this._map = new ReadOnlyDictionary<DBInstance, IDBInstance>(map);
        }

        /// <summary>
        /// Get an instance of DBContext.
        /// </summary>
        internal static IDBContext GetInstance(DBSqlConnection db)
        {
            IList<SystemDB> list = SystemDB.GetDatabaseInstances(db);
            return new DBContext(list);
        }

        public IDBInstance GetDatabaseInstance(DBInstance type)
        {
            IDBInstance instance;
            if (_map.TryGetValue(type, out instance))
                return instance;

            // Not found.
            throw new DBNotFoundException(string.Format("Cannot found a database instance of type '{0}'.", type));
        }
    }
}
