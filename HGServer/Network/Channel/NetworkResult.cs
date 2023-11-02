using HGServer.Network.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGServer.Network.Channel
{
    /// <summary>
    /// Network IO 이벤트 타입
    /// </summary>
    internal enum IOEventType
    {
        None = 0,
        Receive,
        Send,
        Connect,
        Disconnect,
        Accept,
    }
    
    /// <summary>
    /// Network IO 이벤트 완료시 채널에 전달할 구조체
    /// </summary>
    internal struct NetworkResult
    {
        public IOEventType              Type = IOEventType.None;
        public INetworkChannelObject    ChannelObject = null;

        public NetworkResult() { }
    }
}
