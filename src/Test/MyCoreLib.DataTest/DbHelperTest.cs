using MyCoreLib.BaseOrm.DbHelper;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MyCoreLib.DataTest
{
    public class DbHelperTest
    {
        string _cs = "server=13.75.127.122; database=weipandb_dev; username=wp2_dev; password=MySql2015;";
        public void HelperTest()
        {

            var _reader = DbHelper<MySqlCommand, MySqlConnection, MySqlDataReader, MySqlParameter, MySqlTransaction>.ExecuteReader(_cs, "select * from mgmt_users limit 0,1");

        }


        private DbHelper NewSqlHelper()
        {
            return new DbHelper(MySqlClientFactory.Instance, _cs);
        }

        public void HelperTest2()
        {
            var db = NewSqlHelper();

            var commandText = "select* from mgmt_users limit {0}, {1}";
            var p0 = 0;
            var p1 = 10;
            var command = db.CreateCommand(commandText, CommandType.Text, p0, p1);
            var _int = db.ExecuteNonQuery(command, db.CreateConnection());
        }
    }
}
