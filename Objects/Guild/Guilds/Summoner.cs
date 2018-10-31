using Objects.Magic.Interface;
using Objects.Skill.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Guild.Guilds
{
    public class Summoner : BaseGuild
    {
        override protected List<GuildAbility> GenerateSkills()
        {
            List<GuildAbility> skills = new List<GuildAbility>();

            return skills;
        }

        override protected List<GuildAbility> GenerateSpells()
        {
            List<GuildAbility> spells = new List<GuildAbility>();

            return spells;
        }
    }
}
