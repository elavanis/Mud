//using Objects.Damage.Interface;
//using Objects.Die;
//using Objects.Interface;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics.CodeAnalysis;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Objects.Damage.Damage;
//using static Objects.Global.Stats.Stats;
//using Objects.Command.Interface;
//using Objects.Mob.Interface;
//using Objects.Global;
//using Objects.Command;
//using Objects.Language.Interface;
//using Objects.Language;

//namespace Objects.Magic.Spell
//{
//    //FIGURE OUT HOW TO MAKE SKILLS/MAGIC NOT CODE
//    [ExcludeFromCodeCoverage]
//    public class MagicMissile : BaseSpell
//    {

//        private static IDamage _damage;
//        private static IDamage Damage
//        {
//            get
//            {
//                if (_damage == null)
//                {
//                    IDamage tempDamage = new Objects.Damage.Damage(new Dice(4, 7));
//                    tempDamage.BonusDamageStat = Stat.Intelligence;
//                    tempDamage.Type = DamageType.Force;
//                    _damage = tempDamage;
//                }

//                return _damage;
//            }
//        }

//        public MagicMissile()
//        {
//            PerformerNotification = new TranslationMessage("Sparks of light fly from your finger tips to your target.");
//            ManaCost = 14;
//        }


//        public override IResult ProcessSpell(IMobileObject performer, ICommand command)
//        {
//            if (command.Parameters.Count > 1)
//            {
//                IBaseObject baseObject = GlobalReference.GlobalValues.FindObjects.FindObjectOnPersonOrInRoom(performer, command.Parameters[1].ParameterValue, command.Parameters[1].ParameterNumber, false, false, true, true, false);
//                IMobileObject target = baseObject as IMobileObject;
//                if (baseObject != null)
//                {
//                    int totalDamage = performer.CalculateDamage(Damage);
//                    int damageTaken = target.TakeDamage(totalDamage, Damage, performer);

//                    return new Result(true, string.Format("You deal {0} damage to the {1}.", damageTaken, target.SentenceDescription));
//                }
//            }

//            return null;
//        }
//    }
//}
