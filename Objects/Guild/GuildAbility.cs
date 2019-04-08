using Objects.Ability.Interface;

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
