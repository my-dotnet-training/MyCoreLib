
namespace MyCoreLib.BaseData
{
    /// <summary>
    /// get db connection string / db connection in sys_db table/setting file
    /// </summary>
    public interface IDBFactoryImpl
    {
        /// <summary>
        /// Get the connection string for connecting database.
        /// </summary>
        string GetConnectionString(DBInstance type);

        /// <summary>
        /// Get the connection string for connecting database.
        /// </summary>
        string GetConnectionString(DBInstance type, string appName);

        /// <summary>
        /// return an opened SqlConnection object that connected to the database referred by dbRoleName
        /// </summary>
        /// <param name="type">the type of a database instance</param>
        /// <returns>opened SqlConnection object</returns>
        DBSqlConnection GetSQLConnection(DBInstance type);

        /// <summary>
        /// return an opened SqlConnection object that connected to the database referred by dbRoleName
        /// </summary>
        /// <param name="type">the type of a database instance</param>
        /// <param name="appName">the application name of the connection string</param>
        /// <returns>opened SqlConnection object</returns>
        DBSqlConnection GetSQLConnection(DBInstance type, string appName);
    }
}
