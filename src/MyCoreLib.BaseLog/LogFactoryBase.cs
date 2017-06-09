using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyCoreLib.BaseLog
{
    /// <summary>
    /// LogFactory Base class
    /// </summary>
    public abstract class LogFactoryBase : ILogFactory
    {
        /// <summary>
        /// Gets the config file file path.
        /// </summary>
        protected string ConfigFile { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the server instance is running in isolation mode and the multiple server instances share the same logging configuration.
        /// </summary>
        protected bool IsSharedConfig { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogFactoryBase"/> class.
        /// </summary>
        /// <param name="configFile">The config file.</param>
        protected LogFactoryBase(string configFile)
        {
            ConfigFile = configFile;
        }

        /// <summary>
        /// Gets the log by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public abstract ILog GetLog(string name);
    }
}
