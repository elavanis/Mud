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
    public class Woodbrook : BaseZone, IZoneCode
    {

        public Woodbrook() : base(10)
        {
        }

        public IZone Generate()
        {
            Zone.InGameDaysTillReset = 10;
            Zone.Name = nameof(Woodbrook);

            BuildRoomsViaReflection(this.GetType());

            ConnectRooms();

            return Zone;
        }

        #region Rooms
        private IRoom GenerateRoom1()
        {
            IRoom room = OutdoorRoom(1);
            room.ExamineDescription = "While the wooden gates are still strong they have fallen off the hinges and lay strewn on ground.";
            room.LookDescription = "Both sets of gates have fallen off the hinges and allow access to the fort.";
            room.ShortDescription = "Entrance to the fort";

            return room;
        }

        private IRoom GenerateRoom2()
        {
            IRoom room = OutdoorRoom(1);
            room.ExamineDescription = "To the north a collapsed building has fallen down.  To the east and west are stairs leading to the ramparts.";
            room.LookDescription = "The court yard is overgrown with grass and small trees.";
            room.ShortDescription = "Inside the court yard";

            return room;
        }

        private IRoom GenerateRoom3()
        {
            IRoom room = OutdoorRoom(1);
            room.ExamineDescription = "The stairs are worn but look like they should hold your weight.";
            room.LookDescription = "Wooden stairs lead up to the rampart.";
            room.ShortDescription = "Inside the court yard";

            return room;
        }

        private IRoom GenerateRoom4()
        {
            IRoom room = OutdoorRoom(1);
            room.ExamineDescription = "If the well had a cover it is long gone but there is still water in the well.  If only the bucket was around you could get a drink.";
            room.LookDescription = "A small stone well dominates the area here.";
            room.ShortDescription = "Inside the court yard";

            return room;
        }

        private IRoom GenerateRoom5()
        {
            IRoom room = OutdoorRoom(1);
            room.ExamineDescription = "The part of the eastern wall has started to collapse allowing you to see out into the forest.";
            room.LookDescription = "Thick grass has grown up obscuring what this part of the court yard might have been used for.";
            room.ShortDescription = "Inside the court yard";

            return room;
        }

        private IRoom GenerateRoom6()
        {
            IRoom room = OutdoorRoom(1);
            room.ExamineDescription = "It looks like this might have been stalls for horses but the building has long since collapsed.";
            room.LookDescription = "A collapsed building lays before you.";
            room.ShortDescription = "Inside the court yard";

            return room;
        }

        private IRoom GenerateRoom7()
        {
            IRoom room = OutdoorRoom(1);
            room.ExamineDescription = "A several trees have started to grow up pushing the stone pavers out of the way.";
            room.LookDescription = "You stand in what was once the center of the court yard.";
            room.ShortDescription = "Inside the court yard";

            return room;
        }

        private IRoom GenerateRoom8()
        {
            IRoom room = OutdoorRoom(1);
            room.ExamineDescription = "Peeking into the building you can see part of a shelf on the right but not any further as the walls have collapsed.";
            room.LookDescription = "A small building such as a shed stands to the north.";
            room.ShortDescription = "Inside the court yard";

            return room;
        }

        private IRoom GenerateRoom9()
        {
            IRoom room = OutdoorRoom(1);
            room.ExamineDescription = "The door to the building is ajar and hangs only by one nail that refuses to give up.";
            room.LookDescription = "The main building in the fort is to the west while the court yard opens to the east.";
            room.ShortDescription = "Inside the court yard";

            return room;
        }

        private IRoom GenerateRoom10()
        {
            IRoom room = OutdoorRoom(1);
            room.ExamineDescription = "The stairs have long been neglected and have succumb to mother nature and father time.";
            room.LookDescription = "The stairs leading up to the ramparts have collapsed.";
            room.ShortDescription = "Inside the court yard";

            return room;
        }

        private IRoom GenerateRoom11()
        {
            IRoom room = OutdoorRoom(1);
            room.ExamineDescription = "The forest would have been cut back to allow lookouts to see approaching people sooner. How much is hard to say though.";
            room.LookDescription = "The top of the rampart you are able to see the court yard below.  This would have given watchers a good view point of the forest below as well as the court yard.";
            room.ShortDescription = "On the rampart";

            return room;
        }

        private IRoom GenerateRoom12()
        {
            IRoom room = OutdoorRoom(1);
            room.ExamineDescription = "The whole fort seems to let out a low creak as you walk on this part of the rampart.";
            room.LookDescription = "As you step on to this part of the rampart it lets out a grown in protest at your presence.";
            room.ShortDescription = "On the rampart";

            ISound sound = new Sound();
            sound.SoundName = $"{Zone.Name}\\WooodenCreak.mp3";
            room.Sounds.Add(sound);

            return room;
        }

        private IRoom GenerateRoom13()
        {
            IRoom room = OutdoorRoom(1);
            room.ExamineDescription = "At some point someone carved TLJ + CLG inside a big heart.";
            room.LookDescription = "The rampart continues north and south.";
            room.ShortDescription = "On the rampart";

            return room;
        }

        private IRoom GenerateRoom14()
        {
            IRoom room = OutdoorRoom(1);
            room.ExamineDescription = "The wall to the north has begun to fall away and you can see the forest starting to grow through.";
            room.LookDescription = "The rampart to the north has fallen down and is no longer passable.";
            room.ShortDescription = "On the rampart";

            return room;
        }

        private IRoom GenerateRoom15()
        {
            IRoom room = IndoorRoomNoLight();
            room.ExamineDescription = "Large amounts of dust covers the ground.";
            room.LookDescription = "A long dark hallway stretches to the west while the court yard opens to the east.";
            room.ShortDescription = "In Fort Woodbrook";

            return room;
        }

        private IRoom GenerateRoom16()
        {
            IRoom room = IndoorRoomNoLight();
            room.ExamineDescription = "The root proves to be a small tripping hazard but could be an issue if you needed to run through here in a hurry.";
            room.LookDescription = "A tree root grows up through the floor before going back down into the ground.";
            room.ShortDescription = "In Fort Woodbrook";

            return room;
        }

        private IRoom GenerateRoom17()
        {
            IRoom room = IndoorRoomNoLight();
            room.ExamineDescription = "A hall";
            room.LookDescription = "A hall";
            room.ShortDescription = "In Fort Woodbrook";
            room.Attributes.Add(Room.RoomAttribute.NoNPC);

            return room;
        }

        private IRoom GenerateRoom18()
        {
            IRoom room = IndoorRoomNoLight();
            room.ExamineDescription = "A hall";
            room.LookDescription = "A hall";
            room.ShortDescription = "In Fort Woodbrook";

            return room;
        }

        private IRoom GenerateRoom19()
        {
            IRoom room = IndoorRoomNoLight();
            room.ExamineDescription = "A hall";
            room.LookDescription = "A hall";
            room.ShortDescription = "In Fort Woodbrook";

            return room;
        }

        private IRoom GenerateRoom20()
        {
            IRoom room = IndoorRoomNoLight();
            room.ExamineDescription = "Most weapons appear to be of training quality and the little that was of better quality has suffered the ravages of time and neglect.";
            room.LookDescription = "While this room is filled with swords and shields there does not appear to be anything of actual use.";
            room.ShortDescription = "In Fort Woodbrook";

            return room;
        }

        private IRoom GenerateRoom21()
        {
            IRoom room = IndoorRoomNoLight();
            room.ExamineDescription = "It appears over time animals have made this room a favorite sleeping area.";
            room.LookDescription = "What is left of several beds litter the room.";
            room.ShortDescription = "In Fort Woodbrook";

            return room;
        }

        private IRoom GenerateRoom22()
        {
            IRoom room = IndoorRoomNoLight();
            room.ExamineDescription = "A large wooden table has several small spider webs on it from lack of use.";
            room.LookDescription = "A large wooden table with several scattered chairs fill this part of the room.";
            room.ShortDescription = "In Fort Woodbrook";

            room.AddMobileObjectToRoom(BabySpider());

            return room;
        }

        private IRoom GenerateRoom23()
        {
            IRoom room = IndoorRoomNoLight();
            room.ExamineDescription = "A large wooden table has several small spider webs on it from lack of use.";
            room.LookDescription = "A large wooden table with several scattered chairs fill this part of the room.";
            room.ShortDescription = "In Fort Woodbrook";

            room.AddMobileObjectToRoom(BabySpider());

            return room;
        }

        private IRoom GenerateRoom24()
        {
            IRoom room = IndoorRoomNoLight();
            room.ExamineDescription = "The spider webs have become an integral part of the furniture creating an almost haunted look..";
            room.LookDescription = "Spider webs cover large amounts of the chairs and table here.";
            room.ShortDescription = "In Fort Woodbrook";

            room.AddMobileObjectToRoom(BabySpider());

            return room;
        }

        private IRoom GenerateRoom25()
        {
            IRoom room = IndoorRoomNoLight();
            room.ExamineDescription = "A large wooden table has several small spider webs on it from lack of use.";
            room.LookDescription = "A large wooden table with several scattered chairs fill this part of the room.";
            room.ShortDescription = "In Fort Woodbrook";

            room.AddMobileObjectToRoom(BabySpider());

            return room;
        }

        private IRoom GenerateRoom26()
        {
            IRoom room = IndoorRoomNoLight();
            room.ExamineDescription = "The spider webs have become an integral part of the furniture creating an almost haunted look..";
            room.LookDescription = "Spider webs cover large amounts of the chairs and table here.";
            room.ShortDescription = "In Fort Woodbrook";

            room.AddMobileObjectToRoom(BabySpider());

            return room;
        }

        private IRoom GenerateRoom27()
        {
            IRoom room = IndoorRoomNoLight();
            room.ExamineDescription = "The spider webs have grown so complete that they cover the furniture like a giant white sheet.";
            room.LookDescription = "The spider webs have grown thick and cover the furniture completely hiding it from view.";
            room.ShortDescription = "In Fort Woodbrook";

            room.AddMobileObjectToRoom(BabySpider());

            return room;
        }

        private IRoom GenerateRoom28()
        {
            IRoom room = IndoorRoomNoLight();
            room.ExamineDescription = "The spider webs have become an integral part of the furniture creating an almost haunted look..";
            room.LookDescription = "Spider webs cover large amounts of the chairs and table here.";
            room.ShortDescription = "In Fort Woodbrook";

            room.AddMobileObjectToRoom(BabySpider());

            return room;
        }

        private IRoom GenerateRoom29()
        {
            IRoom room = IndoorRoomNoLight();
            room.ExamineDescription = "The spider webs have grown so complete that they cover the furniture like a giant white sheet.";
            room.LookDescription = "The spider webs have grown thick and cover the furniture completely hiding it from view.";
            room.ShortDescription = "In Fort Woodbrook";

            room.AddMobileObjectToRoom(BabySpider());

            return room;
        }

        private IRoom GenerateRoom30()
        {
            IRoom room = IndoorRoomNoLight();
            room.ExamineDescription = "The spider webs criss cross into a solid web wall of web save a small opening to the north.";
            room.LookDescription = "The spider webs cover the entirety of the room from the floor to the ceiling.";
            room.ShortDescription = "In Fort Woodbrook";

            room.AddMobileObjectToRoom(BabySpider());

            return room;
        }

        private IRoom GenerateRoom31()
        {
            IRoom room = IndoorRoomNoLight();
            room.ExamineDescription = "White silken webs are every where creating a silver shimmer ever where you look.";
            room.LookDescription = "A tunnel of spider webs leads north and south.";
            room.ShortDescription = "In Fort Woodbrook";
            room.Attributes.Add(Room.RoomAttribute.NoNPC);
            room.Attributes.Add(Room.RoomAttribute.NoRecall);

            room.AddMobileObjectToRoom(GiantSpider());

            return room;
        }

        private IRoom GenerateRoom32()
        {
            IRoom room = IndoorRoomNoLight();
            room.ExamineDescription = "Examining the dark spots closer reveal them to be corpses of adventures who were victims of the spider.";
            room.LookDescription = "Several dark spots in the spider silk web do the walls of the room.";
            room.ShortDescription = "In Fort Woodbrook";

            LoadEquipment(room);

            return room;
        }



        private INonPlayerCharacter BabySpider()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Other, 6);
            npc.KeyWords.Add("spider");
            npc.Personalities.Add(new Wanderer());
            npc.SentenceDescription = "spider";
            npc.ShortDescription = "A small spider.";
            npc.LookDescription = "A small spider crawls along its web.";
            npc.ExamineDescription = "The spider has the trade mark red hour glass indicating its a black widow.  It might be good to leave this spider alone.";

            return npc;
        }

        private INonPlayerCharacter GiantSpider()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Other, 12);
            npc.KeyWords.Add("spider");

            IGuard guard = new Guard(Direction.North);
            guard.GuardDirections.Add(Direction.South);
            guard.BlockLeaveMessage = "The spider webs slow you down enough that the giant spider is able to get in front of you and block your exit.";

            npc.Personalities.Add(guard);
            npc.SentenceDescription = "spider";
            npc.ShortDescription = "A giant spider.";
            npc.LookDescription = "A giant spider crawls along its web with venom dripping from its fangs.";
            npc.ExamineDescription = "It is a large spider over 12 feet long.  Its eight legs seem to float along the web barley touching it and moving on without sticking.";

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

                IArmor armor = null;

                #region Arms
                armor = Arms(level);
                armor.Material = new Silver();
                armor.ShortDescription = "A silver bracer.";
                armor.LookDescription = "A silver bracer looks like it would be a nice fit and good defense.";
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 1, Object = armor });

                armor = Arms(level);
                armor.Material = new Leather();
                armor.ShortDescription = "A leather bracer.";
                armor.LookDescription = "The bracer is thick, sturdy and leather.";
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 3, Object = armor });

                armor = Arms(level);
                armor.Material = new Steel();
                armor.ShortDescription = "A steel bracer.";
                armor.LookDescription = "The steel bracer has a small dent in it but nothing that will affect its performance.";
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 3, Object = armor });
                #endregion Arms

                #region Chest
                armor = Body(level);
                armor.Material = new Cloth();
                armor.ShortDescription = "A padded vest.";
                armor.LookDescription = "The padded vest is has the emblem of the GrandView Gladiator guild.";
                armor.ExamineDescription = "The vest is a light brown color and the stitching for the padding is well hidden making it look like a regular vest.";
                armor.SentenceDescription = "padded vest";
                armor.KeyWords.Add("vest");
                armor.KeyWords.Add("padded");
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 1, Object = armor });

                armor = Body(level);
                armor.Material = new Leather();
                armor.ShortDescription = "A leather jacket.";
                armor.LookDescription = "The studded leather jacket looks to be intimidating.";
                armor.ExamineDescription = "Black as night the only thing that keeps this from being a good thief's outfit is the metal studs sticking out.";
                armor.SentenceDescription = "studded leather jacket";
                armor.KeyWords.Add("leather");
                armor.KeyWords.Add("jacket");
                armor.KeyWords.Add("studded");
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 3, Object = armor });

                armor = Body(level);
                armor.Material = new Steel();
                armor.ShortDescription = "A steel breastplate.";
                armor.LookDescription = "The steel breastplate is complete with a six pack on the front.";
                armor.ExamineDescription = "The breastplate even has a belly button.  Inside the armor it has the creators initials. {FirstInitial}{SecondInitial}.";
                armor.SentenceDescription = "bracer";
                armor.KeyWords.Add("breastplate");
                armor.FlavorOptions.Add("{FirstInitial}", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" });
                armor.FlavorOptions.Add("{SecondInitial}", new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" });
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 3, Object = armor });
                #endregion Chest

                #region Feet
                armor = Feet(level);
                armor.Material = new Silver();
                armor.KeyWords.Clear();
                armor.ShortDescription = "A silver pair of ballet slippers.";
                armor.LookDescription = "The silver slippers don't look like they would offer much protection.";
                armor.ExamineDescription = "The slippers hum slightly but other than that appear to be normal.";
                armor.SentenceDescription = "silver slippers";
                armor.KeyWords.Add("silver");
                armor.KeyWords.Add("slipper");
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 1, Object = armor });

                armor = Feet(level);
                armor.Material = new Leather();
                armor.ShortDescription = "Leather boots.";
                armor.LookDescription = "The boots are made of leather and appear to be sturdy.";
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 3, Object = armor });

                armor = Feet(level);
                armor.Material = new Steel();
                armor.ShortDescription = "A steel boot.";
                armor.LookDescription = "The steel boots will give the user plenty of protection but will wear them out having to walking in them all day.";
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 3, Object = armor });
                #endregion Feet

                #region Hands
                armor = Hand(level);
                armor.Material = new Leather();
                armor.ShortDescription = "Leather gloves.";
                armor.LookDescription = "The gloves have a fair amount of padding to protect the users hands.";
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 3, Object = armor });
                #endregion Hands

                #region Helmet
                armor = Helmet(level);
                armor.Material = new Gold();
                armor.ShortDescription = "A gold helmet.";
                armor.LookDescription = "A golden helmet looks like it would be a nice fit and good defense.";
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 1, Object = armor });

                armor = Helmet(level);
                armor.Material = new Cloth();
                armor.ShortDescription = "A cloth helmet.";
                armor.LookDescription = "While not the best defense it is better than nothing.";
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 2, Object = armor });

                armor = Helmet(level);
                armor.Material = new Leather();
                armor.ShortDescription = "A leather helmet.";
                armor.LookDescription = "A fightings mans helmet.  Better than cloth and not quite as good as steel but much more affordable.";
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 3, Object = armor });

                armor = Helmet(level);
                armor.Material = new Steel();
                armor.ShortDescription = "A steel helmet.";
                armor.LookDescription = "The helmet is made of steel a tried and true material able to take a bashing while protecting the wearer.";
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 3, Object = armor });
                #endregion Helmet

                #region Legs
                armor = Leg(level);
                armor.Material = new Leather();
                armor.ShortDescription = "Leather pants.";
                armor.LookDescription = "A pair of pants that once belonged to a member of the phoenix guild.";
                loadableRoom.LoadableItems.Add(new LoadPercentage() { PercentageLoad = 3, Object = armor });
                #endregion Legs
            }
        }

        private IWeapon Sword(int level)
        {
            IWeapon weapon = CreateWeapon(WeaponType.Sword, level);
            weapon.ExamineDescription = "The metal sword is approximately two feet long with a guard that has a slight upward curve.  The grip is {grip} and has a {pommel} for the pommel.";
            weapon.LookDescription = "A finely polish metal sword.";
            weapon.ShortDescription = "A metal sword.";
            weapon.SentenceDescription = "sword";
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
            IWeapon weapon = CreateWeapon(WeaponType.Spear, level);
            weapon.ExamineDescription = "The tip of the spear is a bit rusty but nothing that a little polishing wouldn't take care of.";
            weapon.LookDescription = "A oak spear with a iron tip.";
            weapon.ShortDescription = "A wooden spear.";
            weapon.SentenceDescription = "spear";
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

        private IArmor Arms(int level)
        {
            IArmor armor = CreateArmor(AvalableItemPosition.Arms, level);
            armor.ExamineDescription = "The bracer has a intricate design of intersecting lines running up and down the length of the item.";
            armor.SentenceDescription = "bracer";
            armor.KeyWords.Add("bracer");

            return armor;
        }

        private IArmor Body(int level)
        {
            IArmor armor = CreateArmor(AvalableItemPosition.Body, level);

            return armor;
        }

        private IArmor Feet(int level)
        {
            IArmor armor = CreateArmor(AvalableItemPosition.Feet, level);
            armor.ExamineDescription = "The boots still have some cob webs on them but otherwise look to be in good shape.";
            armor.SentenceDescription = "boots";
            armor.KeyWords.Add("boots");

            return armor;
        }

        private IArmor Hand(int level)
        {
            IArmor armor = CreateArmor(AvalableItemPosition.Hand, level);
            armor.ExamineDescription = "The leather gloves have an emblem of a burning phoenix stamped into the back of each glove.";
            armor.SentenceDescription = "leather";
            armor.KeyWords.Add("leather");
            armor.KeyWords.Add("glove");

            return armor;
        }

        private IArmor Helmet(int level)
        {
            IArmor armor = CreateArmor(AvalableItemPosition.Head, level);
            armor.ExamineDescription = "The helmet's insides are padded with soft animal fur.";
            armor.SentenceDescription = "helmet";
            armor.KeyWords.Add("helmet");

            return armor;
        }

        private IArmor Leg(int level)
        {
            IArmor armor = CreateArmor(AvalableItemPosition.Legs, level);
            armor.ExamineDescription = "The leather the design of fire going up each leg.";
            armor.SentenceDescription = "pants";
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

