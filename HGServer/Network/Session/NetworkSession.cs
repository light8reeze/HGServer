using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using HGServer.Network.Messages;
using HGServer.Network.Sockets;
using HGServer.Utility;

namespace HGServer.Network.Session
{
    abstract class NetworkSession<T> : INetworkSession<T> where T : Socket
    {
        #region Data Fields
        
        protected T                                 _socket;
        protected MessageBuffer                     _receiveBuffer;
        protected ConcurrentQueue<MessageSender>    _sendQueue = new ConcurrentQueue<MessageSender>();

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

        #region Constructor
        public NetworkSession()
        {
            SessionAccept   += OnAccept;
            SessionAccepted += OnAccepted;
            DataReceived    += OnReceived;
            DataSended      += OnSended;
            SessionDisconnect   += OnDisconnect;
            SessionDisconnected += OnDisconnected;
        }
        #endregion Constructor

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

        public virtual void PushMessage(MessageSender messageSender)
        {
            _sendQueue.Enqueue(messageSender);
        }
        #endregion Method

        #region Abstract Method
        public abstract void Accept();
        public abstract void Connect(string ipAddress, int port);
        public abstract void Disconnect();
        public abstract void Dispose();
        public abstract void Close();
        public abstract void Initialize();
        public abstract void Receive();
        public abstract void Send<TMessage>(ref TMessage message) where TMessage : struct;
        public abstract void Send(ReadOnlyMemory<byte> messageBuffer);

        #endregion Abstract Method
    }
}
