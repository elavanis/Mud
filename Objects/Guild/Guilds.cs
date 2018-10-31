using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Guild
{
    public abstract class Guild
    {
        public enum Guilds
        {
            //Utility
            Mage,

            //Heal
            Clearic,

            //Animal Companion
            Beastmaster,
            Summoner,

            //Damage focused
            Wizard,

            //Damage with some effects
            Gladiator,

        }
    }
}
