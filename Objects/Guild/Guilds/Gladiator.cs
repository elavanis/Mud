﻿using Objects.Skill.Skills;
using Objects.Skill.Skills.CauseOpponentEffect;
using Objects.Skill.Skills.Damage;
using System.Collections.Generic;

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
            skills.Add(new GuildAbility(new ThrowDirt(), 40));
            skills.Add(new GuildAbility(new BlindFighting(), 50));
            skills.Add(new GuildAbility(new SunShine(), 60));
            skills.Add(new GuildAbility(new PummelStrike(), 70));
            skills.Add(new GuildAbility(new KneeBreaker(), 80));
            skills.Add(new GuildAbility(new ThicketOfBlades(), 90));
            skills.Add(new GuildAbility(new Disarm(), 100));


            return skills;
        }

        override protected List<GuildAbility> GenerateSpells()
        {
            List<GuildAbility> spells = new List<GuildAbility>();

            return spells;
        }
    }
}
