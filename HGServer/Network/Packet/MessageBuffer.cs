using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HGServer.Network.Packet
{
    /// <summary>
    /// 
    /// </summary>
    public class MessageBuffer
    {
        #region Constant
        public readonly int DefaultSize = 1024;
        #endregion Constant

        #region Data Fields
        private byte[] _messageBuffer;
        private int _readIndex;
        private int _writeIndex;
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
            _messageBuffer = GC.AllocateArray<byte>(size, true);
            Clear();
        }

        public void Clear()
        {
            _messageBuffer.Initialize();
            _readIndex = 0;
            _writeIndex = 0;
        }

        public Span<byte> Pop()
        {
            var readSpan = _messageBuffer.AsSpan(_readIndex, _writeIndex);
            _readIndex += readSpan.Length;

            return readSpan;
        }

        public Message TryGetMessage()
        {
            if (_writeIndex - _readIndex < Marshal.SizeOf<Message>())
                return new Message();

            Message msg;
            var readSpan = _messageBuffer.AsSpan(_readIndex, _writeIndex);
            if (MemoryMarshal.TryRead(readSpan, out msg))
                return msg;

            return new Message();
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

        public void Push<T>(ref T data) where T : struct
        {
            if (data is not Message)
                throw new ArgumentException();

            var writeSpan = GetWriteSpan();
            MemoryMarshal.Write(writeSpan, ref data);
            Commit(Marshal.SizeOf(data));
        }

        public bool TryPush<T>(ref T data) where T : struct
        {
            if (data is not Message)
                throw new ArgumentException();

            var writeSpan = GetWriteSpan();
            if (MemoryMarshal.TryWrite(writeSpan, ref data))
            {
                Commit(Marshal.SizeOf(data));
                return true;
            }

            return false;
        }

        public void Commit(int size)
        {
            if (_messageBuffer.Length <= _writeIndex + size)
                throw new IndexOutOfRangeException();

            _writeIndex += size;
        }

        public Memory<byte> GetWriteMemory()
        {
            if (_messageBuffer.Length <= _writeIndex)
                throw new InvalidOperationException();

            return _messageBuffer.AsMemory(_writeIndex, _messageBuffer.Length - _writeIndex);
        }

        public Span<byte> GetWriteSpan()
        {
            if (_messageBuffer.Length <= _writeIndex)
                throw new InvalidOperationException();

            return _messageBuffer.AsSpan(_writeIndex, _messageBuffer.Length - _writeIndex);
        }

        public ReadOnlySpan<byte> GetReadSpan()
        {
            if (_messageBuffer.Length <= _writeIndex)
                throw new InvalidOperationException();

            return _messageBuffer.AsSpan(_readIndex, _writeIndex);
        }
        public ReadOnlyMemory<byte> GetReadMemory()
        {
            if (_messageBuffer.Length <= _writeIndex)
                throw new InvalidOperationException();

            return _messageBuffer.AsMemory(_readIndex, _writeIndex);
        }
        #endregion Method
    }
}
