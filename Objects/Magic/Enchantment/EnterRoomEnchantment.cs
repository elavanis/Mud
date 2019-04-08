using Objects.Mob.Interface;
using Objects.Global;

namespace Objects.Magic.Enchantment
{
    public class EnterRoomEnchantment : BaseEnchantment
    {
        public override void EnterRoom(IMobileObject performer)
        {
            if (GlobalReference.GlobalValues.Random.PercentDiceRoll(ActivationPercent))
            {
                Parameter.Target = performer;
                Effect.ProcessEffect(Parameter);
            }
        }
    }
}
