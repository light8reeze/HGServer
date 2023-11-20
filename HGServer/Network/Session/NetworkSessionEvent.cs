using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using HGServer.Network.Messages;
using HGServer.Utility;

namespace HGServer.Network.Session
{
    abstract partial class NetworkSession
    {
        #region Session Delegate
        /// <summary>
        /// Session Connect callback
        /// </summary>
        /// <param name="session">connection event session</param>
        public delegate void ConnectionEventHandler(NetworkSession session);

        /// <summary>
        /// Session Message IO callback
        /// </summary>
        /// <param name="message">IO Completed Message</param>
        public delegate void DataTransferHandler(Message message);
        #endregion Session Delegate

        #region Event
        /// <summary>
        /// On Accept session event
        /// </summary>
        protected ConnectionEventHandler _sessionAccept;
        public event ConnectionEventHandler SessionAccept
        {
            add => _sessionAccept += value;
            remove => _sessionAccept -= value;
        }

        /// <summary>
        /// Exception handler on accept occured session accept
        /// </summary>
        protected ExceptionEventHandler _acceptException;
        public event ExceptionEventHandler AcceptException
        {
            add => _acceptException += value;
            remove => _acceptException -= value;
        }

        /// <summary>
        /// On Accepted event
        /// </summary>
        protected ConnectionEventHandler      _sessionAccepted;
        public event ConnectionEventHandler SessionAccepted
        {
            add => _sessionAccepted += value;
            remove => _sessionAccepted -= value;
        }

        /// <summary>
        /// On Received event
        /// </summary>
        protected DataTransferHandler         _dataReceived;
        public event DataTransferHandler    DataReceived
        {
            add => _dataReceived += value;
            remove => _dataReceived -= value;
        }

        /// <summary>
        /// Exception handler on accept occured receive
        /// </summary>
        protected ExceptionEventHandler _receiveException;
        public event ExceptionEventHandler ReceiveException
        {
            add => _receiveException += value;
            remove => _receiveException -= value;
        }

        /// <summary>
        /// On Sended event
        /// </summary>
        protected DataTransferHandler       _dataSended;
        public event DataTransferHandler    DataSended
        {
            add => _dataSended += value;
            remove => _dataSended -= value;
        }

        /// <summary>
        /// Exception handler on accept occured session accept
        /// </summary>
        protected ExceptionEventHandler       _sendException;
        public event ExceptionEventHandler  SendException
        {
            add => _sendException += value;
            remove => _sendException -= value;
        }

        /// <summary>
        /// On Closed event
        /// </summary>
        protected ConnectionEventHandler      _sessionDisconnect;
        public event ConnectionEventHandler SessionDisconnect
        {
            add => _sessionDisconnect += value;
            remove => _sessionDisconnect -= value;
        }

        /// <summary>
        /// On Closed event
        /// </summary>
        protected ConnectionEventHandler      _sessionDisconnedted;
        public event ConnectionEventHandler SessionDisconnected
        {
            add => _sessionDisconnedted += value;
            remove => _sessionDisconnedted -= value;
        }

        /// <summary>
        /// On Connected Event
        /// </summary>
        protected ConnectionEventHandler      _sessionConnected;
        public event ConnectionEventHandler SessionConnected
        {
            add => _sessionConnected += value;
            remove => _sessionConnected -= value;
        }

        /// <summary>
        /// Exception handler on accept occured session connect
        /// </summary>
        protected ExceptionEventHandler       _connectException;
        public event ExceptionEventHandler  ConnectException
        {
            add => _connectException += value;
            remove => _connectException -= value;
        }

        #endregion Event
    }
}
