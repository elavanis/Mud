using Objects.Command.Interface;
using Objects.Mob.Interface;
using Objects.Skill.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Command.PC
{
    public class Abilities : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Abilities() : base(nameof(Abilities), ShortCutCharPositions.Any) { }

        public IResult Instructions { get; } = new Result("Abils|Abilities", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Abils", "Abilities" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult? result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            StringBuilder strBldr = new StringBuilder();

            strBldr.AppendLine("Spells");
            if (performer.SpellBook.Count == 0)
            {
                strBldr.AppendLine("<None>");
            }
            else
            {
                foreach (string spell in performer.SpellBook.Keys)
                {
                    strBldr.AppendLine(spell);
                }
            }
            strBldr.AppendLine();

            strBldr.AppendLine("Skills");
            if (performer.KnownSkills.Count == 0)
            {
                strBldr.AppendLine("<None>");
            }
            else
            {
                int skillNameMaxLength = 0;
                foreach (string skill in performer.KnownSkills.Keys)
                {
                    skillNameMaxLength = Math.Max(skillNameMaxLength, skill.Length);
                }

                foreach (string skill in performer.KnownSkills.Keys)
                {
                    ISkill baseskill = performer.KnownSkills[skill];
                    string activePassive;
                    if (baseskill.Passive)
                    {
                        activePassive = "Passive";
                    }
                    else
                    {
                        activePassive = "Active";
                    }
                    strBldr.AppendLine(string.Format("{0} - {1}", skill.PadRight(skillNameMaxLength), activePassive));
                }
            }

            return new Result(strBldr.ToString().Trim(), false);
        }
    }
}