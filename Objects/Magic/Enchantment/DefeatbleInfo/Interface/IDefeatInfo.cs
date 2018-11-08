using Objects.Global.Stats;
using Objects.Mob.Interface;

namespace Objects.Magic.Enchantment.DefeatbleInfo.Interface
{
    public interface IDefeatInfo
    {
        int CurrentEnchantmentPoints { get; set; }
        Stats.Stat MobStat { get; set; }

        bool DoesPayerDefeatEnchantment(IMobileObject target);
    }
}