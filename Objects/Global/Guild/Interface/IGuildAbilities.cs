using System;
using System.Collections.Generic;
using Objects.Guild.Interface;
using Objects.Magic.Interface;
using Objects.Skill.Interface;
using Objects.Guild;
using static Objects.Guild.Guild;

namespace Objects.Global.Guild.Interface
{
    public interface IGuildAbilities
    {
        Dictionary<Guilds, List<GuildAbility>> Skills { get; }
        Dictionary<Guilds, List<GuildAbility>> Spells { get; }
    }
}