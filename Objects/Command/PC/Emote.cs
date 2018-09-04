using Objects.Command.Interface;
using Objects.Global;
using Objects.Global.Notify;
using Objects.Language;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Command.PC
{
    public class Emote : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Emote \"[what you want to emote]\"", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Emote" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            if (command.Parameters.Count <= 0)
            {
                return new Result("What would you like to emote?", true);
            }

            IParameter parm = command.Parameters[0];

            ITranslationMessage message = new TranslationMessage(parm.ParameterValue);
            GlobalReference.GlobalValues.Notify.Room(performer, null, performer.Room, message);
            return new Result(null, true);
        }
    }
}
