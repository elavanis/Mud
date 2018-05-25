using System.Collections.Generic;
using Objects.Command.Interface;
using Objects.Mob.Interface;

namespace Objects.Global.Commands.Interface
{
    public interface ICommandList
    {
        SortedDictionary<string, IMobileObjectCommand> PcCommands { get; }
        SortedDictionary<string, IMobileObjectCommand> GodCommands { get; }
        Dictionary<string, IMobileObjectCommand> PcCommandsLookup { get; }
        Dictionary<string, IMobileObjectCommand> GodCommandsLookup { get; }

        IMobileObjectCommand GetCommand(IMobileObject performer, string command);
    }
}