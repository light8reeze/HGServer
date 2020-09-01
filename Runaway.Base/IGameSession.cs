using System;

namespace Runaway.Base
{
    /// <summary>
    /// Network Session interface
    /// </summary>
    public interface IGameSession
    {
        /// <summary>
        /// Session Initialize
        /// </summary>
        /// <returns>Result number of initialization</returns>
        int Initialize();

        /// <summary>
        /// On Session Accept
        /// </summary>
        void OnAccept();

        /// <summary>
        /// Receive Packet Data to session
        /// </summary>
        /// <param name="data">Packet buffer</param>
        /// <param name="start">Start buffer index</param>
        /// <param name="end">End buffer index</param>
        /// <returns>Received size</returns>
        int Receive(byte[] data, int start, int end);

        /// <summary>
        /// Send Data to session
        /// </summary>
        /// <param name="data">Send Packet data</param>
        /// <param name="start">Send Packet start index</param>
        /// <param name="end">Send Packet end index</param>
        void Send(byte[] data, int start, int end);

        /// <summary>
        /// Disconnect Session
        /// </summary>
        void Disconnect();
    }

    /// <summary>
    /// SessionBase class
    /// </summary>
    /// <typeparam name="TSocket">network socket class</typeparam>
    public abstract class SessionBase<TSocket>
        where TSocket : ISocket
    {
    }
}