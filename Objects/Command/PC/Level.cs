using Objects.Command.Interface;
using System.Collections.Generic;
using Objects.Mob.Interface;

namespace Objects.Command.PC
{
    public class Level : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Level() : base(nameof(Level), ShortCutCharPositions.Any) { }

        public IResult Instructions { get; } = new Result("Level [Stat] {Amount To Level}", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Level" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult? result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            if (command.Parameters.Count == 0)
            {
                string localmessage = string.Format("You have {0} points to spend.", performer.LevelPoints);
                return new Result(localmessage, false);
            }

            if (performer.LevelPoints == 0)
            {
                return new Result("You have no points to spend at this time.", true);
            }


            int increase = 1;
            if (command.Parameters.Count >= 2)
            {
                int.TryParse(command.Parameters[1].ParameterValue, out increase);
                if (increase > performer.LevelPoints)
                {
                    return new Result("You do not have that many points to spend.", true);
                }
            }

            string message;
            performer.LevelPoints -= increase;
            switch (command.Parameters[0].ParameterValue.ToUpper())
            {
                case "STR":
                case "STRENGTH":
                    message = "You feel stronger.";
                    performer.StrengthStat += increase;
                    break;
                case "DEX":
                case "DEXTERITY":
                    message = "You feel more agile.";
                    performer.DexterityStat += increase;
                    break;
                case "CON":
                case "CONSTITUTION":
                    message = "You feel healthier.";
                    performer.ConstitutionStat += increase;
                    break;
                case "INT":
                case "INTELLIGENCE":
                    message = "You feel smarter.";
                    performer.IntelligenceStat += increase;
                    break;
                case "WIS":
                case "WISDOM":
                    message = "You feel wiser.";
                    performer.WisdomStat += increase;
                    break;
                case "CHA":
                case "CHARISMA":
                    message = "You feel better looking.";
                    performer.CharismaStat += increase;
                    break;
                default:
                    //add back the level points that were deducted
                    performer.LevelPoints += increase;
                    return new Result("Unsure which stat to increase.", true);
            }
            performer.ResetMaxStatValues();
            return new Result(message, false);
        }
    }
}
