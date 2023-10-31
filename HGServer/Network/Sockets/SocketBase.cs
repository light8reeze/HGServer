using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using HGServer.Utility;

namespace HGServer.Network.Sockets
{
    abstract class SocketBase : IDisposable
    {
        #region Socket Delegate
        /// <summary>
        /// Accept callback
        /// </summary>
        /// <param name="acceptedSocket"></param>
        /// <param name="sender">event object</param>
        public delegate void AcceptedHandler(SocketBase acceptedSocket, SocketBase sender);

        /// <summary>
        /// socket connect callback
        /// </summary>
        public delegate void ConnectedHandler(SocketBase sender);

        /// <summary>
        /// socket Disconnect callback
        /// </summary>
        public delegate void DisconnectedHandler(SocketBase sender);

        /// <summary>
        /// receive callback
        /// </summary>
        /// <param name="dataSize">received size</param>
        /// <param name="sender">event object</param>
        public delegate void ReceivedHandler(int dataSize, SocketBase sender);

        /// <summary>
        /// send callback
        /// </summary>
        /// <param name="dataSize">sended size</param>
        /// <param name="sender">event object</param>
        public delegate void SendedHandler(int dataSize, SocketBase sender);

        /// <summary>
        /// socket close callback
        /// </summary>
        /// <param name="sender">event object</param>
        public delegate void ClosedHandler(SocketBase sender);
        #endregion Socket Delegate

        #region Event
        /// <summary>
        /// On Accepted event
        /// </summary>
        protected AcceptedHandler    onAccepted;
        public event AcceptedHandler OnAccepted
        {
            add => onAccepted += value;
            remove => onAccepted -= value;
        }

        /// <summary>
        /// On Accept event
        /// </summary>
        protected AcceptedHandler onAccept;
        public event AcceptedHandler OnAccept
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
        protected ReceivedHandler    onReceived;
        public event ReceivedHandler OnReceived
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
        protected SendedHandler      onSended;
        public event SendedHandler   OnSended
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
        protected ClosedHandler      onClosed;
        public event ClosedHandler   OnClosed
        {
            add => onClosed += value;
            remove => onClosed -= value;
        }

        /// <summary>
        /// On Connected Event
        /// </summary>
        protected ConnectedHandler       onConnected;
        public event ConnectedHandler    OnConnected
        {
            add => onConnected += value;
            remove => onConnected -= value;
        }

        /// <summary>
        /// On Connected Event
        /// </summary>
        protected DisconnectedHandler onDisconnected;
        public event DisconnectedHandler OnDisconnected
        {
            add => onDisconnected += value;
            remove => onDisconnected -= value;
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


        #region Data Fields
        protected System.Net.Sockets.Socket _socket;
        #endregion Data Fields 

        #region Method

        public virtual void Bind(string ipAddress, int port)
        {
            var address = IPAddress.Parse(ipAddress);
            var iPEndPoint = new IPEndPoint(address, port);

            _socket.Bind(iPEndPoint);
        }
        public virtual void Listen(int backLog = int.MaxValue)
        {
            if (_socket == null)
                throw new NullReferenceException("Not Initialized socket");

            if (_socket.Connected)
                throw new Exception("Client Aleready Connected");

            _socket.Listen(backLog);
        }
        public virtual void Close()
        {
            if (_socket == null)
                throw new NullReferenceException("Not Initialized Client");

            if (!_socket.Connected)
                throw new Exception("Client Aleready disconnected");

            _socket.Close();
            onClosed?.Invoke(this);
        }
        #endregion Method

        #region Abstract Method
        public abstract void Initialize();
        public abstract void Accept();
        public abstract void Accept(SocketBase socket);
        public abstract void Connect(string ipAddress, int port);
        public abstract void Disconnect();
        public abstract void Dispose();
        public abstract void Receive(Memory<byte> dataMemory);
        public abstract void Send(ReadOnlyMemory<byte> dataMemory);
        #endregion Abstract Method
    }
}
