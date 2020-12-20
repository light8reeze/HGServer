using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace Runaway.Base
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
        public override void Accept()
        {
            if (socket == null) 
                throw new NullReferenceException("Not Initialized Client");
            
            if (_disposed) 
                throw new ObjectDisposedException(ToString());

            var accepted = socket.Accept();
            TcpSocket tcpSocket = new TcpSocket(accepted);
            onAccepted?.Invoke(tcpSocket, this);
        }

        public override void Connect(string ipAddress, int port)
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

        public override void Close()
        {
            if (socket == null) 
                throw new NullReferenceException("Not Initialized Client");
            
            if (!socket.Connected) 
                throw new Exception("Client Aleready disconnected");
            
            if (_disposed) 
                throw new ObjectDisposedException(ToString());

            socket.Close();
            onClosed?.Invoke(this);
        }

        public override void Initialize()
        {
            if (socket != null || socket.Connected) 
                throw new Exception("Already Initialized");
            
            if (_disposed) 
                throw new ObjectDisposedException(ToString());

            if (socket == null)
                socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        public override void Receive(byte[] buffer, int offset, int size)
        {
            if (buffer == null) 
                throw new NullReferenceException();
            
            if (!CheckDataBuffer(buffer, offset, size)) 
                throw new IndexOutOfRangeException();
            
            if (socket == null) 
                throw new NullReferenceException("Not Initialized Client");
            
            if (!socket.Connected) 
                throw new Exception("Client Aleready disconnected");
            
            if (_disposed) 
                throw new ObjectDisposedException(ToString());

            int receivedSize = socket.Receive(buffer, offset, size, SocketFlags.None);
            IOEventArgs args = new IOEventArgs
            {
                buffer = buffer,
                startIndex = offset,
                size = receivedSize
            };
            onReceived?.Invoke(args, this);
        }
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

        public override void Send(byte[] buffer, int offset, int size)
        {
            if (buffer == null) 
                throw new NullReferenceException();
            
            if (!CheckDataBuffer(buffer, offset, size)) 
                throw new IndexOutOfRangeException();
            
            if (socket == null) 
                throw new NullReferenceException("Not Initialized Client");
            
            if (!socket.Connected) 
                throw new Exception("Client Aleready disconnected");
            
            if (_disposed) 
                throw new ObjectDisposedException(ToString());

            int sendedSize = socket.Send(buffer, offset, size, SocketFlags.None);
            IOEventArgs args = new IOEventArgs
            {
                buffer = buffer,
                startIndex = offset,
                size = sendedSize
            };
            onSended?.Invoke(args, this);
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
