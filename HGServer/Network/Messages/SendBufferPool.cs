using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using HGServer.Utility;

namespace HGServer.Network.Messages
{
    class SendBufferPool : Singleton<SendBufferPool>
    {
        #region Constant
        public readonly int DefaultSendSize = 1024;
        public readonly int DefaultSendBufferCount = 50;
        #endregion Constant

        #region Data Field
        private ConcurrentQueue<MessageBuffer> _bufferQueue;
        #endregion Data Field

        #region Property
        public int SendMaxSize
        {
            private set;
            get;
        }

        public int SendMaxCount
        {
            private set;
            get;
        }
        #endregion Property

        #region Constructor
        public SendBufferPool()
        {
            _bufferQueue = new ConcurrentQueue<MessageBuffer>();
        }
        #endregion Constructor

        #region Method
        public void Initialize(int sendSize, int bufferCount)
        {
            if (sendSize <= 0)
                throw new ArgumentException();

            if (bufferCount <= 0)
                throw new ArgumentException();

            for (int i = 0; i < bufferCount; ++i)
                _bufferQueue.Enqueue(new MessageBuffer(sendSize));

            SendMaxSize = sendSize;
            SendMaxCount = bufferCount;
        }

        public MessageBuffer AllocBuffer()
        {
            MessageBuffer messageBuffer;
            bool success = _bufferQueue.TryDequeue(out messageBuffer);
            if(false == success)
                throw new OutOfMemoryException();
            
            return messageBuffer;
        }

        public void ReturnBuffer(MessageBuffer buffer)
        {
            _bufferQueue.Enqueue(buffer);
        }
        #endregion Method
    }
}
