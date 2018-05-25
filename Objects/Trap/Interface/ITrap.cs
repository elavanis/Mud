using System.Collections.Generic;
using Objects.Magic.Interface;
using Objects.Global.Stats;
using static Objects.Trap.Target;
using Objects.Damage.Interface;

namespace Objects.Trap.Interface
{
    public interface ITrap
    {
        List<IEnchantment> Enchantments { get; set; }
        TrapTrigger Trigger { get; set; }
        int DisarmSuccessRoll { get; set; }
        Stats.Stat DisarmStat { get; set; }
        List<string> DisarmWord { get; set; }
        IDamage DisarmFailureDamage { get; set; }
        string DisarmFailureMessage { get; set; }
    }
}