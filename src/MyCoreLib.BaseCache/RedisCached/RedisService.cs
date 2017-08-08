using MyCoreLib.BaseCache.RedisCached.StackExchange;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;

namespace MyCoreLib.BaseCache.RedisCached
{
    public class RedisService : ICacheService
    {
        private static IDatabase cache;
        public bool Delete(string key)
        {
            return cache.KeyDelete(key);
        }

        public bool Exists(string key)
        {
            return cache.KeyExists(key);
        }

        public T Get<T>(string key)
        {
            return Deserialize<T>(cache.StringGet(key));
        }

        public bool Init(string servers = "")
        {
            cache = StackExchangeConn.GetFactionConn.GetDatabase();
            return true;
        }

        public bool Set(string key, object value)
        {
            return cache.StringSet(key, Serialize(value));
        }

        public bool Set(string key, object value, int ttl)
        {
            return cache.StringSet(key, Serialize(value), new TimeSpan(0, 0, ttl));
        }

        public List<CacheEntity> GetAllKeys()
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, T> Gets<T>(string[] keys)
        {
            throw new NotImplementedException();
        }

        static byte[] Serialize(object o)
        {
            if (o == null)
            {
                return null;
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, o);
                byte[] objectDataAsStream = memoryStream.ToArray();
                return objectDataAsStream;
            }
        }

        static T Deserialize<T>(byte[] stream)
        {
            if (stream == null)
            {
                return default(T);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream(stream))
            {
                T result = (T)binaryFormatter.Deserialize(memoryStream);
                return result;
            }
        }
    }
}
