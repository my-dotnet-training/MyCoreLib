using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.Common.Model
{
    public abstract class BaseDisposable : IDisposable
    {
        private bool _disposed = false;

        protected abstract void DoDispose();

        public void Dispose()
        {
            if (!_disposed)
            {
                DoDispose();
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        ~BaseDisposable()
        {
            if (!_disposed)
            {
                DoDispose();
                _disposed = true;
            }
        }
    }
}
