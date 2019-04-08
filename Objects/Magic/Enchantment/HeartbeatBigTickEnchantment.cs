using Objects.Interface;
using Objects.Global;

namespace Objects.Magic.Enchantment
{
    public class HeartbeatBigTickEnchantment : BaseEnchantment
    {
        public override void HeartbeatBigTick(IBaseObject performer)
        {
            if (GlobalReference.GlobalValues.Random.PercentDiceRoll(ActivationPercent))
            {
                Parameter.Target = performer;
                Effect.ProcessEffect(Parameter);
            }
        }
    }
}
