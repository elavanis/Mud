using System.Collections.Generic;
using Objects.Item.Items.Interface;
using Objects.LevelRange.Interface;
using Objects.Personality.Interface;
using Objects.Room.Interface;
using Objects.Interface;
using static Objects.Mob.NonPlayerCharacter;

namespace Objects.Mob.Interface
{
    public interface INonPlayerCharacter : IMobileObject, ILoadable
    {
        int CharismaMax { get; set; }
        int CharismaMin { get; set; }
        int ConstitutionMax { get; set; }
        int ConstitutionMin { get; set; }
        int DexterityMax { get; set; }
        int DexterityMin { get; set; }
        int EXP { get; set; }
        int IntelligenceMax { get; set; }
        int IntelligenceMin { get; set; }
        ILevelRange LevelRange { get; set; }
        List<IEquipment> NpcEquipedEquipment { get; }
        List<IPersonality> Personalities { get; }
        int StrengthMax { get; set; }
        int StrengthMin { get; set; }
        int WisdomMax { get; set; }
        int WisdomMin { get; set; }
        MobType? TypeOfMob { get; set; }
    }
}