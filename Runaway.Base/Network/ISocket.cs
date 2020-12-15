using System;

namespace Runaway.Base
{
    /// <summary>
    /// Socket IO Event Arguments
    /// </summary>
    public class IOEventArgs
    {
        /// <summary>
        /// packet buffer
        /// </summary>
        public byte[] buffer;

        /// <summary>
        /// buffer Start index
        /// </summary>
        public int startIndex;

        /// <summary>
        /// IO Size
        /// </summary>
        public int size;
    }

    #region Socket Delegate
    /// <summary>
    /// Accept callback
    /// </summary>
    /// <param name="acceptedSocket"></param>
    /// <param name="sender">event object</param>
    public delegate void OnAcceptedDelegate(object acceptedSocket, object sender);

    /// <summary>
    /// socket connect callback
    /// </summary>
    public delegate void OnConnectedDelegate(object sender);

    /// <summary>
    /// receive callback
    /// </summary>
    /// <param name="args">receive arguments</param>
    /// <param name="sender">event object</param>
    public delegate void OnReceivedDelegate(IOEventArgs args, object sender);

    /// <summary>
    /// send callback
    /// </summary>
    /// <param name="args">send IO argunements</param>
    /// <param name="sender">event object</param>
    public delegate void OnSendedDelegate(IOEventArgs args, object sender);

    /// <summary>
    /// socket close callback
    /// </summary>
    /// <param name="sender">event object</param>
    public delegate void OnClosedDelegate(object sender);

    #endregion Socket Delegate

    /// <summary>
    /// Socket Interface
    /// </summary>
    public interface ISocket : IDisposable
    {
        #region Method
        /// <summary>
        /// Initialize socket
        /// </summary>
        /// <returns>Result number of initialization</returns>
        void Initialize();

        /// <summary>
        /// accept socket
        /// </summary>
        void Accept();

        /// <summary>
        /// Connect socket
        /// </summary>
        /// <param name="ipAddress">connect address</param>
        /// <param name="port">connect port</param>
        void Connect(string ipAddress, int port);

        /// <summary>
        /// Receive Data from network
        /// </summary>
        /// <param name="buffer">data buffer</param>
        /// <param name="offset">data buffer offset</param>
        /// <param name="size">data buffer size</param>
        void Receive(byte[] buffer, int offset, int size);

        /// <summary>
        /// Send Data
        /// </summary>
        /// <param name="buffer">data buffer</param>
        /// <param name="offset">data buffer offset</param>
        /// <param name="size">data buffer size</param>
        void Send(byte[] buffer, int offset, int size);

        /// <summary>
        /// Close socket
        /// </summary>
        void Close();
        #endregion Method
    }
}