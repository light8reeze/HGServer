using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace Runaway.Base
{
    class TcpSocket : ISocket
    {
        #region Properties
        public Socket Socket
        {
            get;
            private set;
        }

        public bool Connected
        {
            get
            {
                if (Socket == null)
                    return false;

                return Socket.Connected;
            }
        }
        #endregion Properties

        #region Event
        /// <summary>
        /// On Accepted event
        /// </summary>
        private OnAcceptedDelegate _onAccepted;
        public event OnAcceptedDelegate OnAccepted
        {
            add => _onAccepted += value;
            remove => _onAccepted -= value;
        }

        /// <summary>
        /// On Received event
        /// </summary>
        private OnReceivedDelegate _onReceived;
        public event OnReceivedDelegate OnReceived
        {
            add => _onReceived += value;
            remove => _onReceived -= value;
        }

        /// <summary>
        /// On Sended event
        /// </summary>
        private OnSendedDelegate _onSended;
        public event OnSendedDelegate OnSended
        {
            add => _onSended += value;
            remove => _onSended -= value;
        }

        /// <summary>
        /// On Closed event
        /// </summary>
        private OnClosedDelegate _onClosed;
        public event OnClosedDelegate OnClosed
        {
            add => _onClosed += value;
            remove => _onClosed -= value;
        }

        /// <summary>
        /// On Connected Event
        /// </summary>
        private OnConnectedDelegate _onConnected;
        public event OnConnectedDelegate OnConnected
        {
            add => _onConnected += value;
            remove => _onConnected -= value;
        }
        #endregion Event

        #region Data Fields
        private bool _disposed = false;
        #endregion Data Fields

        #region Constructor & Destructor
        public TcpSocket()
        {
        }

        public TcpSocket(Socket socket)
        {
            Socket = socket;
        }

        ~TcpSocket()
        {
            Dispose(false);
        }
        #endregion Constructor & Destructor

        #region Private Method
        private bool CheckDataBuffer(byte[] buffer, int offset, int size)
        {
            if (buffer == null)
                return false;

            if (offset < 0 || buffer.Length <= offset)
                return false;

            if (buffer.Length < offset + size || size < 0)
                return false;

            return true;
        }
        #endregion Private Method

        #region Implement ISocket
        public void Accept()
        {
            if (Socket == null) throw new NullReferenceException("Not Initialized Client");
            if (_disposed) throw new ObjectDisposedException(ToString());

            var accepted = Socket.Accept();
            TcpSocket tcpSocket = new TcpSocket(accepted);
            _onAccepted?.Invoke(tcpSocket, this);
        }

        public void Connect(string ipAddress, int port)
        {
            if (Socket == null) throw new NullReferenceException("Not Initialized Client");
            if (Socket.Connected) throw new Exception("Client Aleready Connected");
            if (_disposed) throw new ObjectDisposedException(ToString());

            Socket.Connect(ipAddress, port);
            _onConnected?.Invoke(this);
        }

        public void Close()
        {
            if (Socket == null) throw new NullReferenceException("Not Initialized Client");
            if (!Socket.Connected) throw new Exception("Client Aleready disconnected");
            if (_disposed) throw new ObjectDisposedException(ToString());

            Socket.Close();
            _onClosed?.Invoke(this);
        }

        public void Initialize()
        {
            if (Socket != null || Socket.Connected) throw new Exception("Already Initialized");
            if (_disposed) throw new ObjectDisposedException(ToString());

            if (Socket == null)
                Socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        public void Receive(byte[] buffer, int offset, int size)
        {
            if (buffer == null) throw new NullReferenceException();
            if (!CheckDataBuffer(buffer, offset, size)) throw new IndexOutOfRangeException();
            if (Socket == null) throw new NullReferenceException("Not Initialized Client");
            if (!Socket.Connected) throw new Exception("Client Aleready disconnected");
            if (_disposed) throw new ObjectDisposedException(ToString());

            int receivedSize = Socket.Receive(buffer, offset, size, SocketFlags.None);
            IOEventArgs args = new IOEventArgs
            {
                buffer = buffer,
                startIndex = offset,
                size = receivedSize
            };
            _onReceived?.Invoke(args, this);
        }

        public void Send(byte[] buffer, int offset, int size)
        {
            if (buffer == null) throw new NullReferenceException();
            if (!CheckDataBuffer(buffer, offset, size)) throw new IndexOutOfRangeException();
            if (Socket == null) throw new NullReferenceException("Not Initialized Client");
            if (!Socket.Connected) throw new Exception("Client Aleready disconnected");
            if (_disposed) throw new ObjectDisposedException(ToString());

            int sendedSize = Socket.Send(buffer, offset, size, SocketFlags.None);
            IOEventArgs args = new IOEventArgs
            {
                buffer = buffer,
                startIndex = offset,
                size = sendedSize
            };
            _onSended?.Invoke(args, this);
        }
        #endregion Implement ISocket

        #region Implement IDisposable
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                Socket?.Dispose();
            }

            _disposed = true;
        }
        #endregion Implement IDisposable
    }
}
