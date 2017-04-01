using MyCoreLib.Common.Enum;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace MyCoreLib.BaseData
{
    /// <summary>
    /// Implement a lock using postgresql advisory lock.
    /// Only one machine could successfully acquire the lock at any time.
    /// </summary>
    public class MasterLock : IDisposable
    {
        private MasterOwnership _lockType;
        private DBSqlConnection _connection;
        private IDbTransaction _tran;

        public MasterLock(MasterOwnership lockType)
        {
            _lockType = lockType;
        }
        public bool TryAcquire(int timeout = 0)
        {
            if (timeout < 0)
                throw new ArgumentOutOfRangeException("timeout");

            // Open the database connection, and begins a new transaction
            _connection = DBFactory.GetFactory().GetSQLConnection(DBInstance.WeipanDB);
            _tran = _connection.BeginTransaction();

            using (MySqlCommand cmd = (MySqlCommand)_connection.CreateCommand())
            {
                cmd.CommandText = "SELECT GET_LOCK(@str, @timeout)";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Transaction = (MySqlTransaction)_tran;

                cmd.Parameters.AddParameterWithValue("@str", MySqlDbType.VarChar, "HSCarLock_" + _lockType);
                cmd.Parameters.AddParameterWithValue("@timeout", MySqlDbType.Int32, timeout);

                /*
                 * Tries to obtain a lock with a name given by the string str, using a timeout of timeout seconds.
                 * Returns 1 if the lock was obtained successfully, 
                 * 0 if the attempt timed out (for example, because another client has previously locked the name), 
                 * or NULL if an error occurred (such as running out of memory or the thread was killed with mysqladmin kill).
                 */
                object val = cmd.ExecuteScalar();
                if (val != DBNull.Value)
                {
                    int integer = Convert.ToInt32(val);
                    return (integer == 1);
                }

                return false;
            }
        }

        public void Dispose()
        {
            if (_tran != null)
            {
                _tran.Commit();
                _tran.Dispose();
            }

            if (_connection != null)
            {
                _connection.Dispose();
            }
        }
    }
}
