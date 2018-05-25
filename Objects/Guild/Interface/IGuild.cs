using System;
using System.Collections.Generic;
using Objects.Magic;
using Objects.Skill.Interface;
using Objects.Magic.Interface;

namespace Objects.Guild.Interface
{
    public interface IGuild
    {
        List<GuildAbility> Skills { get; }
        List<GuildAbility> Spells { get; }
    }
}