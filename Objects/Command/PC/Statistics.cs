using Objects.Command.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Command.PC
{
    public class Statistics : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Statistics() : base(nameof(Statistics), ShortCutCharPositions.Any) { }

        public IResult Instructions { get; } = new Result("(Stats) Statistics", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Stats", "Statistics" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            int colmunPadding = 2;
            string columnPad = "".PadRight(colmunPadding, ' ');

            if (performer is IPlayerCharacter pc)
            {
                int column2Width = Math.Max(7, MaxStat(performer).ToString().Length + colmunPadding);


                StringBuilder strBldr = new StringBuilder();
                strBldr.AppendLine(string.Format("{0}   {1}", pc.Name, pc.Race.ToString()));
                strBldr.AppendLine("Level " + pc.Level);
                strBldr.AppendLine(string.Format("EXP {0}   To Next Level {1}", pc.Experience, GlobalReference.GlobalValues.Experience.GetExpForLevel(pc.Level) - pc.Experience));
                strBldr.Append(GlobalReference.GlobalValues.MoneyToCoins.FormatedAsCoins(pc.Money));

                strBldr.AppendLine();
                strBldr.AppendLine(string.Format("Stat{0}Value{1}Bonus", columnPad, "".PadRight(column2Width - 4)));
                strBldr.AppendLine(string.Format("STR:{0}{1}{2}", columnPad, pc.StrengthStat.ToString().PadRight(column2Width, ' '), pc.StrengthMultiClassBonus));
                strBldr.AppendLine(string.Format("DEX:{0}{1}{2}", columnPad, pc.DexterityStat.ToString().PadRight(column2Width, ' '), pc.DexterityMultiClassBonus));
                strBldr.AppendLine(string.Format("CON:{0}{1}{2}", columnPad, pc.ConstitutionStat.ToString().PadRight(column2Width, ' '), pc.ConstitutionMultiClassBonus));
                strBldr.AppendLine(string.Format("INT:{0}{1}{2}", columnPad, pc.IntelligenceStat.ToString().PadRight(column2Width, ' '), pc.IntelligenceMultiClassBonus));
                strBldr.AppendLine(string.Format("WIS:{0}{1}{2}", columnPad, pc.WisdomStat.ToString().PadRight(column2Width, ' '), pc.WisdomMultiClassBonus));
                strBldr.AppendLine(string.Format("CHA:{0}{1}{2}", columnPad, pc.CharismaStat.ToString().PadRight(column2Width, ' '), pc.CharismaMultiClassBonus));

                return new Result(strBldr.ToString().Trim(), true);
            }
            else
            {
                StringBuilder strBldr = new StringBuilder();
                strBldr.AppendLine(string.Format("{0}", performer.Race.ToString()));
                strBldr.AppendLine("Level " + performer.Level);
                strBldr.Append(GlobalReference.GlobalValues.MoneyToCoins.FormatedAsCoins(performer.Money));

                strBldr.AppendLine();
                strBldr.AppendLine(string.Format("Stat{0}Value", columnPad));
                strBldr.AppendLine(string.Format("STR:{0}{1}", columnPad, performer.StrengthStat.ToString()));
                strBldr.AppendLine(string.Format("DEX:{0}{1}", columnPad, performer.DexterityStat.ToString()));
                strBldr.AppendLine(string.Format("CON:{0}{1}", columnPad, performer.ConstitutionStat.ToString()));
                strBldr.AppendLine(string.Format("INT:{0}{1}", columnPad, performer.IntelligenceStat.ToString()));
                strBldr.AppendLine(string.Format("WIS:{0}{1}", columnPad, performer.WisdomStat.ToString()));
                strBldr.AppendLine(string.Format("CHA:{0}{1}", columnPad, performer.CharismaStat.ToString()));

                return new Result(strBldr.ToString().Trim(), true);
            }
        }

        private int MaxStat(IMobileObject performer)
        {
            return Math.Max(performer.StrengthStat,
                        Math.Max(performer.DexterityStat,
                        Math.Max(performer.ConstitutionStat,
                        Math.Max(performer.IntelligenceStat,
                        Math.Max(performer.WisdomStat, performer.CharismaStat)))));
        }
    }
}
