using HGServer.Network.Channel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using static HGServer.Network.Session.INetworkChannelObject;

namespace HGServer.Network.Session
{
    /// <summary>
    /// Network 전용 채널을 사용하는 오브젝트
    /// </summary>
    internal interface INetworkChannelObject
    {
        public delegate bool NetworkChannelHandler();

        public void RegisterChannel(Channel<NetworkResult> channel);

        public bool OnIoCompleted(IOEventType eventType);

        public bool RequestIo(IOEventType eventType);

        public bool OnError(IOEventType eventType);
    }

    class NetworkChannelObjectEvent
    { 
        private Dictionary<IOEventType, NetworkChannelHandler> _ioEventHandlers = new Dictionary<IOEventType, NetworkChannelHandler>();

        public NetworkChannelHandler this[IOEventType type]
        {
            get
            {
                if (_ioEventHandlers.TryGetValue(type, out NetworkChannelHandler ioEventHandler))
                    return ioEventHandler;

                return null;
            }
            set
            {
                if (_ioEventHandlers.ContainsKey(type))
                    _ioEventHandlers[type] += value;
                else
                    _ioEventHandlers.Add(type, value);
            }
        }
    }

    internal class NetworkChannelObject : INetworkChannelObject
    {
        #region Channel
        protected Channel<NetworkResult> _networkChannel = null;
        protected ChannelWriter<NetworkResult> _networkChannelWriter = null;
        protected ChannelWriter<NetworkResult> ChannelWriter => _networkChannelWriter;
        #endregion Channel

        #region Network IO Event
        protected NetworkChannelObjectEvent _completedEvent = new NetworkChannelObjectEvent();
        public NetworkChannelObjectEvent CompletedEvent => _completedEvent;

        protected NetworkChannelObjectEvent _requestEvent = new NetworkChannelObjectEvent();
        public NetworkChannelObjectEvent RequestEvent => _requestEvent;

        protected NetworkChannelObjectEvent _errorEvent = new NetworkChannelObjectEvent();
        public NetworkChannelObjectEvent ErrorEvent => _errorEvent;
        #endregion Network IO Event


        public bool TryWriteToChannel(IOEventType eventType)
        {
            NetworkResult networkResult;
            networkResult.Type = IOEventType.Send;
            networkResult.ChannelObject = this;

            return ChannelWriter.TryWrite(networkResult);
        }

        #region INetworkChannelObject
        public bool OnIoCompleted(IOEventType eventType)
        {
            return CompletedEvent[eventType]?.Invoke() ?? false;
        }

        public void RegisterChannel(Channel<NetworkResult> channel)
        {
            _networkChannel = channel;
            _networkChannelWriter = _networkChannel?.Writer;
        }

        public bool RequestIo(IOEventType eventType)
        {
            return RequestEvent[eventType]?.Invoke() ?? false;
        }

        public bool OnError(IOEventType eventType)
        {
            return ErrorEvent[eventType]?.Invoke() ?? false;
        }

        #endregion INetworkChannelObject
    }
}
