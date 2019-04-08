using Objects.Command.Interface;
using System;
using System.Collections.Generic;
using Objects.Mob.Interface;
using Objects.Global;

namespace Objects.Command.PC
{
    public class Save : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Save", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Save" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            if (performer is IPlayerCharacter pc)
            {
                pc.RemoveOldCorpses(DateTime.UtcNow.AddMonths(-1));
                GlobalReference.GlobalValues.World.SaveCharcter(pc);

                return new Result("Character Saved", true);
            }
            else
            {
                return new Result("Only PlayerCharacters can save.", true);
            }
        }
    }
}
