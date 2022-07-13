using Objects.Command.Interface;
using System.Collections.Generic;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using static Objects.Mob.MobileObject;

namespace Objects.Command.PC
{
    public class Buy : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Buy() : base(nameof(Buy), ShortCutCharPositions.Awake) { }

        public IResult Instructions { get; } = new Result("Buy {Item Number}", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Buy" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult? result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            foreach (INonPlayerCharacter npc in performer.Room.NonPlayerCharacters)
            {
                foreach (IPersonality personality in npc.Personalities)
                {
                    if (personality is IMerchant merchantPersonality)
                    {
                        if (command.Parameters.Count > 0)
                        {
                            int.TryParse(command.Parameters[0].ParameterValue, out int item);
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