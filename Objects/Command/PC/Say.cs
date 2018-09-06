using Objects.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Mob.Interface;
using static Shared.TagWrapper.TagWrapper;
using Objects.Global;
using Objects.Language.Interface;
using Objects.Language;

namespace Objects.Command.PC
{
    public class Say : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Say [Message]", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Say" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            if (command.Parameters.Count > 0)
            {
                string statement = string.Join(" ", command.Parameters.Select(i => i.ParameterValue));

                string message = string.Format("{0} says {1}", performer.SentenceDescription, statement);
                ITranslationMessage translationMessage = new TranslationMessage(message, TagType.Communication);
                GlobalReference.GlobalValues.Notify.Room(performer, null, performer.Room, translationMessage, new List<IMobileObject>() { performer }, false, true);

                return new Result("", false);
            }
            else
            {
                return new Result("What would you like to say?", true);
            }
        }
    }
}
