using Objects.Damage.Interface;
using Objects.Global.Random.Interface;
using Objects.Item.Interface;
using Objects.Item.Items;
using Objects.Item.Items.Interface;
using Objects.Material;
using Objects.Material.Materials;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using static Objects.Damage.Damage;
using static Objects.Item.Items.Equipment;
using static Objects.Item.Items.Weapon;
using static Objects.Mob.NonPlayerCharacter;

namespace Objects.Global.Random
{
    public class RandomDropGenerator : IRandomDropGenerator
    {
        private static object padlock = new object();
        private static List<BaseMaterial> materials = null;
        private static List<BaseMaterial> Materials
        {
            get
            {
                if (materials == null)
                {
                    lock (padlock)
                    {
                        if (materials == null)
                        {
                            List<BaseMaterial> tempMaterials = new List<BaseMaterial>();
                            List<Type> types = new List<Type>();
                            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
                            {
                                if (t.IsSubclassOf(typeof(BaseMaterial)))
                                {
                                    types.Add(t);
                                }
                            }
                            //List<Type> types = Assembly.GetExecutingAssembly().GetTypes().Where(e => e.IsSubclassOf(typeof(BaseMaterial))).ToList();

                            foreach (Type type in types)
                            {
                                if (type != typeof(NpcInnateArmor))
                                {
                                    tempMaterials.Add((BaseMaterial)Activator.CreateInstance(type));
                                }
                            }
                            materials = tempMaterials;
                        }
                    }
                }

                return materials;
            }
        }

        public IItem GenerateRandomDrop(INonPlayerCharacter nonPlayerCharacter)
        {
            //if the odds of generating an item is 0 then return nothing immediately 
            if (GlobalReference.GlobalValues.Settings.RandomDropPercent == 0)
            {
                return null;
            }

            if (GlobalReference.GlobalValues.Random.PercentDiceRoll(GlobalReference.GlobalValues.Settings.RandomDropPercent))
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
            if (GlobalReference.GlobalValues.Settings.DropBeingPlusOnePercent > 0)
            {
                while (objectGenerateLevelAt < GlobalReference.GlobalValues.Settings.MaxLevel
                    && GlobalReference.GlobalValues.Random.PercentDiceRoll(GlobalReference.GlobalValues.Settings.DropBeingPlusOnePercent))
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
                    weapon.LookDescription = "The club looks to well balanced with a {description} leather grip.";
                    weapon.ShortDescription = "The stout {material} club looks to be well balanced.";
                    weapon.SentenceDescription = "club";
                    weapon.KeyWords.Add("club");
                    weapon.FlavorOptions.Add("{material}", new List<string>() { "wooden", "stone" });
                    weapon.FlavorOptions.Add("{description}", new List<string>() { "frayed", "worn", "strong" });
                    break;
                case WeaponType.Mace:
                    weapon.ExamineDescription = "The head of the mace {shape}.";
                    weapon.LookDescription = "The shaft of the mace is {shaft} and the head of the {head}.";
                    weapon.ShortDescription = "The metal mace has an ornate head used for bashing things.";
                    weapon.SentenceDescription = "mace";
                    weapon.KeyWords.Add("mace");
                    weapon.FlavorOptions.Add("{shaft}", new List<string>() { "smooth", "has intricate scroll work", "has images depicting an ancient battle" });
                    weapon.FlavorOptions.Add("{head}", new List<string>() { "polished", "covered in runes", });
                    weapon.FlavorOptions.Add("{shape}", new List<string>() { "is a round ball", "has {number} {design}", "has {number} sides that resemble the crown of a king", "is round with several distinct layers resembling some type of upside down tower" });
                    weapon.FlavorOptions.Add("{number}", new List<string>() { "four", "five" });
                    weapon.FlavorOptions.Add("{design}", new List<string>() { "rows of triangular pyramids", "dragon heads delicately carved into it", "pairs flanges of coming to a rounded point" });
                    break;
                case WeaponType.WizardStaff:
                    weapon.ExamineDescription = "The wooden staff is constantly in flux as small leaves grow out from the staff, blossom {color2} flowers and then wilt and are reabsorbed into the staff.";
                    weapon.LookDescription = "The wooden staff has {head} for a head{feels}.";
                    weapon.ShortDescription = "The wizards staff hums with a deep sound that resonates deep in your body.";
                    weapon.SentenceDescription = "wizard staff";
                    weapon.KeyWords.Add("wizard staff");
                    weapon.KeyWords.Add("staff");
                    weapon.KeyWords.Add("wizard");
                    weapon.FlavorOptions.Add("{head}", new List<string>() { "gnarled fingers", "gnarled fingers wrapped around a {color} orb", "a {color} orb that floats above the end of the staff", "a {color} stone growing out of the end of the staff" });
                    weapon.FlavorOptions.Add("{feels}", new List<string>() { "", " and feels slightly cold", " and feels warm to the touch", " and vibrates in your hands" });
                    weapon.FlavorOptions.Add("{color}", new List<string>() { "red", "blue", "deep purple", "black as dark as night", "swirling gray blue" });
                    weapon.FlavorOptions.Add("{color2}", new List<string>() { "crimson", "sky blue", "deep purple", "white", "metallic orange", "silver" });
                    break;
                case WeaponType.Axe:
                    weapon.ExamineDescription = "The blade is {blade description} and made of {material}.";
                    weapon.LookDescription = "The axe could have been used by a great warrior of days or the local peasant down the road.  It is hard tell the history just from its looks.";
                    weapon.ShortDescription = "The axe has a large head and strong wooden handle.";
                    weapon.SentenceDescription = "axe";
                    weapon.KeyWords.Add("axe");
                    weapon.FlavorOptions.Add("{blade description}", new List<string>() { "carved runes", "well worn", "full of ornate intersecting lines" });
                    weapon.FlavorOptions.Add("{material}", new List<string>() { "iron", "green glass", "black stone", "granite", "iron with interwoven bands of gold creating a museum worth axe" });
                    break;
                case WeaponType.Sword:
                    weapon.ExamineDescription = "The blade is made from {blade material}.  The guard is {guard} and the handle is wrapped in {handle}.  There is a {pommel} for a pommel.";
                    weapon.LookDescription = "The blade is {condition} and has {sides} sharpened.";
                    weapon.ShortDescription = "A {type} sword used to cut down ones foes.";
                    weapon.SentenceDescription = "sword";
                    weapon.KeyWords.Add("sword");
                    weapon.FlavorOptions.Add("{type}", new List<string>() { "short", "long", "broad" });
                    weapon.FlavorOptions.Add("{condition}", new List<string>() { "pitted", "sharp", "smooth" });
                    weapon.FlavorOptions.Add("{sides}", new List<string>() { "one side", "both sides" });
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
                    weapon.LookDescription = "The blade is {condition} and has a small fuller running the length of the blade.";
                    weapon.ShortDescription = "The dagger is short sharp and pointy.  Perfect for concealing on your person.";
                    weapon.SentenceDescription = "dagger";
                    weapon.KeyWords.Add("dagger");
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
                    weapon.LookDescription = "This pick has a large grooved hammer head and a sharp pick on the back.";
                    weapon.ShortDescription = "This war pick is a versatile weapon used to fight against armored opponents.";
                    weapon.SentenceDescription = "war pick";
                    weapon.KeyWords.Add("war pick");
                    weapon.KeyWords.Add("pick");
                    weapon.KeyWords.Add("war");
                    weapon.FlavorOptions.Add("{head description}", new List<string>() { "is polished smooth and shines slightly", "is slightly rusted", "has dwarven runes", "is covered in elvish runes", "depicts a kings crest" });
                    break;
                case WeaponType.Spear:
                    weapon.ExamineDescription = "The spear head is made of {material}.";
                    weapon.LookDescription = "The spear head is pointed and about nine inches long.";
                    weapon.ShortDescription = "A large pointed spear that can be used to poke holes in ones foes or pick up trash.";
                    weapon.SentenceDescription = "spear";
                    weapon.KeyWords.Add("spear");
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

        public IArmor GenerateRandomArmor(int level, int effectiveLevel)
        {
            AvalableItemPosition itemPosition = PickRandomItemPosition();
            return GenerateRandomArmor(level, effectiveLevel, itemPosition);
        }

        private static AvalableItemPosition PickRandomItemPosition()
        {
            Array values = Enum.GetValues(typeof(AvalableItemPosition));
            int randomValue = GlobalReference.GlobalValues.Random.Next(values.Length - 1);  //don't pick not worn
            AvalableItemPosition itemPosition = (AvalableItemPosition)values.GetValue(randomValue);
            return itemPosition;
        }

        public IArmor GenerateRandomArmor(int level, int effectiveLevel, AvalableItemPosition itemPosition)
        {
            while (itemPosition == AvalableItemPosition.Held
                || itemPosition == AvalableItemPosition.Wield)
            {
                itemPosition = PickRandomItemPosition();
            }

            IArmor armor = new Armor();
            armor.Level = level;
            armor.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(effectiveLevel);
            armor.ItemPosition = itemPosition;

            BaseMaterial randomMaterial = Materials[GlobalReference.GlobalValues.Random.Next(Materials.Count)];
            armor.Material = randomMaterial;

            switch (itemPosition)
            {
                case AvalableItemPosition.Head:
                    armor.ExamineDescription = "The helmet has two small holes cut out for seeing out.";
                    armor.LookDescription = "The helmet is hard and light but well padded giving the ultimate compromise between protection and usability.";
                    armor.ShortDescription = "A well made helmet that looks like it might fit.";
                    armor.SentenceDescription = "helmet";
                    armor.KeyWords.Add("helmet");
                    break;
                case AvalableItemPosition.Neck:
                    armor.ExamineDescription = "A {color} stone rests softly in the middle of the necklace.";
                    armor.LookDescription = "The necklace has a stone attached to it via a round pendent.";
                    armor.ShortDescription = "A delicate necklace fit for any royal lady to wear to any party.";
                    armor.SentenceDescription = "necklace";
                    armor.KeyWords.Add("necklace");
                    armor.FlavorOptions.Add("{color}", new List<string>() { "black", "clear", "royal purple", "crimson red", "ocean blue", "emerald green" });
                    break;
                case AvalableItemPosition.Arms:
                    armor.ExamineDescription = "The bracer is made of strips of material held together with leather.";
                    armor.LookDescription = "Just a hair longer than your arm these bracers look to be a perfect fit.";
                    armor.ShortDescription = "A pair of bracers that look to offer good protection for your arms.";
                    armor.SentenceDescription = "bracer";
                    armor.KeyWords.Add("bracer");
                    break;
                case AvalableItemPosition.Hand:
                    armor.ExamineDescription = "Made of a thin material these gloves have a magical property to them that grants the wearer protection.";
                    armor.LookDescription = "The gloves have a {back} design on the back and a {inside} for the design on the inside.";
                    armor.ShortDescription = "The gloves look to be thin and not offer much protection.";
                    armor.SentenceDescription = "gloves";
                    armor.KeyWords.Add("gloves");
                    armor.FlavorOptions.Add("{back}", new List<string>() { "spider web", "unicorn", "lion", "fountain", "eagle", "griffin" });
                    armor.FlavorOptions.Add("{inside}", new List<string>() { "spider", "scorpion", "knights head", "horse", "sea shell", "mountain" });
                    break;
                case AvalableItemPosition.Finger:
                    armor.ExamineDescription = "The ring once had a design on the inside but has been worn smooth with time.";
                    armor.LookDescription = "The ring is smooth on the outside{design}.";
                    armor.ShortDescription = "The ring is a simple ring with no special markings or anything to suggest it is magical.";
                    armor.SentenceDescription = "ring";
                    armor.KeyWords.Add("ring");
                    armor.FlavorOptions.Add("{design}", new List<string>() { "", " and has a {color} stone on the top" });
                    armor.FlavorOptions.Add("{color}", new List<string>() { "black", "clear", "royal purple", "crimson red", "ocean blue", "emerald green" });
                    break;
                case AvalableItemPosition.Body:
                    armor.ExamineDescription = "There is a large emblem on the front of a {emblem}.";
                    armor.LookDescription = "The breast plate is hard giving the wearer plenty of protection while being light.";
                    armor.ShortDescription = "A strong breast plate that has a small dent in the left side but otherwise is in perfect condition.";
                    armor.SentenceDescription = "breast plate";
                    armor.KeyWords.Add("breastplate");
                    armor.KeyWords.Add("breast");
                    armor.KeyWords.Add("plate");
                    armor.FlavorOptions.Add("{emblem}", new List<string>() { "tree", "griffin", "meteor", "pair of lions on either side of a crown", "pair of lions on either side of a shield" });
                    break;
                case AvalableItemPosition.Waist:
                    armor.ExamineDescription = "The belt is made of an unknown material that shifts colors through all the colors of the rainbow.";
                    armor.LookDescription = "The belt is prismatic.  The color shifts through the rainbow as you move relative to it.";
                    armor.ShortDescription = "The belt is a prismatic color that shifts wildly.";
                    armor.SentenceDescription = "belt";
                    armor.KeyWords.Add("belt");
                    break;
                case AvalableItemPosition.Legs:
                    armor.ExamineDescription = "The pattern on the leggings produce a soft blue glow.";
                    armor.LookDescription = "The leggings have are a dark gray color with delicately carved curving lines on the front forming a intricate pattern.";
                    armor.ShortDescription = "A pair of leggings.";
                    armor.SentenceDescription = "legging";
                    armor.KeyWords.Add("legging");
                    break;
                case AvalableItemPosition.Feet:
                    armor.ExamineDescription = "Three pouches hang off the outside of each boot allowing you to have quick access to small items.";
                    armor.LookDescription = "Made of supple leather the boots are soft and easy to wear at the expense of some protection.";
                    armor.ShortDescription = "A pair of leather boots.";
                    armor.SentenceDescription = "boot";
                    armor.KeyWords.Add("boot");
                    break;
            }

            armor.FinishLoad();

            return armor;
        }
    }
}
