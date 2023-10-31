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

        public TcpSocket(System.Net.Sockets.Socket socket)
        {
            _socket = socket;
        }

        ~TcpSocket()
        {
            Dispose(false);
        }
        #endregion Constructor & Destructor

        #region Method
        public override void Initialize()
        {
            if (_socket is not null || _socket.Connected is true)
                throw new Exception("Already Initialized");

            if (_disposed is true)
                throw new ObjectDisposedException(ToString());

            _socket = new System.Net.Sockets.Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        public override void Accept()
        {
            var tcpSocket = new TcpSocket();
            tcpSocket.Initialize();
            Accept(tcpSocket);
        }

        public override void Accept(SocketBase socket)
        {
            try
            {
                if(socket is not TcpSocket)
                    throw new InvalidOperationException("Not Initialized Client");

                var tcpAcceptSocket = socket as TcpSocket;

                if (socket is null)
                    throw new NullReferenceException("Not Initialized Client");

                if (_disposed is true)
                    throw new ObjectDisposedException("Socket Disposed");

                if (tcpAcceptSocket is null)
                    throw new NullReferenceException("Invalid TcpSocket");

                var accepted = _socket.Accept();
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
                if (_socket is null)
                    throw new NullReferenceException("Not Initialized Client");

                if (_socket.Connected is true)
                    throw new Exception("Client Aleready Connected");

                if (_disposed is true)
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

        public override void Disconnect()
        {
            try
            {
                if (_socket is null)
                    throw new NullReferenceException("Not Initialized Client");

                if (_socket.Connected is true)
                    throw new Exception("Client Aleready Connected");

                if (_disposed is true)
                    throw new ObjectDisposedException(ToString());

                _socket.Disconnect(true);
                onDisconnected?.Invoke(this);
            }
            catch (Exception e)
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
                if (_socket is null)
                    throw new NullReferenceException("Not Initialized Client");

                if (_socket.Connected is false)
                    throw new Exception("Client Aleready disconnected");

                if (_disposed is true)
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
                if (_socket is null)
                    throw new NullReferenceException("Not Initialized Client");

                if (_socket.Connected is false)
                    throw new Exception("Client Aleready disconnected");

                if (_disposed is true)
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
