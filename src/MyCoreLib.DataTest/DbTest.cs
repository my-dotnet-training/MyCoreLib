using MyCoreLib.Common;
using MyCoreLib.Data;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;

namespace MyCoreLib.DataTest
{
    public class DbTest
    {
        public static void Main(string[] args)
        {
            using (DBSqlConnection _conn = ConnectTest())
            {
                DBSelectOpertion(_conn);
            }
            Console.ReadKey();
        }

        private static DBSqlConnection ConnectTest()
        {
            try
            {
                DBSqlConnection _conn = new DBSqlConnection("server=localhost; database=weipandb_unittest; username=root; password=MySql2015;");
                _conn.Open();
                Console.WriteLine("Connection to DB successfull");
                return _conn;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return null;
        }
        private static void DBSelectOpertion(DBSqlConnection _conn)
        {
            try
            {
                string commandText = "select * from mgmt_users limit 0,1";
                IDbCommand sqlCommand = _conn.CreateCommand();
                sqlCommand.CommandText = commandText;
                //sqlCommand.Connection = ((MySqlConnection)(_conn.Connection));
                //sqlCommand.Transaction = ((MySqlTransaction)(_conn.Transaction));

                var _reader = sqlCommand.ExecuteReader();
                if (_reader.Read())
                    Console.WriteLine(_reader["user_name"]);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
