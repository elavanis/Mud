using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Objects.Damage;
using Objects.Damage.Interface;
using Objects.Global;
using Objects.Item.Items.Interface;
using Objects.Language;
using Objects.Mob.Interface;

namespace Objects.Mob.SpecificNPC
{
    public class Hydra : NonPlayerCharacter
    {
        private int DamageToGrowNewHead { get; set; }
        private int NewHeadsToGrow { get; set; }
        private bool TookFireDamage { get; set; }

        private RoundOfDamage RoundOfDamage { get; set; }

        public Hydra() : base()
        {
            DamageToGrowNewHead = MaxHealth / 10;
            Personalities.Add(new Personality.Personalities.Hydra());
            RoundOfDamage = new RoundOfDamage();
        }

        public override int TakeDamage(int totalDamage, IDamage damage, IMobileObject attacker)
        {
            int takenDamage = base.TakeDamage(totalDamage, damage, attacker);

            if (damage.Type == Damage.Damage.DamageType.Fire)
            {
                TookFireDamage = true;
            }

            //we always want to set this to a new round of damage since this is spells, traps etc
            //not the normal combat action
            RoundOfDamage = new RoundOfDamage() { TotalDamage = takenDamage, LastAttacker = attacker };

            ProcessIfHeadCutOff();

            return takenDamage;
        }


        public override int TakeCombatDamage(int totalDamage, IDamage damage, IMobileObject attacker, uint combatRound)
        {
            int takenDamage = base.TakeCombatDamage(totalDamage, damage, attacker, combatRound);

            if (damage.Type == Damage.Damage.DamageType.Fire)
            {
                TookFireDamage = true;
            }

            if (RoundOfDamage.LastAttacker == attacker)
            {
                RoundOfDamage.TotalDamage += Math.Max(0, takenDamage);
            }
            else
            {
                RoundOfDamage = new RoundOfDamage() { TotalDamage = takenDamage, LastAttacker = attacker };
            }

            ProcessIfHeadCutOff();

            return takenDamage;
        }

        private void ProcessIfHeadCutOff()
        {
            if (!TookFireDamage
                && !RoundOfDamage.HeadCut
                && RoundOfDamage.TotalDamage >= DamageToGrowNewHead)
            {
                NewHeadsToGrow += 2;                //queue 2 new heads to grow
                RoundOfDamage.HeadCut = true;       //set the head to cut for this round

                //remove a head
                IWeapon weapon = EquipedWeapon.FirstOrDefault();
                RemoveEquipment(weapon);
            }
        }


        public void RegrowHeads()
        {
            if (TookFireDamage == false)
            {
                for (int i = 0; i < NewHeadsToGrow; i++)
                {
                    AddEquipment(EquipedWeapon.FirstOrDefault());
                    Health += (int)(DamageToGrowNewHead * .8);  //add back 80% of the health after growing a pair of heads
                }

                GlobalReference.GlobalValues.Notify.Room(this, null, Room, new TranslationMessage($"{SentenceDescription} grows {NewHeadsToGrow} new heads."));
            }

            NewHeadsToGrow = 0;
            TookFireDamage = false;
        }
    }

    class RoundOfDamage
    {
        public int TotalDamage { get; set; }
        public IMobileObject LastAttacker { get; set; }
        public bool HeadCut { get; set; }
    }
}
