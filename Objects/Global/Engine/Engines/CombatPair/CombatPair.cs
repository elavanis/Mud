using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Global.Engine.Engines.CombatPair
{
    [ExcludeFromCodeCoverage]
    public class CombatPair
    {
        public IMobileObject Attacker { get; set; }
        public IMobileObject Defender { get; set; }

    }
}
