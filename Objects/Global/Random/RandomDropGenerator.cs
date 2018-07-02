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
                    case MobType.Humanoid:
                        return GenerateRandomEquipment(nonPlayerCharacter);

                    default:
                        return null;
                }
            }
            else
            {
                return null; //no luck
            }
        }

        private IEquipment GenerateRandomEquipment(INonPlayerCharacter nonPlayerCharacter)
        {
            int objectGenerateLevelAt = nonPlayerCharacter.Level;

            //verify the setting is set to generate plus items
            if (GlobalReference.GlobalValues.Settings.OddsOfDropBeingPlusOne > 0)
            {
                while (GlobalReference.GlobalValues.Random.Next(GlobalReference.GlobalValues.Settings.OddsOfDropBeingPlusOne) == 0)
                {
                    objectGenerateLevelAt++;
                }
            }

            return GenerateRandomEquipment(nonPlayerCharacter.Level, objectGenerateLevelAt);
        }

        public IEquipment GenerateRandomEquipment(int level, int effectiveLevel)
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

        private IWeapon GenerateRandomWeapon(int level, int effectiveLevel)
        {
            return GenerateRandomWeapon(level, effectiveLevel, (WeaponType)GlobalReference.GlobalValues.Random.Next(8));
        }

        public IWeapon GenerateRandomWeapon(int level, int effectiveLevel, WeaponType weaponType)
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
                    weapon.ExamineDescription = "The wooden staff is constantly in flux as small leaves grow out from the staff, blossom {color2} flowers and then wilt and are reabsorbed into the staff.";
                    weapon.LongDescription = "The wooden staff has {head} for a head{feels}.";
                    weapon.ShortDescription = "The wizards staff hums with a deep sound that resonates deep in your body.";
                    weapon.SentenceDescription = "wizard staff";
                    weapon.KeyWords.Add("Wizard");
                    weapon.KeyWords.Add("Staff");
                    weapon.FlavorOptions.Add("{head}", new List<string>() { "gnarled fingers", "gnarled fingers wrapped around a {color} orb", "a {color} orb that floats above the end of the staff", "a {color} stone growing out of the end of the staff" });
                    weapon.FlavorOptions.Add("{feels}", new List<string>() { "", " and feels slightly cold", " and feels warm to the touch", " and vibrates in your hands" });
                    weapon.FlavorOptions.Add("{color}", new List<string>() { "red", "blue", "deep purple", "black as dark as night", "swirling gray blue" });
                    weapon.FlavorOptions.Add("{color2}", new List<string>() { "crimson", "sky blue", "deep purple", "white", "metallic orange", "silver" });
                    break;
                case WeaponType.Axe:
                    weapon.ExamineDescription = "The blade is {blade description} and made of {material}.";
                    weapon.LongDescription = "The axe could have been used by a great warrior of days or the local peasant down the road.  It is hard tell the history just from its looks.";
                    weapon.ShortDescription = "The axe has a large head and strong wooden handle.";
                    weapon.SentenceDescription = "axe";
                    weapon.KeyWords.Add("Axe");
                    weapon.FlavorOptions.Add("{blade description}", new List<string>() { "carved runes", "well worn", "full of ornate intersecting lines" });
                    weapon.FlavorOptions.Add("{material}", new List<string>() { "iron", "green glass", "black stone", "granite", "iron with interwoven bands of gold creating a museum worth axe" });
                    break;
                case WeaponType.Sword:
                    weapon.ExamineDescription = "The blade is made from {blade material}.  The guard is {guard} and the handle is wrapped in {handle}.  There is a {pommel} for a pommel.";
                    weapon.LongDescription = "The blade is {condition} and has {sides} sharpened.";
                    weapon.ShortDescription = "A {type} sword used to cut down ones foes.";
                    weapon.SentenceDescription = "sword";
                    weapon.KeyWords.Add("Sword");
                    weapon.FlavorOptions.Add("{type}", new List<string>() { "short", "long", "broad" });
                    weapon.FlavorOptions.Add("{condition}", new List<string>() { "pitted", "sharp", "smooth" });
                    weapon.FlavorOptions.Add("{sides}", new List<string>() { "one", "both" });
                    weapon.FlavorOptions.Add("{blade material}", new List<string>() { "steal", "cold steal", "a black metal that seems to suck the light out of the room", "two different metals.  The first being {metal1} forming the base of the sword with an inlay of {metal2} forming {secondMetalObject}." });
                    weapon.FlavorOptions.Add("{metal1}", new List<string>() { "steal" });
                    weapon.FlavorOptions.Add("{metal2}", new List<string>() { "gold", "copper", "silver" });
                    weapon.FlavorOptions.Add("{secondMetalObject}", new List<string>() { "runes", "intricate weaves", "ancient writings" });
                    weapon.FlavorOptions.Add("{guard}", new List<string>() { "shaped like a pair of wings", "shaped like a pair of dragon heads", "slightly curved upwards" });
                    weapon.FlavorOptions.Add("{handle}", new List<string>() { "{silkColor} silk", "leather", "shark skin" });
                    weapon.FlavorOptions.Add("{silkColor}", new List<string>() { "white", "black", "gold", "silver", "brown", "red", "orange", "yellow", "green", "blue", "purple" });
                    weapon.FlavorOptions.Add("{pommel}", new List<string>() { "dragon claw holding a {pommelStone}", "large {pommelStone}", "skull with a pair of red rubies for eyes" });
                    weapon.FlavorOptions.Add("{pommelStone}", new List<string>() { "amber stone", "piece of amethyst", "aquamarine stone", "bloodstone", "diamond", "emerald", "garnet gem", "jade stone", "moonstone", "piece of onyx", "quartz stone", "rubie", "sapphire", "sunstone", "tigers eye", "topaz stone" });
                    break;
                case WeaponType.Dagger:
                    weapon.ExamineDescription = "The blade is made from {blade material}.  The handle is wrapped in {handle} and there is a small {pommel} for a pommel.";
                    weapon.LongDescription = "The blade is {condition} and has a small fuller running the length of the blade.";
                    weapon.ShortDescription = "The dagger is short sharp and pointy.  Perfect for concealing on your person.";
                    weapon.SentenceDescription = "sword";
                    weapon.KeyWords.Add("Dagger");
                    weapon.FlavorOptions.Add("{condition}", new List<string>() { "pitted", "sharp", "smooth" });
                    weapon.FlavorOptions.Add("{blade material}", new List<string>() { "steal", "cold steal", "a black metal that seems to suck the light out of the room", "two different metals.  The first being {metal1} forming the base of the sword with an inlay of {metal2} forming {secondMetalObject}." });
                    weapon.FlavorOptions.Add("{metal1}", new List<string>() { "steal" });
                    weapon.FlavorOptions.Add("{metal2}", new List<string>() { "gold", "copper", "silver" });
                    weapon.FlavorOptions.Add("{secondMetalObject}", new List<string>() { "runes", "intricate weaves", "ancient writings" });
                    weapon.FlavorOptions.Add("{handle}", new List<string>() { "{silkColor} silk", "leather", "shark skin" });
                    weapon.FlavorOptions.Add("{silkColor}", new List<string>() { "white", "black", "gold", "silver", "brown", "red", "orange", "yellow", "green", "blue", "purple" });
                    weapon.FlavorOptions.Add("{pommel}", new List<string>() { "knights helmet", "small {pommelStone}", "skull with a pair of red rubies for eyes" });
                    weapon.FlavorOptions.Add("{pommelStone}", new List<string>() { "amber", "amethyst", "aquamarine", "bloodstone", "diamond", "emerald", "garnet", "jade", "moonstone", "onyx", "quartz", "rubies", "sapphire", "sunstone", "tigers eye", "topaz" });
                    break;
                case WeaponType.Pick:
                    weapon.ExamineDescription = "The head of the war pick {head description}.";
                    weapon.LongDescription = "This pick has a large grooved hammer head and a sharp pick on the back";
                    weapon.ShortDescription = "This war pick is a versatile weapon used to fight against armored opponents.";
                    weapon.SentenceDescription = "war pick";
                    weapon.KeyWords.Add("Pick");
                    weapon.FlavorOptions.Add("{head description}", new List<string>() { "is polished smooth and shines slightly", "is slightly rusted", "has dwarven runes", "is covered in elvish runes", "depicts a kings crest" });
                    break;
                case WeaponType.Spear:
                    weapon.ExamineDescription = "The spear head is made of {material}.";
                    weapon.LongDescription = "The spear head is pointed and about nine inches long.";
                    weapon.ShortDescription = "A large pointed spear that can be used to poke holes in ones foes or pick up trash.";
                    weapon.SentenceDescription = "spear";
                    weapon.KeyWords.Add("Spear");
                    weapon.FlavorOptions.Add("{material}", new List<string>() { "flint", "black iron", "iron", "steel", "an unidentified blue metal that is warm to the touch" });
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
