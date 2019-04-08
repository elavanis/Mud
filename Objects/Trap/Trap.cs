using Objects.Magic.Interface;
using Objects.Trap.Interface;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Objects.Global.Stats;
using static Objects.Trap.Target;
using Objects.Damage.Interface;

namespace Objects.Trap
{
    public class Trap : ITrap
    {
        [ExcludeFromCodeCoverage]
        public List<IEnchantment> Enchantments { get; set; } = new List<IEnchantment>();
        [ExcludeFromCodeCoverage]
        public TrapTrigger Trigger { get; set; } = TrapTrigger.PC;
        [ExcludeFromCodeCoverage]
        public int DisarmSuccessRoll { get; set; }
        [ExcludeFromCodeCoverage]
        public Stats.Stat DisarmStat { get; set; } = Stats.Stat.Dexterity;
        [ExcludeFromCodeCoverage]
        public List<string> DisarmWord { get; set; } = new List<string>();
        [ExcludeFromCodeCoverage]
        public IDamage DisarmFailureDamage { get; set; } = null;
        [ExcludeFromCodeCoverage]
        public string DisarmFailureMessage { get; set; } = null;
    }
}
