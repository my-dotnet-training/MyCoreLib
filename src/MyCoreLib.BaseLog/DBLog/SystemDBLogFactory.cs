namespace MyCoreLib.BaseLog.DBLog
{
    /// <summary>
    /// Console log factory
    /// </summary>
    public class SystemDBLogFactory : LogFactoryBase
    {
        /// <summary>
        /// Gets the log by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public override ILog GetLog(string name)
        {
            var _log = SystemDBLog.Instance;
            _log.Name = name;
            return _log;
        }
    }
}
