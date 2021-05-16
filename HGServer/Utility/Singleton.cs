using System;
using System.Collections.Generic;
using System.Text;

namespace HGServer.Utility
{
    class Singleton<T> where T : new()
    {
        static private T _instance;

        static public T Instance
        {
            get
            {
                return _instance ??= new T();
            }
        }

        protected Singleton()
        {
        }
    }
}
