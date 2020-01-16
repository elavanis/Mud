using Objects.Global;
using Objects.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;

namespace Objects.Magic.Enchantment
{
    public class PutEnchantment : BaseEnchantment
    {
        public IBaseObjectId MatchingContainerId { get; set; }

        public override void Put(IMobileObject performer, IItem item, IContainer container)
        {
            if (GlobalReference.GlobalValues.Random.PercentDiceRoll(ActivationPercent))
            {
                IBaseObject containerObject = container as BaseObject;
                if (MatchingContainerId == null
                    || (MatchingContainerId.Zone == containerObject.Zone
                        && MatchingContainerId.Id == containerObject.Id)
                   )
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
}
