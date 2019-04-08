using Objects.Command;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Item.Items.Interface;
using Objects.Language;
using Objects.Mob.Interface;
using System.Linq;

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

        public override string TeachMessage => "If your opponent has no weapon then they can't hurt you.";

        public override bool MeetRequirments(IMobileObject performer, IMobileObject target)
        {
            IWeapon weapon = target.EquipedWeapon.FirstOrDefault();
            if (weapon.KeyWords[0] == "BareHands")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override IResult RequirementsFailureMessage => new Result("You can not disarm an unarmed opponent.", true, Shared.TagWrapper.TagWrapper.TagType.Info);


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
