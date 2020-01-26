using Objects.Die.Interface;
using Objects.Global.Stats;

namespace Objects.Damage.Interface
{
    public interface IDamage
    {
        long Id { get; set; }
        Stats.Stat? BonusDamageStat { get; set; }
        Stats.Stat? BonusDefenseStat { get; set; }
        IDice Dice { get; set; }
        Damage.DamageType Type { get; set; }
    }
}