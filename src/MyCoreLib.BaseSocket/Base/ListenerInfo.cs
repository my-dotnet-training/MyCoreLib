﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Authentication;
using System.Text;

namespace MyCoreLib.BaseSocket.Base
{
    /// <summary>
    /// Listener inforamtion
    /// </summary>
    public class ListenerInfo
    {
        /// <summary>
        /// Gets or sets the listen endpoint.
        /// </summary>
        /// <value>
        /// The end point.
        /// </value>
        public IPEndPoint EndPoint { get; set; }

        /// <summary>
        /// Gets or sets the listen backlog.
        /// </summary>
        /// <value>
        /// The back log.
        /// </value>
        public int BackLog { get; set; }

        /// <summary>
        /// Gets or sets the security protocol.
        /// </summary>
        /// <value>
        /// The security.
        /// </value>
        public SslProtocols Security { get; set; }
    }
}
