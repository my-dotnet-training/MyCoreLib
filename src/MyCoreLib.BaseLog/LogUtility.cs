using MyCoreLib.BaseLog.Log4;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.BaseLog
{
    public static class LogUtility
    {
        /// <summary>
        /// Gets or sets the log factory.
        /// </summary>
        /// <value>
        /// The log factory.
        /// </value>
        private static ILoggerProvider m_logFactory;

        public static ILoggerProvider SetupLogFactory()
        {
            m_logFactory = new Log4NetLogFactory();
            return m_logFactory;
        }

        /// <summary>
        /// Creates the logger for the AppServer.
        /// </summary>
        /// <param name="loggerName">Name of the logger.</param>
        /// <returns></returns>
        public static ILog CreateLogger(string loggerName)
        {
            return m_logFactory.GetLog(loggerName);
        }
    }
}
