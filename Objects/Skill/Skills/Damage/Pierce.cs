﻿using Objects.Command;
using Objects.Command.Interface;
using Objects.Damage.Interface;
using Objects.Global;
using Objects.Item.Items.Interface;
using Objects.Language;
using Objects.Mob.Interface;
using static Objects.Damage.Damage;

namespace Objects.Skill.Skills.Damage
{
    public class Pierce : BaseDamageSkill
    {
        public Pierce() : base(nameof(Pierce),
            GlobalReference.GlobalValues.DefaultValues.DiceForSkillLevel(20).Die,
            GlobalReference.GlobalValues.DefaultValues.DiceForSkillLevel(20).Sides,
            DamageType.Pierce)
        {
            PerformerNotificationSuccess = new TranslationMessage("Taking your weapon you thrust it toward {target} piercing them.");
            RoomNotificationSuccess = new TranslationMessage("{performer} quickly thrust their weapon at {target} piercing them.");
            TargetNotificationSuccess = new TranslationMessage("{performer} takes the pointy end of their weapon and in a blink of an eye uses it to pierce you.");
        }

        public override string TeachMessage {get;} = "Keep the pointy end of the weapon toward your opponent and thrust.";

        public override IResult RequirementsFailureMessage {get;} = new Result("You must have a piercing weapon equipped.", true, Shared.TagWrapper.TagWrapper.TagType.Info);

        protected override bool MeetRequirments(IMobileObject performer, IMobileObject target)
        {
            foreach (IWeapon weapon in performer.EquipedWeapon)
            {
                foreach (IDamage damage in weapon.DamageList)
                {
                    if (damage.Type == DamageType.Pierce)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}