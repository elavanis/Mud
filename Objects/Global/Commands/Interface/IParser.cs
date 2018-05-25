using Objects.Command.Interface;

namespace Objects.Global.Commands.Interface
{
    public interface IParser
    {
        ICommand Parse(string text);
    }
}