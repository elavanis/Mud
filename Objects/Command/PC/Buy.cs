using Objects.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using Objects.Personality.Personalities.Interface;

namespace Objects.Command.PC
{
    public class Buy : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Buy {Item Number}", true);


        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Buy" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            foreach (INonPlayerCharacter npc in performer.Room.NonPlayerCharacters)
            {
                foreach (IPersonality personality in npc.Personalities)
                {
                    IMerchant merchantPersonality = personality as IMerchant;
                    if (merchantPersonality != null)
                    {
                        if (command.Parameters.Count > 0)
                        {
                            int item;
                            int.TryParse(command.Parameters[0].ParameterValue, out item);
                            if (item > 0)
                            {
                                return merchantPersonality.Buy(npc, performer, item);
                            }
                            else
                            {
                                return merchantPersonality.List(npc, performer);
                            }
                        }
                        else
                        {
                            return merchantPersonality.List(npc, performer);
                        }
                    }
                }
            }

            return new Result("There is no merchant here to sell to you.", true);
        }
    }
}