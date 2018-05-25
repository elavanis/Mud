using Objects.Global;
using Objects.Item.Items.Interface;

namespace Objects.Item.Items
{
    public class Money : Item, IMoney
    {
        public Money() : this(0)
        {
        }

        public Money(ulong money)
        {
            Value = money;
            AddKeywords();
        }


        private ulong _value;
        public override ulong Value
        {
            get
            {
                return _value;
            }
            set
            {
                SentenceDescription = "coins";
                ShortDescription = "A purse of coins.";
                LongDescription = string.Format("A purse of coins worth {0}.", GlobalReference.GlobalValues.MoneyToCoins.FormatedAsCoins(value));
                ExamineDescription = "While the purse is not worth anything the items in side are worth keeping.";
                _value = value;
            }
        }

        private void AddKeywords()
        {
            KeyWords.Add("money");
            KeyWords.Add("coins");
        }
    }
}
