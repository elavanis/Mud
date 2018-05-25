using Objects.Ability.Interface;
using Objects.Magic.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Guild
{
    public class GuildAbility
    {
        public IAbility Abiltiy { get; set; }
        public int Level { get; set; }

        public GuildAbility()
        {

        }

        public GuildAbility(IAbility ability, int level)
        {
            Abiltiy = ability;
            Level = level;
        }
    }
}
