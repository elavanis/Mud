using Objects.Die.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Item.Items
{
    public class NpcInateArmor : Armor
    {
        public NpcInateArmor(INonPlayerCharacter npc,  int level) : base(GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(level), AvalableItemPosition.Wield, "", "", "", "")
        {
            Material = new Material.Materials.NpcInnateArmor(npc);
        }
    }
}
