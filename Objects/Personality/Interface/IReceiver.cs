using Objects.Command.Interface;
using Objects.Interface;
using Objects.Item.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Interface;

namespace Objects.Personality.Interface
{
    public interface IReceiver : IPersonality
    {
        IItem Reward { get; set; }
        string ResponseMessage { get; set; }
        IBaseObjectId TriggerObjectId { get; set; }
        IResult ReceivedItem(IMobileObject performer, IMobileObject receiver, IItem itemReceived);
    }
}
