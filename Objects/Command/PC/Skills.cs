using Objects.Command.Interface;
using Objects.Mob.Interface;
using Objects.Skill.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace Objects.Command.PC
{
    public class Skills : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Skills", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Skills" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IOrderedEnumerable<ISkill> skills = performer.KnownSkills.Values.OrderBy(e => e.StaminaCost);

            int nameLength = 0;
            foreach (ISkill skill in skills)
            {
                nameLength = Math.Max(nameLength, skill.AbilityName.Length);
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Skill".PadRight(nameLength) + "  Stamina Cost");
            foreach (ISkill skill in skills)
            {
                stringBuilder.AppendLine($"{skill.AbilityName.PadRight(nameLength)}  {skill.StaminaCost}");
            }

            IResult result = new Result(stringBuilder.ToString().Trim(), true, TagType.Info);

            return result;
        }
    }
}
