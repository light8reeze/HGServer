using System;
using System.Collections.Generic;
using System.Text;

namespace Runaway.Base
{
    abstract class NetworkSession<T> : INetworkSession<T>
    {
        #region Data Fields
        protected T socket;
        #endregion Data Fields

        #region Session Delegate
        /// <summary>
        /// Session Connect callback
        /// </summary>
        /// <param name="sender">connection event session</param>
        public delegate void ConnectionEventHandler(object sender);

        /// <summary>
        /// Session Message IO callback
        /// </summary>
        /// <param name="message">IO Completed Message</param>
        /// <param name="sender">IO Completed Object</param>
        public delegate void DataTransferHandler(Message message, object sender);
        #endregion Session Delegate

        #region Event
        /// <summary>
        /// On Accept session event
        /// </summary>
        private ConnectionEventHandler _sessionAccept;
        public event ConnectionEventHandler SessionAccept
        {
            add => _sessionAccept += value;
            remove => _sessionAccept -= value;
        }

        /// <summary>
        /// On Accepted event
        /// </summary>
        private ConnectionEventHandler      _sessionAccepted;
        public event ConnectionEventHandler SessionAccepted
        {
            add => _sessionAccepted += value;
            remove => _sessionAccepted -= value;
        }

        /// <summary>
        /// On Received event
        /// </summary>
        private DataTransferHandler         _dataReceived;
        public event DataTransferHandler    DataReceived
        {
            add => _dataReceived += value;
            remove => _dataReceived -= value;
        }

        /// <summary>
        /// On Sended event
        /// </summary>
        private DataTransferHandler         _dataSended;
        public event DataTransferHandler    DataSended
        {
            add => _dataSended += value;
            remove => _dataSended -= value;
        }

        /// <summary>
        /// On Closed event
        /// </summary>
        private ConnectionEventHandler      _sessionDisconnect;
        public event ConnectionEventHandler SessionDisconnect
        {
            add => _sessionDisconnect += value;
            remove => _sessionDisconnect -= value;
        }

        /// <summary>
        /// On Closed event
        /// </summary>
        private ConnectionEventHandler      _sessionDisconnedted;
        public event ConnectionEventHandler SessionDisconnected
        {
            add => _sessionDisconnedted += value;
            remove => _sessionDisconnedted -= value;
        }

        /// <summary>
        /// On Connected Event
        /// </summary>
        private ConnectionEventHandler      _sessionConnected;
        public event ConnectionEventHandler SessionConnected
        {
            add => _sessionConnected += value;
            remove => _sessionConnected -= value;
        }

        private Func<NetworkSession<T>> _createSession;
        public virtual Func<NetworkSession<T>> CreateSession
        {
            protected get => _createSession ??= (() => { return default; });
            set => _createSession = value;
        }

        private Action<NetworkSession<T>> _deleteSession;
        public virtual Action<NetworkSession<T>> DeleteSession
        {
            protected get => _deleteSession ??= ((session) => { session.Dispose(); });
            set => _deleteSession = value;
        }
        #endregion Event

        #region Constructor & Destructor
        public NetworkSession()
        {
            SessionAccept   += OnAccept;
            SessionAccepted += OnAccepted;
            DataReceived    += OnReceived;
            DataSended      += OnSended;
            SessionDisconnect   += OnDisconnect;
            SessionDisconnected += OnDisconnected;
        }
        #endregion Constructor & Destructor

        #region Method
        public virtual void OnAccept(object sender)
        {
        }
        public virtual void OnAccepted(object sender)
        {
        }
        public virtual void OnReceived(Message message, object sender)
        {
        }
        public virtual void OnSended(Message message, object sender)
        {
        }
        public virtual void OnConnected(object sender)
        {
        }
        public virtual void OnDisconnect(object sender)
        {
        }
        public virtual void OnDisconnected(object sender)
        {
        }
        #endregion Method

        #region Abstract Method
        public abstract void Accept();
        public abstract void OnAcceptEvent(object acceptedSocket, object sender);
        public abstract void Connect(string ipAddress, int port);
        public abstract void Disconnect();
        public abstract void Dispose();
        public abstract void Initialize();
        public abstract void Receive();
        public abstract void Send();
        #endregion Abstract Method
    }
}
