using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HGServer.Network.Sockets
{
    internal class TcpAsyncSocket : TcpSocket
    {
        #region Data Fields
        
        private bool _disposed = false;

        public bool IsAwaitMode { get; set; } = true;
        
        #endregion Data Fields

        #region Method

        public ValueTask<Socket> AcceptAsync()
        {
            TcpAsyncSocket tcpAsyncSocket = new TcpAsyncSocket();
            tcpAsyncSocket.Initialize();
            return AcceptAsync(tcpAsyncSocket);
        }
  

        public async ValueTask<Socket> AcceptAsync(TcpAsyncSocket asyncSocket)
        {
            try
            {
                CancellationToken cancellationToken = new CancellationToken();
                ValueTask<Socket> task = _socket.AcceptAsync(asyncSocket._socket, cancellationToken);

                await task.ConfigureAwait(false);
                onAccepted?.Invoke(asyncSocket, this);
                return task.Result;
            }
            catch (Exception e)
            {
                onAcceptException?.Invoke(this, e);
            }
            finally
            {
                Close();
            }

            return null;
        }

        public async ValueTask<int> ReceiveAsync(Memory<byte> dataBuffer)
        {
            try
            {
                if (null == _socket)
                    throw new NullReferenceException("Not Initialized Client");

                if (false == _socket.Connected)
                    throw new Exception("Client Aleready disconnected");

                if (true == _disposed)
                    throw new ObjectDisposedException(ToString());

                ValueTask<int> task = _socket.ReceiveAsync(dataBuffer, SocketFlags.None);
                await task.ConfigureAwait(false);

                onReceived?.Invoke(task.Result, this);

                return task.Result;
            }
            catch (Exception e)
            {
                onReceiveException?.Invoke(this, e);
            }
            finally
            {
                Close();
            }

            return 0;
        }


        public async ValueTask<int> SendAsync(ReadOnlyMemory<byte> dataBuffer)
        {
            try
            {
                if (null == _socket)
                    throw new NullReferenceException("Not Initialized Client");

                if (false == _socket.Connected)
                    throw new Exception("Client Aleready disconnected");

                if (true == _disposed)
                    throw new ObjectDisposedException(ToString());

                var task = _socket.SendAsync(dataBuffer, SocketFlags.None);
                await task.ConfigureAwait(false);

                onSended?.Invoke(task.Result, this);

                return task.Result;
            }
            catch (Exception e)
            {
                onSendException?.Invoke(this, e);
            }
            finally
            {
                Close();
            }

            return 0;
        }

        #endregion Method
    }
}
