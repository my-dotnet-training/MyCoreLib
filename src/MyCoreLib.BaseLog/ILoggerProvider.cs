
namespace MyCoreLib.BaseLog
{
    /// <summary>
    /// LogFactory Interface
    /// </summary>
    public interface ILoggerProvider
    {
        /// <summary>
        /// Gets the log by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        ILog GetLog(string name);
    }
}
