using Objects.Global.MoneyToCoins.Interface;

namespace Objects.Global.MoneyToCoins
{
    public class MoneyToCoins : IMoneyToCoins
    {
        public string FormatedAsCoins(ulong money)
        {
            ulong platinum = 0;
            ulong gold = 0;
            ulong silver = 0;
            ulong copper = 0;

            ulong multiplier = 1000000000;
            platinum = money / multiplier;
            money -= platinum * multiplier;

            multiplier = 1000000;
            gold = money / multiplier;
            money -= gold * multiplier;

            multiplier = 1000;
            silver = money / multiplier;
            money -= silver * multiplier;

            copper = money;

            string formatedMoney = "";
            if (platinum > 0)
            {
                formatedMoney += string.Format("{0} platinum ", platinum);
            }

            if (gold > 0)
            {
                formatedMoney += string.Format("{0} gold ", gold);
            }

            if (silver > 0)
            {
                formatedMoney += string.Format("{0} silver ", silver);
            }

            if (copper > 0)
            {
                formatedMoney += string.Format("{0} copper ", copper);
            }

            if (formatedMoney == "")
            {
                formatedMoney = "0 copper";
            }
            else
            {
                formatedMoney = formatedMoney.Trim();
            }

            return formatedMoney;
        }
    }
}
