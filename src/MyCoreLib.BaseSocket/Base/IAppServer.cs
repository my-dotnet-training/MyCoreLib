
using MyCoreLib.BaseLog.Log4;
using System;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace MyCoreLib.BaseSocket.Base
{
    public interface IAppServer: IWorkItem, ILoggerProvider
    {
        /// <summary>
        /// Gets the started time.
        /// </summary>
        /// <value>
        /// The started time.
        /// </value>
        DateTime StartedTime { get; }

        /// <summary>
        /// Gets or sets the listeners.
        /// </summary>
        /// <value>
        /// The listeners.
        /// </value>
        ListenerInfo[] Listeners { get; }

        /// <summary>
        /// Gets the Receive filter factory.
        /// </summary>
        object ReceiveFilterFactory { get; }

        /// <summary>
        /// Gets the certificate of current server.
        /// </summary>
        X509Certificate Certificate { get; }

        /// <summary>
        /// Gets the transfer layer security protocol.
        /// </summary>
        SslProtocols BasicSecurity { get; }

        /// <summary>
        /// Creates the app session.
        /// </summary>
        /// <param name="socketSession">The socket session.</param>
        /// <returns></returns>
        IAppSession CreateAppSession(ISocketSession socketSession);

        /// <summary>
        /// Registers the new created app session into the appserver's session container.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        bool RegisterSession(IAppSession session);
        /// <summary>
        /// Gets the app session by ID.
        /// </summary>
        /// <param name="sessionID">The session ID.</param>
        /// <returns></returns>
        IAppSession GetSessionByID(string sessionID);
        /// <summary>
        /// Resets the session's security protocol.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="security">The security protocol.</param>
        void ResetSessionSecurity(IAppSession session, SslProtocols security);
        /// <summary>
        /// Gets the log factory.
        /// </summary>
        ILogFactory LogFactory { get; }
    }
}
