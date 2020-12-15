using System;
using System.Collections.Generic;
using System.Text;

namespace Runaway.Base
{
    /// <summary>
    /// Game user interface
    /// </summary>
    public interface IGameUser<TSession> where TSession : IGameClient
    {
    }
}