using System;

namespace MyCoreLib.BaseCache.Exceptions
{
    /// <summary>
    /// The exception could be raised when a cache is already out of sync with the original data source.
    /// </summary>
    public class CacheOutOfSyncException : Exception
    {
        public CacheOutOfSyncException(string message, Exception lastSyncError)
            : base(message, lastSyncError)
        { }
    }
}
