using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Objects.Global;
using Objects.Item;
using Objects.Item.Interface;
using Objects.Item.Items;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Personalities;
using Objects.Personality.Personalities.Interface;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using Shared.Sound;
using Shared.Sound.Interface;
using static Objects.Global.Direction.Directions;
using static Objects.Item.Item;
using static Objects.Mob.NonPlayerCharacter;

namespace GenerateZones.Zones.GrandView
{
    public class GrandViewStreets : BaseZone, IZoneCode
    {
        public GrandViewStreets() : base(5)
        {
        }

        public IZone Generate()
        {
            Zone.InGameDaysTillReset = 1;
            Zone.Name = nameof(GrandViewStreets);

            int methodCount = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Count();
            for (int i = 1; i <= methodCount; i++)
            {
                string methodName = "GenerateRoom" + i;
                MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
                if (method != null)
                {
                    Room room = (Room)method.Invoke(this, null);
                    room.Zone = Zone.Id;
                    ZoneHelper.AddRoom(Zone, room);
                }
            }

            ConnectRooms();

            return Zone;
        }

        #region Rooms
        #region City
        private IRoom GenerateRoom1()
        {

            IRoom room = CreateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "A huge ancient oak tree about 10 feet in diameter creates the perfect blind for entering or leaving the tunnel below.  Other than that it would also be a nice place for a picnic.";
            room.LookDescription = "A huge ancient oak tree hides a hole in the ground from the street.";
            room.ShortDescription = "Behind a large tree";

            //string message = "Perform the following commands." + Environment.NewLine + "NORTH";
            //room.Enchantments.Add(EnterRoomMessage(message, TagWrapper.TagType.Info));

            return room;
        }

        private IRoom GenerateRoom2()
        {

            IRoom room = CreateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "Upon closer examination you discover a faint path between the street and the oak tree.  You are not the first person to come this way.";
            room.LookDescription = "You stand in a grassy field half way between the street and a huge ancient oak tree.";
            room.ShortDescription = "A grassy field";

            //string message = "You have made it out of the dungeon and stand in the center of a park.  If you wish to continue to learn how to play continue north then to the east.  If at any time you get need to look up how to use a command use the MAN command.";
            //room.Enchantments.Add(EnterRoomMessage(message, TagWrapper.TagType.Info));

            return room;
        }

        private IRoom GenerateRoom3()
        {

            IRoom room = CreateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "The road appears to be of good quality.  No weeds, well drained and smooth stone.  To the south is a field with a small hill and huge tree.  To the north is a pond with some ducks swimming by.  To the east you see a large dome and the west leads down to the heart of the city.";
            room.LookDescription = "The road runs east and west here and a field extends to the south with a huge oak tree dominating a small hill.";
            room.ShortDescription = "A street";

            return room;
        }

        private IRoom GenerateRoom4()
        {

            IRoom room = CreateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = string.Format("The \"Grandview Meadows\" park lies to your west.  To call it a Grandview or a Meadow would be stretching the definition of either but hey, no one consulted you when the named the place.  Maybe before the city was here it was a meadow.  To the north is the stairwell leading to the giant dome which some would call the jewel of the city.  The stairwell is made of stone and consists of {0} steps, one for each God.", GlobalReference.GlobalValues.GameDateTime.YearNames.Count);
            room.LookDescription = "An arched sign extends over the street here and reads \"Grandview Meadows.\"";
            room.ShortDescription = "Entrance to the park";

            room.Items.Add(LionStatue());
            room.Items.Add(LionStatue());

            return room;
        }
        private IItem LionStatue()
        {
            IItem statue = CreateItem<IItem>();
            statue.Attributes.Add(Item.ItemAttribute.NoGet);
            statue.KeyWords.Add("Lion");
            statue.KeyWords.Add("statue");

            statue.ShortDescription = "A lion statue.";
            statue.LookDescription = "The steps are flanked on each side by a pair of black marble lions which seem to almost come to life.";
            statue.ExamineDescription = "Each lion is in a sitting position with one paw holding a shield with the cities crest upright.";
            statue.SentenceDescription = "statue";

            return statue;
        }

        private IRoom GenerateRoom5()
        {

            IRoom room = CreateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "White paving stone extend outward from the center of the dome in a circular pattern.";
            room.LookDescription = "A paved court yard surrounds the giant dome to the north.";
            room.ShortDescription = "South of the dome";

            room.Items.Add(Room13_Fountain());
            room.Sounds.Add(FountainSound());

            return room;
        }

        private ISound FountainSound()
        {
            ISound sound = new Sound();
            sound.Loop = true;
            sound.SoundName = string.Format("{0}\\{1}", Zone.Name, "Fountain.mp3");
            return sound;
        }

        private IItem Room13_Fountain()
        {
            IItem fountain = CreateItem<Fountain>();

            fountain.ShortDescription = "An ornate fountain.";
            fountain.LookDescription = "A large ornate fountain pours water down several tiers.";
            fountain.ExamineDescription = "The fountain is made of blue glass with four tiers.  The first three tiers flow down into the 4th tier and the bottom pool shoots water up into the 4th tier.  The blue color causes the fountain to blend into the water making the whole thing appear as one big moving fountain of water.";
            fountain.SentenceDescription = "fountain";

            return fountain;
        }

        private IRoom GenerateRoom6()
        {

            IRoom room = CreateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "White paving stone extend outward from the center of the dome in a circular pattern.";
            room.LookDescription = "A paved court yard surrounds the giant dome to the north west.";
            room.ShortDescription = "South east of the dome";

            return room;
        }

        private IRoom GenerateRoom7()
        {

            IRoom room = CreateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "White paving stone extend outward from the center of the dome in a circular pattern.";
            room.LookDescription = "A paved court yard surrounds the giant dome to the west.";
            room.ShortDescription = "East of the dome";

            room.Items.Add(Room13_Fountain());
            room.Sounds.Add(FountainSound());

            return room;
        }

        private IRoom GenerateRoom8()
        {

            IRoom room = CreateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "White paving stone extend outward from the center of the dome in a circular pattern.";
            room.LookDescription = "A paved court yard surrounds the giant dome to the south west.";
            room.ShortDescription = "North east of the dome";

            return room;
        }

        private IRoom GenerateRoom9()
        {

            IRoom room = CreateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "White paving stone extend outward from the center of the dome in a circular pattern.";
            room.LookDescription = "A paved court yard surrounds the giant dome to the north.";
            room.ShortDescription = "North of the dome";

            room.Items.Add(Room13_Fountain());
            room.Sounds.Add(FountainSound());

            return room;
        }

        private IRoom GenerateRoom10()
        {

            IRoom room = CreateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "White paving stone extend outward from the center of the dome in a circular pattern.";
            room.LookDescription = "A paved court yard surrounds the giant dome to the south east.";
            room.ShortDescription = "North west of the dome";

            return room;
        }

        private IRoom GenerateRoom11()
        {

            IRoom room = CreateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "White paving stone extend outward from the center of the dome in a circular pattern.";
            room.LookDescription = "A paved court yard surrounds the giant dome to the east.";
            room.ShortDescription = "West of the dome";

            room.Items.Add(Room13_Fountain());
            room.Sounds.Add(FountainSound());

            return room;
        }

        private IRoom GenerateRoom12()
        {

            IRoom room = CreateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "White paving stone extend outward from the center of the dome in a circular pattern.";
            room.LookDescription = "A paved court yard surrounds the giant dome to the north east.";
            room.ShortDescription = "South west of the dome";

            return room;
        }

        private IRoom GenerateRoom13()
        {

            IRoom room = CreateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "The dome appears to 50 feet across and 30 to 40 feet across.  There are 8 pillars 5 feet in diameter made of marble holding up the dome.  Each pillar is made of either white marble or black marble in alternating fashion.  The light dancing through the dome cast interesting patterns on the white cobble stone.  Directly under the center of the dome is a large circular stone that could be used for sitting.  In the center of the stone is an inscription. \"Built 02/14/0024 by Charon\"";
            room.LookDescription = "A giant stained glass dome stands 30 to 40 feet above you.  The light shining through the dome cast interesting patterns on the cobble stone floor causing you to admire its beauty for a moment.";
            room.ShortDescription = "Under the stained glass dome";

            room.Items.Add(Room21_Sign());
            IRecallBeacon beacon = CreateItem<IRecallBeacon>();
            room.Items.Add(beacon);
            return room;
        }

        private IItem Room21_Sign()
        {
            IItem item = CreateItem<IItem>();
            item.Attributes.Add(ItemAttribute.NoGet);
            item.ExamineDescription = "The sign appears to be made of black marble with the words carved out and filled with a silvery metal so they appear to almost shine.  The frame is 1.5 inches and is made of African Blackwood.  All in all the sign creates a striking contrast against the white pillar on which it hangs.";
            item.KeyWords.Add("sign");
            item.LookDescription = @"North: The Great Library
East: The Training Hall";
            item.SentenceDescription = "sign";
            item.ShortDescription = "A sign hangs on a pillar.";

            return item;
        }

        private IRoom GenerateRoom14()
        {

            IRoom room = CreateRoom(5);
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "A stone fence about 3 feet tall line either side of the road here. Cherry blossoms can be seen growing on either side of the fence on the large estates.";
            room.LookDescription = "Two sprawling estates line the road here.";
            room.ShortDescription = "South Rockdale Avenue";
            return room;
        }

        private IRoom GenerateRoom15()
        {

            IRoom room = CreateRoom(5);
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "The stone fence continue north and south along the road.  On either side are the two largest estates in the city.";
            room.LookDescription = "The two largest estates in the city are to the east and the west of here.";
            room.ShortDescription = "Rockdale Avenue";
            return room;
        }

        private IRoom GenerateRoom16()
        {

            IRoom room = CreateRoom(5);
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "A stone fence about 3 feet tall line either side of the road here. Pine trees dot the country side on either side of the road.";
            room.LookDescription = "Two sprawling estates line the road here.";
            room.ShortDescription = "North Rockdale Avenue";
            return room;
        }

        private IRoom GenerateRoom17()
        {

            IRoom room = CreateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "To the north stands the city's library.  Seven five story tall pillars support the roof to this massive structure.  In the center of the library is a large golden dome with windows cut out for viewing the city.";
            room.LookDescription = "The city's library containing the public records, works of fiction and great books of magic stands to the north.  To the south lies Rockdale Avenue.";
            room.ShortDescription = "Before the great library";
            return room;
        }


        #endregion City

        #region City Pt2
        private IRoom GenerateRoom18()
        {

            IRoom room = CreateRoom(5);
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "Off in the distance to the east you can make out a coliseum and to the west is the great dome.  Shops and houses line the road to the north and south.";
            room.LookDescription = "The cobble stones here are quite worn indicating this road it quite popular.";
            room.ShortDescription = "A wide road";

            return room;
        }

        private IRoom GenerateRoom19()
        {

            IRoom room = CreateRoom(5);
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "You stand about half way between the giant dome and the coliseum. Even though the coliseum is still a far ways off it presence looms far above you.";
            room.LookDescription = "The road continues east and west for a long ways.  The shops and houses have given way to a small park.";
            room.ShortDescription = "A wide road";

            return room;
        }
        #endregion City Pt2

        #region Duck Pond
        private IRoom GenerateRoom20()
        {

            IRoom room = CreateRoom(5);
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "Blue water and green lily pads mix to form turquoise reflections that dance on the water.";
            room.LookDescription = "Lily pads float on the surface of the pond to the north creating a checker board effect.";
            room.ShortDescription = "Duck Pond";

            return room;
        }

        private IRoom GenerateRoom21()
        {

            IRoom room = CreateRoom(5);
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "A small tree lays in the stream.  One end was gnawed off by a beaver but it looks like it was unable to pull it back up the bank to be taken to its damn.";
            room.LookDescription = "A small stream of water creating a soothing sound as it babbles over rocks to feed the pond.";
            room.ShortDescription = "Duck Pond";

            return room;
        }

        private IRoom GenerateRoom22()
        {

            IRoom room = CreateRoom(5);
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "Pine trees lie a dozen yards to the west and the pond lies several feet to east.  The cat tails make it hard to see but the pond shimmers in the light.";
            room.LookDescription = "The pond lies to the east and cat tails have grown up here obscuring view of some of the pond.";
            room.ShortDescription = "Duck Pond";

            return room;
        }

        private IRoom GenerateRoom23()
        {

            IRoom room = CreateRoom(5);
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "This area could make a nice picnic area.  There are open areas of grass and a nice view of the pond.  Trees grow to the west and provide shade and an place to take a nice walk for people who are interested.";
            room.LookDescription = "The duck pond extends to the north underneath a wooden bridge to the east.";
            room.ShortDescription = "Duck Pond";

            return room;
        }

        private IRoom GenerateRoom24()
        {

            IRoom room = CreateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "The wooden looks like it was recently built.  The wood is nice and strong and the paint is not flaking.  The top hand rails and the deck where you walk are both painted a pleasant shade of red and the rails are painted white.  It has a pleasant arch that spans the approximate 10 feet pond.";
            room.LookDescription = "Looking over the railing of the bridge you can see fish in the pond a few feet below.";
            room.ShortDescription = "Duck Pond";

            return room;
        }

        private IRoom GenerateRoom25()
        {

            IRoom room = CreateRoom(5);
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "To the east you can see the great dome rising above the rest of the city.  The soft green grass extends in to the east and to the south along the duck pond.";
            room.LookDescription = "The duck pond extends to the north underneath a wooden bridge to the west.";
            room.ShortDescription = "Duck Pond";

            return room;
        }

        private IRoom GenerateRoom26()
        {

            IRoom room = CreateRoom(5);
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "The duck pond extends about 20 feet to the west stopping in a wall of cat tails.  To the east is a grassy field that could be useful for flying a kite of just enjoying a day with the family.";
            room.LookDescription = "This side of the duck pond has a nice stone wall edging that makes it look more like it's part of a town park.";
            room.ShortDescription = "Duck Pond";

            return room;
        }

        private IRoom GenerateRoom27()
        {

            IRoom room = CreateRoom(5);
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "The small duck pond is probably about 20 feet across and about 40 feet long.  A family of ducks some times can be seen swimming out on the waters.";
            room.LookDescription = "The duck pond, another small jewel of the city lies to the north east.";
            room.ShortDescription = "Duck Pond";

            return room;
        }

        private IRoom GenerateRoom28()
        {

            IRoom room = CreateRoom(10);
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "The pond water is a nice cool temperature and small fish swim up to you and then dart away.  Too curious to ignore you but too afraid to stay.";
            room.LookDescription = "You are standing in the center of the duck pond.  Small fish swim around you curious as to who or what has entered their pond.";
            room.ShortDescription = "Duck Pond";

            room.Items.Add(Sign());

            INonPlayerCharacter mommaDuck = MommaDuck();
            mommaDuck.Room = room;
            room.AddMobileObjectToRoom(mommaDuck);
            Wanderer mommaWander = mommaDuck.Personalities[1] as Wanderer;

            for (int x = 0; x < 5; x++)
            {
                INonPlayerCharacter babyDuck = BabyDuck();
                Wanderer babyWander = babyDuck.Personalities[0] as Wanderer;
                babyDuck.Room = room;
                babyDuck.FollowTarget = mommaDuck;
                babyWander.NavigableRooms.AddRange(mommaWander.NavigableRooms);
                room.AddMobileObjectToRoom(babyDuck);
            }

            INonPlayerCharacter daddyDuck = DaddyDuck();
            daddyDuck.Room = room;
            room.AddMobileObjectToRoom(daddyDuck);
            Wanderer daddyWander = daddyDuck.Personalities[1] as Wanderer;
            daddyWander.NavigableRooms.AddRange(mommaWander.NavigableRooms);

            return room;
        }

        private IItem Sign()
        {
            IItem item = new Item();
            item.Attributes.Add(Item.ItemAttribute.NoGet);
            item.KeyWords.Add("sign");
            item.KeyWords.Add("statue");

            item.ShortDescription = "A small metal sign lies on bottom of the pond about two feet deep.";
            item.LookDescription = "The sign says \"IF YOU CAN READ THIS GET OUT OF THE DUCK POND\"";
            item.ExamineDescription = "Made of some type of metal it is firmly attached to the bottom of the pond.  Maybe instead of messing with the sign you should what it says.";
            item.SentenceDescription = "sign";

            return item;
        }

        private INonPlayerCharacter DaddyDuck()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Other, 1);
            npc.KeyWords.Add("duck");
            npc.KeyWords.Add("dad");
            npc.KeyWords.Add("daddy");
            npc.KeyWords.Add("mallard");
            npc.Personalities.Add(new Guardian());
            npc.Personalities.Add(ValidDuckAreaWander());

            npc.SentenceDescription = "daddy duck";
            npc.ShortDescription = "A male mallard duck.";
            npc.LookDescription = "The daddy duck ignores you as he continues on his way.";
            npc.ExamineDescription = "The bright green head gives away that this is an adult male mallard duck.";

            return npc;
        }

        private INonPlayerCharacter MommaDuck()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Other, 1);
            npc.KeyWords.Add("duck");
            npc.KeyWords.Add("mom");
            npc.KeyWords.Add("momma");
            npc.KeyWords.Add("mallard");
            npc.Personalities.Add(new Guardian());
            npc.Personalities.Add(ValidDuckAreaWander());

            npc.SentenceDescription = "momma duck";
            npc.ShortDescription = "A female mallard duck.";
            npc.LookDescription = "The momma duck looks cautiously at you be leaves you alone for the time being.";
            npc.ExamineDescription = "The way mother ducks can be protective of their young you may not want to bother this one.";

            return npc;
        }

        private IWanderer ValidDuckAreaWander()
        {
            IWanderer wanderer = new Wanderer();
            wanderer.NavigableRooms.Add(new RoomId(Zone.Id, 28));
            wanderer.NavigableRooms.Add(new RoomId(Zone.Id, 29));
            wanderer.NavigableRooms.Add(new RoomId(Zone.Id, 30));
            wanderer.NavigableRooms.Add(new RoomId(Zone.Id, 31));
            wanderer.NavigableRooms.Add(new RoomId(Zone.Id, 32));
            wanderer.NavigableRooms.Add(new RoomId(Zone.Id, 33));
            wanderer.NavigableRooms.Add(new RoomId(Zone.Id, 34));
            wanderer.NavigableRooms.Add(new RoomId(Zone.Id, 35));
            wanderer.NavigableRooms.Add(new RoomId(Zone.Id, 36));
            return wanderer;
        }

        private INonPlayerCharacter BabyDuck()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Other, 1);
            npc.KeyWords.Add("duck");
            npc.KeyWords.Add("baby");
            npc.KeyWords.Add("mallard");
            npc.Personalities.Add(new Wanderer(100));

            npc.SentenceDescription = "baby duck";
            npc.ShortDescription = "A baby mallard duck.";
            npc.LookDescription = "The baby duck ignores you continues on its way.";
            npc.ExamineDescription = "The baby duck still has down on it.  It is a dark brown color probably to help it hide from attackers.";

            return npc;
        }
        #endregion Duck Pond

        #endregion End Rooms

        private void ConnectRooms()
        {
            Zone.RecursivelySetZone();

            ZoneHelper.ConnectZone(Zone.Rooms[1], Direction.Down, 6, 8, new DoorInfo("stone", "You move the stone out of the way reveling the opening of the tunnel.", true, "The stone is of good weight, enough to discourage people from moving it but light enough to move when needed."));
            ZoneHelper.ConnectZone(Zone.Rooms[17], Direction.North, 2, 1);
            ZoneHelper.ConnectZone(Zone.Rooms[19], Direction.East, 3, 1);
            ZoneHelper.ConnectZone(Zone.Rooms[4], Direction.South, 7, 5);

            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.North, Zone.Rooms[2]);
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.North, Zone.Rooms[3]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.East, Zone.Rooms[4]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.North, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.East, Zone.Rooms[6]);
            ZoneHelper.ConnectRoom(Zone.Rooms[6], Direction.North, Zone.Rooms[7]);
            ZoneHelper.ConnectRoom(Zone.Rooms[7], Direction.North, Zone.Rooms[8]);
            ZoneHelper.ConnectRoom(Zone.Rooms[8], Direction.West, Zone.Rooms[9]);
            ZoneHelper.ConnectRoom(Zone.Rooms[9], Direction.West, Zone.Rooms[10]);
            ZoneHelper.ConnectRoom(Zone.Rooms[9], Direction.North, Zone.Rooms[14]);
            ZoneHelper.ConnectRoom(Zone.Rooms[10], Direction.South, Zone.Rooms[11]);
            ZoneHelper.ConnectRoom(Zone.Rooms[11], Direction.South, Zone.Rooms[12]);
            ZoneHelper.ConnectRoom(Zone.Rooms[12], Direction.East, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[13], Direction.South, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[13], Direction.East, Zone.Rooms[7]);
            ZoneHelper.ConnectRoom(Zone.Rooms[13], Direction.North, Zone.Rooms[9]);
            ZoneHelper.ConnectRoom(Zone.Rooms[13], Direction.West, Zone.Rooms[11]);
            ZoneHelper.ConnectRoom(Zone.Rooms[14], Direction.North, Zone.Rooms[15]);
            ZoneHelper.ConnectRoom(Zone.Rooms[15], Direction.North, Zone.Rooms[16]);
            ZoneHelper.ConnectRoom(Zone.Rooms[16], Direction.North, Zone.Rooms[17]);

            ZoneHelper.ConnectRoom(Zone.Rooms[7], Direction.East, Zone.Rooms[18]);
            ZoneHelper.ConnectRoom(Zone.Rooms[18], Direction.East, Zone.Rooms[19]);

            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.North, Zone.Rooms[20]);
            ZoneHelper.ConnectRoom(Zone.Rooms[20], Direction.West, Zone.Rooms[21]);
            ZoneHelper.ConnectRoom(Zone.Rooms[21], Direction.North, Zone.Rooms[22]);
            ZoneHelper.ConnectRoom(Zone.Rooms[22], Direction.North, Zone.Rooms[23]);
            ZoneHelper.ConnectRoom(Zone.Rooms[23], Direction.East, Zone.Rooms[24]);
            ZoneHelper.ConnectRoom(Zone.Rooms[24], Direction.East, Zone.Rooms[25]);
            ZoneHelper.ConnectRoom(Zone.Rooms[25], Direction.South, Zone.Rooms[26]);
            ZoneHelper.ConnectRoom(Zone.Rooms[26], Direction.South, Zone.Rooms[27]);
            ZoneHelper.ConnectRoom(Zone.Rooms[27], Direction.West, Zone.Rooms[20]);
            ZoneHelper.ConnectRoom(Zone.Rooms[28], Direction.East, Zone.Rooms[26]);
            ZoneHelper.ConnectRoom(Zone.Rooms[28], Direction.South, Zone.Rooms[20]);
            ZoneHelper.ConnectRoom(Zone.Rooms[28], Direction.West, Zone.Rooms[22]);
        }
    }
}