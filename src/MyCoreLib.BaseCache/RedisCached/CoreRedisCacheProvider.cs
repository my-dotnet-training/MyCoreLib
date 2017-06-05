using Microsoft.Extensions.Caching.Redis;
using MyCoreLib.BaseCache.CacheManager;
using MyCoreLib.Common;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Text;
using System.Threading.Tasks;

namespace MyCoreLib.BaseCache.RedisCached
{
    public class CoreRedisCacheProvider : ICacheProvider, IDisposable
    {
        private const int CacheDurationMinutes = 1;
        protected IDatabase _cache;
        private ConnectionMultiplexer _connection;
        private readonly string _instance;

        public CoreRedisCacheProvider(RedisCacheOptions options, int database = 0)
        {
            _connection = ConnectionMultiplexer.Connect(options.Configuration);
            _cache = _connection.GetDatabase(database);
            _instance = options.InstanceName;
        }
        public string GetKeyForRedis(string key)
        {
            return _instance + key;
        }

        #region sync
        public bool CacheData<T>(int entityType, string key, T data)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return _cache.StringSet(GetKeyForRedis(key), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)), Utility.ConvertToTimeSpan(DateTime.Now.AddMinutes(CacheDurationMinutes)));
        }

        public bool ExistsData(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return _cache.KeyExists(GetKeyForRedis(key));
        }

        public T GetData<T>(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            var value = _cache.StringGet(GetKeyForRedis(key));
            if (!value.HasValue)
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(value);
        }

        public bool RemoveData(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return _cache.KeyDelete(GetKeyForRedis(key));
        }

        #endregion

        #region async

        public Task<bool> CacheDataAsync<T>(int entityType, string key, T data)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return _cache.StringSetAsync(GetKeyForRedis(key), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)), Utility.ConvertToTimeSpan(DateTime.Now.AddMinutes(CacheDurationMinutes)));
        }

        public Task<bool> ExistsDataAsync(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return _cache.KeyExistsAsync(GetKeyForRedis(key));
        }

        public T GetDataAsync<T>(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            var value = _cache.StringGetAsync(GetKeyForRedis(key));
            if (!value.Result.HasValue)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(value.Result);
        }
        public Task<bool> RemoveDataAsync(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return _cache.KeyDeleteAsync(GetKeyForRedis(key));
        }

        #endregion

        public void ClearCache()
        {
        }

        public void Dispose()
        {
            if (_cache != null)
                _cache = null;
            GC.SuppressFinalize(this);
        }
    }
}
