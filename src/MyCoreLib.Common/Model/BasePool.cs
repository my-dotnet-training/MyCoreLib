using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MyCoreLib.Common.Model
{
    public abstract class BasePool<T> : IDisposable
    {
        public int MaxCount { get; set; }
        public ConcurrentQueue<T> Queue { get; set; }

        public virtual void Dispose()
        {
        }
    }
}
