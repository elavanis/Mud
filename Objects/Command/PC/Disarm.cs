using Objects.Command.Interface;
using Objects.Global;
using Objects.Global.Stats;
using Objects.Magic.Interface;
using Objects.Mob.Interface;
using Objects.Trap.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Command.PC
{
    public class Disarm : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result(true, "Disarm [Trap]");

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Disarm" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            if (command.Parameters.Count <= 0)
            {
                return new Result(false, "What would you like to disarm?");
            }

            string itemToDisarm = command.Parameters[0].ParameterValue;
            ITrap trap = performer.Room.Traps.FirstOrDefault(i => i.DisarmWord.Contains(itemToDisarm));

            if (trap == null)
            {
                return new Result(true, $"Unable to find a {command.Parameters[0].ParameterValue} trap.");
            }
            else
            {
                int performerRoll = GetPlayerRoll(trap, performer);
                if (performerRoll >= trap.DisarmSuccessRoll)
                {
                    performer.Room.Traps.Remove(trap);
                    return new Result(true, $"You successfully disarm the {itemToDisarm} trap.");
                }
                else
                {
                    if (trap.DisarmFailureDamage != null)
                    {
                        performer.TakeDamage(trap.DisarmFailureDamage.Dice.RollDice(), trap.DisarmFailureDamage, null);
                        if (trap.DisarmFailureMessage != null)
                        {
                            return new Result(true, trap.DisarmFailureMessage);
                        }
                        else
                        {
                            return new Result(true, "You tried to disarm the trap but accidentally tripped it.");
                        }
                    }
                    else
                    {
                        return new Result(true, $"You were unable to disarm the {itemToDisarm} trap.");
                    }
                }
            }
        }

        private int GetPlayerRoll(ITrap trap, IMobileObject performer)
        {
            switch (trap.DisarmStat)
            {
                case Stats.Stat.Charisma:
                    return GlobalReference.GlobalValues.Random.Next(performer.CharismaEffective);
                case Stats.Stat.Constitution:
                    return GlobalReference.GlobalValues.Random.Next(performer.ConstitutionEffective);
                case Stats.Stat.Dexterity:
                default:
                    return GlobalReference.GlobalValues.Random.Next(performer.DexterityEffective);
                case Stats.Stat.Intelligence:
                    return GlobalReference.GlobalValues.Random.Next(performer.IntelligenceEffective);
                case Stats.Stat.Strength:
                    return GlobalReference.GlobalValues.Random.Next(performer.StrengthEffective);
                case Stats.Stat.Wisdom:
                    return GlobalReference.GlobalValues.Random.Next(performer.WisdomEffective);
            }
        }
    }
}