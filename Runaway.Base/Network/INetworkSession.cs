using System;

namespace Runaway.Base
{

    /// <summary>
    /// Network Session interface
    /// </summary>
    /// <typeparam name="T">Socket Class</typeparam>
    public interface INetworkSession<T> : IDisposable
    {
        #region Session Delegate
        /// <summary>
        /// Session Connect callback
        /// </summary>
        /// <param name="sender">connection event session</param>
        public delegate void SessionConnectionDelegate(object sender);

        /// <summary>
        /// Session Message IO callback
        /// </summary>
        /// <param name="message">IO Completed Message</param>
        /// <param name="sender">IO Completed Object</param>
        public delegate void SessionMessagingDelegate(Message message, object sender);
        #endregion Session Delegate

        #region Method
        /// <summary>
        /// Session Initialize
        /// </summary>
        public void Initialize();

        /// <summary>
        /// Set Session Accept
        /// </summary>
        /// <param name="session">Session that setted accept</param>
        public void Accept(INetworkSession<T> session);

        /// <summary>
        /// Client connect to other endpoint
        /// </summary>
        /// <param name="ipAddress">connection address</param>
        /// <param name="port">connection port</param>
        public void Connect(string ipAddress, int port);

        /// <summary>
        /// Receive Packet Data to session
        /// </summary>
        public void Receive();

        /// <summary>
        /// Send Data to session
        /// </summary>
        public void Send();

        /// <summary>
        /// Disconnect Session
        /// </summary>
        public void Disconnect();
        #endregion Method
    }
}