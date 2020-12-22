using System;
using System.Collections.Generic;
using System.Text;

namespace Runaway.Base
{
    /// <summary>
    /// 
    /// </summary>
    [MessageType(0)]
    public class Message
    {
        public Message()
        {
        }

        public int MessageNo
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MessageQueue
    {
        #region Constant
        public readonly int DefaultSize = 1024;
        #endregion Constant

        #region Data Fields
        private int     _bufferSize;
        private byte[]  _messageBuffer;
        #endregion Data Fields

        #region Constructor
        public MessageQueue()
        {
            Initialize(DefaultSize);
        }

        public MessageQueue(int bufferSize)
        {
            Initialize(bufferSize);
        }
        #endregion Constructor

        #region Method
        public void Initialize(int size)
        {
            _bufferSize = size;

            _messageBuffer = new byte[size];
            _messageBuffer.Initialize();
        }
        #endregion Method
    }
}