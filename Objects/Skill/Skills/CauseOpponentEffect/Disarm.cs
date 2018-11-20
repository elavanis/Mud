using Objects.Command;
using Objects.Command.Interface;
using Objects.Effect;
using Objects.Effect.Interface;
using Objects.Global;
using Objects.Item.Items.Interface;
using Objects.Language;
using Objects.Language.Interface;
using Objects.Magic.Enchantment;
using Objects.Magic.Enchantment.DefeatbleInfo;
using Objects.Magic.Enchantment.DefeatbleInfo.Interface;
using Objects.Magic.Interface;
using Objects.Mob.Interface;
using Objects.Skill.Skills.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Objects.Damage.Damage;
using static Objects.Global.Stats.Stats;

namespace Objects.Skill.Skills.CauseOpponentEffect
{
    public class Disarm : BaseCauseOpponentEffect
    {
        public Disarm() : base(nameof(Disarm), 200)
        {
            Effect = new Objects.Effect.Dummy();
            PerformerNotificationSuccess = new TranslationMessage("You stun {target} causing them to drop their weapon.");
            RoomNotificationSuccess = new TranslationMessage("{performer} stuns {target} causing them to drop their weapon.");
            TargetNotificationSuccess = new TranslationMessage("{performer} stuns you causing you to drop your weapon.");

            PerformerNotificationFailure = new TranslationMessage("You try to stun {target} causing them to drop their weapon but fail.");
            RoomNotificationFailure = new TranslationMessage("{performer} attempts to stun {target} causing them to drop their weapon but fails.");
            TargetNotificationFailure = new TranslationMessage("{performer} tries to stun you causing you to drop your weapon but fails.");
        }

        public override bool IsSuccessful(IMobileObject performer, IMobileObject target)
        {
            if (GlobalReference.GlobalValues.Random.Next(performer.StrengthEffective)
                >= GlobalReference.GlobalValues.Random.Next(target.StrengthEffective))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void AdditionalEffect(IMobileObject performer, IMobileObject target)
        {
            foreach (IWeapon weapon in target.EquipedWeapon)
            {
                if (!weapon.KeyWords.Contains("BareHands"))
                {
                    target.RemoveEquipment(weapon);
                    target.Items.Add(weapon);
                }
            }
        }
    }
}
