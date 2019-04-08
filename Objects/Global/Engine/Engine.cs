using Objects.Global.Engine.Engines;
using Objects.Global.Engine.Engines.Interface;
using Objects.Global.Engine.Interface;
using System.Diagnostics.CodeAnalysis;

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
