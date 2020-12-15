using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace Runaway.Base.Network
{
    class TcpGameClient : IGameClient<TcpClient>
    {
        #region Data Field
        #endregion Data Field

        #region Implement IGameClient<T>
        public TcpClient Client 
        { 
            get;
            set;
        }

        public void Accept()
        {
        }

        public void Connect(string ipAddress, int port)
        {
            if (Client == null) throw new NullReferenceException("Not Initialized Client");
            if (Client.Connected) throw new Exception("Client Aleready Connected");

            Client.Connect(ipAddress, port);
        }

        public void Disconnect()
        {
            if (Client == null) throw new NullReferenceException("Not Initialized Client");
            if (!Client.Connected) throw new Exception("Client Aleready disconnected");

            Client.Close();
        }

        public void Initialize()
        {
            if (Client != null || Client.Connected) throw new Exception("Already Initialized");

            if(Client == null)
                Client = new TcpClient();
        }

        public int Receive(byte[] data, int start, int size)
        {
            if (Client == null) throw new NullReferenceException("Not Initialized Client");
            if (!Client.Connected) throw new Exception("Client Not Connected");
            if (!CheckValidArrayIndex(data, start, size)) throw new ArgumentOutOfRangeException("Invalid Size");
            
            NetworkStream stream = Client.GetStream();
            int receivedSize = stream.Read(data, start, size);

            return receivedSize;
        }

        public void Send(byte[] data, int start, int size)
        {
            if (Client == null) throw new NullReferenceException("Not Initialized Client");
            if (!Client.Connected) throw new Exception("Client Not Connected");
            if (!CheckValidArrayIndex(data, start, size)) throw new ArgumentOutOfRangeException("Invalid Size");

            NetworkStream stream = Client.GetStream();
            stream.Write(data, start, size);
        }
        #endregion Implement IGameClient<T>

        private bool CheckValidArrayIndex(byte[] data, int start, int size)
        {
            if (size < 0)
                return false;

            if (start < 0 || data.Length < start + size)
                return false;

            return true;
        }

        #region Implement IDisposable
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion Implement IDisposable
    }
}
