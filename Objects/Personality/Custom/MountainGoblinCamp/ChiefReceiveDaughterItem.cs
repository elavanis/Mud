﻿using Objects.Command;
using Objects.Command.Interface;
using Objects.Interface;
using Objects.Item.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Personality.Custom.MountainGoblinCamp
{
    public class ChiefReceiveDaughterItem : IReceiver
    {
        [ExcludeFromCodeCoverage]
        public string? Process(INonPlayerCharacter npc, string? command)
        {
            return command;
        }

        [ExcludeFromCodeCoverage]
        public IItem Reward { get; set; }

        [ExcludeFromCodeCoverage]
        public string ResponseMessage { get; set; }

        [ExcludeFromCodeCoverage]
        public IBaseObjectId TriggerObjectId { get; set; }

        public ChiefReceiveDaughterItem(IItem reward, string responseMessage, IBaseObjectId triggerObjectId)
        {
            Reward = reward;
            ResponseMessage = responseMessage;
            TriggerObjectId = triggerObjectId;
        }

        public IResult ReceivedItem(IMobileObject performer, IMobileObject receiver, IItem itemReceived)
        {
            if (itemReceived.Id == TriggerObjectId.Id
                && itemReceived.ZoneId == TriggerObjectId.Zone)
            {
                performer.Items.Add((IItem)Reward.Clone());

                receiver.EnqueueCommand($"say {ResponseMessage}");

                return new Result(null, false);
            }
            else
            {
                receiver.EnqueueCommand($"say Thank you for the gift.");
                return new Result(null, true);
            }
        }
    }
}
