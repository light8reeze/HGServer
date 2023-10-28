using System;
using System.Collections.Generic;
using System.Text;
using HGServer.Utility;

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

    /// <summary>
    /// Message Event class
    /// </summary>
    class MessageEvent
    {
        public delegate void OnMessageDelegate(object receiver, Message message);

        public delegate void OnMessageException(object receiver, Message message, Exception e);

        public OnMessageDelegate OnMessageReceived
        {
            set;
            get;
        }

        public OnMessageException OnMessageEventException
        {
            get;
            set;
        }

        public virtual void OnCatchException(object receiver, Message message, Exception e)
        {
            OnMessageEventException?.Invoke(receiver, message, e);
        }

        public string EventName
        {
            get;
            set;
        }
    }

    class MessageEvents
    {
        private static Dictionary<int, Dictionary<string, MessageEvent>> _eventList = new Dictionary<int, Dictionary<string, MessageEvent>>();

        public delegate void MessageEventDelegate();

        public static void AddEvent(int messageType, MessageEvent messageEvent)
        {
            if (_eventList.ContainsKey(messageType) == false)
            {
                _eventList.Add(messageType, new Dictionary<string, MessageEvent>());
            }

            var events = _eventList[messageType];
            events.Add(messageEvent.EventName, messageEvent);
        }

        public static void Invoke(object receiver, Message message)
        {
            var events = _eventList[message.MessageNo];

            foreach (var eventPair in events)
            {
                var value = eventPair.Value;

                try
                {
                    value?.OnMessageReceived?.Invoke(receiver, message);
                }
                catch (Exception e)
                {
                    value?.OnMessageEventException(receiver, message, e);
                }
            }
        }
    }
}
