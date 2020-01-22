using Objects.Damage.Interface;
using Objects.Die.Interface;
using Objects.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using static Objects.Global.Direction.Directions;

namespace Objects.Effect.Interface
{
    public interface IEffectParameter
    {
        IDamage Damage { get; set; }
        ITranslationMessage PerformerMessage { get; set; }
        ITranslationMessage TargetMessage { get; set; }
        ITranslationMessage RoomMessage { get; set; }
        IBaseObjectId ObjectId { get; set; }
        IRoom ObjectRoom { get; set; }
        IItem Item { get; set; }
        IMobileObject Defender { get; set; }
        IMobileObject Attacker { get; set; }
        IDice Dice { get; set; }
        IBaseObjectId RoomId { get; set; }
        IMobileObject Performer { get; set; }
        IBaseObject Target { get; set; }
        IContainer Container { get; set; }
        Direction Direction { get; set; }
        string Description { get; set; }
    }
}