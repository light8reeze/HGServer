using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace HGServer.Network.Sockets
{
    class TcpAsyncSocket : AsyncSocketBase
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
        public TcpAsyncSocket()
        {
        }

        public TcpAsyncSocket(Socket socket)
        {
            this.socket = socket;
        }

        ~TcpAsyncSocket()
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

        public override async Task AcceptAsync()
        {
            var tcpSocket = new TcpAsyncSocket();
            await AcceptAsync(tcpSocket);
        }

        public override async Task AcceptAsync(object socket)
        {
            try
            {
                var tcpAcceptSocket = socket as TcpAsyncSocket;

                if (socket == null)
                    throw new NullReferenceException("Not Initialized Client");

                if (_disposed)
                    throw new ObjectDisposedException("Socket Disposed");

                if (tcpAcceptSocket == null)
                    throw new NullReferenceException("Invalid TcpSocket");

                var accepted = await SocketTaskExtensions.AcceptAsync(this.socket);
                tcpAcceptSocket.socket = accepted;
                onAccepted?.Invoke(tcpAcceptSocket, this);
            }
            catch (Exception e)
            {
                onAcceptException?.Invoke(this, e);
            }
            finally
            {
                Close();
            }
        }

        public override async Task ConnectAsync(string ipAddress, int port)
        {
            try
            {
                if (socket == null)
                    throw new NullReferenceException("Not Initialized Client");

                if (socket.Connected)
                    throw new Exception("Client Aleready Connected");

                if (_disposed)
                    throw new ObjectDisposedException(ToString());

                await SocketTaskExtensions.ConnectAsync(socket, ipAddress, port);
                onConnected?.Invoke(this);
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

        public override async Task ReceiveAsync(Memory<byte> dataMemory)
        {
            try
            {
                if (socket == null)
                    throw new NullReferenceException("Not Initialized Client");

                if (!socket.Connected)
                    throw new Exception("Client Aleready disconnected");

                if (_disposed)
                    throw new ObjectDisposedException(ToString());

                int receivedSize = await SocketTaskExtensions.ReceiveAsync(socket, dataMemory, SocketFlags.None);
                onReceived?.Invoke(receivedSize, this);
            }
            catch (Exception e)
            {
                onReceiveException?.Invoke(this, e);
            }
            finally
            {
                Close();
            }
        }

        public override async Task SendAsync(ReadOnlyMemory<byte> dataMemory)
        {
            try
            {
                if (socket == null)
                    throw new NullReferenceException("Not Initialized Client");

                if (!socket.Connected)
                    throw new Exception("Client Aleready disconnected");

                if (_disposed)
                    throw new ObjectDisposedException(ToString());

                int sendedSize = await SocketTaskExtensions.SendAsync(socket, dataMemory, SocketFlags.None);
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
