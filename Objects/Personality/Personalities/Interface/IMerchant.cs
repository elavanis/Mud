using Objects.Item.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using Objects.Command.Interface;
using System.Collections.Generic;

namespace Objects.Personality.Personalities.Interface
{
    public interface IMerchant : IPersonality
    {
        double BuyFromPcDecrease { get; set; }
        double SellToPcIncrease { get; set; }
        List<IItem> Sellables { get; set; }

        IResult Buy(INonPlayerCharacter merchant, IMobileObject performer, int itemNumber);
        IResult List(INonPlayerCharacter npc, IMobileObject performer);
        IResult Offer(INonPlayerCharacter merchant, IMobileObject performer);
        IResult Sell(INonPlayerCharacter merchant, IMobileObject performer, IItem item);
    }
}