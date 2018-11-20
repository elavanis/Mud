using Objects.Command.Interface;
using Objects.Damage.Interface;
using Objects.Effect.Interface;
using Objects.Language.Interface;
using Objects.Mob;
using Objects.Mob.Interface;

namespace Objects.Ability.Interface
{
    public interface IAbility
    {
        string AbilityName { get; set; }

        ITranslationMessage RoomNotificationSuccess { get; set; }
        ITranslationMessage TargetNotificationSuccess { get; set; }
        ITranslationMessage PerformerNotificationSuccess { get; set; }

        ITranslationMessage RoomNotificationFailure { get; set; }
        ITranslationMessage TargetNotificationFailure { get; set; }
        ITranslationMessage PerformerNotificationFailure { get; set; }

        IEffect Effect { get; set; }
        IEffectParameter Parameter { get; set; }

        IResult PerformAbility(IMobileObject performer, ICommand command);
        IResult AbilityFailed(IMobileObject performer, IMobileObject target);

        bool MeetRequirments(IMobileObject performer, IMobileObject target);

        bool IsSuccessful(IMobileObject performer, IMobileObject target);


        IResult RequirementsFailureMessage { get; }

        void AdditionalEffect(IMobileObject performer, IMobileObject target);
    }
}