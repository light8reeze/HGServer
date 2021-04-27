using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace HGServer.Network.Sockets
{
    abstract class AsyncSocketBase : SocketBase
    {
        #region Method
        public override sealed void Accept() => AcceptAsync().ConfigureAwait(false);
        public override sealed void Accept(object socket) => AcceptAsync(socket).ConfigureAwait(false);
        public override sealed void Connect(string ipAddress, int port) => ConnectAsync(ipAddress, port).ConfigureAwait(false);
        public override sealed void Receive(Memory<byte> dataMemory) => ReceiveAsync(dataMemory).ConfigureAwait(false);
        public override sealed void Send(ReadOnlyMemory<byte> dataMemory) => SendAsync(dataMemory).ConfigureAwait(false);

        #pragma warning disable CS1998
        public virtual async Task AcceptAsync() => throw new NotImplementedException();
        public virtual async Task AcceptAsync(object socket) => throw new NotImplementedException();
        public virtual async Task ConnectAsync(string ipAddress, int port) => throw new NotImplementedException();
        public virtual async Task ReceiveAsync(Memory<byte> dataMemory) => throw new NotImplementedException();
        public virtual async Task SendAsync(ReadOnlyMemory<byte> dataMemory) => throw new NotImplementedException();
        #pragma warning restore CS1998

        #endregion Method
    }
}