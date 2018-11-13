using Objects.Skill.Skills.Generic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Skill.Skills.CauseOpponentEffect
{
    public class BaseCauseOpponentEffect : SingleTargetSkill
    {
        public BaseCauseOpponentEffect(string skillName, int statminaCost) : base(skillName)
        {
            StaminaCost = statminaCost;
        }
    }
}
