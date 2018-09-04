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
    public class Join : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Join", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Join" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            int targetGuildMasterNumber = command.Parameters.Count == 0 ? 0 : command.Parameters[0].ParameterNumber;
            int currentGuildMaster = 0;

            foreach (INonPlayerCharacter npc in performer.Room.NonPlayerCharacters)
            {
                foreach (IPersonality personality in npc.Personalities)
                {
                    IGuildMaster guildMasterPersonality = personality as IGuildMaster;
                    if (guildMasterPersonality != null)
                    {
                        if (targetGuildMasterNumber == currentGuildMaster++)
                        {
                            return guildMasterPersonality.Join(npc, performer);
                        }
                    }
                }
            }
            return new Result("There is no Guildmaster here to induct you.", true);
        }
    }
}
