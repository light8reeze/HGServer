using System;
using System.Collections.Generic;
using System.Text;
using HGServer.Utility;

namespace HGServer.Network.Packet
{
    class SendMemory : Singleton<SendMemory>
    {
        #region Constant
        public readonly int DefaultSendSize         = 1024;
        public readonly int DefaultSendBufferCount  = 50;
        #endregion Constant

        #region Data Field
        private Queue<MessageBuffer>    _bufferQueue;
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
        public SendMemory()
        {
            _bufferQueue = new Queue<MessageBuffer>();
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

        public MessageBuffer DequeueMemory()
        {
            var buffer = _bufferQueue.Dequeue();
            return buffer;
        }

        public void EnqueueMemory(MessageBuffer buffer)
        {
            _bufferQueue.Enqueue(buffer);
        }
        #endregion Method
    }
}
