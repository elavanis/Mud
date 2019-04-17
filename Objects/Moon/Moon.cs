using Objects.Global;
using Objects.Moon.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Moon
{
    public class Moon : IMoon
    {
        [ExcludeFromCodeCoverage]
        public string Name { get; set; }
        [ExcludeFromCodeCoverage]
        public int MoonPhaseCycleDays { get; set; }

    }
}
