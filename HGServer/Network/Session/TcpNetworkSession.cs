using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using HGServer.Network.Sockets;
using HGServer.Network.Messages;
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

        #region Socket Event Method

        public override void OnReceiveComplete(int dataSize, SocketBase sender)
        {
            _receiveBuffer.Commit(dataSize);

            Message receivedMessage;
            if (_receiveBuffer.TryPeekMessage(out receivedMessage) is false)
                return;

            if (_receiveBuffer.Length < receivedMessage.Size)
                return;

            Span<byte> memorySpan = _receiveBuffer.Pop(receivedMessage.Size);
            // TODO: memorySpan을 처리시키도록 채널에 추가
        }

        public override void OnSendComplete(int dataSize, SocketBase sender)
        {
        }

        #endregion Socket Event Method

        #region Abstract Method
        public override void Initialize()
        {
            if (_socket is null || _socket.Connected is true)
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
            if (_socket is null) 
                throw new NullReferenceException("Not Initialized Client");
            
            if (_socket.Connected is true) 
                throw new Exception("Client Aleready Connected");

            _socket.Connect(ipAddress, port);
        }

        public override void Disconnect()
        {
            if (_socket is null) 
                throw new NullReferenceException("Not Initialized Client");
            
            if (_socket.Connected is false) 
                throw new Exception("Client Aleready disconnected");

            _socket.Disconnect();
        }

        public override void Receive()
        {
            if (_socket is null) 
                throw new NullReferenceException("Not Initialized Client");
            
            if (_socket.Connected is false) 
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

        public override void Close()
        {
            if (_socket is null)
                throw new NullReferenceException("Not Initialized Client");

            _socket?.Close();
        }

        public override void OnReceived(Message message, object sender)
        {
            base.OnReceived(message, sender);
        }

        public void OnSended(Message message, INetworkSession sender)
        {
            MessageSender nextMessage;
            _sendQueue.TryDequeue(out nextMessage);

            Message messageHeader;

            Debug.Assert(nextMessage.TryGetMessage(out messageHeader));
            Debug.Assert(messageHeader.MessageNo == message.MessageNo);

            // 해당 내용도 수정 필요
            if (0 < Interlocked.Decrement(ref _sendCount))
            {
                if (_sendQueue.TryPeek(out nextMessage))
                    sender.Send(nextMessage.GetReadOnlyMemory());
            }
        }

        public override void Send<TMessage>(ref TMessage message) where TMessage : struct
        {
            if (_socket is null) 
                throw new NullReferenceException("Not Initialized Client");
            
            if (_socket.Connected is false) 
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
            if (_socket is null)
                throw new NullReferenceException("Not Initialized Client");

            if (_socket.Connected is false)
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
        #endregion Abstract Method
        public override void PushMessage(MessageSender messageSender)
        {
            base.PushMessage(messageSender);

            if (1 == Interlocked.Add(ref _sendCount, 1))
            {
                MessageSender nextMessage;
                if (_sendQueue.TryPeek(out nextMessage))
                    Send(nextMessage.GetReadOnlyMemory());
            }
        }

        #region Implement IDisposable
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed is true)
                return;

            _socket?.Dispose();
            _disposed = true;
        }
        #endregion Implement IDisposable
    }
}
