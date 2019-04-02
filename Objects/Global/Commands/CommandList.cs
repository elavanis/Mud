using Objects.Command.Interface;
using Objects.Global.Commands.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Objects.Mob.Interface;
using System.Diagnostics;

namespace Objects.Global.Commands
{
    public class CommandList : ICommandList
    {
        [ExcludeFromCodeCoverage]
        public SortedDictionary<string, IMobileObjectCommand> PcCommands { get; } = CreateCommandList("PC");

        [ExcludeFromCodeCoverage]
        public SortedDictionary<string, IMobileObjectCommand> GodCommands { get; } = CreateCommandList("God");


        [ExcludeFromCodeCoverage]
        public Dictionary<string, IMobileObjectCommand> PcCommandsLookup { get; } = CreateCommandLookupList("PC");

        [ExcludeFromCodeCoverage]
        public Dictionary<string, IMobileObjectCommand> GodCommandsLookup { get; } = CreateCommandLookupList("God");



        private static SortedDictionary<string, IMobileObjectCommand> CreateCommandList(string commandType)
        {
            IEnumerable<Type> list = from classes in typeof(CommandList).Assembly.GetTypes()
                                     select classes;

            SortedDictionary<string, IMobileObjectCommand> sortedDictionary = new SortedDictionary<string, IMobileObjectCommand>();

            foreach (Type item in list)
            {
                if (item.GetInterfaces().Contains(typeof(IMobileObjectCommand)))
                {
                    if (item.Namespace.EndsWith(commandType))
                    {
                        IMobileObjectCommand command = (IMobileObjectCommand)Activator.CreateInstance(item);
                        foreach (string trigger in command.CommandTrigger)
                        {
                            sortedDictionary.Add(trigger, command);
                        }
                    }
                }
            }

            return sortedDictionary;
        }

        private static Dictionary<string, IMobileObjectCommand> CreateCommandLookupList(string commandType)
        {
            SortedDictionary<string, IMobileObjectCommand> sortedDictionary = CreateCommandList(commandType);

            //We could have just done a search on the sorted dictionary for the 1st command that matched
            //the whole string passed in but I decided it would be better to go ahead and make a dictionary 
            //entry for every possibility.  Startup is a bit slower this way runtime look up is as fast as possible.
            Dictionary<string, IMobileObjectCommand> allKeysDictionary = new Dictionary<string, IMobileObjectCommand>();

            foreach (string key in sortedDictionary.Keys)
            {
                IMobileObjectCommand mobCommand = sortedDictionary[key];

                for (int i = 1; i <= key.Length; i++)
                {
                    string subKey = key.Substring(0, i).ToUpper();
                    if (!allKeysDictionary.ContainsKey(subKey))
                    {
                        allKeysDictionary.Add(subKey, mobCommand);
                    }
                }
            }

            return allKeysDictionary;
        }

        public IMobileObjectCommand GetCommand(IMobileObject performer, string command)
        {
            if (!PcCommandsLookup.TryGetValue(command, out IMobileObjectCommand iMobileObjectCommand))
            {
                if (performer.God)
                {
                    GodCommandsLookup.TryGetValue(command, out iMobileObjectCommand);
                }
            }

            return iMobileObjectCommand;
        }
    }
}
