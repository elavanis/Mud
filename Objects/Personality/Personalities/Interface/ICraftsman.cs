using Objects.Command.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using static Objects.Damage.Damage;
using static Objects.Item.Items.Equipment;

namespace Objects.Personality.Personalities.Interface
{
    public interface ICraftsman : IPersonality
    {
        double SellToPcIncrease { get; set; }
        IResult Build(INonPlayerCharacter craftsman, IPlayerCharacter performer, AvalableItemPosition position, int level, string sentenceDescription, string keyword, string shortDescription, string lookDescription, string examineDescription, DamageType damageType = DamageType.Acid);
    }
}
