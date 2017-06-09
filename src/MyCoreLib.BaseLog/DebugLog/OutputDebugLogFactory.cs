namespace MyCoreLib.BaseLog.DebugLog
{
    /// <summary>
    /// Console log factory
    /// </summary>
    public class SystemDBLoggFactory : ILogFactory
    {
        /// <summary>
        /// Gets the log by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public ILog GetLog(string name)
        {
            var _log = OutputDebugLog.Instance;
            _log.Name = name;
            return _log;
        }
    }
}
