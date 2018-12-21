using Objects.Command.Interface;
using Objects.Interface;
using Objects.Item.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Personality.Personalities.Interface
{
    public interface IReceiver : IPersonality
    {
        IBaseObjectId TriggerObjectId { get; set; }
        IResult ReceivedItem(IMobileObject performer, IMobileObject receiver, IItem itemReceived);
    }
}
