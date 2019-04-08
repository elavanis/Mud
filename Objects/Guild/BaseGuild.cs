using Objects.Guild.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Guild
{
    public abstract class BaseGuild : IGuild
    {
        private List<GuildAbility> _skills = null;

        public List<GuildAbility> Skills
        {
            get
            {
                if (_skills == null)
                {
                    _skills = GenerateSkills();
                }
                return _skills;
            }
        }

        [ExcludeFromCodeCoverage]
        protected virtual List<GuildAbility> GenerateSkills()
        {
            throw new NotImplementedException();
        }

        private List<GuildAbility> _spells = null;
        public List<GuildAbility> Spells
        {
            get
            {
                if (_spells == null)
                {
                    _spells = GenerateSpells();
                }
                return _spells;
            }
        }

        [ExcludeFromCodeCoverage]
        protected virtual List<GuildAbility> GenerateSpells()
        {
            throw new NotImplementedException();
        }
    }
}