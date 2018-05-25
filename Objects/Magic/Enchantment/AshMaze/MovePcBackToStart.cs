using Objects.Global;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Global.Direction.Directions;

namespace Objects.Magic.Enchantment.AshMaze
{
    public class MovePcBackToStart : BaseEnchantment
    {
        public Direction Direction { get; set; }

        public override void LeaveRoom(IMobileObject performer, Direction direction)
        {
            if (Direction == direction
                && performer is IPlayerCharacter
                && GlobalReference.GlobalValues.Random.PercentDiceRoll(ActivationPercent))
            {
                Parameter.Target = performer;
                Parameter.RoomId = new BaseObjectId(18, 1);
                Effect.ProcessEffect(Parameter);
            }
        }
    }
}
