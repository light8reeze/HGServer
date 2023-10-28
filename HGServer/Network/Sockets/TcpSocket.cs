using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace HGServer.Network.Sockets
{
    class TcpSocket : SocketBase
    {
        #region Properties
        public bool Connected
        {
            get
            {
                if (_socket == null)
                    return false;

                return _socket.Connected;
            }
        }
        #endregion Properties

        #region Data Fields
        private bool _disposed = false;
        #endregion Data Fields

        #region Constructor & Destructor
        public TcpSocket()
        {
        }

        public TcpSocket(Socket socket)
        {
            this._socket = socket;
        }

        ~TcpSocket()
        {
            Dispose(false);
        }
        #endregion Constructor & Destructor

        #region Method
        public override void Initialize()
        {
            if (null != _socket || true == _socket.Connected)
                throw new Exception("Already Initialized");

            if (true == _disposed)
                throw new ObjectDisposedException(ToString());

            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        public override void Accept()
        {
            var tcpSocket = new TcpSocket();
            tcpSocket.Initialize();
            Accept(tcpSocket);
        }

        public override void Accept(object socket)
        {
            try
            {
                if(socket is not TcpSocket)
                    throw new InvalidOperationException("Not Initialized Client");

                var tcpAcceptSocket = socket as TcpSocket;

                if (null == socket)
                    throw new NullReferenceException("Not Initialized Client");

                if (true == _disposed)
                    throw new ObjectDisposedException("Socket Disposed");

                if (null == tcpAcceptSocket)
                    throw new NullReferenceException("Invalid TcpSocket");

                var accepted = this._socket.Accept();
                tcpAcceptSocket._socket = accepted;
                onAccepted?.Invoke(tcpAcceptSocket, this);
            }
            catch(Exception e)
            {
                onAcceptException?.Invoke(this, e);
            }
            finally
            {
                Close();
            }
        }

        public override void Connect(string ipAddress, int port)
        {
            try
            {
                if (null == _socket)
                    throw new NullReferenceException("Not Initialized Client");

                if (true == _socket.Connected)
                    throw new Exception("Client Aleready Connected");

                if (true == _disposed)
                    throw new ObjectDisposedException(ToString());

                _socket.Connect(ipAddress, port);
                onConnected?.Invoke(this);
            }
            catch(Exception e)
            {
                onConnectException?.Invoke(this, e);
            }
            finally
            {
                Close();
            }
        }

        public override void Receive(Memory<byte> dataBuffer)
        {
            try
            {
                if (null == _socket)
                    throw new NullReferenceException("Not Initialized Client");

                if (false == _socket.Connected)
                    throw new Exception("Client Aleready disconnected");

                if (true == _disposed)
                    throw new ObjectDisposedException(ToString());

                var receivedSize = _socket.Receive(dataBuffer.Span, SocketFlags.None);
                onReceived?.Invoke(receivedSize, this);
            }
            catch(Exception e)
            {
                onReceiveException?.Invoke(this, e);
            }
            finally
            {
                Close();
            }
        }

        public override void Send(ReadOnlyMemory<byte> dataBuffer)
        {
            try
            {
                if (null == _socket)
                    throw new NullReferenceException("Not Initialized Client");

                if (false == _socket.Connected)
                    throw new Exception("Client Aleready disconnected");

                if (true == _disposed)
                    throw new ObjectDisposedException(ToString());

                int sendedSize = _socket.Send(dataBuffer.Span, SocketFlags.None);
                onSended?.Invoke(sendedSize, this);
            }
            catch (Exception e)
            {
                onSendException?.Invoke(this, e);
            }
            finally
            {
                Close();
            }
        }
        #endregion Method

        #region Implement IDisposable
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (true == _disposed)
                return;

            if (true == disposing)
            {
                _socket?.Dispose();
            }

            _disposed = true;
        }
        #endregion Implement IDisposable
    }
}
