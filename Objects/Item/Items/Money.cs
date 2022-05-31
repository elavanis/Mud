using Objects.Global;
using Objects.Item.Items.Interface;

namespace Objects.Item.Items
{
    public class Money : Item, IMoney
    {
        public Money() : this(0)
        {
        }

        public Money(ulong money):this(money,
                                        "While the purse is not worth anything the items in side are worth keeping.", 
                                        string.Format("A purse of coins worth {0}.", GlobalReference.GlobalValues.MoneyToCoins.FormatedAsCoins(money)),
                                        "coins",
                                        "A purse of coins.")
        {
        }

        public Money(ulong money, string examineDescription, string lookDescription, string sentenceDescription, string shortDescription) : base(examineDescription, lookDescription, sentenceDescription, shortDescription)
        {
            Value = money;
            AddKeywords();
        }

        public override ulong Value { get; set; }
        
        private void AddKeywords()
        {
            KeyWords.Add("money");
            KeyWords.Add("coins");
        }
    }
}
