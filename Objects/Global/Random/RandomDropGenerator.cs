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
            return GenerateRandomWeapon(level, effectiveLevel, (WeaponType)GlobalReference.GlobalValues.Random.Next(8));
        }

        public IEquipment GenerateRandomWeapon(int level, int effectiveLevel, WeaponType weaponType)
        {
            IWeapon weapon = new Weapon();
            weapon.Level = level;

            IDamage damage = new Damage.Damage();
            damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(effectiveLevel);
            weapon.DamageList.Add(damage);

            weapon.Type = weaponType;
            switch (weaponType)
            {
                case WeaponType.Club:
                    weapon.ExamineDescription = "The club has been worn smooth with several large indentions.  There surly a story for each one but hopefully you were the one telling the story and not the receiving actor.";
                    weapon.LongDescription = "The club looks to well balanced with a {description} leather grip.";
                    weapon.ShortDescription = "The stout {material} club looks to be well balanced.";
                    weapon.SentenceDescription = "club";
                    weapon.KeyWords.Add("Club");
                    weapon.FlavorOptions.Add("{material}", new List<string>() { "wooden", "stone" });
                    weapon.FlavorOptions.Add("{description}", new List<string>() { "frayed", "worn", "strong" });
                    break;
                case WeaponType.Mace:
                    weapon.ExamineDescription = "";
                    weapon.LongDescription = "";
                    weapon.ShortDescription = "";
                    weapon.SentenceDescription = "";
                    weapon.KeyWords.Add("Mace");
                    break;
                case WeaponType.WizardStaff:
                    weapon.ExamineDescription = "";
                    weapon.LongDescription = "";
                    weapon.ShortDescription = "";
                    weapon.SentenceDescription = "";
                    weapon.KeyWords.Add("WizardStaff");
                    break;
                case WeaponType.Axe:
                    weapon.ExamineDescription = "";
                    weapon.LongDescription = "";
                    weapon.ShortDescription = "";
                    weapon.SentenceDescription = "";
                    weapon.KeyWords.Add("Axe");
                    break;
                case WeaponType.Sword:
                    weapon.ExamineDescription = "";
                    weapon.LongDescription = "";
                    weapon.ShortDescription = "";
                    weapon.SentenceDescription = "";
                    weapon.KeyWords.Add("Sword");
                    break;
                case WeaponType.Dagger:
                    weapon.ExamineDescription = "";
                    weapon.LongDescription = "";
                    weapon.ShortDescription = "";
                    weapon.SentenceDescription = "";
                    weapon.KeyWords.Add("Dagger");
                    break;
                case WeaponType.Pick:
                    weapon.ExamineDescription = "";
                    weapon.LongDescription = "";
                    weapon.ShortDescription = "";
                    weapon.SentenceDescription = "";
                    weapon.KeyWords.Add("Pick");
                    break;
                case WeaponType.Spear:
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

            return weapon;
        }

        private IEquipment GenerateRandomArmor(int level, int effectiveLevel)
        {
            throw new NotImplementedException();
        }
    }
}
