using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.BasePush
{
    public abstract class BasePushFactory : IPushFactory
    {
        /// <summary>
        /// Gets the config file file path.
        /// </summary>
        protected string ConfigFile { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="LogFactoryBase"/> class.
        /// </summary>
        /// <param name="configFile">The config file.</param>
        protected BasePushFactory(string configFile)
        {
            ConfigFile = configFile;
        }
        /// <summary>
        /// Gets the log by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public abstract IPush GetPush(string name);
    }
}
