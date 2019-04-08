﻿using Objects.Skill.Skills.Generic;

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
