using Objects.Mob.Interface;
using System.Collections.Generic;

namespace Objects.Command.Interface
{
    public interface IMobileObjectCommand
    {
        IResult Instructions { get; }
        IEnumerable<string> CommandTrigger { get; }
        IResult PerformCommand(IMobileObject performer, ICommand command);
    }
}
