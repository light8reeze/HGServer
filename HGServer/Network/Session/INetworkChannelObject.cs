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
}
