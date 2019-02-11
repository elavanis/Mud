using Objects.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Mob.Interface;
using Objects.Item.Interface;
using Objects.Global;
using Objects.Interface;
using Objects.Item.Items.Interface;
using Objects.Global.MoneyToCoins;

namespace Objects.Command.PC
{
    public class Enchant : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Enchant [Item Name]", false);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Enchant" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            if (command.Parameters.Count >= 1)
            {
                IBaseObject obj = GlobalReference.GlobalValues.FindObjects.FindObjectOnPersonOrInRoom(performer, "Enchantery", 0);

                IEnchantery enchantery = obj as IEnchantery;

                if (enchantery != null)
                {
                    obj = GlobalReference.GlobalValues.FindObjects.FindHeldItemsOnMob(performer, command.Parameters[0].ParameterValue, command.Parameters[0].ParameterNumber);
                    IItem item = obj as IItem;
                    if (item != null)
                    {
                        ulong goldCost = (ulong)(enchantery.CostToEnchantLevel1Item * Math.Pow(GlobalReference.GlobalValues.Settings.Multiplier, item.Level));

                        if (performer.Money >= goldCost)
                        {
                            IResult result = enchantery.Enchant(item);
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
