using Objects.Command.Interface;
using System.Collections.Generic;
using Objects.Mob.Interface;
using Objects.Global;
using static Shared.TagWrapper.TagWrapper;
using Objects.Language;

namespace Objects.Command.PC
{
    public class Logout : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Logout", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Logout" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            if (performer is IPlayerCharacter pc)
            {
                GlobalReference.GlobalValues.World.LogOutCharacter(pc);

                GlobalReference.GlobalValues.Notify.Mob(pc, new TranslationMessage("You have been successfully logged out."));

                return new Result("Exit Connection", false, TagType.Connection);
            }
            else
            {
                return new Result("Only PlayerCharacters can logout.", true);
            }
        }
    }
}
