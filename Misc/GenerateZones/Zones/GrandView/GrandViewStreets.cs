using System;
using System.Linq;
using System.Reflection;
using MiscShared;
using Objects.GameDateTime;
using Objects.Item;
using Objects.Item.Interface;
using Objects.Item.Items;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using Objects.Personality;
using Objects.Personality.Interface;
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
            Zone.Name = nameof(GrandViewStreets);

            BuildRoomsViaReflection(this.GetType());

            ConnectRooms();

            return Zone;
        }

        #region Rooms
        #region City
        private IRoom GenerateRoom1()
        {
            string examineDescription = "A huge ancient oak tree about 10 feet in diameter creates the perfect blind for entering or leaving the tunnel below.  Other than that it would also be a nice place for a picnic.";
            string lookDescription = "A huge ancient oak tree hides a hole in the ground from the street.";
            string shortDescription = "Behind a large tree";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);

            //string message = "Perform the following commands." + Environment.NewLine + "NORTH";
            //room.Enchantments.Add(EnterRoomMessage(message, TagWrapper.TagType.Info));

            return room;
        }

        private IRoom GenerateRoom2()
        {
            string examineDescription = "Upon closer examination you discover a faint path between the street and the oak tree.  You are not the first person to come this way.";
            string lookDescription = "You stand in a grassy field half way between the street and a huge ancient oak tree.";
            string shortDescription = "A grassy field";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);

            //string message = "You have made it out of the dungeon and stand in the center of a park.  If you wish to continue to learn how to play continue north then to the east.  If at any time you get need to look up how to use a command use the MAN command.";
            //room.Enchantments.Add(EnterRoomMessage(message, TagWrapper.TagType.Info));

            return room;
        }

        private IRoom GenerateRoom3()
        {
            string examineDescription = "The road appears to be of good quality.  No weeds, well drained and smooth stone.  To the south is a field with a small hill and huge tree.  To the north is a pond with some ducks swimming by.  To the east you see a large dome and the west leads down to the heart of the city.";
            string lookDescription = "The road runs east and west here and a field extends to the south with a huge oak tree dominating a small hill.";
            string shortDescription = "A street";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom4()
        {
            string examineDescription = string.Format("The \"Grandview Meadows\" park lies to your west.  To call it a Grandview or a Meadow would be stretching the definition of either but hey, no one consulted you when the named the place.  Maybe before the city was here it was a meadow.  To the north is the stairwell leading to the giant dome which some would call the jewel of the city.  The stairwell is made of stone and consists of {0} steps, one for each God.", Enum.GetValues(typeof(Years)).Length);
            string lookDescription = "An arched sign extends over the street here and reads \"Grandview Meadows.\"";
            string shortDescription = "Entrance to the park";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddItemToRoom(LionStatue());
            room.AddItemToRoom(LionStatue());

            return room;
        }

        private IRoom GenerateRoom5()
        {
            string examineDescription = "White paving stone extend outward from the center of the dome in a circular pattern.";
            string lookDescription = "A paved court yard surrounds the giant dome to the north.";
            string shortDescription = "South of the dome";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddItemToRoom(Room13_Fountain());
            room.Sounds.Add(FountainSound());

            return room;
        }

        private IRoom GenerateRoom6()
        {
            string examineDescription = "White paving stone extend outward from the center of the dome in a circular pattern.";
            string lookDescription = "A paved court yard surrounds the giant dome to the north west.";
            string shortDescription = "South east of the dome";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom7()
        {
            string examineDescription = "White paving stone extend outward from the center of the dome in a circular pattern.";
            string lookDescription = "A paved court yard surrounds the giant dome to the west.";
            string shortDescription = "East of the dome";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddItemToRoom(Room13_Fountain());
            room.Sounds.Add(FountainSound());

            return room;
        }

        private IRoom GenerateRoom8()
        {
            string examineDescription = "White paving stone extend outward from the center of the dome in a circular pattern.";
            string lookDescription = "A paved court yard surrounds the giant dome to the south west.";
            string shortDescription = "North east of the dome";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom9()
        {
            string examineDescription = "White paving stone extend outward from the center of the dome in a circular pattern.";
            string lookDescription = "A paved court yard surrounds the giant dome to the north.";
            string shortDescription = "North of the dome";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddItemToRoom(Room13_Fountain());
            room.Sounds.Add(FountainSound());

            return room;
        }

        private IRoom GenerateRoom10()
        {
            string examineDescription = "White paving stone extend outward from the center of the dome in a circular pattern.";
            string lookDescription = "A paved court yard surrounds the giant dome to the south east.";
            string shortDescription = "North west of the dome";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom11()
        {
            string examineDescription = "White paving stone extend outward from the center of the dome in a circular pattern.";
            string lookDescription = "A paved court yard surrounds the giant dome to the east.";
            string shortDescription = "West of the dome";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddItemToRoom(Room13_Fountain());
            room.Sounds.Add(FountainSound());

            return room;
        }

        private IRoom GenerateRoom12()
        {
            string examineDescription = "White paving stone extend outward from the center of the dome in a circular pattern.";
            string lookDescription = "A paved court yard surrounds the giant dome to the north east.";
            string shortDescription = "South west of the dome";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom13()
        {
            string examineDescription = "The dome appears to 50 feet across and 30 to 40 feet across.  There are 8 pillars 5 feet in diameter made of marble holding up the dome.  Each pillar is made of either white marble or black marble in alternating fashion.  The light dancing through the dome cast interesting patterns on the white cobble stone.  Directly under the center of the dome is a large circular stone that could be used for sitting.  In the center of the stone is an inscription. \"Built 02/14/0024 by Charon\"";
            string lookDescription = "A giant stained glass dome stands 30 to 40 feet above you.  The light shining through the dome cast interesting patterns on the cobble stone floor causing you to admire its beauty for a moment.";
            string shortDescription = "Under the stained glass dome";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddItemToRoom(Room21_Sign());
            IRecallBeacon beacon = CreateRecallBeacon();
            room.AddItemToRoom(beacon);
            return room;
        }

        private IItem Room21_Sign()
        {
            string examineDescription = "The sign appears to be made of black marble with the words carved out and filled with a silvery metal so they appear to almost shine.  The frame is 1.5 inches and is made of African Blackwood.  All in all the sign creates a striking contrast against the white pillar on which it hangs.";
            string lookDescription = @"North: The Great Library
East: The Training Hall";
            string sentenceDescription = "sign";
            string shortDescription = "A sign hangs on a pillar.";

            IItem item = CreateItem(examineDescription, lookDescription, sentenceDescription, shortDescription);
            item.Attributes.Add(ItemAttribute.NoGet);
            item.KeyWords.Add("sign");

            return item;
        }

        private IRoom GenerateRoom14()
        {
            string examineDescription = "A stone fence about 3 feet tall line either side of the road here. Cherry blossoms can be seen growing on either side of the fence on the large estates.";
            string lookDescription = "Two sprawling estates line the road here.";
            string shortDescription = "South Rockdale Avenue";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 5);
            return room;
        }

        private IRoom GenerateRoom15()
        {
            string examineDescription = "The stone fence continue north and south along the road.  On either side are the two largest estates in the city.";
            string lookDescription = "The two largest estates in the city are to the east and the west of here.";
            string shortDescription = "Rockdale Avenue";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 5);
            return room;
        }

        private IRoom GenerateRoom16()
        {
            string examineDescription = "A stone fence about 3 feet tall line either side of the road here. Pine trees dot the country side on either side of the road.";
            string lookDescription = "Two sprawling estates line the road here.";
            string shortDescription = "North Rockdale Avenue";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 5);
            return room;
        }

        private IRoom GenerateRoom17()
        {
            string examineDescription = "To the north stands the city's library.  Seven five story tall pillars support the roof to this massive structure.  In the center of the library is a large golden dome with windows cut out for viewing the city.";
            string lookDescription = "The city's library containing the public records, works of fiction and great books of magic stands to the north.  To the south lies Rockdale Avenue.";
            string shortDescription = "Before the great library";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);
            return room;
        }
        #endregion City

        #region City Pt2
        private IRoom GenerateRoom18()
        {
            string examineDescription = "Off in the distance to the east you can make out a coliseum and to the west is the great dome.  Shops and houses line the road to the north and south.";
            string lookDescription = "The cobble stones here are quite worn indicating this road it quite popular.";
            string shortDescription = "A wide road";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom19()
        {
            string examineDescription = "You stand about half way between the giant dome and the coliseum. Even though the coliseum is still a far ways off it presence looms far above you.";
            string lookDescription = "The road continues east and west for a long ways.  The shops and houses have given way to a small park.";
            string shortDescription = "A wide road";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 5);

            return room;
        }
        #endregion City Pt2

        #region Duck Pond
        private IRoom GenerateRoom20()
        {
            string examineDescription = "Blue water and green lily pads mix to form turquoise reflections that dance on the water.";
            string lookDescription = "Lily pads float on the surface of the pond to the north creating a checker board effect.";
            string shortDescription = "Duck Pond";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom21()
        {
            string examineDescription = "A small tree lays in the stream.  One end was gnawed off by a beaver but it looks like it was unable to pull it back up the bank to be taken to its damn.";
            string lookDescription = "A small stream of water creating a soothing sound as it babbles over rocks to feed the pond.";
            string shortDescription = "Duck Pond";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom22()
        {
            string examineDescription = "Pine trees lie a dozen yards to the west and the pond lies several feet to east.  The cat tails make it hard to see but the pond shimmers in the light.";
            string lookDescription = "The pond lies to the east and cat tails have grown up here obscuring view of some of the pond.";
            string shortDescription = "Duck Pond";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom23()
        {
            string examineDescription = "This area could make a nice picnic area.  There are open areas of grass and a nice view of the pond.  Trees grow to the west and provide shade and an place to take a nice walk for people who are interested.";
            string lookDescription = "The duck pond extends to the north underneath a wooden bridge to the east.";
            string shortDescription = "Duck Pond";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom24()
        {
            string examineDescription = "The wooden looks like it was recently built.  The wood is nice and strong and the paint is not flaking.  The top hand rails and the deck where you walk are both painted a pleasant shade of red and the rails are painted white.  It has a pleasant arch that spans the approximate 10 feet pond.";
            string lookDescription = "Looking over the railing of the bridge you can see fish in the pond a few feet below.";
            string shortDescription = "Duck Pond";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom25()
        {
            string examineDescription = "To the east you can see the great dome rising above the rest of the city.  The soft green grass extends in to the east and to the south along the duck pond.";
            string lookDescription = "The duck pond extends to the north underneath a wooden bridge to the west.";
            string shortDescription = "Duck Pond";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom26()
        {
            string examineDescription = "The duck pond extends about 20 feet to the west stopping in a wall of cat tails.  To the east is a grassy field that could be useful for flying a kite of just enjoying a day with the family.";
            string lookDescription = "This side of the duck pond has a nice stone wall edging that makes it look more like it's part of a town park.";
            string shortDescription = "Duck Pond";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom27()
        {
            string examineDescription = "The small duck pond is probably about 20 feet across and about 40 feet long.  A family of ducks some times can be seen swimming out on the waters.";
            string lookDescription = "The duck pond, another small jewel of the city lies to the north east.";
            string shortDescription = "Duck Pond";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom28()
        {
            string examineDescription = "The pond water is a nice cool temperature and small fish swim up to you and then dart away.  Too curious to ignore you but too afraid to stay.";
            string lookDescription = "You are standing in the center of the duck pond.  Small fish swim around you curious as to who or what has entered their pond.";
            string shortDescription = "Duck Pond";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription, 10);

            room.AddItemToRoom(Sign());

            INonPlayerCharacter mommaDuck = MommaDuck(room);
            mommaDuck.Room = room;
            room.AddMobileObjectToRoom(mommaDuck);
            Wanderer mommaWander = mommaDuck.Personalities[1] as Wanderer;

            for (int x = 0; x < 5; x++)
            {
                INonPlayerCharacter babyDuck = BabyDuck(room);
                Wanderer babyWander = babyDuck.Personalities[0] as Wanderer;
                babyDuck.Room = room;
                babyDuck.FollowTarget = mommaDuck;
                babyWander.NavigableRooms.AddRange(mommaWander.NavigableRooms);
                room.AddMobileObjectToRoom(babyDuck);
            }

            INonPlayerCharacter daddyDuck = DaddyDuck(room);
            daddyDuck.Room = room;
            room.AddMobileObjectToRoom(daddyDuck);
            Wanderer daddyWander = daddyDuck.Personalities[1] as Wanderer;
            daddyWander.NavigableRooms.AddRange(mommaWander.NavigableRooms);

            return room;
        }

    
        #endregion Duck Pond

        private IRoom GenerateRoom29()
        {
            string examineDescription = "The entrance to the temple of Charon can be seen on the hill to the west.";
            string lookDescription = "A main road of the city it the cobble stones have been warn smooth.";
            string shortDescription = "A main road.";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom30()
        {
            string examineDescription = "A coin has become wedged in between to stones but try as you might it won't come loose.";
            string lookDescription = "A few dandelions push their way up through the cracks in the road.";
            string shortDescription = "A main road.";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom31()
        {
            string examineDescription = "Dandelions grow in the grass along side the road creating a blanket of yellow.";
            string lookDescription = "Part of the road has sunken down causing a small dip on the right side of the road.";
            string shortDescription = "A main road.";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddItemToRoom(BrokenCart());

            return room;
        }

        #endregion End Rooms

        #region Npcs
        private INonPlayerCharacter DaddyDuck(IRoom room)
        {
            string examineDescription = "The bright green head gives away that this is an adult male mallard duck.";
            string lookDescription = "The daddy duck ignores you as he continues on his way.";
            string sentenceDescription = "daddy duck";
            string shortDescription = "A male mallard duck.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Other, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 1);
            npc.KeyWords.Add("duck");
            npc.KeyWords.Add("dad");
            npc.KeyWords.Add("daddy");
            npc.KeyWords.Add("mallard");
            npc.Personalities.Add(new Guardian());
            npc.Personalities.Add(ValidDuckAreaWander());

            return npc;
        }

        private INonPlayerCharacter MommaDuck(IRoom room)
        {
            string examineDescription = "The way mother ducks can be protective of their young you may not want to bother this one.";
            string lookDescription = "The momma duck looks cautiously at you be leaves you alone for the time being.";
            string sentenceDescription = "momma duck";
            string shortDescription = "A female mallard duck.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Other, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 1);
            npc.KeyWords.Add("duck");
            npc.KeyWords.Add("mom");
            npc.KeyWords.Add("momma");
            npc.KeyWords.Add("mallard");
            npc.Personalities.Add(new Guardian());
            npc.Personalities.Add(ValidDuckAreaWander());

            return npc;
        }

        private IWanderer ValidDuckAreaWander(int? wanderAmount = null)
        {
            IWanderer wanderer = new Wanderer();
            if (wanderAmount != null)
            {
                wanderer.MovementPercent = (int)wanderAmount;
            }
            wanderer.NavigableRooms.Add(new RoomId(Zone.Id, 20));
            wanderer.NavigableRooms.Add(new RoomId(Zone.Id, 21));
            wanderer.NavigableRooms.Add(new RoomId(Zone.Id, 22));
            wanderer.NavigableRooms.Add(new RoomId(Zone.Id, 23));
            wanderer.NavigableRooms.Add(new RoomId(Zone.Id, 24));
            wanderer.NavigableRooms.Add(new RoomId(Zone.Id, 25));
            wanderer.NavigableRooms.Add(new RoomId(Zone.Id, 26));
            wanderer.NavigableRooms.Add(new RoomId(Zone.Id, 27));
            wanderer.NavigableRooms.Add(new RoomId(Zone.Id, 28));
            return wanderer;
        }

        private INonPlayerCharacter BabyDuck(IRoom room)
        {
            string examineDescription = "The baby duck still has down on it.  It is a dark brown color probably to help it hide from attackers.";
            string lookDescription = "The baby duck ignores you continues on its way.";
            string sentenceDescription = "baby duck";
            string shortDescription = "A baby mallard duck.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Other, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 1);
            npc.KeyWords.Add("duck");
            npc.KeyWords.Add("baby");
            npc.KeyWords.Add("mallard");
            npc.Personalities.Add(ValidDuckAreaWander(100));

            return npc;
        }
        #endregion Npcs


        #region Items
        private IItem LionStatue()
        {
            string examineDescription = "Each lion is in a sitting position with one paw holding a shield with the cities crest upright.";
            string lookDescription = "The steps are flanked on each side by a pair of black marble lions which seem to almost come to life.";
            string sentenceDescription = "statue";
            string shortDescription = "A lion statue.";

            IItem statue = CreateItem(examineDescription, lookDescription, sentenceDescription, shortDescription);
            statue.Attributes.Add(Item.ItemAttribute.NoGet);
            statue.KeyWords.Add("Lion");
            statue.KeyWords.Add("statue");

            return statue;
        }

        private ISound FountainSound()
        {
            ISound sound = new Sound();
            sound.Loop = true;
            sound.SoundName = string.Format("{0}\\{1}", Zone.Name, "Fountain.mp3");
            return sound;
        }

        private Fountain Room13_Fountain()
        {
            string examineDescription = "The fountain is made of blue glass with four tiers.  The first three tiers flow down into the 4th tier and the bottom pool shoots water up into the 4th tier.  The blue color causes the fountain to blend into the water making the whole thing appear as one big moving fountain of water.";
            string lookDescription = "A large ornate fountain pours water down several tiers.";
            string sentenceDescription = "fountain";
            string shortDescription = "An ornate fountain.";

            Fountain fountain = CreateFountain(examineDescription, lookDescription, sentenceDescription, shortDescription);
            return fountain;
        }

        private IItem Sign()
        {
            string examineDescription = "Made of some type of metal it is firmly attached to the bottom of the pond.  Maybe instead of messing with the sign you should what it says.";
            string lookDescription = "The sign says \"IF YOU CAN READ THIS GET OUT OF THE DUCK POND\"";
            string sentenceDescription = "sign";
            string shortDescription = "A small metal sign lies on bottom of the pond about two feet deep.";

            IItem item = CreateItem(examineDescription, lookDescription, sentenceDescription, shortDescription);
            item.Attributes.Add(Item.ItemAttribute.NoGet);
            item.KeyWords.Add("sign");
            item.KeyWords.Add("statue");

            return item;
        }

        private IItem BrokenCart()
        {
            string examineDescription = "While the cart looks to be freshly left its contents have been emptied.";
            string lookDescription = "A broken down cart has been left on the side of the road.";
            string sentenceDescription = "cart";
            string shortDescription = "A broken down cart.";

            IItem item = CreateItem(examineDescription, lookDescription, sentenceDescription, shortDescription);
            item.KeyWords.Add("Cart");
            item.Attributes.Add(ItemAttribute.NoGet);

            return item;
        }

        #endregion Items

        private void ConnectRooms()
        {
            string openMessage = "You move the stone out of the way reveling the opening of the tunnel.";
            string closeMessage = "Sliding the stone back you close up the tunnel.";
            string description = "The stone is of good weight, enough to discourage people from moving it but light enough to move when needed.";
            ZoneHelper.ConnectZone(Zone.Rooms[1], Direction.Down, 6, 8, new DoorInfo("stone", openMessage, closeMessage, true, description));
            ZoneHelper.ConnectZone(Zone.Rooms[17], Direction.North, 2, 1);
            ZoneHelper.ConnectZone(Zone.Rooms[19], Direction.East, 3, 1);
            ZoneHelper.ConnectZone(Zone.Rooms[4], Direction.South, 7, 5);
            ZoneHelper.ConnectZone(Zone.Rooms[30], Direction.South, 11, 9);
            ZoneHelper.ConnectZone(Zone.Rooms[29], Direction.South, 21, 5);

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
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.West, Zone.Rooms[29]);
            ZoneHelper.ConnectRoom(Zone.Rooms[29], Direction.West, Zone.Rooms[30]);
            ZoneHelper.ConnectRoom(Zone.Rooms[30], Direction.West, Zone.Rooms[31]);
        }
    }
}
