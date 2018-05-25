using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Command.Interface
{
    public interface IMobileObjectCommand
    {
        IResult Instructions { get; }
        IEnumerable<string> CommandTrigger { get; }
        IResult PerformCommand(IMobileObject performer, ICommand command);
    }
}
