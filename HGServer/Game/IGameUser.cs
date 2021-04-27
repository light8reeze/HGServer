using System;
using System.Collections.Generic;
using System.Text;
using HGServer.Network.Session;

namespace HGServer.Game
{
    /// <summary>
    /// Game user interface
    /// </summary>
    public interface IGameUser<TSession> where TSession : INetworkSession<TSession>
    {
    }
}