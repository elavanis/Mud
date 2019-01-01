using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Objects.Damage;
using Objects.Damage.Interface;
using Objects.Global;
using Objects.Language;
using Objects.Mob.Interface;

namespace Objects.Mob.SpecificNPC
{
    public class Hydra : NonPlayerCharacter
    {
        private int DamageToGrowNewHead { get; set; }
        private int NewHeadsToGrow { get; set; }
        private bool TookFireDamage { get; set; }
        public Hydra() : base()
        {
            DamageToGrowNewHead = MaxHealth / 10;
            Personalities.Add(new Personality.Personalities.Hydra());

        }

        public override void ProcessCombatRoundDamage(List<DamageDealt> damageDealts)
        {
            int totalDamage = 0;

            foreach (DamageDealt damageDealt in damageDealts)
            {
                totalDamage += damageDealt.Amount;

                if (damageDealt.DamageType == Damage.Damage.DamageType.Fire)
                {
                    TookFireDamage = true;
                    break;
                }
            }

            if (!TookFireDamage
                && totalDamage >= DamageToGrowNewHead)
            {
                NewHeadsToGrow++;
            }



            base.ProcessCombatRoundDamage(damageDealts);
        }

        public void RegrowHeads()
        {
            if (TookFireDamage == false)
            {
                for (int i = 0; i < NewHeadsToGrow; i++)
                {
                    AddEquipment(EquipedWeapon.FirstOrDefault());
                }

                GlobalReference.GlobalValues.Notify.Room(this, null, Room, new TranslationMessage($"{SentenceDescription} grows {NewHeadsToGrow} new heads."));
            }

            NewHeadsToGrow = 0;
            TookFireDamage = false;
        }
    }
}
