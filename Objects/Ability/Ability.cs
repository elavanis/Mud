using Objects.Ability.Interface;
using Objects.Command.Interface;
using Objects.Damage.Interface;
using Objects.Effect;
using Objects.Effect.Interface;
using Objects.Global;
using Objects.Language.Interface;
using Objects.Mob;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Objects.Ability
{
    public abstract class Ability : IAbility
    {
        [ExcludeFromCodeCoverage]
        public ITranslationMessage RoomNotification { get; set; }
        [ExcludeFromCodeCoverage]
        public ITranslationMessage TargetNotification { get; set; }
        [ExcludeFromCodeCoverage]
        public ITranslationMessage PerformerNotification { get; set; }

        [ExcludeFromCodeCoverage]
        public IEffect Effect { get; set; }
        [ExcludeFromCodeCoverage]
        public IEffectParameter Parameter { get; set; } = new EffectParameter();
        [ExcludeFromCodeCoverage]
        public string AbilityName { get; set; }


        [ExcludeFromCodeCoverage]
        public IResult PerformAbility(IMobileObject performer, ICommand command)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        public virtual bool MeetRequirments(IMobileObject performer, IMobileObject target)
        {
            return true;
        }

        [ExcludeFromCodeCoverage]
        public virtual IResult RequirementsFailureMessage { get => throw new NotImplementedException(); }
    }
}

