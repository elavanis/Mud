using System.Collections.Generic;

namespace Objects.Guild.Interface
{
    public interface IGuild
    {
        List<GuildAbility> Skills { get; }
        List<GuildAbility> Spells { get; }
    }
}