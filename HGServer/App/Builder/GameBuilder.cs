using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGServer.App.Builder
{
    internal class GameBuilder : IGameBuilder
    {
        private List<IComponent> _components = new List<IComponent>();
        private List<INetworkService> _services = new List<INetworkService>();

        public IGameBuilder AddComponent(IComponent component)
        {
            component.Initialize();
            _components.Add(component);

            return this;
        }

        public IGameBuilder AddService(INetworkService service)
        {
            _services.Add(service);

            return this;
        }

        public IGameApplication Build()
        {
            var app = new GameApplication(_components, _services);

            return app;
        }
    }
}
