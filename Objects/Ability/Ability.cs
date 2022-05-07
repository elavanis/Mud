using Objects.Ability.Interface;
using Objects.Command;
using Objects.Command.Interface;
using Objects.Effect;
using Objects.Effect.Interface;
using Objects.Language;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Ability
{
    public abstract class Ability : IAbility
    {
        [ExcludeFromCodeCoverage]
        public ITranslationMessage RoomNotificationSuccess { get; set; } 
        [ExcludeFromCodeCoverage]
        public ITranslationMessage TargetNotificationSuccess { get; set; }
        [ExcludeFromCodeCoverage]
        public ITranslationMessage PerformerNotificationSuccess { get; set; }

        [ExcludeFromCodeCoverage]
        public ITranslationMessage RoomNotificationFailure { get; set; }
        [ExcludeFromCodeCoverage]
        public ITranslationMessage TargetNotificationFailure { get; set; }
        [ExcludeFromCodeCoverage]
        public ITranslationMessage PerformerNotificationFailure { get; set; }


        [ExcludeFromCodeCoverage]
        public IEffect Effect { get; set; }
        [ExcludeFromCodeCoverage]
        public IEffectParameter Parameter { get; set; } = new EffectParameter();
        [ExcludeFromCodeCoverage]
        public string AbilityName { get; set; }

        [ExcludeFromCodeCoverage]
        public virtual IResult RequirementsFailureMessage
        {
            get => new Result("Unspecified requirements failure", true);
        }

        [ExcludeFromCodeCoverage]
        protected IResult AbilityFailed(IMobileObject performer, IMobileObject target)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        protected virtual bool MeetRequirments(IMobileObject performer, IMobileObject target)
        {
            return true;
        }

        [ExcludeFromCodeCoverage]
        protected virtual bool IsSuccessful(IMobileObject performer, IMobileObject target)
        {
            return true;
        }

        [ExcludeFromCodeCoverage]
        protected virtual void AdditionalEffect(IMobileObject performer, IMobileObject target)
        {

        }
    }
}

