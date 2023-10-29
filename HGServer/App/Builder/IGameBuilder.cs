using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGServer.App
{
    interface IGameBuilder
    {
        public IGameBuilder AddService(INetworkService service);

        public IGameBuilder AddComponent(IComponent component);

        public IGameApplication Build();
    }
}
