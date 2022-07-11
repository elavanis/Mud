namespace Objects.Skill
{
    public abstract class BasePassiveSkill : BaseSkill
    {
        public BasePassiveSkill(string skillName) : base(skillName, 0, true)
        {
        }
    }
}
