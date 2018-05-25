using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Mob.Interface;
using Objects.Global;
using Objects.Item.Interface;

namespace Objects.Magic.Enchantment
{
    public class DropEnchantment : BaseEnchantment
    {
        public override void Drop(IMobileObject performer, IItem item)
        {
            if (GlobalReference.GlobalValues.Random.PercentDiceRoll(ActivationPercent))
            {
                Parameter.ObjectRoom = performer.Room;
                Parameter.Target = performer;
                Effect.ProcessEffect(Parameter);
            }
        }
    }
}
