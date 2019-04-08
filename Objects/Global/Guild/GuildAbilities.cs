using Objects.Global.Guild.Interface;
using Objects.Guild;
using Objects.Guild.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using static Objects.Guild.Guild;

namespace Objects.Global.Guild
{
    public class GuildAbilities : IGuildAbilities
    {
        public Dictionary<Guilds, List<GuildAbility>> Skills { get; } = PopulateSkills();
        public Dictionary<Guilds, List<GuildAbility>> Spells { get; } = PopulateSpells();

        private static Dictionary<Guilds, List<GuildAbility>> PopulateSkills()
        {
            Dictionary<Guilds, List<GuildAbility>> skills = new Dictionary<Guilds, List<GuildAbility>>();
            foreach (IGuild guild in Guilds)
            {
                string guildName = guild.GetType().Name.ToString();
                foreach (var v in Enum.GetValues(typeof(Guilds)))
                {
                    if (v.ToString() == guildName)
                    {
                        skills.Add((Guilds)v, guild.Skills);
                    }
                }
            }
            return skills;
        }

        private static Dictionary<Guilds, List<GuildAbility>> PopulateSpells()
        {
            Dictionary<Guilds, List<GuildAbility>> spells = new Dictionary<Guilds, List<GuildAbility>>();
            foreach (IGuild guild in Guilds)
            {
                string guildName = guild.GetType().Name.ToString();
                foreach (var v in Enum.GetValues(typeof(Guilds)))
                {
                    if (v.ToString() == guildName)
                    {
                        spells.Add((Guilds)v, guild.Spells);
                    }
                }
            }
            return spells;
        }

        private static List<IGuild> Guilds
        {
            get
            {
                IEnumerable<Type> list = from classes in typeof(GuildAbilities).Assembly.GetTypes()
                                         select classes;
                List<IGuild> guilds = new List<IGuild>();

                foreach (Type item in list)
                {
                    if (item.GetInterfaces().Contains(typeof(IGuild)))
                    {
                        if (!item.IsAbstract)
                        {
                            IGuild guild = (IGuild)Activator.CreateInstance(item);
                            guilds.Add(guild);
                        }
                    }
                }

                return guilds;
            }
        }
    }
}
