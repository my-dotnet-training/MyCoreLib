using System;

namespace MyCoreLib.BaseWeb.Caching
{
    /// <summary>
    /// The base class of all cache manager implementations.
    /// </summary>
    public abstract class CacheManagerBase : ICacheManager
    {
        protected ICacheProvider Provider
        {
            get;
            private set;
        }

        /// <summary>
        /// ctor.
        /// </summary>
        protected CacheManagerBase(ICacheProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");
            this.Provider = provider;
        }
        protected T GetData<T>(int entityType, string key, Func<T> callback)
        {
            var data = (T)Provider.GetData(key);
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
