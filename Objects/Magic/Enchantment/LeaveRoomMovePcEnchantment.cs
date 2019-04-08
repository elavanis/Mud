using Objects.Global;
using Objects.Mob.Interface;
using System.Diagnostics.CodeAnalysis;
using static Objects.Global.Direction.Directions;

namespace Objects.Magic.Enchantment
{
    public class LeaveRoomMovePcEnchantment : BaseEnchantment
    {
        [ExcludeFromCodeCoverage]
        public Direction Direction { get; set; }

        [ExcludeFromCodeCoverage]
        public BaseObjectId RoomId { get; set; }

        public override void LeaveRoom(IMobileObject performer, Direction direction)
        {
            if (Direction == direction
                && performer is IPlayerCharacter
                && GlobalReference.GlobalValues.Random.PercentDiceRoll(ActivationPercent))
            {
                Parameter.Target = performer;
                Parameter.RoomId = RoomId;
                Effect.ProcessEffect(Parameter);
            }
        }
    }
}
