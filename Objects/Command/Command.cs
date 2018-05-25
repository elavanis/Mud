using Objects.Command.Interface;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Command
{
    public class Command : ICommand
    {
        [ExcludeFromCodeCoverage]
        public string CommandName { get; set; }

        [ExcludeFromCodeCoverage]
        public List<IParameter> Parameters { get; set; } = new List<IParameter>();
    }
}
