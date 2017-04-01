using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace MyCoreLib.BaseCache.LocalCached
{
    public class BaseTimerCache : BaseCache, IDisposable
    {
        private Timer _dataRefreshTimer;

        private static readonly object s_instanceLock = new object();
        private static BaseTimerCache s_instance;

        public static BaseTimerCache GetInstance(Action<Hashtable> callback)
        {
            if (s_instance == null)
            {
                BaseTimerCache cache = new BaseTimerCache(callback);
                if (Interlocked.CompareExchange(ref s_instance, cache, null) != null)
                {
                    cache.Dispose();
                }
                else
                {
                    cache.StartTimer();
                }
            }
            return s_instance;
        }
        private BaseTimerCache(Action<Hashtable> callback)
        {
            _dataTable = new Hashtable();
            _dataRefreshTimer = new Timer(DoRefresh, callback, Timeout.Infinite, Timeout.Infinite);
        }

        private void StartTimer()
        {
            _dataRefreshTimer.Change(2000, Timeout.Infinite);
        }

        /// <summary>
        /// main function
        /// </summary>
        /// <param name="state"></param>
        private void DoRefresh(object state)
        {
            try
            {
                ((Action<Hashtable>)state).Invoke(_dataTable);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Failed to refresh data groups: {0}", ex));
            }
            finally
            {
                _dataRefreshTimer.Change(2 * 60 * 1000, Timeout.Infinite);
            }
        }

        public void Dispose()
        {
            Timer oldTimer = Interlocked.Exchange(ref _dataRefreshTimer, null);
            if (oldTimer != null) oldTimer.Dispose();
            if (_dataTable != null)
            {
                _dataTable.Clear();
                _dataTable = null;
            }
        }
    }
}
