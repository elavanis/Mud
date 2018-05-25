using System.Collections.Generic;

namespace Objects.Command.Interface
{
    public interface ICommand
    {
        string CommandName { get; set; }
        List<IParameter> Parameters { get; set; }
    }
}