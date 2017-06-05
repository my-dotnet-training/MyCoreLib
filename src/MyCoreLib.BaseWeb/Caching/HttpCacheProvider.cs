using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.BaseWeb.Caching
{
    public class HttpCacheProvider : ICacheProvider
    {
        internal const string CachePrefix = "sys_";
        private const string CacheRootKey = CachePrefix + "root";
        private const int CacheDurationMinutes = 1;
        private readonly Cache _cache;

        public HttpCacheProvider()
        {
            _cache = HttpRuntime.Cache;

            // Make sure the cache root does exist.
            this.UpdateCacheRoot();
        }

        public object GetData(string key)
        {
            return HttpRuntime.Cache[key];
        }

        public void CacheData(int entityType, string key, object data)
        {
            // Cache for up to 1 minute.
            _cache.Insert(key, data,
                GetCacheDependency(),
                DateTime.Now.AddMinutes(CacheDurationMinutes), Cache.NoSlidingExpiration);
        }

        public void ClearCache()
        {
            UpdateCacheRoot();
        }

        private void UpdateCacheRoot()
        {
            // Update the root key.
            _cache.Insert(CacheRootKey, new object());
        }

        private static CacheDependency GetCacheDependency()
        {
            return new CacheDependency(null, new string[] { CacheRootKey });
        }
    }
}
