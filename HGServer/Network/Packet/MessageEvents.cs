using System;
using System.Collections.Generic;
using System.Text;

namespace HGServer.Network.Packet
{
    /// <summary>
    /// Event Target enum
    /// </summary>
    enum EventTarget
    {
        OnlySession,
        All,
    }

    class EventKey
    {
        public int EventNumber
        {
            get; set;
        }

    }

    /// <summary>
    /// Message Event class
    /// </summary>
    class MessageEvent
    {
        public delegate void OnMessageDelegate(object receiver, Message message);

        public OnMessageDelegate OnMessageReceived
        {
            get;
            internal set;
        }
    }

    class MessageEvents
    {
        private Dictionary<int, MessageEvent> _eventList;

        public delegate void MessageEventDelegate();
    }
}
