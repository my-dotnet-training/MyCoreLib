using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MyCoreLib.BaseSocket.Base
{
    /// <summary>
    /// The basic session interface
    /// </summary>
    public interface ISessionBase
    {

        /// <summary>
        /// Gets the session ID.
        /// </summary>
       string SessionID { get; }

        /// <summary>
        /// Gets the remote endpoint.
        /// </summary>
        IPEndPoint RemoteEndPoint { get; }
    }
}
