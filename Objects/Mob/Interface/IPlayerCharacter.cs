using Objects.Crafting;
using Objects.Crafting.Interface;
using Objects.Interface;
using Objects.Item.Items.Interface;
using Objects.Room;
using Objects.Room.Interface;
using System;
using System.Collections.Generic;

namespace Objects.Mob.Interface
{
    public interface IPlayerCharacter : IMobileObject
    {
        int CharismaBonus { get; set; }
        int CharismaMultiClassBonus { get; set; }
        int ConstitutionBonus { get; set; }
        int ConstitutionMultiClassBonus { get; set; }
        List<ICorpse> Corpses { get; }
        List<ICraftsmanObject> CraftsmanObjects { get; }
        bool Debug { get; set; }
        int DexterityBonus { get; set; }
        int DexterityMultiClassBonus { get; set; }
        int Experience { get; set; }
        int IntelligenceBonus { get; set; }
        int IntelligenceMultiClassBonus { get; set; }
        string Name { get; set; }
        string Password { get; set; }
        int StrengthBonus { get; set; }
        int StrengthMultiClassBonus { get; set; }
        int WisdomBonus { get; set; }
        int WisdomMultiClassBonus { get; set; }
        void RemoveOldCorpses(DateTime utcDate);
        IBaseObjectId RespawnPoint { get; set; }
        string GotoEnterMessage { get; set; }
        string GotoLeaveMessage { get; set; }
    }
}