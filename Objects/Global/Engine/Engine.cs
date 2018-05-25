using Objects.Global.Engine.Engines;
using Objects.Global.Engine.Engines.Interface;
using Objects.Global.Engine.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Global.Engine
{
    public class Engine : IEngine
    {
        [ExcludeFromCodeCoverage]
        public ICombat Combat { get; set; }

        [ExcludeFromCodeCoverage]
        public IEvent Event { get; set; }

        public Engine()
        {
            Combat = new Combat();
            Event = new Event();
        }
    }
}
