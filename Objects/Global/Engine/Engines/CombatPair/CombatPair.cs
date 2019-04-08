using Objects.Mob.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Global.Engine.Engines.CombatPair
{
    [ExcludeFromCodeCoverage]
    public class CombatPair
    {
        public IMobileObject Attacker { get; set; }
        public IMobileObject Defender { get; set; }

    }
}
