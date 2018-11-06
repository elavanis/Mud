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

        ITranslationMessage RoomNotification { get; set; }
        ITranslationMessage TargetNotification { get; set; }
        ITranslationMessage PerformerNotification { get; set; }

        IEffect Effect { get; set; }
        IEffectParameter Parameter { get; set; }

        IResult PerformAbility(IMobileObject performer, ICommand command);

        bool MeetRequirments(IMobileObject performer, IMobileObject target);

        IResult RequirementsFailureMessage { get; }

        void AdditionalEffect(IMobileObject performer, IMobileObject target);
    }
}