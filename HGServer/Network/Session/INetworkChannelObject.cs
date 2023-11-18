using HGServer.Network.Channel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace HGServer.Network.Session
{
    /// <summary>
    /// Network 전용 채널을 사용하는 오브젝트
    /// </summary>
    internal interface INetworkChannelObject
    {
        public void RegisterChannel(Channel<NetworkResult> channel);

        public bool OnIoCompleted();

        public bool RequestIo();
    }

    internal abstract class NetworkChannelObject : INetworkChannelObject
    {
        protected Channel<NetworkResult> _networkChannel = null;
        protected ChannelWriter<NetworkResult> _networkChannelWriter = null;
        protected ChannelWriter<NetworkResult> ChannelWriter => _networkChannelWriter;

        public bool TryWriteToChannel(IOEventType eventType)
        {
            NetworkResult networkResult;
            networkResult.Type = IOEventType.Send;
            networkResult.ChannelObject = this;

            return ChannelWriter.TryWrite(networkResult);
        }

        #region INetworkChannelObject
        public abstract bool OnIoCompleted();
        
        public virtual void RegisterChannel(Channel<NetworkResult> channel)
        {
            _networkChannel = channel;
            _networkChannelWriter = _networkChannel?.Writer;
        }

        public abstract bool RequestIo();

        #endregion INetworkChannelObject
    }
}
