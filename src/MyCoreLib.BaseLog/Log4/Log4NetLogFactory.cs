﻿using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace MyCoreLib.BaseLog.Log4
{
    /// <summary>
    /// Log4NetLogFactory
    /// </summary>
    public class Log4NetLogFactory : LogFactoryBase
    { /// <summary>
      /// Initializes a new instance of the <see cref="Log4NetLogFactory"/> class.
      /// </summary>
        public Log4NetLogFactory()
            : this("log4net.config")
        {

        }

        private log4net.Repository.ILoggerRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetLogFactory"/> class.
        /// </summary>
        /// <param name="log4netConfig">The log4net config.</param>
        public Log4NetLogFactory(string log4netConfig)
            : base(log4netConfig)
        {
            repository = log4net.LogManager.CreateRepository("MyRepository");
            if (!IsSharedConfig)
            {
                log4net.Config.XmlConfigurator.Configure(repository, new FileInfo(ConfigFile));
            }
            else
            {
                //Disable Performance logger
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(File.ReadAllText(log4netConfig));
                var docElement = xmlDoc.DocumentElement;
                //var perfLogNode = docElement.SelectSingleNode("logger[@name='Performance']");
                //if (perfLogNode != null)
                //    docElement.RemoveChild(perfLogNode);

                log4net.Config.XmlConfigurator.Configure(repository, docElement);
            }
        }

        /// <summary>
        /// Gets the log by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public override ILog GetLog(string name)
        {
            return new Log4NetLog(LogManager.GetLogger(repository.Name, name));
        }
    }
}
