using Objects.Damage.Interface;
using Objects.Global.Stats;
using System.Collections.Generic;

namespace Objects.Item.Items.Interface
{
    public interface IWeapon : IEquipment
    {
        Stats.Stat AttackerStat { get; set; }
        List<IDamage> DamageList { get; }
        Weapon.WeaponType Type { get; set; }
        Stats.Stat DeffenderStat { get; set; }
        int RequiredHands { get; set; }
        int Speed { get; set; }
    }
}