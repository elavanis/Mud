using Objects.Global;
using Objects.Item.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using Objects.Personality.Personalities.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Command.Interface;
using Objects.Command;

namespace Objects.Personality.Personalities
{
    public class Merchant : IMerchant
    {
        [ExcludeFromCodeCoverage]
        public double BuyFromPcDecrease { get; set; } = .1;

        [ExcludeFromCodeCoverage]
        public double SellToPcIncrease { get; set; } = 10;

        [ExcludeFromCodeCoverage]
        public List<IItem> Sellables { get; set; } = new List<IItem>();

        private double CharismaEffect(INonPlayerCharacter merchant, IMobileObject performer)
        {
            return ((double)merchant.CharismaEffective) / performer.CharismaEffective;
        }

        #region Sell
        public IResult Sell(INonPlayerCharacter merchant, IMobileObject performer, IItem item)
        {
            if (item.Level <= merchant.Level)
            {
                ulong amount = BuyPrice(merchant, performer, item);

                performer.Money += amount;
                performer.Items.Remove(item);

                string message = string.Format("You sold the {0} for {1}.", item.SentenceDescription, GlobalReference.GlobalValues.MoneyToCoins.FormatedAsCoins(amount));
                return new Result(message, false);
            }
            else
            {
                return new Result("The merchant was unwilling to buy that from you.", true);
            }
        }

        private ulong BuyPrice(INonPlayerCharacter merchant, IMobileObject performer, IItem item)
        {
            return (ulong)(item.Value * BuyFromPcDecrease / CharismaEffect(merchant, performer));
        }

        public IResult Offer(INonPlayerCharacter merchant, IMobileObject performer)
        {
            StringBuilder strBldr = new StringBuilder();
            foreach (IItem item in performer.Items)
            {
                strBldr.AppendLine(string.Format("{0} - {1}", item.ShortDescription, BuyPrice(merchant, performer, item)));
            }

            return new Result(strBldr.ToString().Trim(), false);
        }
        #endregion Sell

        #region Buy
        public IResult Buy(INonPlayerCharacter merchant, IMobileObject performer, int itemNumber)
        {
            int realItemNumber = itemNumber - 1;
            if (realItemNumber < Sellables.Count)
            {
                IItem item = Sellables[realItemNumber];
                ulong amount = SellPrice(merchant, performer, item);

                if (performer.Money >= amount)
                {
                    performer.Money -= amount;
                    performer.Items.Add(item);

                    string message = string.Format("You bought the {0} for {1}.", item.SentenceDescription, GlobalReference.GlobalValues.MoneyToCoins.FormatedAsCoins(amount));
                    return new Result(message, false);
                }
                else
                {
                    string message = string.Format("You need {0} to buy the {1}.", GlobalReference.GlobalValues.MoneyToCoins.FormatedAsCoins(amount), item.SentenceDescription);
                    return new Result(message, true);
                }
            }
            else
            {
                return new Result("The merchant does not carry that many items.", true);
            }
        }

        private ulong SellPrice(INonPlayerCharacter merchant, IMobileObject performer, IItem item)
        {
            return (ulong)(item.Value * SellToPcIncrease * CharismaEffect(merchant, performer));
        }

        public IResult List(INonPlayerCharacter npc, IMobileObject performer)
        {
            StringBuilder strBlrd = new StringBuilder();

            int padLength = Math.Max(4, Sellables.Count.ToString().Length);
            int padNameLength = 0;
            foreach (IItem item in Sellables)
            {
                padNameLength = Math.Max(padNameLength, item.ShortDescription.Length);
            }

            strBlrd.AppendLine(string.Format("{0} {1} {2}", "Item".PadRight(padLength), "Name".PadRight(padNameLength), "Price"));
            for (int i = 0; i < Sellables.Count; i++)
            {
                IItem item = Sellables[i];
                string formatedItemLine = string.Format("{0} {1} {2}", (i + 1).ToString().PadRight(padLength),
                                                                        item.ShortDescription.PadRight(padNameLength),
                                                                        GlobalReference.GlobalValues.MoneyToCoins.FormatedAsCoins(SellPrice(npc, performer, item)));
                strBlrd.AppendLine(formatedItemLine);
            }

            return new Result(strBlrd.ToString().Trim(), false);
        }
        #endregion Buy

        public string Process(INonPlayerCharacter npc, string command)
        {
            return command;
        }
    }
}
