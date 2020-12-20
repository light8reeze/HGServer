using System;
using System.Collections.Generic;
using System.Text;

namespace Runaway.Base.Network
{
    class NetworkSession<T> : INetworkSession<T>
    {
        #region Data Fields
        protected T socket;

        private Func<NetworkSession<T>> _creatorFunc;
        public Func<NetworkSession<T>>  CreatorFunc
        {
            protected get
            {
                _creatorFunc ??= (() => { return new NetworkSession<T>(); });

                return _creatorFunc;
            }
            set
            {
                _creatorFunc = value;
            }
        }
        
        private Action<NetworkSession<T>>   _deleterFunc;
        public Action<NetworkSession<T>>    DeleterFunc
        {
            protected get
            {
                _deleterFunc ??= ((session) => { session = null; });

                return _deleterFunc;
            }
            set
            {
                _deleterFunc = value;
            }
        }
        #endregion Data Fields

        #region Event
        /// <summary>
        /// On Accept session event
        /// </summary>
        private INetworkSession<T>.SessionConnectionDelegate _onAccept;
        public event INetworkSession<T>.SessionConnectionDelegate OnAccept
        {
            add => _onAccept += value;
            remove => _onAccept -= value;
        }

        /// <summary>
        /// On Accepted event
        /// </summary>
        private INetworkSession<T>.SessionConnectionDelegate _onAccepted;
        public event INetworkSession<T>.SessionConnectionDelegate OnAccepted
        {
            add => _onAccepted += value;
            remove => _onAccepted -= value;
        }

        /// <summary>
        /// On Received event
        /// </summary>
        private INetworkSession<T>.SessionMessagingDelegate _onReceived;
        public event INetworkSession<T>.SessionMessagingDelegate OnReceived
        {
            add => _onReceived += value;
            remove => _onReceived -= value;
        }

        /// <summary>
        /// On Sended event
        /// </summary>
        private INetworkSession<T>.SessionMessagingDelegate _onSended;
        public event INetworkSession<T>.SessionMessagingDelegate OnSended
        {
            add => _onSended += value;
            remove => _onSended -= value;
        }

        /// <summary>
        /// On Closed event
        /// </summary>
        private INetworkSession<T>.SessionConnectionDelegate _onDisconnect;
        public event INetworkSession<T>.SessionConnectionDelegate OnDisconnect
        {
            add => _onDisconnect += value;
            remove => _onDisconnect -= value;
        }

        /// <summary>
        /// On Closed event
        /// </summary>
        private INetworkSession<T>.SessionConnectionDelegate _onDisconnedted;
        public event INetworkSession<T>.SessionConnectionDelegate OnDisconnected
        {
            add => _onDisconnedted += value;
            remove => _onDisconnedted -= value;
        }

        /// <summary>
        /// On Connected Event
        /// </summary>
        private INetworkSession<T>.SessionConnectionDelegate _onConnected;
        public event INetworkSession<T>.SessionConnectionDelegate OnConnected
        {
            add => _onConnected += value;
            remove => _onConnected -= value;
        }
        #endregion Event

        #region Method
        public virtual void OnAcceptSession(INetworkSession<T> session)
        {
            throw new NotImplementedException();
        }

        public virtual void Accept(INetworkSession<T> session)
        {
            throw new NotImplementedException();
        }

        public virtual void Connect(string ipAddress, int port)
        {
            throw new NotImplementedException();
        }

        public virtual void Disconnect()
        {
            throw new NotImplementedException();
        }

        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }

        public virtual void Initialize()
        {
            throw new NotImplementedException();
        }

        public virtual void Receive()
        {
            throw new NotImplementedException();
        }

        public virtual void Send()
        {
            throw new NotImplementedException();
        }
        #endregion Method
    }
}
