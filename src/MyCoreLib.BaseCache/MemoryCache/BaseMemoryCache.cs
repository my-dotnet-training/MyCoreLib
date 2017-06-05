using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace MyCoreLib.BaseCache.MemoryCache
{
    public class BaseMemoryCache<T> : BaseCache
    {
        private class CacheEntry<TEntry>
        {
            public TEntry Entity { get; private set; }
            public DateTime TimeExpired { get; private set; }

            /// <summary>
            /// Determines whether the cached data is expired or not.
            /// </summary>
            public bool IsExpired { get { return TimeExpired < DateTime.Now; } }

            public CacheEntry(TEntry entity)
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.Entity = entity;
                this.TimeExpired = DateTime.Now.AddSeconds(TimeToLiveSeconds);
            }
        }

        private const int TimeToLiveSeconds = 300; // 5 minutes
        private ConcurrentDictionary<int, CacheEntry<T>> _cache;
        private static BaseMemoryCache<T> s_instance;
        /// <summary>
        /// Get the singleton instance of the class.
        /// </summary>
        private static BaseMemoryCache<T> Instance
        {
            get
            {
                if (s_instance == null)
                {
                    Interlocked.CompareExchange(ref s_instance, new BaseMemoryCache<T>(), null);
                }
                return s_instance;
            }
        }

        public BaseMemoryCache()
        {
            _cache = new ConcurrentDictionary<int, CacheEntry<T>>();
        }

        public T GetEntity(int id, Func<int, T> getFromDB)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException("id");

            // Try to read cache first
            CacheEntry<T> cacheEntry;
            var cache = Instance._cache;
            if (cache.TryGetValue(id, out cacheEntry) && !cacheEntry.IsExpired)
            {
                return cacheEntry.Entity;
            }

            // Read from database then
            T user = getFromDB(id);
            if (user == null)
                return default(T);

            cache[id] = new CacheEntry<T>(user);
            return user;
        }
    }
}
