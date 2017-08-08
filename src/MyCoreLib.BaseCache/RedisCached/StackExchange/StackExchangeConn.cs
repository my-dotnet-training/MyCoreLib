using System;
using StackExchange.Redis;

namespace MyCoreLib.BaseCache.RedisCached.StackExchange
{
    public class StackExchangeConn
    {
        private static ConnectionMultiplexer _connection;
        private static readonly object SyncObject = new object();
        public static ConnectionMultiplexer GetFactionConn
        {
            get
            {
                if (_connection == null || !_connection.IsConnected)
                {
                    lock (SyncObject)
                    {
                        var configurationOptions = new ConfigurationOptions()
                        {
                            Password = PubConstant.RedisPassword,
                            EndPoints = { { PubConstant.RedisIp, Convert.ToInt32(PubConstant.RedisPort) } }
                        };
                        //"192.168.100.37,password=123456";
                        _connection = ConnectionMultiplexer.Connect(configurationOptions);
                    }
                }
                return _connection;
            }
        }

        //private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        //{
        //    var config = new ConfigurationOptions();
        //    config.EndPoints.Add("192.168.154.131", 6379);

        //    config.SyncTimeout = 10000;
        //    config.AbortOnConnectFail = false;
        //    config.ResolveDns = false;
        //    config.Password = "123456";

        //    config.Ssl = false;

        //    var connectionMultiplexer = ConnectionMultiplexer.Connect(config);
        //    connectionMultiplexer.PreserveAsyncOrder = false;

        //    return connectionMultiplexer;
        //});
    }
}
