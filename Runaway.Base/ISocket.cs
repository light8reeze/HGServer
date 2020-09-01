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
        /// buffer end index
        /// </summary>
        public int endIndex;
    }

    /// <summary>
    /// Accept callback
    /// </summary>
    /// <param name="session">Accepted session</param>
    public delegate void OnAcceptedDelegate(IGameSession session);

    /// <summary>
    /// receive callback
    /// </summary>
    /// <param name="args">receive arguments</param>
    /// <param name="size">received size</param>
    public delegate void OnReceivedDelegate(IOEventArgs args, int size);

    /// <summary>
    /// send callback
    /// </summary>
    /// <param name="args">send IO argunements</param>
    /// <param name="size">sended size</param>
    public delegate void OnSendedDelegate(IOEventArgs args, int size);

    /// <summary>
    /// socket close callback
    /// </summary>
    public delegate void OnClosedDelegate();

    /// <summary>
    /// Socket Interface
    /// </summary>
    public interface ISocket
    {
        /// <summary>
        /// Initialize socket
        /// </summary>
        /// <returns>Result number of initialization</returns>
        int Initialize();

        /// <summary>
        /// accept socket
        /// </summary>
        /// <returns>
        /// result number of acception
        /// </returns>
        int Accept(ISocket socket);

        /// <summary>
        /// Receive IO
        /// </summary>
        /// <param name="args">Event Arguments</param>
        /// <returns>Recieved size</returns>
        int Receive(IOEventArgs args);

        /// <summary>
        /// Send IO
        /// </summary>
        /// <param name="args">Event Arguments</param>
        /// <returns>Sended size</returns>
        int Send(IOEventArgs args);

        /// <summary>
        /// Close socket
        /// </summary>
        void Close();

        /// <summary>
        /// On Accepted event
        /// </summary>
        OnAcceptedDelegate  OnAccepted  { get; set; }

        /// <summary>
        /// On Received event
        /// </summary>
        OnReceivedDelegate  OnReceived  { get; set; }

        /// <summary>
        /// On Sended event
        /// </summary>
        OnSendedDelegate    OnSended    { get; set; }

        /// <summary>
        /// On Closed event
        /// </summary>
        OnClosedDelegate    OnClosed    { get; set; }
    }
}