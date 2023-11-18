using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using HGServer.Network.Sockets;
using HGServer.Network.Messages;
using HGServer.Utility;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.Threading.Channels;
using HGServer.Network.Channel;

namespace HGServer.Network.Session
{
    /// <summary>
    /// Listener class
    /// </summary>
    class NetworkListener : INetworkChannelObject, IDisposable
    {
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리형 상태(관리형 개체)를 삭제합니다.
                }

                // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

         // TODO: 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
         ~NetworkListener()
         {
             // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
             Dispose(disposing: false);
         }

        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void RegisterChannel(Channel<NetworkResult> channel)
        {
            throw new NotImplementedException();
        }

        public bool OnIoCompleted()
        {
            throw new NotImplementedException();
        }

        public bool RequestIo()
        {
            throw new NotImplementedException();
        }
    }
}
