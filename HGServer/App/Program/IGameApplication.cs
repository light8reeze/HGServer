using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace HGServer.App
{
    interface IGameApplication
    {
        bool Start();

        IReadOnlyCollection<T> GetComponent<T>() where T : IComponent;

        IReadOnlyCollection<T> GetService<T>() where T : INetworkService;    
    }
}
