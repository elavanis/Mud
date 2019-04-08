using Objects.Global;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;

namespace Objects.Magic.Enchantment
{
    public class PutEnchantment : BaseEnchantment
    {
        public override void Put(IMobileObject performer, IItem item, IContainer container)
        {
            if (GlobalReference.GlobalValues.Random.PercentDiceRoll(ActivationPercent))
            {
                Parameter.ObjectRoom = performer.Room;
                Parameter.Item = item;
                Parameter.Target = performer;
                Parameter.Container = container;

                Effect.ProcessEffect(Parameter);
            }
        }
    }
}
