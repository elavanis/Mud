using Objects.Magic.Interface;
using Objects.Magic.Spell;
using Objects.Magic.Spell.Damage;
using Objects.Skill.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Guild.Guilds
{
    public class Wizard : BaseGuild
    {
        override protected List<GuildAbility> GenerateSkills()
        {
            List<GuildAbility> skills = new List<GuildAbility>();

            return skills;
        }

        override protected List<GuildAbility> GenerateSpells()
        {
            List<GuildAbility> spells = new List<GuildAbility>();
            spells.Add(new GuildAbility(new AcidBolt(), 20));
            spells.Add(new GuildAbility(new FireBall(), 70));
            spells.Add(new GuildAbility(new Freeze(), 40));
            spells.Add(new GuildAbility(new LightBurst(), 80));
            spells.Add(new GuildAbility(new LightningBolt(), 30));
            spells.Add(new GuildAbility(new MagicMissle(), 1));
            spells.Add(new GuildAbility(new PoisonBreath(), 60));
            spells.Add(new GuildAbility(new PyschicScream(), 50));
            spells.Add(new GuildAbility(new Rot(), 90));
            spells.Add(new GuildAbility(new Smite(), 100));
            spells.Add(new GuildAbility(new ThunderClap(), 10));

            return spells;
        }
    }
}
