using Objects.Command.Interface;
using Objects.Item.Interface;
using Objects.Item.Items;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Damage.Damage;
using static Objects.Item.Items.Equipment;

namespace Objects.Personality.Personalities.Interface
{
    public interface ICraftsman : IPersonality
    {
        double SellToPcIncrease { get; set; }
        IResult Build(INonPlayerCharacter craftsman, IPlayerCharacter performer, AvalableItemPosition position, int level, string sentenceDescription, string keyword, string shortDescription, string longDescription, string examineDescription, DamageType damageType = DamageType.Acid);
    }
}
