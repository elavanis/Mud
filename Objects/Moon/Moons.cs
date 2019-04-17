using Objects.Moon.Interface;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Moon
{
    public class Moons
    {
        [ExcludeFromCodeCoverage]
        public List<IMoon> MoonList { get; set; } = new List<IMoon>();
    }
}
