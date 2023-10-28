using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace HGServer.Utility
{
    class Singleton<T> where T : new()
    {
        static private Lazy<T> _instance = new Lazy<T>();

        static public T Instance => _instance.Value;
    }
}
