using Objects.Command.Interface;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Command
{
    public class Command : ICommand
    {
        public Command() : this("", new List<IParameter>()) { }

        public Command(string commandName, List<IParameter> parameters)
        {
            CommandName = commandName;
            Parameters = parameters;
        }

        [ExcludeFromCodeCoverage]
        public string CommandName { get; set; }

        [ExcludeFromCodeCoverage]
        public List<IParameter> Parameters { get; set; }
    }
}
