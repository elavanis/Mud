using Objects.Command.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Command.PC
{
    public class Title : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Title {Message Id}", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Title" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IPlayerCharacter pc = performer as IPlayerCharacter;

            if (pc == null)
            {
                return new Result("Only players can set a title.", true);
            }

            if (command.Parameters.Count == 0)
            {
                StringBuilder stringBuilder = new StringBuilder();
                int titlePosition = 0;
                foreach (string title in pc.AvailableTitles)
                {
                    stringBuilder.AppendLine($"{titlePosition++} \t title");
                }

                return new Result(stringBuilder.ToString().Trim(), true);
            }
            else
            {
                int titleId;
                if (int.TryParse(command.Parameters[0].ParameterValue, out titleId))
                {
                    int titlePostion = 0;
                    foreach (string title in pc.AvailableTitles)
                    {
                        if (titlePostion == titleId)
                        {
                            pc.Title = title;

                            return new Result($"Title updated to {title}", true);
                        }
                        else
                        {
                            titlePostion++;
                        }
                    }

                    return new Result("No title found at that position.", true);

                }
                return Instructions;
            }
        }
    }
}
