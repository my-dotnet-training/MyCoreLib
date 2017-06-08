using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace MyCoreLib.BaseOrm.DbHelper
{
    public static class DbHelper<TDbCommand, TDbConnection, TDbDataReader, TDbParameter, TDbTransaction>
          where TDbCommand : DbCommand, new()
          where TDbConnection : DbConnection, new()
          where TDbParameter : DbParameter, new()
          where TDbDataReader : DbDataReader
          where TDbTransaction : DbTransaction
    {

        #region Private Helpers
        private static void FillFromReader(DbDataReader reader, int startRecord, int maxRecords, Action<DbDataReader> action)
        {
            if (startRecord < 0)
                throw new ArgumentOutOfRangeException("startRecord", "StartRecord must be zero or higher.");

            while (startRecord > 0)
            {
                if (!reader.Read())
                    return;
                startRecord--;
            }
            if (maxRecords > 0)
            {
                int i = 0;
                while (i < maxRecords && reader.Read())
                {
                    action(reader);
                    i++;
                }
            }
            else
            {
                while (reader.Read())
                    action(reader);
            }
        }
        private static async Task FillFromReaderAsync(DbDataReader reader, int startRecord, int maxRecords, Action<DbDataReader> action)
        {
            if (startRecord < 0)
                throw new ArgumentOutOfRangeException("startRecord", "StartRecord must be zero or higher.");

            while (startRecord > 0)
            {
                if (!await reader.ReadAsync())
                    return;

                startRecord--;
            }

            if (maxRecords > 0)
            {
                int i = 0;

                while (i < maxRecords && await reader.ReadAsync())
                {
                    action(reader);
                    i++;
                }
            }
            else
            {
                while (await reader.ReadAsync())
                    action(reader);
            }
        }
        private static string GetProviderParameterFormatString()
        {
            //var builder = Factory.CreateCommandBuilder();
            //Type type = builder.GetType();
            //MethodInfo method = type.GetMethod("GetParameterPlaceholder", BindingFlags.NonPublic | BindingFlags.Instance);
            //var index = 42;
            //var parameterName = method.Invoke(builder, new object[] { index }).ToString();
            //return parameterName.Replace(index.ToString(CultureInfo.InvariantCulture), "{0}");
            return "@p{0}";
        }

        #endregion

        #region Helper Methods and Extension Points

        public static TDbConnection CreateConnection(string connectionString)
        {
            TDbConnection _conn = new TDbConnection();
            _conn.ConnectionString = connectionString;
            return _conn;
        }

        public static TDbCommand CreateCommand(string commandText, CommandType commandType = CommandType.Text, params object[] parameters)
        {
            var _len = parameters.Length;
            var _command = new TDbCommand();
            _command.CommandType = commandType;
            if (_len > 0)
            {
                var _formatValues = new string[_len];
                for (var i = 0; i < _len; i++)
                {
                    var _parameter = parameters[i];
                    var _rawValue = _parameter as RawValue;

                    if (_rawValue != null)
                    {
                        _formatValues[i] = _rawValue.Value;
                    }
                    else
                    {
                        var _dbParameter = new TDbParameter();
                        var _name = CreateParameterName(i);
                        _dbParameter.ParameterName = _name;
                        _dbParameter.Value = _parameter ?? DBNull.Value;

                        _formatValues[i] = _name;
                        _command.Parameters.Add(_dbParameter);

                    }
                }
                _command.CommandText = String.Format(commandText, _formatValues);
            }
            else
            {
                _command.CommandText = commandText;
            }
            return _command;
        }
        private static string _parameterFormat;

        private static string CreateParameterName(int index)
        {
            if (_parameterFormat == null)
                _parameterFormat = GetProviderParameterFormatString();
            return string.Format(_parameterFormat, index);
        }

        private static Converter<object, T> GetTypeConverter<T>()
        {
            return (object o) => (T)DBConvert.To<T>(o);
        }

        private static Converter<TDbDataReader, T> GetDataReaderConverter<T>()
            where T : new()
        {
            return new DataReaderConverter<T>().Convert;
        }

        #endregion

        public static int ExecuteNonQuery(string connectionString, string commandText, CommandType cmdType = CommandType.Text, params object[] parameters)
        {
            TDbConnection _connection = CreateConnection(connectionString);
            TDbCommand _command = CreateCommand(commandText, cmdType);
            _command.Connection = _connection;
            return _command.ExecuteNonQuery();
        }
        public static Task<int> ExecuteNonQueryAsync(string connectionString, string commandText, CommandType cmdType = CommandType.Text, params object[] parameters)
        {
            TDbConnection _connection = CreateConnection(connectionString);
            TDbCommand _command = CreateCommand(commandText, cmdType);
            _command.Connection = _connection;
            return _command.ExecuteNonQueryAsync();
        }

        public static TDbDataReader ExecuteReader(string connectionString, string commandText, CommandType cmdType = CommandType.Text, params object[] parameters)
        {
            TDbConnection _connection = CreateConnection(connectionString);
            TDbCommand _command = CreateCommand(commandText, cmdType);
            _command.Connection = _connection;
            return (TDbDataReader)_command.ExecuteReader();
        }

    }
}
