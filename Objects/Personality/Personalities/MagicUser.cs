using Objects.Global;
using Objects.Guild;
using Objects.Guild.Interface;
using Objects.Magic.Spell.Damage;
using Objects.Magic.Spell.Heal.Health;
using Objects.Mob.Interface;
using Objects.Personality.Personalities.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Objects.Personality.Personalities
{
    public class MagicUser : IMagicUser
    {
        public List<BaseDamageSpell> DamageSpells { get; set; } = new List<BaseDamageSpell>();
        public List<BaseCureSpell> CureSpell { get; set; } = new List<BaseCureSpell>();

        public void AddSpells(INonPlayerCharacter npc, IGuild guild)
        {
            foreach (GuildAbility ability in guild.Spells)
            {
                if (ability.Level <= npc.Level)
                {
                    BaseDamageSpell baseDamageSpell = ability.Abiltiy as BaseDamageSpell;

                    if (baseDamageSpell != null)
                    {
                        npc.SpellBook.Add(baseDamageSpell.SpellName.ToUpper(), baseDamageSpell);
                        DamageSpells.Add(baseDamageSpell);
                    }

                    BaseCureSpell baseCureSpell = ability.Abiltiy as BaseCureSpell;

                    if (baseCureSpell != null)
                    {
                        npc.SpellBook.Add(baseCureSpell.SpellName.ToUpper(), baseCureSpell);
                        CureSpell.Add(baseCureSpell);
                    }
                }
            }

            DamageSpells = DamageSpells.OrderByDescending(e => e.ManaCost).ToList();
            CureSpell = CureSpell.OrderByDescending(e => e.ManaCost).ToList();
        }

        public string Process(INonPlayerCharacter npc, string command)
        {
            if (command == null)
            {
                if (GlobalReference.GlobalValues.TickCounter % 5 == 0) //slow down casting to prevent spamming
                {
                    if ((npc.Health * 1.0) / npc.MaxHealth < .5)
                    {
                        //Less than half health.  Cure.
                        command = Heal(npc, command);
                    }
                    else
                    {
                        if (npc.IsInCombat)
                        {
                            //More than half health.  Attack
                            command = Attack(npc, command);
                        }

                    }
                }
            }
            return command;
        }

        private string Heal(INonPlayerCharacter npc, string command)
        {
            foreach (BaseCureSpell spell in CureSpell)
            {
                if (spell.ManaCost <= npc.Mana)
                {
                    command = $"Cast {spell.SpellName}";
                    break;
                }
            }

            return command;
        }

        private string Attack(INonPlayerCharacter npc, string command)
        {
            foreach (BaseDamageSpell spell in DamageSpells)
            {
                if (spell.ManaCost <= npc.Mana)
                {
                    command = $"Cast {spell.SpellName}";
                    break;
                }
            }

            return command;
        }
    }
}
