using Objects.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Mob.Interface;
using Objects.Global;
using static Shared.TagWrapper.TagWrapper;
using Objects.Language;

namespace Objects.Command.PC
{
    public class Logout : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result(true, "Logout");

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Logout" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IPlayerCharacter pc = performer as IPlayerCharacter;
            if (pc != null)
            {
                IMobileObjectCommand foundCommand = GlobalReference.GlobalValues.CommandList.PcCommandsLookup["SAVE"];
                foundCommand.PerformCommand(performer, command);

                pc.Room.RemoveMobileObjectFromRoom(pc);
                pc.Room = null;

                GlobalReference.GlobalValues.Notify.Mob(pc, new TranslationMessage("You have been successfully logged out."));

                return new Result(true, "Exit Connection", TagType.Connection);
            }
            else
            {
                return new Result(false, "Only PlayerCharacters can logout.");
            }
        }
    }
}
