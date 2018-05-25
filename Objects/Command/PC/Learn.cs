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
    public class Learn : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result(true, "Learn [Skill/Spell Name]");

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Learn" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            foreach (INonPlayerCharacter npc in performer.Room.NonPlayerCharacters)
            {
                foreach (IPersonality personality in npc.Personalities)
                {
                    IGuildMaster guildMasterPersonality = personality as IGuildMaster;
                    if (guildMasterPersonality != null)
                    {
                        if (command.Parameters.Count > 0)
                        {
                            return guildMasterPersonality.Teach(npc, performer, command.Parameters[0].ParameterValue);
                        }
                        else
                        {
                            return guildMasterPersonality.Teachable(npc, performer);
                        }
                    }
                }
            }

            return new Result(false, "There is no GuildMaster here to teach you.");
        }
    }
}
