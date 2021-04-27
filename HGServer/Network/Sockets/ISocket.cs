using System;

namespace HGServer.Network.Sockets
{
    /// <summary>
    /// Socket Interface
    /// </summary>
    public interface ISocket : IDisposable
    {
        #region Socket Delegate
        /// <summary>
        /// Accept callback
        /// </summary>
        /// <param name="acceptedSocket"></param>
        /// <param name="sender">event object</param>
        public delegate void AcceptedHandler(object acceptedSocket, object sender);

        /// <summary>
        /// socket connect callback
        /// </summary>
        public delegate void ConnectedHandler(object sender);

        /// <summary>
        /// receive callback
        /// </summary>
        /// <param name="dataSize">received size</param>
        /// <param name="sender">event object</param>
        public delegate void ReceivedHandler(int dataSize, object sender);

        /// <summary>
        /// send callback
        /// </summary>
        /// <param name="dataSize">sended size</param>
        /// <param name="sender">event object</param>
        public delegate void SendedHander(int dataSize, object sender);

        /// <summary>
        /// socket close callback
        /// </summary>
        /// <param name="sender">event object</param>
        public delegate void ClosedHandler(object sender);
        #endregion Socket Delegate

        #region Method
        /// <summary>
        /// Initialize socket
        /// </summary>
        /// <returns>Result number of initialization</returns>
        void Initialize();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        void Bind(string ipAddress, int port);

        /// <summary>
        /// Socket Listen
        /// </summary>
        /// <param name="backLog">count of backlog</param>
        void Listen(int backLog);

        /// <summary>
        /// accept socket
        /// </summary>
        void Accept();

        /// <summary>
        /// accept socket
        /// </summary>
        /// <param name="socket">socket that will accepted</param>
        void Accept(object socket);

        /// <summary>
        /// Connect socket
        /// </summary>
        /// <param name="ipAddress">connect address</param>
        /// <param name="port">connect port</param>
        void Connect(string ipAddress, int port);

        /// <summary>
        /// Receive Data from network
        /// </summary>
        /// <param name="dataMemory">data buffer memory</param>
        void Receive(Memory<byte> dataMemory);

        /// <summary>
        /// Send Data
        /// </summary>
        /// <param name="dataMemory">send data buffer memory</param>
        void Send(ReadOnlyMemory<byte> dataMemory);

        /// <summary>
        /// Close socket
        /// </summary>
        void Close();
        #endregion Method
    }
}