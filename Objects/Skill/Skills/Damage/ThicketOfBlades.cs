using Objects.Command;
using Objects.Command.Interface;
using Objects.Damage.Interface;
using Objects.Global;
using Objects.Item.Items.Interface;
using Objects.Language;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Damage.Damage;

namespace Objects.Skill.Skills.Damage
{
    public class ThicketOfBlades : BaseDamageSkill
    {
        public ThicketOfBlades() : base(nameof(ThicketOfBlades),
          GlobalReference.GlobalValues.DefaultValues.DiceForSkillLevel(90).Die,
          GlobalReference.GlobalValues.DefaultValues.DiceForSkillLevel(90).Sides,
          DamageType.Bludgeon)
        {
            PerformerNotificationSuccess = new TranslationMessage("Moving your blade so quick it becomes a blur of flashes that leave {target} with hundreds of cuts..");
            RoomNotificationSuccess = new TranslationMessage("{performer} moves their blade so quickly it a blur of flashes that {target} is unable to defend against.");
            TargetNotificationSuccess = new TranslationMessage("{performer} begins to move so fast their blade disappears and becomes a flashing light that feels like your running through a thicket of thorns.");
        }

        public override string TeachMessage => "Move your blade in a circular path like this.  Good now faster, faster.  Perfect.";

        public override bool MeetRequirments(IMobileObject performer, IMobileObject target)
        {
            foreach (IWeapon weapon in performer.EquipedWeapon)
            {
                foreach (IDamage damage in weapon.DamageList)
                {
                    if (damage.Type == DamageType.Slash)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override IResult RequirementsFailureMessage => new Result("You must have a slashing weapon equipped.", true, Shared.TagWrapper.TagWrapper.TagType.Info);
    }
}
