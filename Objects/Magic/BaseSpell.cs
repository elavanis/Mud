﻿using Objects.Command;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Global.Language;
using Objects.Magic.Interface;
using Objects.Mob.Interface;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Magic
{
    public abstract class BaseSpell : Ability.Ability, ISpell
    {
        protected BaseSpell(string spellName, int manaCost) : base(spellName)
        {
            ManaCost = manaCost;
        }

        [ExcludeFromCodeCoverage]
        public int ManaCost { get; set; }

        [ExcludeFromCodeCoverage]
        public string SpellName
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
                return "Repeat after me.  " + GlobalReference.GlobalValues.Translator.Translate(Translator.Languages.Magic, SpellName);
            }
        }

        public virtual IResult ProcessSpell(IMobileObject performer, ICommand command)
        {
            if (performer.Mana >= ManaCost)
            {
                performer.Mana -= ManaCost;
                Effect.ProcessEffect(Parameter);
                IMobileObject targetMob = Parameter.Target as IMobileObject;
                List<IMobileObject> exclusions = new List<IMobileObject>() { performer };

                if (targetMob != null
                    && !exclusions.Contains(targetMob))
                {
                    exclusions.Add(targetMob);
                }

                if (RoomNotificationSuccess != null)
                {
                    GlobalReference.GlobalValues.Notify.Room(performer, Parameter.Target, performer.Room, RoomNotificationSuccess, exclusions);
                }

                if (TargetNotificationSuccess != null)
                {
                    if (targetMob != null)
                    {
                        GlobalReference.GlobalValues.Notify.Mob(performer, targetMob, targetMob, TargetNotificationSuccess);
                    }
                }

                string message = GlobalReference.GlobalValues.StringManipulator.UpdateTargetPerformer(performer.SentenceDescription, targetMob?.SentenceDescription, PerformerNotificationSuccess.GetTranslatedMessage(performer));
                return new Result(message, false, null);
            }
            else
            {
                return new Result($"You need {ManaCost} mana to cast the spell {command.Parameters[0].ParameterValue}.", true);
            }
        }
    }
}
