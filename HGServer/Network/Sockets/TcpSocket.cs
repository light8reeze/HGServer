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
                if (socket == null)
                    return false;

                return socket.Connected;
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
            this.socket = socket;
        }

        ~TcpSocket()
        {
            Dispose(false);
        }
        #endregion Constructor & Destructor

        #region Method
        public override void Initialize()
        {
            if (socket != null || socket.Connected)
                throw new Exception("Already Initialized");

            if (_disposed)
                throw new ObjectDisposedException(ToString());

            if (socket == null)
                socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        public override void Accept()
        {
            var tcpSocket = new TcpSocket();
            Accept(tcpSocket);
        }

        public override void Accept(object socket)
        {
            try
            {
                var tcpAcceptSocket = socket as TcpSocket;

                if (socket == null)
                    throw new NullReferenceException("Not Initialized Client");

                if (_disposed)
                    throw new ObjectDisposedException("Socket Disposed");

                if (tcpAcceptSocket == null)
                    throw new NullReferenceException("Invalid TcpSocket");

                var accepted = this.socket.Accept();
                tcpAcceptSocket.socket = accepted;
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
                if (socket == null)
                    throw new NullReferenceException("Not Initialized Client");

                if (socket.Connected)
                    throw new Exception("Client Aleready Connected");

                if (_disposed)
                    throw new ObjectDisposedException(ToString());

                socket.Connect(ipAddress, port);
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

        public override void Receive(Span<byte> dataBuffer)
        {
            try
            {
                if (socket == null)
                    throw new NullReferenceException("Not Initialized Client");

                if (!socket.Connected)
                    throw new Exception("Client Aleready disconnected");

                if (_disposed)
                    throw new ObjectDisposedException(ToString());

                var receivedSize = socket.Receive(dataBuffer, SocketFlags.None);
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

        public override void Send(ReadOnlySpan<byte> dataBuffer)
        {
            try
            {
                if (socket == null)
                    throw new NullReferenceException("Not Initialized Client");

                if (!socket.Connected)
                    throw new Exception("Client Aleready disconnected");

                if (_disposed)
                    throw new ObjectDisposedException(ToString());

                var sendedSize = socket.Send(dataBuffer, SocketFlags.None);
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
            if (_disposed)
                return;

            if (disposing)
            {
                socket?.Dispose();
            }

            _disposed = true;
        }
        #endregion Implement IDisposable
    }
}
