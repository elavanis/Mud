using Objects.Command.Interface;
using Objects.Global;
using Objects.Global.Stats;
using Objects.Mob.Interface;
using Objects.Trap.Interface;
using System.Collections.Generic;
using System.Linq;
using static Objects.Mob.MobileObject;

namespace Objects.Command.PC
{
    public class Disarm : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Disarm() : base(nameof(Disarm), ShortCutCharPositions.Standing) { }

        public IResult Instructions { get; } = new Result("Disarm [Trap]", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Disarm" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            if (command.Parameters.Count <= 0)
            {
                return new Result("What would you like to disarm?", true);
            }

            string itemToDisarm = command.Parameters[0].ParameterValue;
            ITrap trap = performer.Room.Traps.FirstOrDefault(i => i.DisarmWord.Contains(itemToDisarm));

            if (trap == null)
            {
                return new Result($"Unable to find a {command.Parameters[0].ParameterValue} trap.", true);
            }
            else
            {
                int performerRoll = GetPlayerRoll(trap, performer);
                if (performerRoll >= trap.DisarmSuccessRoll)
                {
                    performer.Room.Traps.Remove(trap);
                    return new Result($"You successfully disarm the {itemToDisarm} trap.", false);
                }
                else
                {
                    if (trap.DisarmFailureDamage != null)
                    {
                        performer.TakeDamage(trap.DisarmFailureDamage.Dice.RollDice(), trap.DisarmFailureDamage, trap.Description);
                        if (trap.DisarmFailureMessage != null)
                        {
                            return new Result(trap.DisarmFailureMessage, false);
                        }
                        else
                        {
                            return new Result("You tried to disarm the trap but accidentally tripped it.", false);
                        }
                    }
                    else
                    {
                        return new Result($"You were unable to disarm the {itemToDisarm} trap.", false);
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