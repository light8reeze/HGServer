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
        private bool            _disposed;

        public MessageSender(MessageBuffer sendBuffer)
        {
            _sendBuffer = sendBuffer;
        }

        public void PushMessage<T>(ref T message) where T : struct
        {
            _sendBuffer?.Push(ref message);
        }

        public void Send<T>(NetworkSession<T> session) where T : Socket
        {
            session.PushMessage(this);
        }

        public ReadOnlyMemory<byte> GetReadOnlyMemory()
        {
            return _sendBuffer.GetReadMemory();
        }

        public Message TryGetMessage()
        {
            return _sendBuffer.TryGetMessage();
        }

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
