using Objects.Ability.Interface;
using Objects.Command.Interface;
using Objects.Interface;
using Objects.Mob.Interface;

namespace Objects.Skill.Interface
{
    public interface ISkill : ILearnable, IAbility
    {
        bool Passive { get; set; }
        int StaminaCost { get; set; }
        string SkillName { get; set; }

        IResult ProcessSkill(IMobileObject attacker, ICommand command);
    }
}