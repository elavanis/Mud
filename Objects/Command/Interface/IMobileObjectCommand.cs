using Objects.Mob.Interface;
using System.Collections.Generic;
using static Objects.Mob.MobileObject;

namespace Objects.Command.Interface
{
    public interface IMobileObjectCommand
    {
        string CommandName { get; }
        HashSet<CharacterPosition> AllowedCharacterPositions { get; }
        IResult Instructions { get; }
        IEnumerable<string> CommandTrigger { get; }
        IResult PerformCommand(IMobileObject performer, ICommand command);
    }
}
