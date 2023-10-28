using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using HGServer.Network.Sockets;
using HGServer.Network.Packet;
using HGServer.Utility;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace HGServer.Network.Session
{
    /// <summary>
    /// Tcp Game Session Class
    /// </summary>
    class TcpNetworkSession : NetworkSession<TcpSocket>
    {
        #region Data Field
        private bool    _disposed = false;
        private int     _sendCount = 0;
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
            if (_socket != null || _socket.Connected == true)
                throw new Exception("Already Initialized");

            _socket = new TcpSocket();
            _socket.Initialize();

            _receiveBuffer    = new MessageBuffer();
        }

        public override void Accept()
        {
            try
            {
                _socket?.Accept();
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
            if (_socket == null) 
                throw new NullReferenceException("Not Initialized Client");
            
            if (_socket.Connected == true) 
                throw new Exception("Client Aleready Connected");

            _socket.Connect(ipAddress, port);
        }

        public override void Disconnect()
        {
            if (_socket == null) 
                throw new NullReferenceException("Not Initialized Client");
            
            if (_socket.Connected == false) 
                throw new Exception("Client Aleready disconnected");

            _socket.Close();
        }

        public override void Receive()
        {
            if (_socket == null) 
                throw new NullReferenceException("Not Initialized Client");
            
            if (_socket.Connected == false) 
                throw new Exception("Client Not Connected");

            var packetBuffer = _receiveBuffer.GetWriteMemory();
            
            try
            {
                _socket?.Receive(packetBuffer);
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

        public override void OnSended(Message message, object sender)
        {
            base.OnSended(message, sender);

            MessageSender nextMessage;
            _sendQueue.TryDequeue(out nextMessage);

            Debug.Assert(nextMessage.TryGetMessage().MessageNo == message.MessageNo);

            // 해당 내용도 수정 필요
            if (0 < Interlocked.Decrement(ref _sendCount))
            {
                if (_sendQueue.TryPeek(out nextMessage))
                    Send(nextMessage.GetReadOnlyMemory());
            }
        }

        public override void PushMessage(MessageSender messageSender)
        {
            base.PushMessage(messageSender);

            if(1 == Interlocked.Add(ref _sendCount, 1))
            {
                MessageSender nextMessage;
                if(_sendQueue.TryPeek(out nextMessage))
                    Send(nextMessage.GetReadOnlyMemory());
            }
        }

        public override void Send<TMessage>(ref TMessage message) where TMessage : struct
        {
            if (_socket == null) 
                throw new NullReferenceException("Not Initialized Client");
            
            if (_socket.Connected == false) 
                throw new Exception("Client Not Connected");

            try
            {
                var sendBuffer = SendBufferPool.Instance.AllocBuffer();
                sendBuffer.Push(ref message);
                var sendPacket = sendBuffer.GetReadMemory();
                _socket?.Send(sendPacket);
            }
            catch (Exception e)
            {
                _sendException?.Invoke(this, e);
            }
        }
 

        public override void Send(ReadOnlyMemory<byte> data)
        {
            if (_socket == null)
                throw new NullReferenceException("Not Initialized Client");

            if (_socket.Connected == false)
                throw new Exception("Client Not Connected");

            try
            {
                _socket?.Send(data);
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
            if (_disposed == true)
                return;

            _socket?.Dispose();
            _disposed = true;
        }
        #endregion Implement IDisposable
    }
}
