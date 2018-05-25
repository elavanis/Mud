using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Mob.Interface;
using Objects.Global;
using Objects.Global.Direction;
using static Objects.Global.Direction.Directions;

namespace Objects.Magic.Enchantment
{
    public class LeaveRoomEnchantment : BaseEnchantment
    {
        public Direction Direction { get; set; }

        public override void LeaveRoom(IMobileObject performer, Direction direction)
        {
            if (Direction == direction
                && GlobalReference.GlobalValues.Random.PercentDiceRoll(ActivationPercent))
            {
                Parameter.Target = performer;
                Effect.ProcessEffect(Parameter);
            }
        }
    }
}
