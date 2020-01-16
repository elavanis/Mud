using Objects.Mob.Interface;
using Objects.Global;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Interface;

namespace Objects.Magic.Enchantment
{
    public class GetEnchantment : BaseEnchantment
    {
        public IBaseObjectId MatchingContainerId { get; set; }

        public override void Get(IMobileObject performer, IItem item, IContainer container)
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
