using Objects.Damage.Interface;
using Objects.Global.Random.Interface;
using Objects.Item.Interface;
using Objects.Item.Items;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Damage.Damage;
using static Objects.Item.Items.Weapon;
using static Objects.Mob.NonPlayerCharacter;

namespace Objects.Global.Random
{
    public class RandomDropGenerator : IRandomDropGenerator
    {
        public IItem GenerateRandomDrop(INonPlayerCharacter nonPlayerCharacter)
        {
            //if the odds of generating an item is 0 then return nothing immediately 
            if (GlobalReference.GlobalValues.Settings.OddsOfGeneratingRandomDrop == 0)
            {
                return null;
            }

            if (GlobalReference.GlobalValues.Random.Next(GlobalReference.GlobalValues.Settings.OddsOfGeneratingRandomDrop) == 0)
            {
                switch (nonPlayerCharacter.TypeOfMob)
                {
                    case MobType.Other:
                        return null;
                        break;
                    case MobType.Humanoid:
                        return GenerateRandomEquipment(nonPlayerCharacter);
                        break;

                    default:
                        return null;
                        break;
                }
            }
            else
            {
                return null; //no luck
            }
        }

        private IItem GenerateRandomEquipment(INonPlayerCharacter nonPlayerCharacter)
        {
            int objectGenerateLevelAt = nonPlayerCharacter.Level;
            while (GlobalReference.GlobalValues.Random.Next(GlobalReference.GlobalValues.Settings.OddsOfDropBeingPlusOne) == 0)
            {
                objectGenerateLevelAt++;
            }

            return GenerateRandomEquipment(nonPlayerCharacter.Level, objectGenerateLevelAt);
        }

        public IItem GenerateRandomEquipment(int level, int effectiveLevel)
        {
            IEquipment equipment;
            int randomValue = GlobalReference.GlobalValues.Random.Next(9);

            if (randomValue == 0)
            {
                equipment = GenerateRandomWeapon(level, effectiveLevel);
            }
            else
            {
                equipment = GenerateRandomArmor(level, effectiveLevel);
            }

            return equipment;
        }

        private IEquipment GenerateRandomWeapon(int level, int effectiveLevel)
        {
            IWeapon weapon = new Weapon();
            weapon.Level = level;

            IDamage damage = new Damage.Damage();
            damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(effectiveLevel);
            switch (GlobalReference.GlobalValues.Random.Next(8))
            {
                case 0:
                    weapon.Type = WeaponType.Club;
                    weapon.ExamineDescription = "The club has been worn smooth with several large indentions.  There surly a story for each one but hopefully you were the one telling the story and not the receiving actor.";
                    weapon.LongDescription = "What once must have been a strong tree branch has been reduced to a wooden club.";
                    weapon.ShortDescription = "The stout wooden club looks to be well balanced.";
                    weapon.SentenceDescription = "club";
                    weapon.KeyWords.Add("Club");
                    break;
                case 1:
                    weapon.Type = WeaponType.Mace;
                    weapon.ExamineDescription = "";
                    weapon.LongDescription = "";
                    weapon.ShortDescription = "";
                    weapon.SentenceDescription = "";
                    weapon.KeyWords.Add("Mace");
                    break;
                case 2:
                    weapon.Type = WeaponType.WizardStaff;
                    weapon.ExamineDescription = "";
                    weapon.LongDescription = "";
                    weapon.ShortDescription = "";
                    weapon.SentenceDescription = "";
                    weapon.KeyWords.Add("WizardStaff");
                    break;
                case 3:
                    weapon.Type = WeaponType.Axe;
                    weapon.ExamineDescription = "";
                    weapon.LongDescription = "";
                    weapon.ShortDescription = "";
                    weapon.SentenceDescription = "";
                    weapon.KeyWords.Add("Axe");
                    break;
                case 4:
                    weapon.Type = WeaponType.Sword;
                    weapon.ExamineDescription = "";
                    weapon.LongDescription = "";
                    weapon.ShortDescription = "";
                    weapon.SentenceDescription = "";
                    weapon.KeyWords.Add("Sword");
                    break;
                case 5:
                    weapon.Type = WeaponType.Dagger;
                    weapon.ExamineDescription = "";
                    weapon.LongDescription = "";
                    weapon.ShortDescription = "";
                    weapon.SentenceDescription = "";
                    weapon.KeyWords.Add("Dagger");
                    break;
                case 6:
                    weapon.Type = WeaponType.Pick;
                    weapon.ExamineDescription = "";
                    weapon.LongDescription = "";
                    weapon.ShortDescription = "";
                    weapon.SentenceDescription = "";
                    weapon.KeyWords.Add("Pick");
                    break;
                case 7:
                    weapon.Type = WeaponType.Spear;
                    weapon.ExamineDescription = "";
                    weapon.LongDescription = "";
                    weapon.ShortDescription = "";
                    weapon.SentenceDescription = "";
                    weapon.KeyWords.Add("Spear");
                    break;
            }

            switch (weapon.Type)
            {
                case WeaponType.Club:
                case WeaponType.Mace:
                case WeaponType.WizardStaff:
                    damage.Type = DamageType.Bludgeon;
                    break;
                case WeaponType.Axe:
                case WeaponType.Sword:
                    damage.Type = DamageType.Slash;
                    break;
                case WeaponType.Dagger:
                case WeaponType.Pick:
                case WeaponType.Spear:
                    damage.Type = DamageType.Pierce;
                    break;
            }
            weapon.DamageList.Add(damage);

            return weapon;
        }

        private IEquipment GenerateRandomArmor(int level, int effectiveLevel)
        {
            throw new NotImplementedException();
        }
    }
}
