using Objects.Global;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Magic.Enchantment
{
    public class OpenEnchantment : BaseEnchantment
    {
        public override void Open(IMobileObject performer, IItem item)
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
