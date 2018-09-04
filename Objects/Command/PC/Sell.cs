using Objects.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using Objects.Personality.Personalities.Interface;
using Objects.Item.Interface;

namespace Objects.Command.PC
{
    public class Sell : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Sell {Item Keyword}", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Sell" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            foreach (INonPlayerCharacter npc in performer.Room.NonPlayerCharacters)
            {
                foreach (IPersonality personality in npc.Personalities)
                {
                    IMerchant merchantMasterPersonality = personality as IMerchant;
                    if (merchantMasterPersonality != null)
                    {
                        if (command.Parameters.Count > 0)
                        {
                            IItem item = null;
                            foreach (IItem i in performer.Items)
                            {
                                if (i.KeyWords.Contains(command.Parameters[0].ParameterValue, StringComparer.CurrentCultureIgnoreCase))
                                {
                                    item = i;
                                    break;
                                }
                            }
                            if (item != null)
                            {
                                return merchantMasterPersonality.Sell(npc, performer, item);
                            }
                            else
                            {
                                return merchantMasterPersonality.Offer(npc, performer);
                            }
                        }
                        else
                        {
                            return merchantMasterPersonality.Offer(npc, performer);
                        }
                    }
                }
            }

            return new Result("There is no merchant here to sell to.", true);
        }
    }
}
