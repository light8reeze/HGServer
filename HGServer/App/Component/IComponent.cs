using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGServer.App
{
    interface IComponent
    {
        public bool Initialize();

        public bool Finalize();
    }
}
