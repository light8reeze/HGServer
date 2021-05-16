using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using HGServer.Network.Sockets;
using HGServer.Network.Packet;
using HGServer.Utility;
using System.Runtime.InteropServices;

namespace HGServer.Network.Session
{
    /// <summary>
    /// Tcp Game Session Class
    /// </summary>
    class TcpNetworkSession : NetworkSession<TcpAsyncSocket>
    {
        #region Data Field
        private bool _disposed = false;
        #endregion Data Field

        #region Constructor & Destructor
        public TcpNetworkSession()
        {
        }

        ~TcpNetworkSession()
        {
            Dispose(false);
        }
        #endregion Constructor & Destructor

        #region Method
        public override void Initialize()
        {
            if (socket != null || socket.Connected)
                throw new Exception("Already Initialized");

            socket = new TcpAsyncSocket();
            socket.Initialize();

            receiveBuffer    = new MessageBuffer();
        }

        public override void Accept()
        {
            try
            {
                socket?.Accept();
            }
            catch(Exception e)
            {
                _acceptException?.Invoke(this, e);
                return ;
            }

            _sessionAccept?.Invoke(this);
        }

        public override void Connect(string ipAddress, int port)
        {
            if (socket == null) 
                throw new NullReferenceException("Not Initialized Client");
            
            if (socket.Connected) 
                throw new Exception("Client Aleready Connected");

            socket.Connect(ipAddress, port);
        }

        public override void Disconnect()
        {
            if (socket == null) 
                throw new NullReferenceException("Not Initialized Client");
            
            if (!socket.Connected) 
                throw new Exception("Client Aleready disconnected");

            socket.Close();
        }

        public override void Receive()
        {
            if (socket == null) 
                throw new NullReferenceException("Not Initialized Client");
            
            if (!socket.Connected) 
                throw new Exception("Client Not Connected");

            var packetBuffer = receiveBuffer.GetWriteSpan();
            try
            {
                socket?.Receive(packetBuffer);
            }
            catch(Exception e)
            {
                _receiveException?.Invoke(this, e);
            }
        }

        public override void OnReceived(Message message, object sender)
        {
            base.OnReceived(message, sender);
        }

        public override void Send<TMessage>(ref TMessage message) where TMessage : struct
        {
            if (socket == null) 
                throw new NullReferenceException("Not Initialized Client");
            
            if (!socket.Connected) 
                throw new Exception("Client Not Connected");

            try
            {
                var sendBuffer = SendMemory.Instance.DequeueMemory();
                sendBuffer.Push(ref message);
                var sendPacket = sendBuffer.GetBuffer();
                socket?.Send(sendPacket);
            }
            catch (Exception e)
            {
                _sendException?.Invoke(this, e);
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
