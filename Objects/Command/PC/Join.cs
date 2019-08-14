using Objects.Command.Interface;
using System.Collections.Generic;
using Objects.Mob.Interface;
using Objects.Personality.Interface;

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
                    if (personality is IGuildMaster guildMasterPersonality)
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
