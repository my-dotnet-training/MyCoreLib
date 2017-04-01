
using MyCoreLib.BaseData.Entity;
using MyCoreLib.Entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace MyCoreLib.BaseData
{
    public static class DBExtensions
    {
        /// <summary>
        /// type of database
        /// </summary>
        public static DBProviderType ProviderType { get; internal set; }

        /// <summary>
        /// The database call has to run in an active transaction.
        /// </summary>
        public static T RunInTransaction<T>(this DBSqlConnection db, Func<T> method)
        {
            if (db.State == ConnectionState.Closed)
                db.Open();
            bool tranStarted = false;

            // The method has to run in a transaction.
            if (db.Transaction == null)
            {
                db.BeginTransaction();
                tranStarted = true;
            }

            try
            {
                T result = method();
                if (tranStarted)
                    db.Commit();

                return result;
            }
            catch
            {
                if (tranStarted)
                    db.Rollback();

                throw;
            }
        }

        /// <summary>
        /// Run the database operations with at most 3 attempts.
        /// </summary>
        public static T RunWithRetry<T>(this DBSqlConnection db, Func<T> method)
        {
            int numAttempts = 0;
            const int MaxAttempts = 3;
            do
            {
                try
                {
                    return method();
                }
                catch (MySqlException sqlEx)
                {
                    if (sqlEx.Number == (int)MySqlErrorCode.LockDeadlock)
                    {
                        if (++numAttempts < MaxAttempts)
                        {
                            System.Diagnostics.Debug.WriteLine("Retry the transaction for attempt #{0}", numAttempts);
                            continue;
                        }
                    }

                    throw;
                }
            } while (true);
        }

        /// <summary>
        /// Read an array of entities from the specified data reader.
        /// return entity list
        /// </summary>
        public static List<T> ReadEntities<T>(this IDataReader dr)
             where T : IDbEntity, new()
        {
            List<T> list = new List<T>();

            while (dr.Read())
            {
                T p = new T();
                p.ReadFromDataReader(dr);

                list.Add(p);
            }

            return list;
        }

        /// <summary>
        /// Read an array of entities from the specified data reader.
        /// return interface list
        /// </summary>
        public static List<I> ReadEntities<T, I>(this IDataReader dr)
             where T : IDbEntity, I, new()
        {
            List<I> list = new List<I>();

            while (dr.Read())
            {
                T p = new T();
                p.ReadFromDataReader(dr);

                list.Add((I)p);
            }

            return list;
        }

        /// <summary>
        /// Read an array of entities from the specified data reader.
        /// for pagging
        /// return entity list
        /// </summary>
        public static List<T> ReadEntities<T>(this IDataReader dr, out int total)
             where T : IDbEntity, new()
        {
            var list = ReadEntities<T>(dr);

            if (dr.NextResult() && dr.Read())
            {
                total = System.Convert.ToInt32(dr[0]);
                return list;
            }

            throw new System.InvalidOperationException("Failed to read total number of matching records.");
        }

        /// <summary>
        /// Read an array of entities from the specified data reader.
        /// for pagging
        /// return interface list
        /// </summary>
        public static List<I> ReadEntities<T, I>(this IDataReader dr, out int total)
             where T : IDbEntity, I, new()
        {
            var list = ReadEntities<T, I>(dr);

            if (dr.NextResult() && dr.Read())
            {
                total = System.Convert.ToInt32(dr[0]);
                return list;
            }

            throw new System.InvalidOperationException("Failed to read total number of matching records.");
        }

        /// <summary>
        /// Read an array of entities from the specified data reader.
        /// return entity
        /// </summary>
        public static T ReadEntity<T>(this IDataReader dr)
             where T : class, IDbEntity, new()
        {
            if (dr.Read())
            {
                T entity = new T();
                entity.ReadFromDataReader(dr);
                return entity;
            }

            return null;
        }

        /// <summary>
        /// add parameter
        /// </summary>
        public static MySqlParameter AddParameterWithValue(this MySqlParameterCollection parameters, string parameterName, MySqlDbType dbType, object value)
        {
            MySqlParameter p = parameters.Add(parameterName, dbType);
            p.Value = value;
            return p;
        }

        /// <summary>
        /// add parameter with size property
        /// </summary>
        public static MySqlParameter AddParameterWithValue(this MySqlParameterCollection parameters, string parameterName, MySqlDbType dbType, int size, object value)
        {
            MySqlParameter p = parameters.Add(parameterName, dbType, size);
            p.Value = value;
            return p;
        }

        /// <summary>
        /// Determines whether the exception is caused by foreign key references.
        /// </summary>
        public static bool IsForeignKeyFailure(this MySqlException ex)
        {
            /*
             * Error: 1451 SQLSTATE: 23000 (ER_ROW_IS_REFERENCED_2)
             * Message: Cannot delete or update a parent row: a foreign key constraint fails (%s)
             */
            return ex.Number == (int)MySqlErrorCode.RowIsReferenced2;
        }
    }
}
