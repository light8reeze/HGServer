using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace HGServer.Network.Packet
{
    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Message
    {
        public int Size
        {
            get;
            set;
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
    public class MessageBuffer
    {
        #region Constant
        public readonly int DefaultSize = 1024;
        #endregion Constant

        #region Data Fields
        private byte[]  _messageBuffer;
        private int     _readIndex;
        private int     _writeIndex;
        #endregion Data Fields

        #region Constructor
        public MessageBuffer()
        {
            Initialize(DefaultSize);
        }

        public MessageBuffer(int bufferSize)
        {
            Initialize(bufferSize);
        }
        #endregion Constructor

        #region Method
        public void Initialize(int size)
        {
            _messageBuffer = new byte[size];
            Clear();
        }

        public void Clear()
        {
            _messageBuffer.Initialize();
            _readIndex = 0;
            _writeIndex = 0;
        }

        public T Pop<T>()
        {
            throw new NotImplementedException();
        }

        public Memory<byte> Pop()
        {
            throw new NotImplementedException();
        }

        public void Push(byte[] data)
        {
            data.CopyTo(_messageBuffer, _writeIndex);
            _writeIndex += data.Length;
        }
        public void Push(Span<byte> data)
        {
            var buffer = _messageBuffer.AsSpan(_writeIndex);
            data.CopyTo(buffer);
            _writeIndex += data.Length;
        }

        public void Push(byte[] data, int index)
        {
            var dataSpan = data.AsSpan(index);
            Push(dataSpan);
        }

        public void Push(byte[] data, int index, int size)
        {
            var dataSpan = data.AsSpan(index, size);
            Push(dataSpan);
        }

        public void OnDataPushed(Span<byte> pushedData)
        {
            OnDataPushed(pushedData.Length);
        }

        public void OnDataPushed(int size)
        {
            _writeIndex += size;
        }

        public Memory<byte> GetWriteMemory()
        {
            return _messageBuffer.AsMemory<byte>(_readIndex, _messageBuffer.Length - _readIndex);
        }
        #endregion Method
    }
}