using Objects.Command;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using Objects.Skill.Interface;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Skill
{
    public abstract class BaseSkill : Ability.Ability, ISkill
    {
        protected BaseSkill(string skillName, int staminaCost, bool passive) : base(skillName)
        {
            StaminaCost = staminaCost;
            Passive = passive;
        }

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



        public virtual IResult ProcessSkill(IMobileObject performer, ICommand command)
        {
            IMobileObject targetMob = Parameter.Target as IMobileObject;

            if (MeetRequirments(performer, targetMob))
            {
                if (performer.Stamina >= StaminaCost)
                {
                    performer.Stamina -= StaminaCost;
                    SetParameterFields(performer);


                    if (IsSuccessful(performer, targetMob))
                    {
                        return PerformSuccess(performer, targetMob);
                    }
                    else
                    {
                        return PerformFailure(performer, targetMob);
                    }
                }
                else
                {
                    return new Result($"You need {StaminaCost} stamina to use the skill {command.Parameters[0].ParameterValue}.", true);
                }
            }
            else
            {
                return RequirementsFailureMessage;
            }
        }

        private IResult PerformFailure(IMobileObject performer, IMobileObject targetMob)
        {
            Parameter.PerformerMessage = PerformerNotificationFailure;
            Parameter.TargetMessage = TargetNotificationFailure;
            Parameter.RoomMessage = RoomNotificationFailure;

            List<IMobileObject> exclusions = new List<IMobileObject>() { performer };
            if (targetMob != null)
            {
                exclusions.Add(targetMob);
            }

            GlobalReference.GlobalValues.Notify.Mob(targetMob, TargetNotificationFailure);

            if (RoomNotificationFailure != null)
            {
                GlobalReference.GlobalValues.Notify.Room(performer, targetMob, performer.Room, RoomNotificationFailure, exclusions);
            }

            string message = GlobalReference.GlobalValues.StringManipulator.UpdateTargetPerformer(performer.SentenceDescription, targetMob?.SentenceDescription, PerformerNotificationFailure.GetTranslatedMessage(performer));
            return new Result(message, false, null);
        }

        private IResult PerformSuccess(IMobileObject performer, IMobileObject targetMob)
        {
            Parameter.PerformerMessage = PerformerNotificationSuccess;
            Parameter.TargetMessage = TargetNotificationSuccess;
            Parameter.RoomMessage = RoomNotificationSuccess;

            Effect.ProcessEffect(Parameter);
            List<IMobileObject> exclusions = new List<IMobileObject>() { performer };
            if (targetMob != null
                && !exclusions.Contains(targetMob))
            {
                exclusions.Add(targetMob);
            }

            if (RoomNotificationSuccess != null)
            {
                GlobalReference.GlobalValues.Notify.Room(performer, targetMob, performer.Room, RoomNotificationSuccess, exclusions);
            }


            AdditionalEffect(performer, targetMob);

            string message = GlobalReference.GlobalValues.StringManipulator.UpdateTargetPerformer(performer.SentenceDescription, targetMob?.SentenceDescription, PerformerNotificationSuccess.GetTranslatedMessage(performer));
            return new Result(message, false, null);
        }

        protected void SetParameterFields(IMobileObject performer)
        {
            Parameter.Performer = performer;
        }
    }
}