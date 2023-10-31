using HGServer.Network.Session;
using HGServer.Network.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HGServer.App.Service
{
    struct ConnectionConfig
    {
        public int      ConnectionID;
        public int      Port;
        public string   IPAddress;
    }

    internal class ConnectionService : INetworkService
    {
        private Dictionary<int, ConnectionConfig> _configDictionary = new Dictionary<int, ConnectionConfig>();
        private Dictionary<int, TcpNetworkSession> _connectorDictionary = new Dictionary<int, TcpNetworkSession>();

        public ConnectionService() 
        {
        }

        public void AddConfig(ConnectionConfig config)
        {
            _configDictionary.Add(config.ConnectionID, config);
        }

        #region Implement IService

        public bool OnAppInitialize()
        {
            try
            {
                foreach (var config in _configDictionary.Values)
                {
                    TcpNetworkSession connector = new TcpNetworkSession();
                    connector.Initialize();

                    _connectorDictionary.Add(config.ConnectionID, connector);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);

                return false;
            }

            return true;
        }

        public void CloseService()
        {
            try
            {
                foreach (var connecter in _connectorDictionary.Values)
                {
                    connecter.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public bool StartService()
        {
            try
            {
                ConnectionConfig config;
                foreach (var connectorPair in _connectorDictionary)
                {
                    if (_configDictionary.TryGetValue(connectorPair.Key, out config))
                        connectorPair.Value.Connect(config.IPAddress, config.Port);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return false;
            }

            return true;
        }

        #endregion Implement IService
    }
}
