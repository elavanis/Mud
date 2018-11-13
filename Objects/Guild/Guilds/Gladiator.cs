using Objects.Magic.Interface;
using Objects.Magic.Spell;
using Objects.Skill.Interface;
using Objects.Skill.Skills;
using Objects.Skill.Skills.CauseOpponentEffect;
using Objects.Skill.Skills.Damage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Guild.Guilds
{
    public class Gladiator : BaseGuild
    {
        override protected List<GuildAbility> GenerateSkills()
        {
            List<GuildAbility> skills = new List<GuildAbility>();
            skills.Add(new GuildAbility(new RagingBull(), 1));
            skills.Add(new GuildAbility(new ShoulderBash(), 10));
            skills.Add(new GuildAbility(new Pierce(), 20));
            skills.Add(new GuildAbility(new SpittingCobra(), 30));
            skills.Add(new GuildAbility(new BlindFighting(), 40));
            skills.Add(new GuildAbility(new ThrowDirt(), 50));
            return skills;
        }

        override protected List<GuildAbility> GenerateSpells()
        {
            List<GuildAbility> spells = new List<GuildAbility>();

            return spells;
        }
    }
}
