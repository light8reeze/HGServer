using System;
using System.Collections.Generic;
using System.Text;

namespace HGServer.Network.Packet
{
    [AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
    class MessageTypeAttribute : Attribute
    {
        public MessageTypeAttribute(int messageNo, Type type)
        {
            MessageNo = messageNo;
            MessageConverter.InsertMessageType(messageNo, type);
        }

        public int MessageNo
        {
            get;
            set;
        }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Delegate | AttributeTargets.Event)]
    class MessageHandlerAttribute : Attribute
    {
        public MessageHandlerAttribute(int type)
        {
            Type = type;
        }

        public int Type
        {
            get;
            set;
        }
    }
}