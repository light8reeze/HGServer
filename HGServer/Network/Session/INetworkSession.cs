using System;
using HGServer.Network.Messages;

namespace HGServer.Network.Session
{
    /// <summary>
    /// Network Session interface
    /// </summary>
    public interface INetworkSession : IDisposable 
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
        public void Send<TMessage>(ref TMessage message) where TMessage : struct;

        /// <summary>
        /// Send Data to session
        /// </summary>
        public void Send(ReadOnlyMemory<byte> messageBuffer);

        /// <summary>
        /// Disconnect Session
        /// </summary>
        public void Disconnect();
        #endregion Method
    }
}