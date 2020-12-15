using System;

namespace Runaway.Base
{
    /// <summary>
    /// Network Session interface
    /// </summary>
    /// <typeparam name="T">Socket Class</typeparam>
    public interface IGameClient<T> : IDisposable
    {
        /// <summary>
        /// Socket Property
        /// </summary>
        public T Client
        {
            get;
            set;
        }

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
        /// <param name="data">Packet buffer</param>
        /// <param name="start">Start buffer index</param>
        /// <param name="size">Receive Size</param>
        /// <returns>Received size</returns>
        public int Receive(byte[] data, int start, int size);

        /// <summary>
        /// Send Data to session
        /// </summary>
        /// <param name="data">Send Packet data</param>
        /// <param name="start">Send Packet start index</param>
        /// <param name="size">Send Data Size</param>
        public void Send(byte[] data, int start, int size);

        /// <summary>
        /// Disconnect Session
        /// </summary>
        public void Disconnect();

        #endregion Method
    }
}