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
    public class GetEnchantment : BaseEnchantment
    {
        public override void Get(IMobileObject performer, IItem item)
        {
            if (GlobalReference.GlobalValues.Random.PercentDiceRoll(ActivationPercent))
            {
                Parameter.ObjectRoom = performer.Room;
                Parameter.Item = item;
                Parameter.Target = performer;
                Effect.ProcessEffect(Parameter);
            }
        }
    }
}
