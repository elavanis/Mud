using Objects.Command.Interface;
using Objects.Global;
using Objects.Language;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using System.Collections.Generic;
using System.Text;

namespace Objects.Command.PC
{
    public class Emote : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Emote [your emote message]", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Emote" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            if (command.Parameters.Count <= 0)
            {
                return new Result("What would you like to emote?", true);
            }

            string emoteMessage = BuildEmoteMessage(performer, command);

            ITranslationMessage message = new TranslationMessage(emoteMessage);
            GlobalReference.GlobalValues.Notify.Room(performer, null, performer.Room, message);
            return new Result("", true);
        }

        private static string BuildEmoteMessage(IMobileObject performer, ICommand command)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(GlobalReference.GlobalValues.StringManipulator.CapitalizeFirstLetter(performer.KeyWords[0]));
            stringBuilder.Append(" ");
            foreach (IParameter parameter in command.Parameters)
            {
                stringBuilder.Append(parameter.ParameterValue);
                stringBuilder.Append(" ");
            }

            return stringBuilder.ToString().Trim();
        }
    }
}
