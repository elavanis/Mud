using Objects.Command.Interface;
using Objects.Global;
using Objects.Item.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using Objects.Personality.Personalities.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Objects.Command.PC
{
    public class Give : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Give [Item Name] [Person]", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Give" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            if (command.Parameters.Count < 2)
            {
                return Instructions;
            }
            else
            {
                IItem item = GlobalReference.GlobalValues.FindObjects.FindHeldItemsOnMob(performer, command.Parameters[0].ParameterValue).FirstOrDefault();

                if (item == null)
                {
                    return new Result($"You do not seem to be carrying {command.Parameters[0].ParameterValue}.", true);
                }

                IMobileObject receiver = GlobalReference.GlobalValues.FindObjects.FindNpcInRoom(performer.Room, command.Parameters[1].ParameterValue).FirstOrDefault();
                if (receiver == null)
                {
                    receiver = GlobalReference.GlobalValues.FindObjects.FindPcInRoom(performer.Room, command.Parameters[1].ParameterValue).FirstOrDefault();

                    if (receiver == null)
                    {
                        return new Result($"You could not find {command.Parameters[1].ParameterValue}.", true);
                    }
                }

                performer.Items.Remove(item);
                receiver.Items.Add(item);

                if (receiver is INonPlayerCharacter npcReceiver)
                {
                    foreach (IPersonality personality in npcReceiver.Personalities)
                    {
                        if (personality is IReceiver receiverPersonality)
                        {
                            return receiverPersonality.ReceivedItem(performer, npcReceiver, item);
                        }
                    }
                }

                return new Result($"You give {receiver.SentenceDescription} {item.SentenceDescription}.", false);
            }
        }
    }
}
