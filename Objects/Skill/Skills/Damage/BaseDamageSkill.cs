using Objects.Global;
using Objects.Skill.Skills.Generic;
using static Objects.Damage.Damage;

namespace Objects.Skill.Skills.Damage
{
    public abstract class BaseDamageSkill : SingleTargetSkill
    {
        public BaseDamageSkill(string skillName, int die, int sides, DamageType damageType, int statminaCost = -1) : base(skillName)
        {
            Effect = new Objects.Effect.Damage();
            Parameter.Dice = GlobalReference.GlobalValues.DefaultValues.ReduceValues(die, sides);
            Parameter.Damage = new Objects.Damage.Damage(Parameter.Dice);
            Parameter.Damage.Type = damageType;

            if (statminaCost == -1)
            {
                StaminaCost = sides * die / 20;
            }
            else
            {
                StaminaCost = statminaCost;
            }
        }
    }
}
