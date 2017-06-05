using Microsoft.Extensions.Caching.Memory;
using MyCoreLib.BaseCache.CacheManager;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCoreLib.BaseCache.MemoryCache
{
    public class CoreMemoryCacheProvider : ICacheProvider, IDisposable
    {
        internal const string CachePrefix = "wp_";
        private const string CacheRootKey = CachePrefix + "root";
        private const int CacheDurationMinutes = 1;
        private IMemoryCache _cache;

        public CoreMemoryCacheProvider(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        #region sync
        public bool CacheData<T>(int entityType, string key, T data)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (ExistsData(key) && !RemoveData(key))
                return false;
            _cache.Set(CachePrefix + key, data, DateTime.Now.AddMinutes(CacheDurationMinutes));
            return ExistsData(key);
        }

        public bool RemoveData(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            _cache.Remove(key);
            return !ExistsData(key);
        }

        public bool ExistsData(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            object cached;
            return _cache.TryGetValue(key, out cached);
        }

        public T GetData<T>(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            T _result;
            if (_cache.TryGetValue(CachePrefix + key, out _result))
                return _result;
            return default(T);
        }
        #endregion

        #region async 
        public T GetDataAsync<T>(string key)
        {
            throw new NotImplementedException();
        }
        
        public Task<bool> CacheDataAsync<T>(int entityType, string key, T data)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveDataAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsDataAsync(string key)
        {
            throw new NotImplementedException();
        }
        #endregion

        public void ClearCache()
        {
        }

        public void Dispose()
        {
            if (_cache != null)
                _cache.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}
