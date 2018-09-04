using Objects.Command;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Guild;
using Objects.Guild.Interface;
using Objects.Magic.Interface;
using Objects.Mob;
using Objects.Mob.Interface;
using Objects.Personality.Personalities.Interface;
using Objects.Skill.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Objects.Guild.Guild;

namespace Objects.Personality.Personalities
{
    public class GuildMaster : IGuildMaster
    {
        public GuildMaster()
        {

        }

        public GuildMaster(Guilds guild)
        {
            Guild = guild;
        }

        [ExcludeFromCodeCoverage]
        public Guilds Guild { get; set; }

        #region Join
        public IResult Join(IMobileObject guildMaster, IMobileObject peformer)
        {
            if (!peformer.Guild.Contains(Guild))
            {
                return JoinGuild(guildMaster, peformer);
            }
            else
            {
                guildMaster.EnqueueCommand(string.Format("Tell {0} You are already a member of the {1} guild.", peformer.KeyWords.FirstOrDefault(), Guild));
                return new Result(null, true);
            }
        }

        private IResult JoinGuild(IMobileObject guildMaster, IMobileObject peformer)
        {
            if (peformer.GuildPoints > 0)
            {
                guildMaster.EnqueueCommand(string.Format("Tell {0} Welcome to the {1} guild.", peformer.KeyWords.FirstOrDefault(), Guild));
                peformer.Guild.Add(Guild);
                peformer.GuildPoints--;
                return new Result(null, false);
            }
            else
            {
                guildMaster.EnqueueCommand(string.Format("Tell {0} You need to gain more experience in the world before I can allow you to join the {1} guild.", peformer.KeyWords.FirstOrDefault(), Guild));
                return new Result(null, true);

            }
        }
        #endregion Join

        #region Teach
        public IResult Teach(IMobileObject guildMaster, IMobileObject learner, string parameter)
        {
            if (learner.Guild.Contains(Guild))
            {
                return TeachSpellSkill(Guild, guildMaster, learner, parameter);
            }
            else
            {
                if (learner.GuildPoints >= 1)
                {
                    guildMaster.EnqueueCommand(string.Format("Tell {0} Please join our guild so that I may teach you our ways.", learner.KeyWords.FirstOrDefault()));
                }
                else
                {
                    guildMaster.EnqueueCommand(string.Format("Tell {0} I'm sorry but I can not teach you until you gain more experience and join our guild.", learner.KeyWords[0]));
                }

                return new Result(null, true);
            }
        }

        private IResult TeachSpellSkill(Guilds guild, IMobileObject guildMaster, IMobileObject learner, string parameter)
        {
            GuildAbility skill = GlobalReference.GlobalValues.GuildAbilities.Skills[guild].FirstOrDefault(i => string.Equals(i.Abiltiy.ToString(), parameter, StringComparison.CurrentCultureIgnoreCase));
            if (skill != null)
            {
                if (skill.Level <= learner.Level)
                {
                    return TeachSkill((ISkill)skill.Abiltiy, guildMaster, learner);
                }
                else
                {
                    return NotHighEnoughLevel(guildMaster, learner);
                }
            }

            GuildAbility spell = GlobalReference.GlobalValues.GuildAbilities.Spells[guild].FirstOrDefault(i => string.Equals(i.Abiltiy.ToString(), parameter, StringComparison.CurrentCultureIgnoreCase));
            if (spell != null)
            {
                if (spell.Level <= learner.Level)
                {
                    return TeachSpell((ISpell)spell.Abiltiy, guildMaster, learner);
                }
                else
                {
                    return NotHighEnoughLevel(guildMaster, learner);
                }
            }

            guildMaster.EnqueueCommand(string.Format("Tell {0} I can not teach you that.", learner.KeyWords[0]));
            return new Result(null, true);
        }

        private IResult NotHighEnoughLevel(IMobileObject guildMaster, IMobileObject learner)
        {
            guildMaster.EnqueueCommand(string.Format("Tell {0} You are not high enough level for that yet.", learner.KeyWords[0]));
            return new Result(null, true);
        }

        private IResult TeachSkill(ISkill skill, IMobileObject guildMaster, IMobileObject learner)
        {
            string skillName = skill.ToString().ToUpper();
            if (!learner.KnownSkills.Keys.Contains(skillName))
            {
                learner.KnownSkills.Add(skillName, skill);
                guildMaster.EnqueueCommand(string.Format("Tell {0} {1}", learner.KeyWords[0], skill.TeachMessage));
                return new Result(null, false);
            }
            else
            {
                guildMaster.EnqueueCommand(string.Format("Tell {0} You already know that skill.", learner.KeyWords[0]));
                return new Result(null, true);
            }
        }

        private IResult TeachSpell(ISpell spell, IMobileObject guildMaster, IMobileObject learner)
        {
            string spellName = spell.ToString().ToUpper();
            if (!learner.SpellBook.Keys.Contains(spellName))
            {
                learner.SpellBook.Add(spellName, spell);
                guildMaster.EnqueueCommand(string.Format("Tell {0} {1}", learner.KeyWords[0], spell.TeachMessage));
                return new Result(null, false);
            }
            else
            {
                guildMaster.EnqueueCommand(string.Format("Tell {0} You already know that spell.", learner.KeyWords[0]));
                return new Result(null, true);
            }
        }


        public IResult Teachable(IMobileObject guildMaster, IMobileObject actor)
        {
            StringBuilder strBldr = new StringBuilder();
            foreach (GuildAbility skillPair in GlobalReference.GlobalValues.GuildAbilities.Skills[Guild])
            {
                if (actor.Level >= skillPair.Level
                    && !actor.KnownSkills.Keys.Contains(skillPair.Abiltiy.AbilityName))
                {
                    strBldr.AppendLine(skillPair.Abiltiy.ToString());
                }
            }

            foreach (GuildAbility spellPair in GlobalReference.GlobalValues.GuildAbilities.Spells[Guild])
            {
                if (actor.Level >= spellPair.Level
                    && !actor.SpellBook.Keys.Contains(spellPair.Abiltiy.AbilityName))
                {
                    strBldr.AppendLine(spellPair.Abiltiy.ToString());
                }
            }

            if (strBldr.ToString() != string.Empty)
            {
                guildMaster.EnqueueCommand(string.Format("Tell {0} I can teach you the following. {1}{2}", actor.KeyWords.FirstOrDefault(), Environment.NewLine, strBldr.ToString().Trim()));
            }
            else
            {
                guildMaster.EnqueueCommand(string.Format("Tell {0} I can not teach you anything at this time.", actor.KeyWords.FirstOrDefault()));
            }
            return new Result(null, false);
        }

        #endregion Teach

        public string Process(INonPlayerCharacter npc, string command)
        {
            return command;
        }
    }
}
