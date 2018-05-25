using System.Diagnostics.CodeAnalysis;

namespace Objects.Skill.Skills
{
    public class BlindFighting : BasePassiveSkill
    {
        [ExcludeFromCodeCoverage]
        public BlindFighting() : base()
        {
        }

        public override string TeachMessage
        {
            get
            {
                return "The best way to learn to fight blind is to wear a blindfold while training.";
            }
        }
    }
}
