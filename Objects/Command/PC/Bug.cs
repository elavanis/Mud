using Objects.Command.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Objects.Command.PC
{
    public class Bug : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Bug [Bug Description]", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Bug" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            if (command.Parameters.Count <= 0)
            {
                return new Result("Please describe the bug.", true);
            }
            else
            {
                string filer = performer.KeyWords[0];
                string fileName = Path.Combine(GlobalReference.GlobalValues.Settings.BugDirectory, $"{filer} - {DateTime.Now.ToString("yyyyMMddhhmmss")}.bug");

                GlobalReference.GlobalValues.FileIO.EnsureDirectoryExists(GlobalReference.GlobalValues.Settings.BugDirectory);
                GlobalReference.GlobalValues.FileIO.WriteFile(fileName, command.Parameters[0].ParameterValue);

                return new Result("New bug filed.", true);
            }
        }
    }
}
