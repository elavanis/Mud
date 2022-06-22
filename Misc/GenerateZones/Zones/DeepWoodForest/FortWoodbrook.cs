using System.Collections.Generic;
using System.Linq;
using Objects.Zone.Interface;
using System.Reflection;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Mob.Interface;
using Objects.Global;
using Objects.Personality.Interface;
using static Objects.Global.Direction.Directions;
using Objects.Item.Items.Interface;
using Objects.Material.Materials;
using static Objects.Item.Items.Equipment;
using Shared.Sound.Interface;
using Shared.Sound;
using Objects.Personality;
using Objects.Damage;
using Objects.Damage.Interface;
using Objects.Global.Stats;
using Objects.Interface;
using Objects.LoadPercentage;
using static Objects.Mob.NonPlayerCharacter;
using static Objects.Item.Items.Weapon;
using MiscShared;

namespace GenerateZones.Zones.DeepWoodForest
{
    public class FortWoodbrook : BaseZone, IZoneCode
    {

        public FortWoodbrook() : base(10)
        {
        }

        public IZone Generate()
        {
            Zone.InGameDaysTillReset = 10;
            Zone.Name = nameof(FortWoodbrook);

            BuildRoomsViaReflection(this.GetType());

            ConnectRooms();

            return Zone;
        }

        #region Rooms
        private IRoom GenerateRoom1()
        {
            string examineDescription = "While the wooden gates are still strong they have fallen off the hinges and lay strewn on ground.";
            string lookDescription = "Both sets of gates have fallen off the hinges and allow access to the fort.";
            string shortDescription = "Entrance to the fort";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 1);

            return room;
        }

        private IRoom GenerateRoom2()
        {
            string examineDescription = "To the north a collapsed building has fallen down.  To the east and west are stairs leading to the ramparts.";
            string lookDescription = "The court yard is overgrown with grass and small trees.";
            string shortDescription = "Inside the court yard";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 1);

            return room;
        }

        private IRoom GenerateRoom3()
        {
            string examineDescription = "The stairs are worn but look like they should hold your weight.";
            string lookDescription = "Wooden stairs lead up to the rampart.";
            string shortDescription = "Inside the court yard";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 1);

            return room;
        }

        private IRoom GenerateRoom4()
        {
            string examineDescription = "If the well had a cover it is long gone but there is still water in the well.  If only the bucket was around you could get a drink.";
            string lookDescription = "A small stone well dominates the area here.";
            string shortDescription = "Inside the court yard";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 1);

            return room;
        }

        private IRoom GenerateRoom5()
        {
            string examineDescription = "The part of the eastern wall has started to collapse allowing you to see out into the forest.";
            string lookDescription = "Thick grass has grown up obscuring what this part of the court yard might have been used for.";
            string shortDescription = "Inside the court yard";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 1);

            return room;
        }

        private IRoom GenerateRoom6()
        {
            string examineDescription = "It looks like this might have been stalls for horses but the building has long since collapsed.";
            string lookDescription = "A collapsed building lays before you.";
            string shortDescription = "Inside the court yard";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 1);

            return room;
        }

        private IRoom GenerateRoom7()
        {
            string examineDescription = "A several trees have started to grow up pushing the stone pavers out of the way.";
            string lookDescription = "You stand in what was once the center of the court yard.";
            string shortDescription = "Inside the court yard";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 1);

            return room;
        }

        private IRoom GenerateRoom8()
        {
            string examineDescription = "Peeking into the building you can see part of a shelf on the right but not any further as the walls have collapsed.";
            string lookDescription = "A small building such as a shed stands to the north.";
            string shortDescription = "Inside the court yard";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 1);

            return room;
        }

        private IRoom GenerateRoom9()
        {
            string examineDescription = "The door to the building is ajar and hangs only by one nail that refuses to give up.";
            string lookDescription = "The main building in the fort is to the west while the court yard opens to the east.";
            string shortDescription = "Inside the court yard";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 1);

            return room;
        }

        private IRoom GenerateRoom10()
        {
            string examineDescription = "The stairs have long been neglected and have succumb to mother nature and father time.";
            string lookDescription = "The stairs leading up to the ramparts have collapsed.";
            string shortDescription = "Inside the court yard";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 1);

            return room;
        }

        private IRoom GenerateRoom11()
        {
            string examineDescription = "The forest would have been cut back to allow lookouts to see approaching people sooner. How much is hard to say though.";
            string lookDescription = "The top of the rampart you are able to see the court yard below.  This would have given watchers a good view point of the forest below as well as the court yard.";
            string shortDescription = "On the rampart";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 1);

            return room;
        }

        private IRoom GenerateRoom12()
        {
            string examineDescription = "The whole fort seems to let out a low creak as you walk on this part of the rampart.";
            string lookDescription = "As you step on to this part of the rampart it lets out a grown in protest at your presence.";
            string shortDescription = "On the rampart";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 1);

            ISound sound = new Sound();
            sound.SoundName = $"{Zone.Name}\\WooodenCreak.mp3";
            room.Sounds.Add(sound);

            return room;
        }

        private IRoom GenerateRoom13()
        {
            string examineDescription = "At some point someone carved TLJ + CLG inside a big heart.";
            string lookDescription = "The rampart continues north and south.";
            string shortDescription = "On the rampart";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 1);

            return room;
        }

        private IRoom GenerateRoom14()
        {
            string examineDescription = "The wall to the north has begun to fall away and you can see the forest starting to grow through.";
            string lookDescription = "The rampart to the north has fallen down and is no longer passable.";
            string shortDescription = "On the rampart";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 1);

            return room;
        }

        private IRoom GenerateRoom15()
        {
            string examineDescription = "Large amounts of dust covers the ground.";
            string lookDescription = "A long dark hallway stretches to the west while the court yard opens to the east.";
            string shortDescription = "In Fort Woodbrook";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom16()
        {
            string examineDescription = "The root proves to be a small tripping hazard but could be an issue if you needed to run through here in a hurry.";
            string lookDescription = "A tree root grows up through the floor before going back down into the ground.";
            string shortDescription = "In Fort Woodbrook";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom17()
        {
            string examineDescription = "A hall";
            string lookDescription = "A hall";
            string shortDescription = "In Fort Woodbrook";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);
            room.Attributes.Add(Room.RoomAttribute.NoNPC);

            return room;
        }

        private IRoom GenerateRoom18()
        {
            string examineDescription = "A hall";
            string lookDescription = "A hall";
            string shortDescription = "In Fort Woodbrook";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom19()
        {
            string examineDescription = "A hall";
            string lookDescription = "A hall";
            string shortDescription = "In Fort Woodbrook";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom20()
        {
            string examineDescription = "Most weapons appear to be of training quality and the little that was of better quality has suffered the ravages of time and neglect.";
            string lookDescription = "While this room is filled with swords and shields there does not appear to be anything of actual use.";
            string shortDescription = "In Fort Woodbrook";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom21()
        {
            string examineDescription = "It appears over time animals have made this room a favorite sleeping area.";
            string lookDescription = "What is left of several beds litter the room.";
            string shortDescription = "In Fort Woodbrook";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom22()
        {
            string examineDescription = "A large wooden table has several small spider webs on it from lack of use.";
            string lookDescription = "A large wooden table with several scattered chairs fill this part of the room.";
            string shortDescription = "In Fort Woodbrook";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(BabySpider(room));

            return room;
        }

        private IRoom GenerateRoom23()
        {
            string examineDescription = "A large wooden table has several small spider webs on it from lack of use.";
            string lookDescription = "A large wooden table with several scattered chairs fill this part of the room.";
            string shortDescription = "In Fort Woodbrook";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(BabySpider(room));

            return room;
        }

        private IRoom GenerateRoom24()
        {
            string examineDescription = "The spider webs have become an integral part of the furniture creating an almost haunted look..";
            string lookDescription = "Spider webs cover large amounts of the chairs and table here.";
            string shortDescription = "In Fort Woodbrook";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(BabySpider(room));

            return room;
        }

        private IRoom GenerateRoom25()
        {
            string examineDescription = "A large wooden table has several small spider webs on it from lack of use.";
            string lookDescription = "A large wooden table with several scattered chairs fill this part of the room.";
            string shortDescription = "In Fort Woodbrook";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(BabySpider(room));

            return room;
        }

        private IRoom GenerateRoom26()
        {
            string examineDescription = "The spider webs have become an integral part of the furniture creating an almost haunted look..";
            string lookDescription = "Spider webs cover large amounts of the chairs and table here.";
            string shortDescription = "In Fort Woodbrook";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(BabySpider(room));

            return room;
        }

        private IRoom GenerateRoom27()
        {
            string examineDescription = "The spider webs have grown so complete that they cover the furniture like a giant white sheet.";
            string lookDescription = "The spider webs have grown thick and cover the furniture completely hiding it from view.";
            string shortDescription = "In Fort Woodbrook";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(BabySpider(room));

            return room;
        }

        private IRoom GenerateRoom28()
        {
            string examineDescription = "The spider webs have become an integral part of the furniture creating an almost haunted look..";
            string lookDescription = "Spider webs cover large amounts of the chairs and table here.";
            string shortDescription = "In Fort Woodbrook";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(BabySpider(room));

            return room;
        }

        private IRoom GenerateRoom29()
        {
            string examineDescription = "The spider webs have grown so complete that they cover the furniture like a giant white sheet.";
            string lookDescription = "The spider webs have grown thick and cover the furniture completely hiding it from view.";
            string shortDescription = "In Fort Woodbrook";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(BabySpider(room));

            return room;
        }

        private IRoom GenerateRoom30()
        {
            string examineDescription = "The spider webs criss cross into a solid web wall of web save a small opening to the north.";
            string lookDescription = "The spider webs cover the entirety of the room from the floor to the ceiling.";
            string shortDescription = "In Fort Woodbrook";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(BabySpider(room));

            return room;
        }

        private IRoom GenerateRoom31()
        {
            string examineDescription = "White silken webs are every where creating a silver shimmer ever where you look.";
            string lookDescription = "A tunnel of spider webs leads north and south.";
            string shortDescription = "In Fort Woodbrook";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);
            room.Attributes.Add(Room.RoomAttribute.NoNPC);
            room.Attributes.Add(Room.RoomAttribute.NoRecall);

            room.AddMobileObjectToRoom(GiantSpider(room));

            return room;
        }

        private IRoom GenerateRoom32()
        {
            string examineDescription = "Examining the dark spots closer reveal them to be corpses of adventures who were victims of the spider.";
            string lookDescription = "Several dark spots in the spider silk web do the walls of the room.";
            string shortDescription = "In Fort Woodbrook";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            LoadEquipment(room);

            return room;
        }



        private INonPlayerCharacter BabySpider(IRoom room)
        {
            string examineDescription = "The spider has the trade mark red hour glass indicating its a black widow.  It might be good to leave this spider alone.";
            string lookDescription = "A small spider crawls along its web.";
            string sentenceDescription = "spider";
            string shortDescription = "A small spider.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Other, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 6);
            npc.KeyWords.Add("spider");
            npc.Personalities.Add(new Wanderer());

            return npc;
        }

        private INonPlayerCharacter GiantSpider(IRoom room)
        {
            IGuard guard = new Guard(Direction.North);
            guard.GuardDirections.Add(Direction.South);
            guard.BlockLeaveMessage = "The spider webs slow you down enough that the giant spider is able to get in front of you and block your exit.";

            string examineDescription = "It is a large spider over 12 feet long.  Its eight legs seem to float along the web barley touching it and moving on without sticking.";
            string lookDescription = "A giant spider crawls along its web with venom dripping from its fangs.";
            string sentenceDescription = "spider";
            string shortDescription = "A giant spider.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Other, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 12);
            npc.KeyWords.Add("spider");
            npc.Personalities.Add(guard);

            return npc;
        }
        #endregion Rooms

        #region Equipment
        private void LoadEquipment(IRoom room)
        {
            ILoadableItems loadableRoom = (ILoadableItems)room;
            for (int level = 8; level < 15; level++)
            {
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 7, Object = Sword(level) });
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 7, Object = Spear(level) });

                IArmor armor = null!;
                string examineDescription = null;
                string lookDescription = null!;
                string sentenceDescription = null!;
                string shortDescription = null!;

                #region Arms
                lookDescription = "A silver bracer looks like it would be a nice fit and good defense.";
                shortDescription = "A silver bracer.";
                armor = Arms(level, lookDescription, shortDescription);
                armor.Material = new Silver();
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 1, Object = armor });

                lookDescription = "The bracer is thick, sturdy and leather.";
                shortDescription = "A leather bracer.";
                armor = Arms(level, lookDescription, shortDescription);
                armor.Material = new Leather();
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 3, Object = armor });

                lookDescription = "The steel bracer has a small dent in it but nothing that will affect its performance.";
                shortDescription = "A steel bracer.";
                armor = Arms(level, lookDescription, shortDescription);
                armor.Material = new Steel();
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 3, Object = armor });
                #endregion Arms

                #region Chest
                examineDescription = "The vest is a light brown color and the stitching for the padding is well hidden making it look like a regular vest.";
                lookDescription = "The padded vest is has the emblem of the GrandView Gladiator guild.";
                sentenceDescription = "padded vest";
                shortDescription = "A padded vest.";
                armor = Body(level, examineDescription, lookDescription, sentenceDescription, shortDescription);
                armor.Material = new Cloth();
                armor.KeyWords.Add("vest");
                armor.KeyWords.Add("padded");
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 1, Object = armor });

                examineDescription = "Black as night the only thing that keeps this from being a good thief's outfit is the metal studs sticking out.";
                lookDescription = "The studded leather jacket looks to be intimidating.";
                sentenceDescription = "studded leather jacket";
                shortDescription = "A leather jacket.";
                armor = Body(level, examineDescription, lookDescription, sentenceDescription, shortDescription);
                armor.Material = new Leather();
                armor.KeyWords.Add("leather");
                armor.KeyWords.Add("jacket");
                armor.KeyWords.Add("studded");
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 3, Object = armor });

                examineDescription = "The breastplate even has a belly button.  Inside the armor it has the creators initials. {FirstInitial}{SecondInitial}.";
                lookDescription = "The steel breastplate is complete with a six pack on the front.";
                sentenceDescription = "bracer";
                shortDescription = "A steel breastplate.";
                armor = Body(level, examineDescription, lookDescription, sentenceDescription, shortDescription);
                armor.Material = new Steel();
                armor.KeyWords.Add("breastplate");
                armor.FlavorOptions.Add("{FirstInitial}", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" });
                armor.FlavorOptions.Add("{SecondInitial}", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" });
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 3, Object = armor });
                #endregion Chest

                #region Feet
                examineDescription = "The slippers hum slightly but other than that appear to be normal.";
                lookDescription = "The silver slippers don't look like they would offer much protection.";
                sentenceDescription = "silver slippers";
                shortDescription = "A silver pair of ballet slippers.";
                armor = Feet(level, lookDescription, shortDescription);
                armor.LookDescription = lookDescription;
                armor.ShortDescription = shortDescription;
                armor.Material = new Silver();
                armor.KeyWords.Clear();
                armor.KeyWords.Add("silver");
                armor.KeyWords.Add("slipper");
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 1, Object = armor });

                lookDescription = "The boots are made of leather and appear to be sturdy.";
                shortDescription = "Leather boots.";
                armor = Feet(level, lookDescription, shortDescription);
                armor.Material = new Leather();
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 3, Object = armor });

                lookDescription = "The steel boots will give the user plenty of protection but will wear them out having to walking in them all day.";
                shortDescription = "A steel boot.";
                armor = Feet(level, lookDescription, shortDescription);
                armor.Material = new Steel();
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 3, Object = armor });
                #endregion Feet

                #region Hands
                lookDescription = "The gloves have a fair amount of padding to protect the users hands.";
                shortDescription = "Leather gloves.";
                armor = Hand(level, lookDescription, shortDescription);
                armor.Material = new Leather();
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 3, Object = armor });
                #endregion Hands

                #region Helmet
                lookDescription = "A golden helmet looks like it would be a nice fit and good defense.";
                shortDescription = "A gold helmet.";
                armor = Helmet(level, lookDescription, shortDescription);
                armor.Material = new Gold();
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 1, Object = armor });

                lookDescription = "While not the best defense it is better than nothing.";
                shortDescription = "A cloth helmet.";
                armor = Helmet(level, lookDescription, shortDescription);
                armor.Material = new Cloth();
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 2, Object = armor });

                lookDescription = "A fightings mans helmet.  Better than cloth and not quite as good as steel but much more affordable.";
                shortDescription = "A leather helmet.";
                armor = Helmet(level, lookDescription, shortDescription);
                armor.Material = new Leather();
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 3, Object = armor });

                lookDescription = "The helmet is made of steel a tried and true material able to take a bashing while protecting the wearer.";
                shortDescription = "A steel helmet.";
                armor = Helmet(level, lookDescription, shortDescription);
                armor.Material = new Steel();
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 3, Object = armor });
                #endregion Helmet

                #region Legs
                lookDescription = "A pair of pants that once belonged to a member of the phoenix guild.";
                shortDescription = "Leather pants.";
                armor = Leg(level, lookDescription, shortDescription);
                armor.Material = new Leather();
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 3, Object = armor });
                #endregion Legs
            }
        }

        private IWeapon Sword(int level)
        {
            string examineDescription = "The metal sword is approximately two feet long with a guard that has a slight upward curve.  The grip is {grip} and has a {pommel} for the pommel.";
            string lookDescription = "A finely polish metal sword.";
            string sentenceDescription = "sword";
            string shortDescription = "A metal sword.";

            IWeapon weapon = CreateWeapon(WeaponType.Sword, level, examineDescription, lookDescription, sentenceDescription, shortDescription);
            weapon.KeyWords.Add("sword");
            weapon.AttackerStat = Stats.Stat.Dexterity;
            weapon.DeffenderStat = Stats.Stat.Dexterity;
            weapon.FlavorOptions.Add("{grip}", new List<string>() { "wrapped in brown leather", "wrapped in black leather", "made of a textured metal", "wrapped in cord" });
            weapon.FlavorOptions.Add("{pommel}", new List<string>() { "crystal blue stone", "white iridescent opal stone", "small metal skull", "lions head", "pair of vipers wrapping around a metal hexagon striking each other", "large metal disc", "wolfs head", "dragons head" });

            IDamage damage = new Damage();
            damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(weapon.Level);
            damage.Type = Damage.DamageType.Slash;
            weapon.DamageList.Add(damage);

            return weapon;
        }

        private IWeapon Spear(int level)
        {
            string examineDescription = "The tip of the spear is a bit rusty but nothing that a little polishing wouldn't take care of.";
            string lookDescription = "A oak spear with a iron tip.";
            string sentenceDescription = "spear";
            string shortDescription = "A wooden spear.";

            IWeapon weapon = CreateWeapon(WeaponType.Spear, level, examineDescription, lookDescription, sentenceDescription, shortDescription);
            weapon.KeyWords.Add("spear");
            weapon.AttackerStat = Stats.Stat.Dexterity;
            weapon.DeffenderStat = Stats.Stat.Dexterity;

            IDamage damage = new Damage();
            damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(weapon.Level - 2);
            damage.Type = Damage.DamageType.Pierce;
            weapon.DamageList.Add(damage);
            weapon.DamageList.Add(damage);
            weapon.DamageList.Add(damage);

            return weapon;
        }

        private IArmor Arms(int level, string lookDescription, string shortDescription)
        {
            string examineDescription = "The bracer has a intricate design of intersecting lines running up and down the length of the item.";
            string sentenceDescription = "bracer";

            IArmor armor = CreateArmor(AvalableItemPosition.Arms, level, examineDescription, lookDescription, sentenceDescription, shortDescription);
            armor.KeyWords.Add("bracer");

            return armor;
        }

        private IArmor Body(int level, string examineDescription, string lookDescription, string sentenceDescription, string shortDescription)
        {
            IArmor armor = CreateArmor(AvalableItemPosition.Body, level, examineDescription, lookDescription, sentenceDescription, shortDescription);

            return armor;
        }

        private IArmor Feet(int level, string lookDescription, string shortDescription)
        {
            string examineDescription = "The boots still have some cob webs on them but otherwise look to be in good shape.";
            string sentenceDescription = "boots";

            IArmor armor = CreateArmor(AvalableItemPosition.Feet, level, examineDescription, lookDescription, sentenceDescription, shortDescription);

            armor.KeyWords.Add("boots");

            return armor;
        }

        private IArmor Hand(int level, string lookDescription, string shortDescription)
        {
            string examineDescription = "The leather gloves have an emblem of a burning phoenix stamped into the back of each glove.";
            string sentenceDescription = "leather";

            IArmor armor = CreateArmor(AvalableItemPosition.Hand, level, examineDescription, lookDescription, sentenceDescription, shortDescription);

            armor.KeyWords.Add("leather");
            armor.KeyWords.Add("glove");

            return armor;
        }

        private IArmor Helmet(int level, string lookDescription, string shortDescription)
        {
            string examineDescription = "The helmet's insides are padded with soft animal fur.";
            string sentenceDescription = "helmet";

            IArmor armor = CreateArmor(AvalableItemPosition.Head, level, examineDescription, lookDescription, sentenceDescription, shortDescription);

            armor.KeyWords.Add("helmet");

            return armor;
        }

        private IArmor Leg(int level, string lookDescription, string shortDescription)
        {
            string examineDescription = "The leather the design of fire going up each leg.";
            string sentenceDescription = "pants";

            IArmor armor = CreateArmor(AvalableItemPosition.Legs, level, examineDescription, lookDescription, sentenceDescription, shortDescription);
            armor.KeyWords.Add("leather");
            armor.KeyWords.Add("pants");
            armor.KeyWords.Add("pant");

            return armor;
        }
        #endregion Equipment

        private void ConnectRooms()
        {
            ZoneHelper.ConnectZone(Zone.Rooms[1], Direction.South, 8, 36);

            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.North, Zone.Rooms[2]);
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.North, Zone.Rooms[7]);
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.East, Zone.Rooms[3]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.North, Zone.Rooms[4]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.Up, Zone.Rooms[11]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.North, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[6], Direction.East, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[7], Direction.North, Zone.Rooms[6]);
            ZoneHelper.ConnectRoom(Zone.Rooms[7], Direction.East, Zone.Rooms[4]);
            ZoneHelper.ConnectRoom(Zone.Rooms[8], Direction.East, Zone.Rooms[6]);
            ZoneHelper.ConnectRoom(Zone.Rooms[9], Direction.North, Zone.Rooms[8]);
            ZoneHelper.ConnectRoom(Zone.Rooms[9], Direction.East, Zone.Rooms[7]);
            ZoneHelper.ConnectRoom(Zone.Rooms[10], Direction.North, Zone.Rooms[9]);
            ZoneHelper.ConnectRoom(Zone.Rooms[10], Direction.East, Zone.Rooms[2]);
            ZoneHelper.ConnectRoom(Zone.Rooms[11], Direction.East, Zone.Rooms[12]);
            ZoneHelper.ConnectRoom(Zone.Rooms[12], Direction.North, Zone.Rooms[13]);
            ZoneHelper.ConnectRoom(Zone.Rooms[13], Direction.North, Zone.Rooms[14]);
            ZoneHelper.ConnectRoom(Zone.Rooms[15], Direction.East, Zone.Rooms[9]);
            ZoneHelper.ConnectRoom(Zone.Rooms[16], Direction.North, Zone.Rooms[21]);
            ZoneHelper.ConnectRoom(Zone.Rooms[16], Direction.East, Zone.Rooms[15]);
            ZoneHelper.ConnectRoom(Zone.Rooms[17], Direction.East, Zone.Rooms[16]);
            ZoneHelper.ConnectRoom(Zone.Rooms[18], Direction.North, Zone.Rooms[17]);
            ZoneHelper.ConnectRoom(Zone.Rooms[19], Direction.North, Zone.Rooms[18]);
            ZoneHelper.ConnectRoom(Zone.Rooms[20], Direction.East, Zone.Rooms[19]);
            ZoneHelper.ConnectRoom(Zone.Rooms[22], Direction.North, Zone.Rooms[25]);
            ZoneHelper.ConnectRoom(Zone.Rooms[22], Direction.East, Zone.Rooms[17]);
            ZoneHelper.ConnectRoom(Zone.Rooms[23], Direction.North, Zone.Rooms[26]);
            ZoneHelper.ConnectRoom(Zone.Rooms[23], Direction.East, Zone.Rooms[22]);
            ZoneHelper.ConnectRoom(Zone.Rooms[24], Direction.North, Zone.Rooms[27]);
            ZoneHelper.ConnectRoom(Zone.Rooms[24], Direction.East, Zone.Rooms[23]);
            ZoneHelper.ConnectRoom(Zone.Rooms[25], Direction.North, Zone.Rooms[28]);
            ZoneHelper.ConnectRoom(Zone.Rooms[26], Direction.North, Zone.Rooms[29]);
            ZoneHelper.ConnectRoom(Zone.Rooms[26], Direction.East, Zone.Rooms[25]);
            ZoneHelper.ConnectRoom(Zone.Rooms[27], Direction.North, Zone.Rooms[30]);
            ZoneHelper.ConnectRoom(Zone.Rooms[27], Direction.East, Zone.Rooms[26]);
            ZoneHelper.ConnectRoom(Zone.Rooms[29], Direction.East, Zone.Rooms[28]);
            ZoneHelper.ConnectRoom(Zone.Rooms[30], Direction.North, Zone.Rooms[31]);
            ZoneHelper.ConnectRoom(Zone.Rooms[30], Direction.East, Zone.Rooms[29]);
            ZoneHelper.ConnectRoom(Zone.Rooms[31], Direction.North, Zone.Rooms[32]);
        }
    }
}

