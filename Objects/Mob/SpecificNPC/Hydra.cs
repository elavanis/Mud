using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Objects.Damage.Interface;
using Objects.Global;
using Objects.Item.Items;
using Objects.Item.Items.Interface;
using Objects.Language;
using Objects.Mob.Interface;
using Objects.Mob.SpecificNPC.Interface;
using Objects.Room.Interface;
using static Objects.Damage.Damage;

namespace Objects.Mob.SpecificNPC
{
    public class Hydra : NonPlayerCharacter, IHydra
    {
        public Hydra(IRoom room, string examineDescription, string lookDescription, string sentenceDescription, string shortDescription, string? corpseDescription = null) : base(room, examineDescription, lookDescription, sentenceDescription, shortDescription, corpseDescription)
        {
            Personalities.Add(new Personality.Hydra());
            RoundOfDamage = new RoundOfDamage();
            AddAttribute(MobileAttribute.NoDisarm);
        }

        [ExcludeFromCodeCoverage]
        private int NewHeadsToGrow { get; set; }

        [ExcludeFromCodeCoverage]
        private bool TookFireDamage { get; set; }

        [ExcludeFromCodeCoverage]
        private RoundOfDamage RoundOfDamage { get; set; }

        public override int Level
        {
            get
            {
                return base.Level;
            }

            set
            {
                //remove all the old weapons
                List<IWeapon> weapons = (List<IWeapon>)EquipedWeapon;
                foreach (IWeapon weapon in weapons)
                {
                    RemoveEquipment(weapon);
                }

                IWeapon head = new Weapon("hydra head", "hydra head", "hydra head", "hydra head");
                Damage.Damage damage = new Damage.Damage() { Type = DamageType.Pierce, Dice = GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(Math.Max(1, value - 5)) };
                head.DamageList.Add(damage);

                for (int i = 0; i < 5; i++)
                {
                    AddEquipment(head);
                }

                base.Level = value;
            }
        }

        public override int TakeDamage(int totalDamage, IDamage damage, IMobileObject attacker)
        {
            int takenDamage = base.TakeDamage(totalDamage, damage, attacker);

            if (damage.Type == DamageType.Fire)
            {
                TookFireDamage = true;
            }

            //we always want to set this to a new round of damage since this is spells, traps etc
            //not the normal combat action
            RoundOfDamage = new RoundOfDamage() { TotalDamage = takenDamage, LastAttacker = attacker };

            ProcessIfHeadCutOff(attacker);

            return takenDamage;
        }

        public override int TakeCombatDamage(int totalDamage, IDamage damage, IMobileObject attacker, uint combatRound)
        {
            int takenDamage = base.TakeCombatDamage(totalDamage, damage, attacker, combatRound);

            if (damage.Type == DamageType.Fire)
            {
                TookFireDamage = true;
            }

            if (RoundOfDamage.LastAttacker == attacker && RoundOfDamage.CombatRound == combatRound)
            {
                RoundOfDamage.TotalDamage += Math.Max(0, takenDamage);
            }
            else
            {
                RoundOfDamage = new RoundOfDamage() { TotalDamage = takenDamage, LastAttacker = attacker, CombatRound = combatRound };
            }

            ProcessIfHeadCutOff(attacker);

            return takenDamage;
        }

        private void ProcessIfHeadCutOff(IMobileObject attacker)
        {
            if (!RoundOfDamage.HeadCut
                && RoundOfDamage.TotalDamage >= (MaxHealth / 10))
            {
                if (!TookFireDamage)
                {
                    NewHeadsToGrow += 2;                //queue 2 new heads to grow
                }

                RoundOfDamage.HeadCut = true;       //set the head to cut for this round

                //remove a head
                IWeapon weapon = EquipedWeapon.FirstOrDefault();
                RemoveEquipment(weapon);

                GlobalReference.GlobalValues.Notify.Mob(attacker, new TranslationMessage("You cut off on of the hydras heads."));
                GlobalReference.GlobalValues.Notify.Room(attacker, this, Room, new TranslationMessage("{performer} cut off on of the hydras heads."));
            }
        }

        public void RegrowHeads()
        {
            if (TookFireDamage == false)
            {
                for (int i = 0; i < NewHeadsToGrow; i++)
                {
                    AddEquipment(EquipedWeapon.FirstOrDefault());
                    Health += (int)(MaxHealth * .08);  //add back 80% of the health it took to loose a head after growing a pair of heads
                }

                GlobalReference.GlobalValues.Notify.Room(this, null, Room, new TranslationMessage($"{SentenceDescription} grows {NewHeadsToGrow} new heads."));
            }

            NewHeadsToGrow = 0;
            TookFireDamage = false;
        }
    }

    public class RoundOfDamage
    {
        [ExcludeFromCodeCoverage]
        public int TotalDamage { get; set; }

        [ExcludeFromCodeCoverage]
        public IMobileObject LastAttacker { get; set; }

        [ExcludeFromCodeCoverage]
        public uint CombatRound { get; set; }

        [ExcludeFromCodeCoverage]
        public bool HeadCut { get; set; }
    }
}
