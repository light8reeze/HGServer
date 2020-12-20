using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace Runaway.Base
{
    class SocketBase : ISocket
    {
        #region Data Fields
        protected Socket socket;
        #endregion Data Fields

        #region Event
        /// <summary>
        /// On Accepted event
        /// </summary>
        protected ISocket.OnAcceptedDelegate    onAccepted;
        public event ISocket.OnAcceptedDelegate OnAccepted
        {
            add => onAccepted += value;
            remove => onAccepted -= value;
        }

        /// <summary>
        /// On Received event
        /// </summary>
        protected ISocket.OnReceivedDelegate    onReceived;
        public event ISocket.OnReceivedDelegate OnReceived
        {
            add => onReceived += value;
            remove => onReceived -= value;
        }

        /// <summary>
        /// On Sended event
        /// </summary>
        protected ISocket.OnSendedDelegate      onSended;
        public event ISocket.OnSendedDelegate   OnSended
        {
            add => onSended += value;
            remove => onSended -= value;
        }

        /// <summary>
        /// On Closed event
        /// </summary>
        protected ISocket.OnClosedDelegate      onClosed;
        public event ISocket.OnClosedDelegate   OnClosed
        {
            add => onClosed += value;
            remove => onClosed -= value;
        }

        /// <summary>
        /// On Connected Event
        /// </summary>
        protected ISocket.OnConnectedDelegate       onConnected;
        public event ISocket.OnConnectedDelegate    OnConnected
        {
            add => onConnected += value;
            remove => onConnected -= value;
        }
        #endregion Event

        #region Method
        public virtual void Accept()
        {
            throw new NotImplementedException();
        }

        public virtual void Close()
        {
            throw new NotImplementedException();
        }

        public virtual void Connect(string ipAddress, int port)
        {
            throw new NotImplementedException();
        }

        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }

        public virtual void Initialize()
        {
            throw new NotImplementedException();
        }

        public virtual void Receive(byte[] buffer, int offset, int size)
        {
            throw new NotImplementedException();
        }

        public virtual void Send(byte[] buffer, int offset, int size)
        {
            throw new NotImplementedException();
        }
        #endregion Method
    }
}
