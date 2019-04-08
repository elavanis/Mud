using Objects.Personality.Personalities.Interface;
using System.Collections.Generic;
using Objects.Global.Direction;
using Objects.Mob.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Personality.Personalities
{
    public class Guard : IGuard
    {
        public Guard()
        {

        }

        public Guard(Directions.Direction direction)
        {
            GuardDirections.Add(direction);
        }

        [ExcludeFromCodeCoverage]
        public HashSet<Directions.Direction> GuardDirections { get; set; } = new HashSet<Directions.Direction>();

        [ExcludeFromCodeCoverage]
        public string BlockLeaveMessage { get; set; } = "You attempt to leave but your way is suddenly blocked by a guard.";

        public string Process(INonPlayerCharacter npc, string command)
        {
            return command;
        }
    }
}
