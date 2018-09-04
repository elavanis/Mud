using Objects.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Mob.Interface;
using Objects.Skill.Interface;
using Objects.Global;

namespace Objects.Command.PC
{
    public class Perform : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("(P)erform [Skill Name] {Parameter(s)}", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "P", "Perform" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            if (command.Parameters.Count <= 0)
            {
                return new Result("What skill would you like to use?", true);
            }

            ISkill skill;
            performer.KnownSkills.TryGetValue(command.Parameters[0].ParameterValue.ToUpper(), out skill);
            if (skill != null)
            {
                GlobalReference.GlobalValues.Engine.Event.Perform(performer, skill.ToString());
                return skill.ProcessSkill(performer, command);
            }
            else
            {
                return new Result("You do not know that skill.", true);
            }
        }
    }
}
