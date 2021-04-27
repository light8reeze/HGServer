using System;
using HGServer.Network.Packet;

namespace HGServer.Network.Session
{
    /// <summary>
    /// Network Session interface
    /// </summary>
    /// <typeparam name="T">Socket Class</typeparam>
    public interface INetworkSession<T> : IDisposable
    {
        #region Method
        /// <summary>
        /// Session Initialize
        /// </summary>
        public void Initialize();

        /// <summary>
        /// Set Session Accept
        /// </summary>
        public void Accept();

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
        public void Send(Message message);

        /// <summary>
        /// Disconnect Session
        /// </summary>
        public void Disconnect();
        #endregion Method
    }
}