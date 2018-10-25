using Objects.Command;
using Objects.Command.Interface;
using Objects.Damage.Interface;
using Objects.Global;
using Objects.Item.Items.Interface;
using Objects.Language;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;
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
            PerformerNotification = new TranslationMessage("Lowering your shoulder you bash into {target} knocking them back slightly.");
            RoomNotification = new TranslationMessage("{performer} moves in close to {target} before lowering their shoulder and bashing into {target} knocking them back slightly.");
            TargetNotification = new TranslationMessage("{performer} knocks their shoulder into you knocking you back slightly.");
        }

        public override string TeachMessage => "The trick to a good shoulder bash it to get a firm stance before making contact with your opponent.";

        public override bool MeetRequirments(IMobileObject performer, IMobileObject target)
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

        public override IResult RequirementsFailureMessage => new Result("You must have a piercing weapon equipped.", true, Shared.TagWrapper.TagWrapper.TagType.Info);
    }
}