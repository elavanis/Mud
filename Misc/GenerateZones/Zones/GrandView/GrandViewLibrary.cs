using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Objects.Zone.Interface;
using Objects.Room;
using Objects.Damage;
using Objects.Personality;
using static Objects.Guild.Guild;
using Objects.Global.Stats;
using Objects.Global;
using Objects.Material.Materials;
using static Objects.Global.Direction.Directions;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.Item.Items.Interface;
using static Objects.Mob.NonPlayerCharacter;
using static Objects.Item.Items.Weapon;
using static Objects.Item.Items.Equipment;
using MiscShared;

namespace GenerateZones.Zones
{
    public class GrandViewLibrary : BaseZone, IZoneCode
    {
        public GrandViewLibrary() : base(2)
        {
        }

        public IZone Generate()
        {
            Zone.Name = nameof(GrandViewLibrary);

            BuildRoomsViaReflection(this.GetType());

            ConnectRooms();

            return Zone;
        }

        #region Rooms
        #region Library Basement

        private IRoom GenerateRoom1()
        {
            IRoom room = IndoorRoomLight();
            room.Attributes.Add(Room.RoomAttribute.NoNPC);

            room.ExamineDescription = "The floor is a beautiful mosaic of the surrounding areas.  The mountains to the north and west and the forest to the east are both represented. For some reason the map maker left out the south.  Still the map must be old because fort Woodbrook is shown miles from the forest and it has long since been overgrown and lies deep in heart of the forest.";
            room.LookDescription = "The entrance to the library is a sandstone entry way.  The ceiling is domed and has \"Cave ab homine unius libri.\" written on it.  The floor is a mosaic of the surrounding lands.";
            room.ShortDescription = "Entrance to the great library";
            return room;
        }

        private IRoom GenerateRoom2()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom3()
        {
            return BasementSteps();
        }

        private IRoom GenerateRoom4()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom5()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom6()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom7()
        {
            IRoom room = IndoorRoomLight();

            room.ExamineDescription = "This corner of the library is used by the wizards as their guild hall.  Scrolls and books are scattered about with stacks ranging from a few feet to as hight as the ceiling.";
            room.LookDescription = "This corner of the basement is designated as the wizards guild.  Dimly lit candles burn at desks with scrolls inviting practitioners of magic to learn something new.";
            room.ShortDescription = "Library Basement";

            INonPlayerCharacter guildMaster = WizardGuildMaster();

            room.AddMobileObjectToRoom(guildMaster);

            return room;
        }

        private INonPlayerCharacter WizardGuildMaster()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, 20);
            npc.ExamineDescription = "The Guildmaster is dressed in a tattered gray cloak that looks to at one point been white.  He has a beard that is almost as long as he is tall and is long since lost any sign of color.";
            npc.LookDescription = "He stares into space as if contemplating things you couldn't even imagine.  Occasionally he says something as if he is talking to someone yet you can not see who.  Has he gone mad or talking to something beyond this realm?";
            npc.ShortDescription = "The wizard Guildmaster.";
            npc.SentenceDescription = "Guildmaster";
            npc.KeyWords.Add("GuildMaster");
            npc.KeyWords.Add("Wizard");
            npc.Personalities.Add(new GuildMaster(Guilds.Wizard));
            return npc;
        }

        private IRoom GenerateRoom8()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom9()
        {
            IRoom room = LibraryBasement();

            INonPlayerCharacter apprentice = Male_Apprentice();
            room.AddMobileObjectToRoom(apprentice);
            apprentice.Room = room;

            return room;
        }

        private IRoom GenerateRoom10()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom11()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom12()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom13()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom14()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom15()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom16()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom17()
        {
            IRoom room = LibraryBasement();

            INonPlayerCharacter apprentice = Female_Apprentice();
            room.AddMobileObjectToRoom(apprentice);
            apprentice.Room = room;
            apprentice.AddEquipment(WizardStaff());

            return room;
        }

        private IEquipment WizardStaff()
        {
            IWeapon staff = CreateWeapon(WeaponType.WizardStaff, 1);

            staff.ExamineDescription = "Examining the staff reveals the slight shimmer is a thin layer of frost.  The head of the staff is emitting extreme cold that could useful in battle or drinks at parties.";
            staff.LookDescription = "The gnarled staff is twisted age seems to have a slight shimmer at the head of the staff.";
            staff.ShortDescription = "A wizards staff hewn from an oak tree.";
            staff.SentenceDescription = "wizard staff";
            staff.KeyWords.Add("staff");
            staff.KeyWords.Add("ice");
            staff.AttackerStat = Stats.Stat.Dexterity;
            staff.DeffenderStat = Stats.Stat.Dexterity;

            Damage damage = new Damage();
            damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(staff.Level);
            damage.Type = Damage.DamageType.Bludgeon;
            staff.DamageList.Add(damage);

            damage = new Damage();
            damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(staff.Level);
            damage.Type = Damage.DamageType.Cold;
            damage.BonusDamageStat = Stats.Stat.Intelligence;
            staff.DamageList.Add(damage);

            return staff;
        }

        private IRoom GenerateRoom18()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom19()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom20()
        {
            IRoom room = LibraryBasement();

            INonPlayerCharacter apprentice = Female_Apprentice();
            room.AddMobileObjectToRoom(apprentice);
            apprentice.Room = room;

            return room;
        }

        private IRoom GenerateRoom21()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom22()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom23()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom24()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom25()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom26()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom27()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom28()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom29()
        {
            IRoom room = LibraryBasement();

            INonPlayerCharacter apprentice = Male_Apprentice();
            room.AddMobileObjectToRoom(apprentice);
            apprentice.Room = room;

            return room;
        }

        private IRoom GenerateRoom30()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom31()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom32()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom33()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom34()
        {
            IRoom room = LibraryBasement();

            INonPlayerCharacter apprentice = Female_Apprentice();
            room.AddMobileObjectToRoom(apprentice);
            apprentice.Room = room;

            return room;
        }

        private IRoom GenerateRoom35()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom36()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom37()
        {
            IRoom room = LibraryBasement();

            INonPlayerCharacter apprentice = Male_Apprentice();
            room.AddMobileObjectToRoom(apprentice);
            apprentice.Room = room;

            apprentice.AddEquipment(Ring());

            return room;
        }

        private IRoom BasementSteps()
        {
            IRoom room = IndoorRoomLight();

            room.ExamineDescription = "The stairs are eerily quiet, to quiet to be exact.  Perhaps since it is a library there is some magic that helps maintain the quietness.";
            room.LookDescription = "Worn stone steps connect the basement to the entrance of the library.";
            room.ShortDescription = "Basement Stairs";
            return room;
        }

        private IRoom LibraryBasement()
        {
            IRoom room = IndoorRoomLight();

            room.ExamineDescription = "The books are quite dusty from being in the basement for so long but the cool temperature has helped preserve the oldest ones.";
            room.LookDescription = "Piles of books are strewn across the floor here and there.";
            room.ShortDescription = "Library Basement";
            return room;
        }

        private IArmor Ring()
        {
            IArmor ring = CreateArmor(AvalableItemPosition.Legs, 1, new Gold());

            ring.ExamineDescription = "You throughly examine the ring but can find nothing of interest.  It appears to be nothing more than a gold ring.";
            ring.LookDescription = "A small round gold ring which otherwise is quite ordinary.";
            ring.ShortDescription = "A small gold ring.";
            ring.SentenceDescription = "gold ring";
            ring.KeyWords.Add("gold");
            ring.KeyWords.Add("ring");
            ring.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(ring.Level);

            return ring;
        }

        private INonPlayerCharacter Female_Apprentice()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, 2);
            npc.Personalities.Add(new Wanderer());

            npc.ExamineDescription = "She glances at you staring at her but quickly returns her task.";
            npc.LookDescription = "She wears a {adjective} {color} robe with {embroiderment} embroiderment.  A white sash is draped over her shoulders indicating her status of a {year} level apprentice.";
            npc.ShortDescription = "An female apprentice is wandering around looking for books.";
            npc.SentenceDescription = "Female apprentice";
            npc.KeyWords.Add("female");
            npc.KeyWords.Add("apprentice");

            List<string> adjective = new List<string>() { "dark", "light" };
            List<string> colors = new List<string>() { "red", "blue", "green", "purple", "yellow" };
            List<string> embroiderment = new List<string>() { "gold", "silver" };
            List<string> year = new List<string>() { "first", "second", "third" };

            npc.FlavorOptions.Add("{adjective}", adjective);
            npc.FlavorOptions.Add("{color}", colors);
            npc.FlavorOptions.Add("{embroiderment}", embroiderment);
            npc.FlavorOptions.Add("{year}", year);
            return npc;
        }

        private INonPlayerCharacter Male_Apprentice()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, 2);
            npc.Personalities.Add(new Wanderer());

            npc.ExamineDescription = "The apprentice is wandering around the basement aimlessly.";
            npc.LookDescription = "He wears a {adjective} {color} robe with {embroiderment} embroiderment.  A white sash is draped over his shoulders indicating his status of a {year} level apprentice.";
            npc.ShortDescription = "An male apprentice is wandering around looking for books.";
            npc.SentenceDescription = "Male apprentice";
            npc.KeyWords.Add("male");
            npc.KeyWords.Add("apprentice");

            List<string> adjective = new List<string>() { "dark", "light" };
            List<string> colors = new List<string>() { "red", "blue", "green", "purple", "yellow" };
            List<string> embroiderment = new List<string>() { "gold", "silver" };
            List<string> year = new List<string>() { "first", "second", "third" };

            npc.FlavorOptions.Add("{adjective}", adjective);
            npc.FlavorOptions.Add("{color}", colors);
            npc.FlavorOptions.Add("{embroiderment}", embroiderment);
            npc.FlavorOptions.Add("{year}", year);
            return npc;
        }
        #endregion Library Basement

        #region Library Upstairs
        private IRoom GenerateRoom38()
        {
            IRoom room = LibraryShelves();
            room.ExamineDescription = "Looking through all the old books on the shelves you come across a copy of \"Land of Nothing.\"";
            room.LookDescription = "Light streams through the stained glass to the west depicting a beautiful green field with mountains in the background.  Several small flowers are depicted breaking up the large expanse of green grass.";

            return room;
        }

        private IRoom GenerateRoom39()
        {
            return LibraryTables();
        }

        private IRoom GenerateRoom40()
        {
            IRoom room = LibraryShelves();
            room.ExamineDescription = "Looking through all the old books on the shelves you come across a copy of \"Black Shaman.\"";
            room.LookDescription = "Light streams through the stained glass to the east.  The scene is from the perspective of a person looking out a balcony overlooking a town below.";

            return room;
        }

        private IRoom GenerateRoom41()
        {
            IRoom room = LibraryShelves();
            room.ExamineDescription = "Looking through all the old books on the shelves you come across a copy of \"The Neverending Song.\"";
            room.LookDescription = "Light streams through the stained glass to the west depicting a well. The bucket sits on the edge of the well and a path leads towards a village but no one is around.";

            return room;
        }

        private IRoom GenerateRoom42()
        {
            IRoom room = LibraryTables();
            INonPlayerCharacter npc = LibaryPatron();
            room.AddMobileObjectToRoom(npc);
            npc.Room = room;

            return room;
        }

        private IRoom GenerateRoom43()
        {
            IRoom room = LibraryShelves();
            room.ExamineDescription = "Looking through all the old books on the shelves you come across a copy of \"Star Saga.\"";
            room.LookDescription = "Light streams through the stained glass to the east depicting a majestic dragon flying low over an ocean while a lighting storms strikes in the distance.";

            return room;
        }

        private IRoom GenerateRoom44()
        {
            IRoom room = LibraryShelves();
            room.ExamineDescription = "Looking through all the old books on the shelves you come across a copy of \"The Storm Game.\"";
            room.LookDescription = "Light streams through the stained glass to the west.  The stained glass is of a mountain vista overlooking a lake.  Hues of greens meld into too hues of blue.  Snow covered mountain tops give stark contrast to the image below.";

            return room;
        }

        private IRoom GenerateRoom45()
        {
            IRoom room = LibraryTables();
            INonPlayerCharacter npc = LibaryPatron();
            room.AddMobileObjectToRoom(npc);
            npc.Room = room;

            return room;
        }

        private IRoom GenerateRoom46()
        {
            IRoom room = LibraryShelves();
            room.ExamineDescription = "Looking through all the old books on the shelves you come across a copy of \"The Cry of the Rose.\"";
            room.LookDescription = "Light streams through the stained glass to the east.  The stained glass windows is of a two armies battling each other.  Swords and spears are drawn as each side charges the other.";

            return room;
        }

        private IRoom GenerateRoom47()
        {
            IRoom room = LibraryShelves();
            room.ExamineDescription = "Looking through all the old books on the shelves you come across a copy of \"The Chair's Trader.\"";
            room.LookDescription = "Light streams through the stained glass to the west depicting a foggy sunset.  The setting sun causes the light to be hues of orange and red while the fog causes the landscape colors to be hidden.  Combined the image caused the artist to go for a monochromatic scene broken only by silhouette of evergreens.";

            return room;
        }

        private IRoom GenerateRoom48()
        {
            return LibraryTables();
        }

        private IRoom GenerateRoom49()
        {
            IRoom room = LibraryShelves();
            room.ExamineDescription = "Looking through all the old books on the shelves you come across a copy of \"The Magic Argument.\"";
            room.LookDescription = "Light streams through the stained glass to the east depicting a humming bird eating from a red flower.  Drops of dew hang from the petals indicating it is still morning.";

            return room;
        }

        private IRoom GenerateRoom50()
        {
            IRoom room = LibraryShelves();
            room.ExamineDescription = "Looking through all the old books on the shelves you come across a copy of \"The Son of Winter.\"";
            room.LookDescription = "Light streams through the stained glass to the west which is of a cloud covered moon over looking an ocean port.  A sailing ship can be seen leaving port.";

            return room;
        }

        private IRoom GenerateRoom51()
        {
            IRoom room = LibraryTables();
            INonPlayerCharacter npc = LibaryPatron();
            room.AddMobileObjectToRoom(npc);
            npc.Room = room;

            return room;
        }

        private IRoom GenerateRoom52()
        {
            IRoom room = LibraryShelves();
            room.ExamineDescription = "Looking through all the old books on the shelves you come across a copy of \"The Atlas's Code.\"";
            room.LookDescription = "Light streams through the stained glass to the east which is of a mermaid sitting on a rock while a tall sailing ship is anchored in a harbor behind her.";

            return room;
        }

        private IRoom LibraryShelves()
        {
            IRoom room = IndoorRoomLight();

            room.ShortDescription = "Library shelves";
            return room;
        }

        private IRoom LibraryTables()
        {
            IRoom room = IndoorRoomLight();

            room.ExamineDescription = "The tables are strong and well built with many chairs on either side.  The tables sits on a pure white stone floor causing you to double check you didn't track anything into the library.";
            room.LookDescription = "Two large tables stretch from the west to east shelves filling up most of the room except for a center isle.";
            room.ShortDescription = "Library tables";
            return room;
        }

        private IRoom GenerateRoom53()
        {
            IRoom room = LibraryStairs();
            INonPlayerCharacter npc = LibaryPatron();
            room.AddMobileObjectToRoom(npc);
            npc.Room = room;

            return room;
        }

        private IRoom GenerateRoom54()
        {
            IRoom room = LibraryBalcony();
            INonPlayerCharacter npc = LibaryPatron();
            room.AddMobileObjectToRoom(npc);
            npc.Room = room;

            return room;
        }

        private IRoom GenerateRoom55()
        {
            return LibraryStairs();
        }

        private IRoom GenerateRoom56()
        {
            IRoom room = LibraryBalcony();
            INonPlayerCharacter npc = LibaryPatron();
            room.AddMobileObjectToRoom(npc);
            npc.Room = room;

            return room;
        }

        private IRoom GenerateRoom57()
        {
            return LibraryBalcony();
        }

        private IRoom GenerateRoom58()
        {
            return LibraryBalcony();
        }

        private IRoom GenerateRoom59()
        {
            IRoom room = LibraryBalcony();
            INonPlayerCharacter npc = LibaryPatron();
            room.AddMobileObjectToRoom(npc);
            npc.Room = room;

            return room;
        }

        private IRoom GenerateRoom60()
        {
            return LibraryBalcony();
        }

        private IRoom GenerateRoom61()
        {
            return LibraryBalcony();
        }

        private IRoom GenerateRoom62()
        {
            return LibraryStairs();
        }

        private IRoom GenerateRoom63()
        {
            return LibraryBalcony();
        }

        private IRoom GenerateRoom64()
        {
            return LibraryStairs();
        }

        private IRoom LibraryStairs()
        {
            IRoom room = IndoorRoomLight();

            room.ExamineDescription = "The spiral stairs are well worn with age and use.";
            room.LookDescription = "While there are no books in this part of the library it has seen its fair use of traffic as well.";
            room.ShortDescription = "Spiral Staircase";
            return room;
        }

        private IRoom LibraryBalcony()
        {
            IRoom room = IndoorRoomLight();

            room.ExamineDescription = "You look around the room but are drawn back to the balcony and its simple beauty.";
            room.LookDescription = "There are books up here but the true prize is the balcony over looking the library below and the arched dome above.";
            room.ShortDescription = "Library Balcony";
            return room;
        }

        private INonPlayerCharacter LibaryPatron()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, 2);
            npc.Personalities.Add(new Wanderer());

            npc.ExamineDescription = "The library patron is carrying a stack of {BookCount} books.";
            npc.LookDescription = "The library patron glances at you and smiles then returns to looking for the next book on their list.";
            npc.ShortDescription = "The library patron is wandering around the library looking for another book to read.";
            npc.SentenceDescription = "A library patron";
            npc.KeyWords.Add("library");
            npc.KeyWords.Add("patron");


            List<string> bookCount = new List<string>() { "two", "three", "four" };
            npc.FlavorOptions.Add("{BookCount}", bookCount);

            return npc;
        }
        #endregion Library Upstairs
        #endregion End Rooms

        private void ConnectRooms()
        {
            ZoneHelper.ConnectZone(Zone.Rooms[1], Direction.South, 5, 17);
            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.Down, Zone.Rooms[3]);
            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.North, Zone.Rooms[39]);

            #region Basement
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.East, Zone.Rooms[3]);
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.South, Zone.Rooms[8]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.East, Zone.Rooms[4]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.South, Zone.Rooms[9]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.East, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.South, Zone.Rooms[10]);
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.East, Zone.Rooms[6]);
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.South, Zone.Rooms[11]);
            ZoneHelper.ConnectRoom(Zone.Rooms[6], Direction.East, Zone.Rooms[7]);
            ZoneHelper.ConnectRoom(Zone.Rooms[6], Direction.South, Zone.Rooms[12]);
            ZoneHelper.ConnectRoom(Zone.Rooms[7], Direction.South, Zone.Rooms[13]);

            ZoneHelper.ConnectRoom(Zone.Rooms[8], Direction.East, Zone.Rooms[9]);
            ZoneHelper.ConnectRoom(Zone.Rooms[8], Direction.South, Zone.Rooms[14]);
            ZoneHelper.ConnectRoom(Zone.Rooms[9], Direction.East, Zone.Rooms[10]);
            ZoneHelper.ConnectRoom(Zone.Rooms[9], Direction.South, Zone.Rooms[15]);
            ZoneHelper.ConnectRoom(Zone.Rooms[10], Direction.East, Zone.Rooms[11]);
            ZoneHelper.ConnectRoom(Zone.Rooms[10], Direction.South, Zone.Rooms[16]);
            ZoneHelper.ConnectRoom(Zone.Rooms[11], Direction.East, Zone.Rooms[12]);
            ZoneHelper.ConnectRoom(Zone.Rooms[11], Direction.South, Zone.Rooms[17]);
            ZoneHelper.ConnectRoom(Zone.Rooms[12], Direction.East, Zone.Rooms[13]);
            ZoneHelper.ConnectRoom(Zone.Rooms[12], Direction.South, Zone.Rooms[18]);
            ZoneHelper.ConnectRoom(Zone.Rooms[13], Direction.South, Zone.Rooms[19]);

            ZoneHelper.ConnectRoom(Zone.Rooms[14], Direction.East, Zone.Rooms[15]);
            ZoneHelper.ConnectRoom(Zone.Rooms[14], Direction.South, Zone.Rooms[20]);
            ZoneHelper.ConnectRoom(Zone.Rooms[15], Direction.East, Zone.Rooms[16]);
            ZoneHelper.ConnectRoom(Zone.Rooms[15], Direction.South, Zone.Rooms[21]);
            ZoneHelper.ConnectRoom(Zone.Rooms[16], Direction.East, Zone.Rooms[17]);
            ZoneHelper.ConnectRoom(Zone.Rooms[16], Direction.South, Zone.Rooms[22]);
            ZoneHelper.ConnectRoom(Zone.Rooms[17], Direction.East, Zone.Rooms[18]);
            ZoneHelper.ConnectRoom(Zone.Rooms[17], Direction.South, Zone.Rooms[23]);
            ZoneHelper.ConnectRoom(Zone.Rooms[18], Direction.East, Zone.Rooms[19]);
            ZoneHelper.ConnectRoom(Zone.Rooms[18], Direction.South, Zone.Rooms[24]);
            ZoneHelper.ConnectRoom(Zone.Rooms[19], Direction.South, Zone.Rooms[25]);

            ZoneHelper.ConnectRoom(Zone.Rooms[20], Direction.East, Zone.Rooms[21]);
            ZoneHelper.ConnectRoom(Zone.Rooms[20], Direction.South, Zone.Rooms[26]);
            ZoneHelper.ConnectRoom(Zone.Rooms[21], Direction.East, Zone.Rooms[22]);
            ZoneHelper.ConnectRoom(Zone.Rooms[21], Direction.South, Zone.Rooms[27]);
            ZoneHelper.ConnectRoom(Zone.Rooms[22], Direction.East, Zone.Rooms[23]);
            ZoneHelper.ConnectRoom(Zone.Rooms[22], Direction.South, Zone.Rooms[28]);
            ZoneHelper.ConnectRoom(Zone.Rooms[23], Direction.East, Zone.Rooms[24]);
            ZoneHelper.ConnectRoom(Zone.Rooms[23], Direction.South, Zone.Rooms[29]);
            ZoneHelper.ConnectRoom(Zone.Rooms[24], Direction.East, Zone.Rooms[25]);
            ZoneHelper.ConnectRoom(Zone.Rooms[24], Direction.South, Zone.Rooms[30]);
            ZoneHelper.ConnectRoom(Zone.Rooms[25], Direction.South, Zone.Rooms[31]);

            ZoneHelper.ConnectRoom(Zone.Rooms[26], Direction.East, Zone.Rooms[27]);
            ZoneHelper.ConnectRoom(Zone.Rooms[26], Direction.South, Zone.Rooms[32]);
            ZoneHelper.ConnectRoom(Zone.Rooms[27], Direction.East, Zone.Rooms[28]);
            ZoneHelper.ConnectRoom(Zone.Rooms[27], Direction.South, Zone.Rooms[33]);
            ZoneHelper.ConnectRoom(Zone.Rooms[28], Direction.East, Zone.Rooms[29]);
            ZoneHelper.ConnectRoom(Zone.Rooms[28], Direction.South, Zone.Rooms[34]);
            ZoneHelper.ConnectRoom(Zone.Rooms[29], Direction.East, Zone.Rooms[30]);
            ZoneHelper.ConnectRoom(Zone.Rooms[29], Direction.South, Zone.Rooms[35]);
            ZoneHelper.ConnectRoom(Zone.Rooms[30], Direction.East, Zone.Rooms[31]);
            ZoneHelper.ConnectRoom(Zone.Rooms[30], Direction.South, Zone.Rooms[36]);
            ZoneHelper.ConnectRoom(Zone.Rooms[31], Direction.South, Zone.Rooms[37]);

            ZoneHelper.ConnectRoom(Zone.Rooms[32], Direction.East, Zone.Rooms[33]);
            ZoneHelper.ConnectRoom(Zone.Rooms[33], Direction.East, Zone.Rooms[34]);
            ZoneHelper.ConnectRoom(Zone.Rooms[34], Direction.East, Zone.Rooms[35]);
            ZoneHelper.ConnectRoom(Zone.Rooms[35], Direction.East, Zone.Rooms[36]);
            ZoneHelper.ConnectRoom(Zone.Rooms[36], Direction.East, Zone.Rooms[37]);

            #endregion Basement

            #region Library Upstairs

            ZoneHelper.ConnectRoom(Zone.Rooms[38], Direction.North, Zone.Rooms[41]);
            ZoneHelper.ConnectRoom(Zone.Rooms[38], Direction.East, Zone.Rooms[39]);
            ZoneHelper.ConnectRoom(Zone.Rooms[39], Direction.North, Zone.Rooms[42]);
            ZoneHelper.ConnectRoom(Zone.Rooms[39], Direction.East, Zone.Rooms[40]);
            ZoneHelper.ConnectRoom(Zone.Rooms[40], Direction.North, Zone.Rooms[43]);

            ZoneHelper.ConnectRoom(Zone.Rooms[41], Direction.North, Zone.Rooms[44]);
            ZoneHelper.ConnectRoom(Zone.Rooms[41], Direction.East, Zone.Rooms[42]);
            ZoneHelper.ConnectRoom(Zone.Rooms[42], Direction.North, Zone.Rooms[45]);
            ZoneHelper.ConnectRoom(Zone.Rooms[42], Direction.East, Zone.Rooms[43]);
            ZoneHelper.ConnectRoom(Zone.Rooms[43], Direction.North, Zone.Rooms[46]);

            ZoneHelper.ConnectRoom(Zone.Rooms[44], Direction.North, Zone.Rooms[47]);
            ZoneHelper.ConnectRoom(Zone.Rooms[44], Direction.East, Zone.Rooms[45]);
            ZoneHelper.ConnectRoom(Zone.Rooms[45], Direction.North, Zone.Rooms[48]);
            ZoneHelper.ConnectRoom(Zone.Rooms[45], Direction.East, Zone.Rooms[46]);
            ZoneHelper.ConnectRoom(Zone.Rooms[46], Direction.North, Zone.Rooms[49]);

            ZoneHelper.ConnectRoom(Zone.Rooms[47], Direction.North, Zone.Rooms[50]);
            ZoneHelper.ConnectRoom(Zone.Rooms[47], Direction.East, Zone.Rooms[48]);
            ZoneHelper.ConnectRoom(Zone.Rooms[48], Direction.North, Zone.Rooms[51]);
            ZoneHelper.ConnectRoom(Zone.Rooms[48], Direction.East, Zone.Rooms[49]);
            ZoneHelper.ConnectRoom(Zone.Rooms[49], Direction.North, Zone.Rooms[52]);

            ZoneHelper.ConnectRoom(Zone.Rooms[50], Direction.East, Zone.Rooms[51]);
            ZoneHelper.ConnectRoom(Zone.Rooms[51], Direction.East, Zone.Rooms[52]);

            ZoneHelper.ConnectRoom(Zone.Rooms[53], Direction.Down, Zone.Rooms[38]);
            ZoneHelper.ConnectRoom(Zone.Rooms[55], Direction.Down, Zone.Rooms[40]);
            ZoneHelper.ConnectRoom(Zone.Rooms[62], Direction.Down, Zone.Rooms[50]);
            ZoneHelper.ConnectRoom(Zone.Rooms[64], Direction.Down, Zone.Rooms[52]);

            ZoneHelper.ConnectRoom(Zone.Rooms[53], Direction.East, Zone.Rooms[54]);
            ZoneHelper.ConnectRoom(Zone.Rooms[54], Direction.East, Zone.Rooms[55]);
            ZoneHelper.ConnectRoom(Zone.Rooms[62], Direction.East, Zone.Rooms[63]);
            ZoneHelper.ConnectRoom(Zone.Rooms[63], Direction.East, Zone.Rooms[64]);

            ZoneHelper.ConnectRoom(Zone.Rooms[53], Direction.North, Zone.Rooms[56]);
            ZoneHelper.ConnectRoom(Zone.Rooms[56], Direction.North, Zone.Rooms[58]);
            ZoneHelper.ConnectRoom(Zone.Rooms[58], Direction.North, Zone.Rooms[60]);
            ZoneHelper.ConnectRoom(Zone.Rooms[60], Direction.North, Zone.Rooms[62]);

            ZoneHelper.ConnectRoom(Zone.Rooms[55], Direction.North, Zone.Rooms[57]);
            ZoneHelper.ConnectRoom(Zone.Rooms[57], Direction.North, Zone.Rooms[59]);
            ZoneHelper.ConnectRoom(Zone.Rooms[59], Direction.North, Zone.Rooms[61]);
            ZoneHelper.ConnectRoom(Zone.Rooms[61], Direction.North, Zone.Rooms[64]);

            #endregion Library Upstairs
        }

    }
}
