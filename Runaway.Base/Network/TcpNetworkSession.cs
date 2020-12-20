using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace Runaway.Base.Network
{
    /// <summary>
    /// Tcp Game Session Class
    /// </summary>
    class TcpNetworkSession : NetworkSession<TcpSocket>
    {
        #region Data Field
        #endregion Data Field

        #region Method
        public override void Initialize()
        {
            if (socket != null || socket.Connected)
                throw new Exception("Already Initialized");

            socket = new TcpSocket();
            socket.Initialize();
        }

        public override void OnAcceptSession(INetworkSession<TcpSocket> session)
        {

        }

        public override void Accept(INetworkSession<TcpSocket> session)
        {
            var tcpNetworkSession = session as TcpNetworkSession;
            if(tcpNetworkSession == null)
                throw new Exception("Not Match Session Class");

            socket?.Accept();
        }

        public override void Connect(string ipAddress, int port)
        {
            if (socket == null) 
                throw new NullReferenceException("Not Initialized Client");
            
            if (socket.Connected) 
                throw new Exception("Client Aleready Connected");

            socket.Connect(ipAddress, port);
        }

        public override void Disconnect()
        {
            if (socket == null) 
                throw new NullReferenceException("Not Initialized Client");
            
            if (!socket.Connected) 
                throw new Exception("Client Aleready disconnected");

            socket.Close();
        }

        public override void Receive()
        {
            if (socket == null) 
                throw new NullReferenceException("Not Initialized Client");
            
            if (!socket.Connected) 
                throw new Exception("Client Not Connected");
            
            socket.Receive(data, start, size);
        }

        public override void Send()
        {
            if (Client == null) 
                throw new NullReferenceException("Not Initialized Client");
            
            if (!Client.Connected) 
                throw new Exception("Client Not Connected");
            
            NetworkStream stream = Client.GetStream();
            stream.Write(data, start, size);
        }
        #endregion Method

        #region Implement IDisposable
        public override void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion Implement IDisposable
    }
}
