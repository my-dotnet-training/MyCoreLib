using Memcached.ClientLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.BaseCache
{
    public interface ICacheService
    {
        bool Set(string key, object value);
        bool Set(string key, object value, int ttl);
        bool Delete(string key);
        bool Exists(string key);
        T Get<T>(string key);
        IDictionary<string, T> Gets<T>(string[] keys);
    }
    
    public static class CacheKey
    {
        public static string Build(Type type, object key)
        {
            if (type == null || key == null)
            {
                throw new ArgumentNullException("type or key is null.");
            }

            StringBuilder sb = new StringBuilder(32);
            sb.Append(type.FullName).Append("-");
            sb.Append(key.GetType().FullName);
            sb.Append("-").Append(key);

            return sb.ToString();
            //return StringHelper.MD5(sb.ToString());
        }
    }

}
