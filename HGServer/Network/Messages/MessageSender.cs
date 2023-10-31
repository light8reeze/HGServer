using HGServer.Network.Session;
using HGServer.Network.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGServer.Network.Messages
{
    internal class MessageSender : IDisposable
    {
        private MessageBuffer   _sendBuffer;
        private int             _sendedSize;
        private bool            _disposed;

        public MessageSender(MessageBuffer sendBuffer)
        {
            _sendBuffer = sendBuffer;
            _sendedSize = 0;
        }

        public void PushMessage<T>(ref T message) where T : struct => _sendBuffer?.Push(ref message);

        public void Send<T>(NetworkSession<T> session) where T : SocketBase => session.PushMessage(this);

        public ReadOnlyMemory<byte> GetReadOnlyMemory() => _sendBuffer.GetReadMemory();

        public bool TryGetMessage(out Message msg) => _sendBuffer.TryPeekMessage(out msg);

        public void OnDataSended(int size) => _sendedSize += size;

        public bool IsSendCompleted() => _sendBuffer.Length() == _sendedSize;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed is true)
                return;

            _disposed = true;
            SendBufferPool.Instance.ReturnBuffer(_sendBuffer);
            _sendBuffer = null;
        }

        ~MessageSender()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
