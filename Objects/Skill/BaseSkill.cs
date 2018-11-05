using Objects.Command;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Mob;
using Objects.Mob.Interface;
using Objects.Skill.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Skill
{
    public abstract class BaseSkill : Ability.Ability, ISkill
    {
        [ExcludeFromCodeCoverage]
        public bool Passive { get; set; } = false;
        [ExcludeFromCodeCoverage]
        public int StaminaCost { get; set; }
        [ExcludeFromCodeCoverage]
        public string SkillName
        {
            get
            {
                return AbilityName;
            }
            set
            {
                AbilityName = value;
            }
        }

        public virtual string TeachMessage
        {
            get
            {
                return "The best way to learn is with lots practice.";
            }
        }

        public BaseSkill(string skillName)
        {
            SkillName = skillName;
        }

        public virtual IResult ProcessSkill(IMobileObject performer, ICommand command)
        {
            if (performer.Stamina > StaminaCost)
            {
                performer.Stamina -= StaminaCost;
                SetParameterFields(performer);
                Effect.ProcessEffect(Parameter);
                IMobileObject targetMob = Parameter.Target as IMobileObject;
                List<IMobileObject> exclusions = new List<IMobileObject>() { performer };
                if (targetMob != null
                    && !exclusions.Contains(targetMob))
                {
                    exclusions.Add(targetMob);
                }

                if (RoomNotification != null)
                {
                    GlobalReference.GlobalValues.Notify.Room(performer, targetMob, performer.Room, RoomNotification, exclusions);
                }

                //don't think we need to notify the target as they will be notified in the Effect.ProcessEffect if there is anything to notify them about
                //if (TargetNotification != null)
                //{
                //    if (targetMob != null)
                //    {
                //        GlobalReference.GlobalValues.Notify.Mob(targetMob, TargetNotification);
                //    }
                //}

                AdditionalEffect(performer, targetMob);

                string message = GlobalReference.GlobalValues.StringManipulator.UpdateTargetPerformer(performer.SentenceDescription, targetMob?.SentenceDescription, PerformerNotification.GetTranslatedMessage(performer));
                return new Result(message, false, null);
            }
            else
            {
                return new Result($"You need {StaminaCost} stamina to use the skill {command.Parameters[0].ParameterValue}.", true);
            }
        }

        private void SetParameterFields(IMobileObject performer)
        {
            Parameter.Performer = performer;
            Parameter.PerformerMessage = PerformerNotification;
            Parameter.TargetMessage = TargetNotification;
            Parameter.RoomMessage = RoomNotification;
        }
    }
}