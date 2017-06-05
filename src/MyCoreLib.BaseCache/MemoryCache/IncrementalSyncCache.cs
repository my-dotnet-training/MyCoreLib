using MyCoreLib.BaseCache.Exceptions;
using MyCoreLib.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyCoreLib.BaseCache.MemoryCache
{
    public abstract class IncrementalSyncCache : IDisposable
    {
        private Task _fullSyncTask;
        private Exception _fullSyncError;

        // The date and time when the cache will be considered as out-of-sync.
        private DateTime _outOfSyncTime;
        private Exception _lastSyncError;
        private int _cacheSyncIntervalInMS;
        private Timer _cacheSyncTimer;

        // The date and time when the cache was lastly synced.
        private DateTime _lastSyncTime;
        private int _cacheHit;

        private ReaderWriterLockSlim _rwLock;
        private volatile bool _stopped;

        /// <summary>
        /// Returns the number of successful cache hit.
        /// </summary>
        public int CacheHit { get { return _cacheHit; } }

        /// <summary>
        /// Get the name of the cache.
        /// </summary>
        public string CacheName { get; private set; }

        /// <summary>
        /// Determines whether the initial full sync is completed or not.
        /// </summary>
        protected bool IsFullSyncCompleted { get { return _fullSyncTask.IsCompleted; } }

        protected IncrementalSyncCache(string name, int cacheSyncIntervalInMS)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            if (cacheSyncIntervalInMS <= 0) throw new ArgumentOutOfRangeException("cacheSyncIntervalInMS");

            this.CacheName = name;
            this._cacheSyncIntervalInMS = cacheSyncIntervalInMS;
            _rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            _cacheSyncTimer = new Timer(IncrementalSync, null, Timeout.Infinite, Timeout.Infinite);

            // Create a background task to perform the initial full sync
            _fullSyncTask = Task.Factory.StartNew(this.FullSync);
        }

        /// <summary>
        /// Mark the cache as released. We will need to create a new cache instance in the next call.
        /// </summary>
        protected virtual void ReleaseCache()
        {
            _stopped = true;
            Dispose();
        }

        protected T ReadCache<T>(Func<T> callback) where T : class
        {
            if (_fullSyncError != null)
            {
                ReleaseCache();
                throw new CacheOutOfSyncException(string.Format("{0}: Failed to sync data from database.", CacheName), _fullSyncError);
            }
            // Verify if the cache is still in sync or not?
            if (DateTime.UtcNow > _outOfSyncTime)
            {
                ReleaseCache();
                throw new CacheOutOfSyncException(string.Format("{0}: The cache is already out of sync.", CacheName), _lastSyncError);
            }
            if (!_rwLock.TryEnterReadLock(30 * 1000))
                throw new InvalidOperationException("Failed to acquire reader lock to read cache.");
            try
            {
                T data = callback();
                if (data != null)
                {
                    Interlocked.Increment(ref _cacheHit);
                }
                return data;
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Performs a full sync with database, and returns database time when the data was synced.
        /// </summary>
        protected abstract DateTime DoFullSync();

        /// <summary>
        /// Performs an incremental sync with database, and returns database time when the data was synced.
        /// </summary>
        protected abstract bool DoIncrementalSync(DateTime lastSyncDbTime, out DateTime updatedDbTime);

        protected bool TryAcquireWriteLock()
        {
            if (!_rwLock.TryEnterWriteLock(30 * 1000))
            {
                throw new InvalidOperationException("Failed to acquire writer lock to update cache.");
            }
            return true;
        }

        protected void ReleaseWriteLock()
        {
            _rwLock.ExitWriteLock();
        }

        private void FullSync()
        {
            Utility.OutputDebugString("[{0}] Start the full cache sync.", CacheName);
            try
            {
                _lastSyncTime = DoFullSync();

                // Start a timer to sync data with database periodically.
                _cacheSyncTimer.Change(_cacheSyncIntervalInMS, Timeout.Infinite);
                _outOfSyncTime = DateTime.UtcNow.AddMilliseconds(_cacheSyncIntervalInMS * 5);

                Utility.OutputDebugString("[{0}] Completed the full cache sync.", CacheName);
            }
            catch (Exception ex)
            {
                this._fullSyncError = ex;
                Utility.OutputDebugString("[{0}] Failed to do full cache sync: {1}", CacheName, ex);
            }
        }

        private void IncrementalSync(object state)
        {
            if (_stopped)
                return;

            Utility.OutputDebugString("[{0}] Start the incremental cache sync.", CacheName);
            try
            {
                DateTime updatedDbTime;
                if (DoIncrementalSync(_lastSyncTime, out updatedDbTime))
                {
                    _lastSyncTime = updatedDbTime;
                    _outOfSyncTime = DateTime.UtcNow.AddMilliseconds(_cacheSyncIntervalInMS * 5);

                    // Clear sync error if any
                    _lastSyncError = null;
                    Utility.OutputDebugString("[{0}] Completed the incremental cache sync.", CacheName);
                }
                else
                {
                    Utility.OutputDebugString("[{0}] #### the cache instance is already released.", CacheName);
                }
            }
            catch (Exception ex)
            {
                // Save the sync error.
                _lastSyncError = ex;
                Utility.OutputDebugString("[{0}] Failed to do incremental cache sync: {1}", CacheName, ex);
            }
            finally
            {
                if (!_stopped)
                {
                    _cacheSyncTimer.Change(_cacheSyncIntervalInMS, Timeout.Infinite);
                }
            }
        }

        public void Dispose()
        {
            this._stopped = true;

            // Avoid dispose the timer twice once Dispose method was called for multiple times.
            Timer oldTimer = Interlocked.Exchange(ref _cacheSyncTimer, null);
            if (oldTimer != null)
                oldTimer.Dispose();
        }
    }
}
