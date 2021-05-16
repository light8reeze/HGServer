using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using HGServer.Utility;

namespace HGServer.Network.Sockets
{
    abstract class SocketBase : ISocket
    {
        #region Data Fields
        protected Socket        socket;
        #endregion Data Fields

        #region Event
        /// <summary>
        /// On Accepted event
        /// </summary>
        protected ISocket.AcceptedHandler    onAccepted;
        public event ISocket.AcceptedHandler OnAccepted
        {
            add => onAccepted += value;
            remove => onAccepted -= value;
        }

        /// <summary>
        /// On Accept event
        /// </summary>
        protected ISocket.AcceptedHandler onAccept;
        public event ISocket.AcceptedHandler OnAccept
        {
            add => onAccept += value;
            remove => onAccept -= value;
        }

        /// <summary>
        /// On Accept Excpetion event
        /// </summary>
        protected ExceptionEventHandler onAcceptException;
        public event ExceptionEventHandler OnAcceptException
        {
            add => onAcceptException += value;
            remove => onAcceptException -= value;
        }

        /// <summary>
        /// On Received event
        /// </summary>
        protected ISocket.ReceivedHandler    onReceived;
        public event ISocket.ReceivedHandler OnReceived
        {
            add => onReceived += value;
            remove => onReceived -= value;
        }

        /// <summary>
        /// On Receive Excpetion event
        /// </summary>
        protected ExceptionEventHandler onReceiveException;
        public event ExceptionEventHandler OnReceiveException
        {
            add => onReceiveException += value;
            remove => onReceiveException -= value;
        }

        /// <summary>
        /// On Sended event
        /// </summary>
        protected ISocket.SendedHander      onSended;
        public event ISocket.SendedHander   OnSended
        {
            add => onSended += value;
            remove => onSended -= value;
        }

        /// <summary>
        /// On Send Excpetion event
        /// </summary>
        protected ExceptionEventHandler onSendException;
        public event ExceptionEventHandler OnSendException
        {
            add => onSendException += value;
            remove => onSendException -= value;
        }

        /// <summary>
        /// On Closed event
        /// </summary>
        protected ISocket.ClosedHandler      onClosed;
        public event ISocket.ClosedHandler   OnClosed
        {
            add => onClosed += value;
            remove => onClosed -= value;
        }

        /// <summary>
        /// On Connected Event
        /// </summary>
        protected ISocket.ConnectedHandler       onConnected;
        public event ISocket.ConnectedHandler    OnConnected
        {
            add => onConnected += value;
            remove => onConnected -= value;
        }

        /// <summary>
        /// On Connect Excpetion event
        /// </summary>
        protected ExceptionEventHandler onConnectException;
        public event ExceptionEventHandler OnConnectException
        {
            add => onConnectException += value;
            remove => onConnectException -= value;
        }
        #endregion Event

        #region Method
        public virtual void Bind(string ipAddress, int port)
        {
            var address = IPAddress.Parse(ipAddress);
            var iPEndPoint = new IPEndPoint(address, port);

            socket.Bind(iPEndPoint);
        }
        public virtual void Listen(int backLog = int.MaxValue)
        {
            if (socket == null)
                throw new NullReferenceException("Not Initialized socket");

            if (socket.Connected)
                throw new Exception("Client Aleready Connected");

            socket.Listen(backLog);
        }
        public virtual void Close()
        {
            if (socket == null)
                throw new NullReferenceException("Not Initialized Client");

            if (!socket.Connected)
                throw new Exception("Client Aleready disconnected");

            socket.Close();
            onClosed?.Invoke(this);
        }
        #endregion Method

        #region Abstract Method
        public abstract void Initialize();
        public abstract void Accept();
        public abstract void Accept(object socket);
        public abstract void Connect(string ipAddress, int port);
        public abstract void Dispose();
        public abstract void Receive(Span<byte> dataBuffer);
        public abstract void Send(ReadOnlySpan<byte> dataBuffer);
        #endregion Abstract Method
    }
}
