using System;
using System.Collections.Generic;
using System.Text;

namespace Runaway.Base
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    class MessageTypeAttribute : Attribute
    {
        public MessageTypeAttribute(int type)
        {
            Type = type;
        }

        public int Type
        {
            get;
            set;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    class MessageHandlerAttribute : Attribute
    {
    }
}