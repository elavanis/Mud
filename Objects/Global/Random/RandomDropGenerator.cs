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
                    weapon.ExamineDescription = "The club has been worn smooth with several large indentions.  There surly a story for each one but hopefully you were the story teller and not the receiving actor.";
                    weapon.LongDescription = "The club looks to well balanced with a {description} leather grip.";
                    weapon.ShortDescription = "The stout {material} club looks to be well balanced.";
                    weapon.SentenceDescription = "club";
                    weapon.KeyWords.Add("Club");
                    weapon.FlavorOptions.Add("{material}", new List<string>() { "wooden", "stone" });
                    weapon.FlavorOptions.Add("{description}", new List<string>() { "frayed", "worn", "strong" });
                    break;
                case WeaponType.Mace:
                    weapon.ExamineDescription = "The head of the mace {shape}.";
                    weapon.LongDescription = "The shaft of the mace is {shaft} and the head of the {head}.";
                    weapon.ShortDescription = "The metal mace has an ornate head used for bashing things.";
                    weapon.SentenceDescription = "mace";
                    weapon.KeyWords.Add("Mace");
                    weapon.FlavorOptions.Add("{shaft}", new List<string>() { "smooth", "has intricate scroll work", "has images depicting an ancient battle" });
                    weapon.FlavorOptions.Add("{head}", new List<string>() { "polished", "covered in runes", });
                    weapon.FlavorOptions.Add("{shape}", new List<string>() { "is a round ball", "has {number} {design}", "has {number} sides that resemble the crown of a king", "is round with several distinct layers resembling some type of upside down tower" });
                    weapon.FlavorOptions.Add("{number}", new List<string>() { "four", "five" });
                    weapon.FlavorOptions.Add("{design}", new List<string>() { "rows of triangular pyramids", "dragon heads delicately carved into it", "pairs flanges of coming to a rounded point" });
                    break;
                case WeaponType.WizardStaff:
                    weapon.ExamineDescription = "The wooden staff is constantly in flux as small leaves grow out from the staff, blossom {color2} flowers and then wilt and are reabsorbed into the staff ";
                    weapon.LongDescription = "The wooden staff has {head} for a head{feels}.";
                    weapon.ShortDescription = "The wizards staff hums with a deep sound that resonates deep in your body.";
                    weapon.SentenceDescription = "wizard staff";
                    weapon.KeyWords.Add("WizardStaff");
                    weapon.FlavorOptions.Add("{head}", new List<string>() { "gnarled fingers", "gnarled fingers wrapped around a {color} orb", "a {color} orb that floats above the end of the staff", "a {color} stone growing out of the end of the staff" });
                    weapon.FlavorOptions.Add("{feels}", new List<string>() { "", " and feels slightly cold", " and feels warm to the touch", " and vibrates in your hands" });
                    weapon.FlavorOptions.Add("{color}", new List<string>() { "red", "blue", "deep purple", "black as dark as night", "swirling gray blue" });
                    weapon.FlavorOptions.Add("{color2}", new List<string>() { "crimson", "sky blue", "deep purple", "white", "metallic orange", "silver" });
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

            weapon.FinishLoad();

            return weapon;
        }

        private IEquipment GenerateRandomArmor(int level, int effectiveLevel)
        {
            throw new NotImplementedException();
        }
    }
}
