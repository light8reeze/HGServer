using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGServer.App
{
    internal interface INetworkService
    {
        public bool OnAppInitialize();
        public bool StartService();
        public void CloseService();
    }
}
