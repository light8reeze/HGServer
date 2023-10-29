using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace HGServer.Network.Messages
{
    class MessageConverter
    {
        private static Dictionary<int, Type> _messageTypeList = new Dictionary<int, Type>();

        internal static void InsertMessageType(int messageNo, Type messageType)
        {
            if (_messageTypeList.ContainsKey(messageNo))
            {
                throw new ArgumentException($"Message Type overlapped {messageNo}");
            }

            _messageTypeList.Add(messageNo, messageType);
        }

        internal static Span<T> ConvertToMessage<T>(Span<byte> dataArray) where T : struct
        {
            return MemoryMarshal.Cast<byte, T>(dataArray);
        }

        internal static Span<byte> ConvertToByteArray<T>(Span<T> messageSpan) where T : struct
        {
            return MemoryMarshal.Cast<T, byte>(messageSpan);
        }
    }
}
