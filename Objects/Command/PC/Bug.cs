using Objects.Command.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using System.Collections.Generic;
using System.IO;

namespace Objects.Command.PC
{
    public class Bug : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Bug() : base(nameof(Bug), ShortCutCharPositions.Any) { }

        public IResult Instructions { get; } = new Result("Bug [Bug Description]", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Bug" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult? result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            if (command.Parameters.Count <= 0)
            {
                return new Result("Please describe the bug.", true);
            }
            else
            {
                string filer = performer.KeyWords[0];
                string fileName = Path.Combine(GlobalReference.GlobalValues.Settings.BugDirectory, $"{filer} - {GlobalReference.GlobalValues.Time.CurrentDateTime.ToString("yyyyMMddhhmmss")}.bug");

                GlobalReference.GlobalValues.FileIO.WriteFile(fileName, command.Parameters[0].ParameterValue);

                return new Result("New bug filed.", true);
            }
        }
    }
}
