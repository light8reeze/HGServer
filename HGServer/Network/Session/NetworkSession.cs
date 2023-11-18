using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Channels;
using System.Transactions;
using HGServer.Network.Channel;
using HGServer.Network.Messages;
using HGServer.Network.Sockets;

namespace HGServer.Network.Session
{
    abstract partial class NetworkSession : NetworkChannelObject, IDisposable
    {
        protected Socket        _socket = null;
        protected MessageBuffer _receiveBuffer = null;

        protected ConcurrentQueue<MessageSender> _sendQueue = new ConcurrentQueue<MessageSender>();
        protected int _sendCount = 0;

        private bool _disposedValue;
        private bool _closed = false;

        public NetworkSession()
        {
        }

        ~NetworkSession()
        {
            Dispose(false);
        }

        #region Method

        public void PushMessage(MessageSender messageSender)
        {
            _sendQueue.Enqueue(messageSender);
        }

        public bool TrySend()
        {
            if (_sendQueue.Count <= 0)
                return false;

            if (0 == Interlocked.CompareExchange(ref _sendCount, 1, 0))
            {
                MessageSender nextMessage;
                if (_sendQueue.TryPeek(out nextMessage))
                {
                    Send(nextMessage.GetReadSpan());
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Session Initialize
        /// </summary>
        public void Initialize()
        {
            if (_socket is null || _socket.Connected is true)
                throw new Exception("Already Initialized");
            
            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _receiveBuffer = new MessageBuffer();
        }

        /// <summary>
        /// Receive Packet Data to session
        /// </summary>
        public void Receive()
        {
            if (_socket is null)
                throw new NullReferenceException("Not Initialized Client");

            if (_socket.Connected is false)
                throw new Exception("Not Connected");

            if(_receiveBuffer.RemainSize < Marshal.SizeOf<Message>())
                throw new Exception("Receive Buffer is Full");

            // 예외 처리 추가
            var packetBuffer = _receiveBuffer.GetWriteSpan();
            _socket.BeginReceive(packetBuffer.ToArray(), 0, packetBuffer.Length, SocketFlags.None, OnReceiveComplete, this);
        }

        /// <summary>
        /// Send Data to session
        /// </summary>
        public void Send(ReadOnlySpan<byte> messageBuffer)
        {
            if (_socket is null)
                throw new NullReferenceException("Not Initialized Client");

            if (_socket.Connected is false)
                throw new Exception("Not Connected");

            _socket.BeginSend(messageBuffer.ToArray(), 0, messageBuffer.Length, SocketFlags.None, OnReceiveComplete, this);
        }

        public void Close()
        {
            if (_socket?.Connected is true)
                Disconnect();

            _socket?.Dispose();
        }

        /// <summary>
        /// Disconnect Session
        /// </summary>
        public void Disconnect()
        {
            if (_socket?.Connected is false)
                return;

            _socket?.BeginDisconnect(true, OnDisconnected, this);
        }
        #endregion Method

        #region Socket Event Method

        public virtual void OnReceiveComplete(IAsyncResult result)
        {
            int receivedSize = _socket.EndReceive(result);
            if (0 <= receivedSize)
            {
                Disconnect();
                return;
            }

            _receiveBuffer.Commit(receivedSize);

            Message receivedMessage;
            // 처리할 메세지가 없는경우 Receive를 시도하고 종료한다.
            if (_receiveBuffer.TryPeekMessage(out receivedMessage) is false)
            {
                Receive();
                return;
            }

            if (_receiveBuffer.Length < receivedMessage.Size)
            {
                Receive();
                return;
            }

            // 처리할 메세지가 있을경우 Channel에 전달하여 처리하도록 한다.
            // TODO: 실패시 처리 추가
            TryWriteToChannel(IOEventType.Receive);
        }
         
        public virtual void OnSendComplete(IAsyncResult result)
        {
            int sendedSize = _socket.EndSend(result);
            if (0 <= sendedSize)
            {
                Disconnect();
                return;
            }

            MessageSender nextMessage;
            bool isTrySuccess = _sendQueue.TryDequeue(out nextMessage);
            Debug.Assert(isTrySuccess);

            Message messageHeader;
            isTrySuccess = nextMessage.TryGetMessage(out messageHeader);
            Debug.Assert(isTrySuccess);

            if(sendedSize != messageHeader.Size)
            {
                Interlocked.CompareExchange(ref _sendCount, 0, 1);
                Disconnect();
                return;
            }

            Interlocked.CompareExchange(ref _sendCount, 0, 1);
            TrySend();

            TryWriteToChannel(IOEventType.Send);
        }
        
        public virtual void OnDisconnected(IAsyncResult result)
        {
            _socket.EndDisconnect(result);

            if(_closed is false)
                TryWriteToChannel(IOEventType.Disconnect);
        }

        #endregion Socket Event Method

        #region INetworkChannelObject

        public override void RegisterChannel(Channel<NetworkResult> channel)
        {
            base.RegisterChannel(channel);
        }

        public override bool OnIoCompleted()
        {
            throw new NotImplementedException();
        }

        public override bool RequestIo()
        {
            throw new NotImplementedException();
        }

        #endregion INetworkChannelObject

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue is true)
                return;

            // Unmanaged 리소스 관리
            if (disposing is false)
            {
                _socket?.Close();
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion IDisposable
    }
}