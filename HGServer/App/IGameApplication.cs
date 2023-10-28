using System;
using System.Collections.Generic;
using System.Text;

namespace HGServer.App
{
    interface IGameApplication
    {
        public bool Initialize();

        public bool Start();

        public bool Run();

        public bool Close();
    }
}
