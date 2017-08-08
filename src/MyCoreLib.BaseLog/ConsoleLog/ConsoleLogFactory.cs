using System;

namespace MyCoreLib.BaseLog.ConsoleLog
{
    /// <summary>
    /// Console log factory
    /// </summary>
    public class ConsoleLogFactory : LogFactoryBase
    {
        /// <summary>
        /// Gets the log by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public override ILog GetLog(string name)
        {
            var _log = ConsoleLog.Instance;
            _log.Name = name;
            return _log;
        }
    }
}
