using System.Collections.Generic;

namespace Objects.Command.Interface
{
    public interface ICommand
    {
        string CommandName { get; }
        List<IParameter> Parameters { get; }
    }
}