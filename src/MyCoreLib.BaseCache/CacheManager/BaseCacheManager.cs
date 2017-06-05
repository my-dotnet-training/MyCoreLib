using System;

namespace MyCoreLib.BaseCache.CacheManager
{
    /// <summary>
    /// The base class of all cache manager implementations.
    /// </summary>
    public abstract class BaseCacheManager : ICacheManager
    {
        protected ICacheProvider Provider
        {
            get;
            private set;
        }

        /// <summary>
        /// ctor.
        /// </summary>
        protected BaseCacheManager(ICacheProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");
            this.Provider = provider;
        }
        protected T GetData<T>(int entityType, string key, Func<T> callback)
        {
            var data = (T)Provider.GetData<T>(key);
            if (data == null)
            {
                data = callback();
                Provider.CacheData(entityType, key, data);
            }

            return data;
        }

        public void ClearCache()
        {
            Provider.ClearCache();
        }
    }
}
