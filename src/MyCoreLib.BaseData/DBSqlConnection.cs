// TODO: Need to investigate how to support sql retry.
#define DISABLE_SQL_RETRY

using MyCoreLib.BaseIoC;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;

namespace MyCoreLib.BaseData
{
    public class DBSqlConnection : IDbConnection
    {
        private const string NotInitializedMsg = "Connection object not initialized";
        private const int DBConnectionRetries = 5;
        private DbConnection connection = null;
        private DbTransaction transaction = null;

        /// <summary>
        /// Construct a DBSqlConnection object
        /// </summary>
        /// <example>
        /// <code>
        /// DBSqlConnection r = new DBSqlConnection();
        /// </code></example>
        public DBSqlConnection()
        {
            connection = new MySqlConnection();
        }
        /// <summary>
        /// Construct a DBSqlConnection object
        /// </summary>
        /// <example>
        /// <code>
        /// DBSqlConnection r = new DBSqlConnection(connectionstring);
        /// </code></example>
        /// <param name="connectionString">The connection string to the database</param>
        public DBSqlConnection(string connectionString)
        {
            //use mysql
            connection = new MySqlConnection();
            connection.ConnectionString = connectionString;
        }

        /// <summary>
        /// Construct a DBSqlConnection object from a SqlConnection
        /// </summary>
        /// <example>
        /// <code>
        /// DBSqlConnection r = new DBSqlConnection(sqlConnection);
        /// </code></example>
        /// <param name="sqlConnection">The connection string to the database</param>
        [System.CLSCompliant(false)] // Npgsql is not CLS compliant.
        public DBSqlConnection(DbConnection sqlConnection)
        {
            connection = sqlConnection;
        }

        /// <summary>
        /// Dispose a DBSqlConnection object
        /// </summary>
        public virtual void Dispose()
        {
            if (connection != null)
                connection.Dispose();
        }

        /// <summary>
        /// Change the Database within the same Server
        /// </summary>
        /// <param name="dbname">The new database name</param>
        public void ChangeDatabase(string dbname)
        {
            if (connection == null)
                throw new Exception();

            connection.ChangeDatabase(dbname);
        }

        /// <summary>
        /// Open the connection to the server
        /// </summary>
        public virtual void Open()
        {
            if (connection == null)
                throw new Exception(NotInitializedMsg);

            int NbRetries = 1;//max open is 5
            while (NbRetries <= DBConnectionRetries && connection.State != ConnectionState.Open)
            {
                NbRetries++;
                try
                {
                    connection.Open();
                }
                catch
                {
                    if (NbRetries > DBConnectionRetries)
                        throw;
                }
            }
        }

        /// <summary>
        /// Create a Command from the database connection
        /// </summary>
        public IDbCommand CreateCommand()
        {
            if (connection == null)
                throw new Exception(NotInitializedMsg);
            return connection.CreateCommand();
        }

        /// <summary>
        /// Begin a transaction
        /// </summary>
        public IDbTransaction BeginTransaction()
        {
#if !DISABLE_SQL_RETRY
            if (connection == null)
                throw new Exception(NotInitializedMsg);
            int retryCount = 1;
            while (retryCount <= DBConnectionRetries)
            {
                try
                {
#else
            {
                {
#endif
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    this.transaction = connection.BeginTransaction(); ;
                    return this.transaction;
#if DISABLE_SQL_RETRY
                }
            }
#else
                }
                catch (SqlException e)
                {
                    if (e.Number == COMMAND_FAIL_OVER_SQL_CODE
                            || e.Number == CONNECTION_FAIL_OVER_SQL_CODE)
                    {// no check CONNECTION_DOWN_SQL_CODE, SQL already down, no need to retry, so new WS calls will fails right away.
                        FlushConnectionPool();
                        if (retryCount++ >= 5)
                            throw;
                        continue;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            throw new Exception("Unable to begin transaction after failing 5 times due to unavailable SQL Server");
#endif
        }

        /// <summary>
        /// Begin a transaction with a specified Isolation level
        /// </summary>
        /// <param name="lvl">The level of the isolation of the transaction</param>
        public IDbTransaction BeginTransaction(IsolationLevel lvl)
        {
#if DISABLE_SQL_RETRY
            {
                {
#else
            if (connection == null)
                throw new Exception(NotInitializedMsg);
            int retryCount = 1;
            while (retryCount <= DBConnectionRetries)
            {
                try
                {
#endif
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    this.transaction = connection.BeginTransaction(lvl);
                    return this.transaction;
#if DISABLE_SQL_RETRY
                }
            }
#else
                }
                catch (SqlException e)
                {
                    if (e.Number == COMMAND_FAIL_OVER_SQL_CODE
                            || e.Number == CONNECTION_FAIL_OVER_SQL_CODE)
                    {
                        FlushConnectionPool();
                        if (retryCount++ >= 5)
                        {
                            throw;
                        }
                        continue;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            throw new Exception("Unable to begin transaction after failing 5 times due to unavailable SQL Server");
#endif
        }

        /// <summary>
        /// Commits the current transaction on the connection if there is one, and close the connection.
        /// Will throw an exception if there is no Transaction started, check this by checking the Transaction object for null.
        /// </summary>
        public void Commit()
        {
            if (this.transaction != null)
            {
                this.transaction.Commit();
                this.Close();
            }
            else
            {
                throw new Exception("Commit Failed: No transaction started on connection");
            }
        }

        /// <summary>
        /// Rollsback the current transaction on the connection if there is one, and close the connection.
        /// Will throw an exception if there is no Transaction started, check this by checking the Transaction object for null.
        /// </summary>
        public void Rollback()
        {
            if (this.transaction != null)
            {
                this.transaction.Rollback();
                this.Close();
            }
            else
            {
                throw new Exception("Rollback Failed: No transaction started on connection");
            }
        }

        /// <summary>
        /// Close the connection to the database server
        /// </summary>
        public void Close()
        {
            if (connection == null)
                throw new Exception(NotInitializedMsg);
            connection.Close();
        }

        /// <summary>
        /// Return the internal connection object
        /// </summary>
        public IDbConnection Connection
        {
            get
            {
                return connection;
            }
        }
        /// <summary>
        /// Return the internal SqlConnection object
        /// </summary>
        [System.CLSCompliant(false)] // Npgsql is not CLS compliant.
        public DbConnection InternalSqlConnection
        {
            get
            {
                return connection;
            }
        }
        /// <summary>
        /// Return the internal transaction object
        /// </summary>
        public IDbTransaction Transaction
        {
            get
            {
                return this.transaction;
            }
        }
        /// <summary>
        /// Get/Set the connection string on the connection
        /// </summary>
        public string ConnectionString
        {
            get
            {
                if (connection == null)
                {
                    throw new Exception(NotInitializedMsg);
                }
                return connection.ConnectionString;
            }
            set
            {
                if (connection == null)
                {
                    throw new Exception(NotInitializedMsg);
                }
                connection.ConnectionString = value;
            }
        }

        /// <summary>
        /// Return the database of the connection
        /// </summary>
        public string Database
        {
            get
            {
                if (connection == null)
                {
                    throw new Exception(NotInitializedMsg);
                }
                return connection.Database;
            }
        }

        /// <summary>
        /// Resturn the Connection timeout of the connection
        /// </summary>
        public int ConnectionTimeout
        {
            get
            {
                if (connection == null)
                {
                    throw new Exception(NotInitializedMsg);
                }
                return connection.ConnectionTimeout;
            }
        }

        /// <summary>
        /// Return the state of the connection
        /// </summary>
        public ConnectionState State
        {
            get
            {
                if (connection == null)
                {
                    throw new Exception(NotInitializedMsg);
                }
                return connection.State;
            }
        }
    }
}
