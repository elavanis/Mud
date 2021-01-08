using Objects.Command.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Command.PC
{
    public class Title : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Title() : base(nameof(Title), ShortCutCharPositions.Any) { }

        public IResult Instructions { get; } = new Result("Title {Message Id}", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Title" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            IPlayerCharacter pc = performer as IPlayerCharacter;

            if (pc == null)
            {
                return new Result("Only players can set a title.", true);
            }

            if (command.Parameters.Count == 0)
            {
                if (pc.AvailableTitles.Count == 0)
                {
                    return new Result("Sorry, you have not earned any titles yet.", true);
                }


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
