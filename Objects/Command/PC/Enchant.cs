using Objects.Command.Interface;
using System;
using System.Collections.Generic;
using Objects.Mob.Interface;
using Objects.Item.Interface;
using Objects.Global;
using Objects.Interface;
using Objects.Item.Items.Interface;

namespace Objects.Command.PC
{
    public class Enchant : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Enchant() : base(nameof(Enchant), ShortCutCharPositions.Standing) { }

        public IResult Instructions { get; } = new Result("Enchant [Item Name]", false);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Enchant" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult? result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            if (command.Parameters.Count >= 1)
            {
                IBaseObject obj = GlobalReference.GlobalValues.FindObjects.FindObjectOnPersonOrInRoom(performer, "Enchantery", 0);

                if (obj is IEnchantery enchantery)
                {
                    obj = GlobalReference.GlobalValues.FindObjects.FindHeldItemsOnMob(performer, command.Parameters[0].ParameterValue, command.Parameters[0].ParameterNumber);
                    if (obj is IItem item)
                    {
                        ulong goldCost = (ulong)(enchantery.CostToEnchantLevel1Item * Math.Pow(GlobalReference.GlobalValues.Settings.Multiplier, item.Level));

                        if (performer.Money >= goldCost)
                        {
                            result = enchantery.Enchant(item);
                            if (result == null)
                            {
                                //unable to enchant for some reason
                                return new Result("Nothing seems to happen.", true);
                            }
                            else
                            {
                                performer.Money -= goldCost;
                                if (result.AllowAnotherCommand)
                                {
                                    performer.Items.Remove(item);
                                }
                            }
                            return result;
                        }
                        else
                        {
                            return new Result(string.Format("You need {0} to bind the enchantment.", GlobalReference.GlobalValues.MoneyToCoins.FormatedAsCoins(goldCost)), true);
                        }
                    }
                    else
                    {
                        return new Result(string.Format("Unable to find the {0}.", command.Parameters[0].ParameterValue), true);
                    }
                }
                else
                {
                    return new Result("There is nothing to enchant with here.", true);
                }
            }
            else
            {
                return new Result("What would you like to enchant?", true);
            }
        }
    }
}
