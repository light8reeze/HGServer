using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HGServer.Utility.Memory
{
    internal class MemoryBuffer
    {
        #region Data Field
        private byte[]  _buffer;
        public int Size { get; } = 0;
        #endregion Data Field

        MemoryBuffer(int allocSize)
        {
            _buffer = GC.AllocateArray<byte>(allocSize, pinned : true);
            Size = allocSize;
        }

        public Span<byte> ToSpan(int start, int length)
        {
            return new Span<byte>(_buffer, start, length);
        }

        public void Write<T>(T value, int offset = 0) where T : struct 
        {
            unsafe
            {
                if (Size <= (offset + Marshal.SizeOf<T>()))
                    throw new OutOfMemoryException();

                fixed (byte* ptr = _buffer)
                {
                    byte* write_ptr = ptr + offset;
                    Marshal.StructureToPtr(value, (IntPtr)write_ptr, false);
                }
            }
        }

        public T Read<T>(int offset = 0) where T : struct
        {
            unsafe
            {
                if (Size <= offset)
                    throw new OutOfMemoryException();

                fixed (byte* ptr = _buffer)
                {
                    byte* read_ptr = ptr + offset;
                    return Marshal.PtrToStructure<T>((IntPtr)read_ptr);
                }
            }
        }
    }
}
