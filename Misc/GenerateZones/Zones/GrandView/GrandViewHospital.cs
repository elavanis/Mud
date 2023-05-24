using System.Collections.Generic;
using System.Linq;
using Objects.Zone.Interface;
using System.Reflection;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Magic.Enchantment;
using Objects.Effect;
using Objects.Mob.Interface;
using Objects;
using static Objects.Global.Direction.Directions;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Damage.Interface;
using Objects.Global;
using static Objects.Damage.Damage;
using static Objects.Global.Stats.Stats;
using static Objects.Item.Item;
using Objects.Material.Materials;
using static Objects.Item.Items.Equipment;
using Objects.Personality;
using Objects.Magic.Interface;
using Objects.Magic.Spell.Generic;
using Objects.Global.Language;
using Objects.Language;
using Objects.Language.Interface;
using static Shared.TagWrapper.TagWrapper;
using static Objects.Mob.NonPlayerCharacter;
using static Objects.Item.Items.Weapon;
using MiscShared;

namespace GenerateZones.Zones.GrandView
{
    public class GrandViewHospital : BaseZone, IZoneCode
    {
        public GrandViewHospital() : base(7)
        {
        }

        public IZone Generate()
        {
            Zone.Name = nameof(GrandViewHospital);

            BuildRoomsViaReflection(this.GetType());

            ConnectRooms();

            return Zone;
        }

        #region Rooms
        /// <summary>
        /// This is the room you will enter after leaving the starting room and coming back.
        /// </summary>
        /// <returns></returns>
        private IRoom GenerateRoom1()
        {
            string examineDescription = "The place is dry and well lit.  The beds are comfortable and clean.  While the initial scan of the place seems nice it is still a hospital.  People are here because they are sick.";
            string lookDescription = "This corner of the hospital is quiet.  One or two beds are occupied by sleeping patients.";
            string shortDescription = "A quiet corner of the hospital";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);
            room.Attributes.Add(Room.RoomAttribute.Peaceful);

            room.AddMobileObjectToRoom(Nurse(room));
            room.AddItemToRoom(HospitalGown());
            room.AddItemToRoom(HospitalGown());
            room.AddItemToRoom(HospitalGown());
            return room;
        }
       
        private IRoom GenerateRoom4()
        {
            string examineDescription = "The hallway way is fairly long and is covered from the floor to the walls in tie.  Foot steps echo up and down giving an empty cold and sterile feeling.";
            string lookDescription = "You stand at the intersection of the recovery, surgery and entrance of the hospital.";
            string shortDescription = "A hallway";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);
            return room;
        }

        private IRoom GenerateRoom5()
        {
            string examineDescription = "The entrance to GrandView Hospital presents a nice face for what could be an awful experience.  The receptions desk is situated such that it is the first thing you see.  Made of some type of imported wood it stands several feet tall and has a stone top.  Several pieces of paper and pens sit neatly arranged on the desk.";
            string lookDescription = "The entrance of GrandView Hospital is before you.  By the entrance is the receptions desk and general seating is behind there.  To the west is the surgery and recovery wing and to the south is the morgue.";
            string shortDescription = "The entrance of GrandView Hospital";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);
            return room;
        }

        private IRoom GenerateRoom6()
        {
            string examineDescription = "The operating table sits in the middle of the room.  It has straps for holding the patient in place while the operation is underway.  There is a tray of surgical utensils next to the table and on the far west wall a poster labeling the different body parts.";
            string lookDescription = "You have entered the main operating room.  It looks like the room is ready for an operation but there is no one here.";
            string shortDescription = "Surgery";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddItemToRoom(Scalpel());
            room.AddItemToRoom(Scalpel());
            room.AddItemToRoom(Scalpel());
            return room;
        }

        private IRoom GenerateRoom7()
        {
            string examineDescription = "Each of the shelves hold five bodies wrapped in white sheets.  There are a pair of bodies in the corner drained of blood being filed with embalming fluids.";
            string lookDescription = "The morgue is filled with shelves of dead bodies wrapped in white sheets.";
            string shortDescription = "Morgue";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddItemToRoom(GlowingJar());
            room.AddItemToRoom(GlowingJar());
            room.AddItemToRoom(GlowingJar());
            room.AddItemToRoom(GlowingJar());
            room.AddItemToRoom(GlowingJar());

            return room;
        }

        /// <summary>
        /// This is the room you will enter after dieing on the lifeboat on the starting ship.
        /// </summary>
        /// <returns></returns>
        private IRoom GenerateRoom2()
        {
            string examineDescription = "The place is dry and well lit.  The beds are comfortable and clean.  While the initial scan of the place seems nice it is still a hospital.  People are here because they are sick.";
            string lookDescription = "This corner of the hospital is quiet.  One or two beds are occupied by sleeping patients.";
            string shortDescription = "A quiet corner of the hospital";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);
            room.Attributes.Add(Room.RoomAttribute.Peaceful);

            EnterRoomEnchantment enter = new EnterRoomEnchantment();
            enter.ActivationPercent = 100;
            enter.Effect = new SetRespawnPoint();
            enter.Parameter.ObjectId = new BaseObjectId(7, 3); //hospital
            room.Enchantments.Add(enter);

            INonPlayerCharacter npc = Nurse(room);
            enter = new EnterRoomEnchantment();
            enter.ActivationPercent = 100;
            enter.Effect = new Tell();
            enter.Parameter.ObjectId = new BaseObjectId(Zone.Id, npc.Id);
            enter.Parameter.TargetMessage = new TranslationMessage("It's good to have you among the living again.  Next time be more careful because dieing isn't fun and your going to have a nasty scar to remind you of it.", TagType.Communication);
            npc.Enchantments.Add(enter);
            room.AddMobileObjectToRoom(npc);
            room.AddItemToRoom(HospitalGown());
            room.AddItemToRoom(HospitalGown());
            room.AddItemToRoom(HospitalGown());
            return room;
        }

        /// <summary>
        /// This is the room you will respawn in after the ship.
        /// </summary>
        /// <returns></returns>
        private IRoom GenerateRoom3()
        {
            string examineDescription = "The place is dry and well lit.  The beds are comfortable and clean.  While the initial scan of the place seems nice it is still a hospital.  People are here because they are sick.";
            string lookDescription = "This corner of the hospital is quiet.  One or two beds are occupied by sleeping patients.";
            string shortDescription = "A quiet corner of the hospital";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);
            room.Attributes.Add(Room.RoomAttribute.Peaceful);

            EnterRoomEnchantment enter = new EnterRoomEnchantment();
            enter.ActivationPercent = 100;
            enter.Effect = new SetRespawnPoint();
            enter.Parameter.ObjectId = new BaseObjectId(7, 2); //hospital
            room.Enchantments.Add(enter);

            INonPlayerCharacter npc = Nurse(room);
            enter = new EnterRoomEnchantment();
            enter.ActivationPercent = 100;
            enter.Effect = new Tell();
            enter.Parameter.ObjectId = new RoomId(Zone.Id, npc.Id);
            enter.Parameter.TargetMessage = new TranslationMessage("It's good to have you conscious again.  The other nurses and I were getting worried when you wouldn't wake up for 2 weeks.  We called in the best healers but they were beginning to loose hope.  Still we had hope because at night would scream and thrash like you were on fire.  It is no wonder since over 85% of your body was burned.  I heard the ship you were on was attacked by the pair of dragons off the coast.  One of the passengers said you were about to make it in to the life boat when you were knocked overboard by a fireball from one of the dragons.  One of the passengers Noah Davies jumped in and saved you from drowning but you were unconscious when they pulled you into the boat.  You floated on the ocean adrift for 3 days before you were picked up by Captain Reynolds.  Once you and the rest of the survivors were picked up you were brought here to GrandView Hospital where we have been treating you.", TagType.Communication);
            npc.Enchantments.Add(enter);
            room.AddMobileObjectToRoom(npc);
            room.AddItemToRoom(HospitalGown());
            room.AddItemToRoom(HospitalGown());
            room.AddItemToRoom(HospitalGown());
            return room;
        }

        /// <summary>
        /// This is the room you will respawn while dieing on the ship.
        /// </summary>
        /// <returns></returns>
        private IRoom GenerateRoom8()
        {
            string examineDescription = "The place is dry and well lit.  The beds are comfortable and clean.  While the initial scan of the place seems nice it is still a hospital.  People are here because they are sick.";
            string lookDescription = "This corner of the hospital is quiet.  One or two beds are occupied by sleeping patients.";
            string shortDescription = "A quiet corner of the hospital";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);
            room.Attributes.Add(Room.RoomAttribute.Peaceful);

            EnterRoomEnchantment enter = new EnterRoomEnchantment();
            enter.ActivationPercent = 100;
            enter.Effect = new SetRespawnPoint();
            enter.Parameter.ObjectId = new BaseObjectId(7, 2); //hospital
            room.Enchantments.Add(enter);

            INonPlayerCharacter npc = Nurse(room);
            enter = new EnterRoomEnchantment();
            enter.ActivationPercent = 100;
            enter.Effect = new Tell();
            enter.Parameter.ObjectId = new RoomId(Zone.Id, npc.Id);
            enter.Parameter.TargetMessage = new TranslationMessage("It's good to have you conscious again.  The other nurses and I were getting worried when you wouldn't wake up for 2 weeks.  We called in the best healers but they were beginning to loose hope.  Still we had hope because at night would scream and thrash like you were on fire.  It is no wonder since over 85% of your body was burned.  I heard the ship you were on was attacked by the pair of dragons off the coast.  One of the passengers said you were still on the ship when it exploded. One of the passengers Noah Davies jumped in and saved you from drowning but you were unconscious when they pulled you into the boat.  You floated on the ocean adrift for 3 days before you were picked up by Captain Reynolds.  Once you and the rest of the survivors were picked up you were brought here to GrandView Hospital where we have been treating you.", TagType.Communication);
            npc.Enchantments.Add(enter);
            room.AddMobileObjectToRoom(npc);
            room.AddItemToRoom(HospitalGown());
            room.AddItemToRoom(HospitalGown());
            room.AddItemToRoom(HospitalGown());
            return room;
        }
        #endregion Rooms

        #region Npcs
        private INonPlayerCharacter Nurse(IRoom room)
        {
            string examineDescription = "The nurse is dressed in blue dress with a white apron that covers the front of her and ties in the back.  On her head she wears a white cloth that drapes down on either side of her head.";
            string lookDescription = "A nurse busily attends to her patients, taking care to make sure each one is as comfortable as possible.";
            string sentenceDescription = "nurse";
            string shortDescription = "A nurse attending patients.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 20); ;
            npc.KeyWords.Add("Nurse");

            npc.Personalities.Add(new Healer() { CastPercent = 10 });
            npc.SpellBook.Add("HEALTH", Health());
            npc.SpellBook.Add("MAGIC", Magic());
            npc.SpellBook.Add("STAMIA", Stamina());

            return npc;
        }

        #endregion Npcs

        #region Items
        private IArmor HospitalGown()
        {
            string examineDescription = "Thin and airy this would not provide much defense against anything other than people staring at you.  On second thought if you wore this out in public, people would stare.  This really wouldn't do much good.";
            string lookDescription = "This is your everyday hospital gown.  White with little blue dots with a tie in the back that exposes a little to much of your rear.";
            string sentenceDescription = "hospital gown";
            string shortDescription = "A loose fitting hospital gown";

            IArmor armor = CreateArmor(AvalableItemPosition.Body, 1, examineDescription, lookDescription, sentenceDescription, shortDescription, new Cloth());
            armor.KeyWords.Add("Hospital");
            armor.KeyWords.Add("Gown");

            armor.FinishLoad();
            return armor;
        }

        private IWeapon Scalpel()
        {
            string examineDescription = "Useful for making surgical cuts in the hands of a doctor and looking like you escaped from a mental hospital in yours.  Perhaps you should try to find a better, less psycho imagery inducing weapon.";
            string lookDescription = "Made of precision surgical steel it as reflective as it is sharp.";
            string sentenceDescription = "surgical scalpel";
            string shortDescription = "A surgical scalpel.";

            IWeapon scalpel = CreateWeapon(WeaponType.Dagger, 1, examineDescription, lookDescription, sentenceDescription, shortDescription);
            scalpel.KeyWords.Clear();
            scalpel.KeyWords.Add("Scalpel");
            scalpel.AttackerStat = Stat.Dexterity;
            scalpel.DeffenderStat = Stat.Dexterity;
            scalpel.FinishLoad();
            return scalpel;
        }

        private IItem GlowingJar()
        {
            string examineDescription = "A small jar about 1.5 inches tall.  It glows with a soft {color} light that is capable of lighting a room without being harsh on the eyes.";
            string lookDescription = "While not very big it does produces a soft {color} glow that is capable of lighting up a room.";
            string sentenceDescription = "glowing jar";
            string shortDescription = "A glowing jar.";

            IEquipment item = CreateEquipment(AvalableItemPosition.Held, 1, examineDescription, lookDescription, sentenceDescription, shortDescription);
            item.Attributes.Add(ItemAttribute.Light);
            item.KeyWords.Add("Jar");
            item.KeyWords.Add("{color}");
            item.FlavorOptions.Add("{color}", new List<string>() { "green", "blue", "white", "yellow", "red", "purple" });

            return item;
        }

        #endregion Items

        #region Spells
        private ISpell Health()
        {
            ISpell heal = BuildSpell("health", 0);

            heal.TargetNotificationSuccess = new TranslationMessage("You feel healthier.");
            heal.Effect = new RecoverHealth();

            return heal;
        }

        private static ISpell BuildSpell(string spellName, int manaCost)
        {
            ISpell spell = new SingleTargetSpell(spellName, manaCost);
            spell.SpellName = spellName;
            spell.ManaCost = 0;
            spell.Parameter.Dice = GlobalReference.GlobalValues.DefaultValues.ReduceValues(1, 100);
            string message = "The nurse says {0} and is briefly surrounded in a aura of light.";
            List<ITranslationPair> translate = new List<ITranslationPair>();
            ITranslationPair translationPair = new TranslationPair(Translator.Languages.Magic, spellName);
            translate.Add(translationPair);
            ITranslationMessage translationMessage = new TranslationMessage(message, TagType.Info, translate);
            spell.RoomNotificationSuccess = translationMessage;
            spell.PerformerNotificationSuccess = new TranslationMessage("you cast a spell");
            return spell;
        }

        private ISpell Magic()
        {
            ISpell heal = BuildSpell("magic", 0);
            heal.TargetNotificationSuccess = new TranslationMessage("You feel more magical.");
            heal.Effect = new RecoverMana();

            return heal;
        }

        private ISpell Stamina()
        {
            ISpell heal = BuildSpell("stamina", 0);
            heal.TargetNotificationSuccess = new TranslationMessage("You feel more energetic.");
            heal.Effect = new RecoverStamina();

            return heal;
        }
        #endregion Spells

        private void ConnectRooms()
        {
            //we do these rooms like this because we don't want to connect back
            Zone.Rooms[2].East = new Exit() { Zone = 7, Room = 4 };
            Zone.Rooms[3].East = new Exit() { Zone = 7, Room = 4 };
            Zone.Rooms[8].East = new Exit() { Zone = 7, Room = 4 };

            ZoneHelper.ConnectZone(Zone.Rooms[5], Direction.North, 5, 4);

            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.East, Zone.Rooms[4]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.East, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.North, Zone.Rooms[6]);
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.South, Zone.Rooms[7]);
        }
    }
}
