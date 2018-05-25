using System.Collections.Generic;
using Objects.Guild;
using Objects.Guild.Interface;
using Objects.Magic.Spell.Damage;
using Objects.Magic.Spell.Heal.Health;
using Objects.Mob.Interface;
using Objects.Personality.Interface;

namespace Objects.Personality.Personalities.Interface
{
    public interface IMagicUser : IPersonality
    {
        List<BaseCureSpell> CureSpell { get; set; }
        List<BaseDamageSpell> DamageSpells { get; set; }

        void AddSpells(INonPlayerCharacter npc, IGuild guild);
    }
}