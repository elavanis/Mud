using System.Collections.Generic;
using System.Linq;
using Objects.Zone.Interface;
using System.Reflection;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Item;
using Objects.Item.Interface;
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
using Objects.Damage.Interface;
using static Objects.Damage.Damage;
using static Objects.Mob.NonPlayerCharacter;
using static Objects.Item.Items.Weapon;
using MiscShared;

namespace GenerateZones.Zones.DeepWoodForest
{
    public class DeepWoodForest : BaseZone, IZoneCode
    {
        public DeepWoodForest() : base(8)
        {
        }

        public IZone Generate()
        {
            Zone.Name = nameof(DeepWoodForest);

            BuildRoomsViaReflection(this.GetType());

            ConnectRooms();
            AddSounds();

            return Zone;
        }

        private void AddSounds()
        {
            ISound sound = new Sound();
            sound.Loop = true;
            sound.SoundName = string.Format("{0}\\{1}", Zone.Name, "Forest.mp3");
            foreach (IRoom room in Zone.Rooms.Values)
            {
                room.Sounds.Add(sound);
            }

            sound = new Sound();
            sound.Loop = true;
            sound.SoundName = string.Format("{0}\\{1}", Zone.Name, "Stream.mp3");
            Zone.Rooms[35].Sounds.Add(sound);
            Zone.Rooms[27].Sounds.Add(sound);
            Zone.Rooms[28].Sounds.Add(sound);
            Zone.Rooms[29].Sounds.Add(sound);
            Zone.Rooms[21].Sounds.Add(sound);
            Zone.Rooms[13].Sounds.Add(sound);
            Zone.Rooms[5].Sounds.Add(sound);
            Zone.Rooms[45].Sounds.Add(sound);
            Zone.Rooms[53].Sounds.Add(sound);
            Zone.Rooms[61].Sounds.Add(sound);
            Zone.Rooms[69].Sounds.Add(sound);
            Zone.Rooms[77].Sounds.Add(sound);
            Zone.Rooms[78].Sounds.Add(sound);
        }

        #region Rooms
        private IRoom GenerateRoom1()
        {
            string examineDescription = "The forest is thick and almost seems alive.  It slowly over the years has been encroaching on the meadows.";
            string lookDescription = "You stand at the edge of the forest.  To the west is green meadows and to the east is a thick forest.  The forest runs north and south as far as the eye can see.";
            string shortDescription = "Edge of the forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom2()
        {
            string examineDescription = "The path you are standing on is warn enough to allow you to find your way can tell there is not much traffic through the area.";
            string lookDescription = "To the west you can see the opening of the forest down the path.  To the east the forest continues into the distance.";
            string shortDescription = "Slightly in the forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom3()
        {
            string examineDescription = "The tree was once a mighty oak that towered proudly in the forest providing shelter to many animals.  When it was standing it was about 50 feet wide, counting the rings you can tell it was 546 years old when it fell.";
            string lookDescription = "I large tree has fallen over the path here.  The section that laid over the path has been cut away.  At one time you can image that there was a large collection of fire wood but as people have come this way gotten some with each trip.  There is hardly any wood left.";
            string shortDescription = "Forest path";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom4()
        {
            string examineDescription = "There are a several pairs of trees here that bend slightly into the path and join together above you.  Their thick canopies forms a bit of an arch that serves both as a natural shelter and a symbolic arch, almost as if to say you are entering a different part of the forest here.";
            string lookDescription = "The path continues through a section of thick evergreen trees.";
            string shortDescription = "Forest path";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom5()
        {
            string examineDescription = "Looking to the north up stream you can not see anything of interest before it turns to the east.  Looking down stream to the south you the stream flattens out into a wide area of shallow rocks.  There are several animals tracks that you recognize including deer and badger as well as a few larger ones you do not recognize.";
            string lookDescription = "The forest is washed out here by a small stream that runs north and south.";
            string shortDescription = "Forest path";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom6()
        {
            string examineDescription = "The meadow is waist hight with wild grasses.  Flowers of purple and yellow jot the meadow from the north to the south.";
            string lookDescription = "Here the forest gives way to a small meadow.  A small pond can be seen to the south.";
            string shortDescription = "Edge of the Meadow";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom7()
        {
            string examineDescription = "The meadow is waist high with wild grasses.  Flowers of purple and yellow jot the meadow from the north to the south.";
            string lookDescription = "A small stream flows through the meadow to the south to the pond.";
            string shortDescription = "Edge of the Meadow";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom8()
        {
            string examineDescription = "Trees grow up on either of the side creating a quiet area the blocks out most of the light from above.  Small mushrooms grow on the cool dark forest floor in small groups creating a soft rich layer of dirt perfect for growing plants.";
            string lookDescription = "A deer path runs to the east and west leading off into the distance.";
            string shortDescription = "Forest path";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom9()
        {
            string examineDescription = "Everywhere you look is green.  The only place that you can see brown bark is when you look up and see tree branches that are not covered in vines yet.";
            string lookDescription = "The forest is filled with lush green undergrowth.  Vines grow up the sides of trees creating a soft green everywhere you look.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom10()
        {
            string examineDescription = "The dead tree stands about 60 feet tall and its branches stretch out as if trying to reach sun light to once again grow.";
            string lookDescription = "A single dead tree stands in the middle of a clearing.  Other trees have grown around it but almost as if to give respect to the dead tree none have grown into its space.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            room.AddMobileObjectToRoom(Squirrel(room));

            return room;
        }

        private IRoom GenerateRoom11()
        {
            string examineDescription = "The red ferns are growing in a circular shape extending 15 feet.  Why they stop after that is a good guess but it does make for a nice breaking in scenery.";
            string lookDescription = "A section of red ferns grown on the forest floor providing a break in the sea of green with brown buildings.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom12()
        {
            string examineDescription = "Movement is difficult and every where you look looks the same.  The undergrowth is so thick you could hide a baby deer 3 feet in front of you and not know it.";
            string lookDescription = "The forest has become thick with undergrowth.  Small bushes and new trees fight for what little light comes down to the forest floor.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom13()
        {
            string examineDescription = "The bottom water fall is about 1 foot tall and the upper water fall is about 1.5 feet tall.  The grass growing on either of side of the bank is lush green and soft.  You can see where an animal has bedded down by the bank of the stream but it is gone for the moment.";
            string lookDescription = "The stream flows over two small water falls and makes soft gurgling sounds.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom14()
        {
            string examineDescription = "Sounds of a small stream can be heard from the west.  Small birds can be heard in the forest to the north and the west and a beautiful meadow extends to the south and the east.";
            string lookDescription = "The forest extends a bit into the meadow creating a small blind that could be advantageous when hunting or disadvantageous when hunted.";
            string shortDescription = "Edge of the Meadow";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }


        private IRoom GenerateRoom15()
        {
            string examineDescription = "The east wall is has completely fallen in and the west wall is leaning haphazardly into the main room.  A strong gust of wind might blow it the rest of the way in.";
            string lookDescription = "What looks like a hunters hut has been burned down.  The building was burned a while ago and it appears to have been picked over already.";
            string shortDescription = "Edge of the Meadow";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            room.AddMobileObjectToRoom(Mouse(room));

            return room;
        }

        private IRoom GenerateRoom16()
        {
            string examineDescription = "Pine trees descend into the white mist that covers the forest to the east.  One by one each tree since lower and lower until just the tips of trees are showing and eventually even they are gone from sight.";
            string lookDescription = "From where you stand on the top of a hill you can see to the east the forest stretch out to the east.  Pine trees descend into a white mist below.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom17()
        {
            string examineDescription = "A soft layer of grass has grown up on the forest floor which provides bedding for several woodland animals.";
            string lookDescription = "The forest is intermingled with evergreens and deciduous trees, each one trying to be the dominate type of tree here.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            room.AddMobileObjectToRoom(Fox(room));

            return room;
        }

        private IRoom GenerateRoom18()
        {
            string examineDescription = "Standing in the forest you can hear the peaceful silence and then eventually you hear a squirrel jump through from one tree limb to another.  An owl gives a mighty hoot and an unidentified rustling of a limb.";
            string lookDescription = "Pine trees dominate the landscape here.  Towering 100 feet into the air all you can see above you is a sea of green.  Brown pine needles cover the ground creating a picture that looks like it came out of a painting.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom19()
        {
            string examineDescription = "The deer trail winds through the forest.  It heads north around and over a fallen log and over some overgrowth.  To the south it quickly disappears around a bend.";
            string lookDescription = "You have stumbled upon a deer trail that runs north and south.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom20()
        {
            string examineDescription = "Trees have grown together forming an almost implementable wall to the north.  The size of the trees are hard to tell as over the years individual trees have grown up and intertangled with each other almost like braided rope.";
            string lookDescription = "The forest to the north becomes very dense with overgrowth.  To the south it opens up more and a stream can be heard to the east.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom21()
        {
            string examineDescription = "The damn was built in a slow moving part of the stream.  Here the stream had widened out to about 10 feet.  Thanks to the beaver the north side of the stream is now about 3.5 feet deep, enough to take a swim.";
            string lookDescription = "A beaver has built a damn here making the north side of the stream higher than the south side.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom22()
        {
            string examineDescription = "A fog rolls in from the west making it hard to see what is in front of you.  Occasionally an own makes a noise but other than that the forest is quiet.";
            string lookDescription = "Pine trees cover the landscape as far as the eye can see.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom23()
        {
            string examineDescription = "To the east the signs of a forest fire are undeniable.  The forest has begun to creep back in from the west and the circle of life continues.";
            string lookDescription = "Burned trees stumps stretch to the east as far as the eye can see.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom24()
        {
            string examineDescription = "Lifeless charred stumps give the forest an eerie lifeless feeling.  The soft sound of ashes breaking can be heard underfoot.  Yet there are signs the forest is recovering.  Small saplings are beginning to sprout up and reach for the sky.";
            string lookDescription = "Burned trees stumps stretch in all directions.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            room.AddMobileObjectToRoom(Owl(room));

            return room;
        }

        private IRoom GenerateRoom25()
        {
            string examineDescription = "Oak leaves blanket the forest floor as old oak trees tower hundreds of feet in the air.  A small set of fairy mushrooms form a circle about a foot in diameter by a large oak.";
            string lookDescription = "Giant oaks tower high above your head.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom26()
        {
            string examineDescription = "Several small pine trees are trying to catch sunlight so they can grow up to be mighty trees.";
            string lookDescription = "A mixture of oak and redwoods grow high above the few pine trees trying to get sunlight amongst all the shade.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom27()
        {
            string examineDescription = "The stream for some reason makes a sharp turn here to the east.  For a moment you ponder why it would decide to turn like that but decide its questions like that are best left the philosophers.";
            string lookDescription = "A stream makes a sharp turn flowing from the north to the east.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom28()
        {
            string examineDescription = "The rocks range anywhere from 6 feet in diameter to 7 inches.  Each has been worn smooth from the passage of time in the stream.  Moss covers each rock giving each rock a slightly softer look.";
            string lookDescription = "Large moss covered rocks lie in the stream making walking in the stream bed a tricky task.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom29()
        {
            string examineDescription = "The waterfall is 10 feet tall and is split into 3 different channels.  The largest being the middle.  The two sides do not carry as much water as the main with the left carrying the least.";
            string lookDescription = "Water from the west stream pours down a 10 foot ravine and then heads to the south.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom30()
        {
            string examineDescription = "The tree has seared marks where it was struck by lightning and has split the tree down the middle.  The power to split such a huge tree down the middle is staggering to the mind.";
            string lookDescription = "A seared fallen tree lays across your path.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom31()
        {
            string examineDescription = "Pine trees grow up twisted and crooked along a ridge looking down over a burned out forest.  Spots of green among the sea of blank show where new trees are starting to grow on the forest below.";
            string lookDescription = "Gnarly pine trees grow along a natural ridge to the east.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom32()
        {
            string examineDescription = "You stand in the middle of a burned forest yet before you stands an untouched oak tree.  Not a single leaf is singed and grass extends 5 feet around the base then stops giving way to burned ashes.";
            string lookDescription = "Before an unburned tree.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            room.AddMobileObjectToRoom(Crow(room));

            return room;
        }

        private IRoom GenerateRoom33()
        {
            string examineDescription = "You stand at a vista looking out at the forest to the north.  The lush green forest looks like an emerald city that stretches to the horizon.";
            string lookDescription = "An vista to the forest.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            room.AddMobileObjectToRoom(Squirrel(room));

            return room;
        }

        private IRoom GenerateRoom34()
        {
            string examineDescription = "Hardly any light can enter from above making even the day look almost like night.  This part of the forest is called the black forest.";
            string lookDescription = "Dark green pine trees grow so close together that the block out any light from above";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }


        private IRoom GenerateRoom35()
        {
            string examineDescription = "You stand by a small pond that has a tributary to the north and a distributary to the south.  A few small lily pad grow on the west side of the blue green water.";
            string lookDescription = "A small pond.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            room.AddMobileObjectToRoom(Fish(room));

            return room;
        }


        private IRoom GenerateRoom36()
        {
            string examineDescription = "You have come across what is left of Fort Woodbrook.  Now it has been abandoned and the forest has grown up around it.";
            string lookDescription = "The once mighty Fort Woodbrook stands to the north.";
            string shortDescription = "Fort Woodbrook";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }


        private IRoom GenerateRoom37()
        {
            string examineDescription = "The trees here reach up the sky but have become so large that they have bent back down to the ground under their own weight.  Moss grows down from the trees as if trying to reach the ground.";
            string lookDescription = "Tangled trees reach up to the sky before bending back to the ground.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom38()
        {
            string examineDescription = "The wagon trail head to the west and east as far as the eye can see.  The cooking fire is long since out and impossible to tell how long.";
            string lookDescription = "Several sets of wagon trails can be seen running east and west.  Ashes of a cooking fire sit in a dug out fire pit.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom39()
        {
            string examineDescription = "Deep groves from many wagon trips still scar the forest floor.  It is hard to tell if the trail is still used or not though.";
            string lookDescription = "An old wagon trail runs east and west through the forest here.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom40()
        {
            string examineDescription = "The wagon trail heading to the wast is the main feature here.  To the east it gets lost in as it transitioned from softer soil to a harder stone ground.";
            string lookDescription = "A wagon trail can be seen heading to the west but fades into the forest to the east.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom41()
        {
            string examineDescription = "This does remind you of something out a painting.  The painting hanging over reception desk at the hospital.  Other than the fallen log it does appear to be the same place.";
            string lookDescription = "The forest is light and airy with not much over growth, almost as if something out of a painting.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom42()
        {
            string examineDescription = "Something that smells bad can be smelt in the air.";
            string lookDescription = "The forest is sparse as if trying to get away from what ever is creating that awful smell.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom43()
        {
            string examineDescription = "Something near by has died and is rotting.";
            string lookDescription = "The forest is green and lush while the foul stench of death can be smelt near by.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom44()
        {
            string examineDescription = "Something that smells bad is near by.";
            string lookDescription = "A beautiful stretch of flowers grows on the forest floor.  You would consider staying except for the foul stench you smell.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom45()
        {
            string examineDescription = "The shallow water of the stream run slowly down stream.  Small tadpoles can be seen swimming to and fro in the water.";
            string lookDescription = "A small stream runs north to the south here.  The water is just deep enough to get your feet wet.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom46()
        {
            string examineDescription = "Tinny flowers make the grass looks like a polka dot quilt of colors red, blue, yellow and white.";
            string lookDescription = "Fields of grass stretch out before you to the North, East and South while a wall of trees stands to the West.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom47()
        {
            string examineDescription = "The yellow flowers in the meadow make the hills look like waves of golden silk.  Its almost enough to make you want to lay down and sleep and dream of being an emperor.";
            string lookDescription = "The meadow has some small hills here.  Enough to make the ground look like a rippling banner.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom48()
        {
            string examineDescription = "Tall evergreens stand on either side of the meadow as if acting as color guards protecting the meadow.";
            string lookDescription = "A small part of the meadow has made its way into this part of the forest dividing the trees between north and south.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom49()
        {
            string examineDescription = "Some type of large animal with claws appears to be marking the trees around here.";
            string lookDescription = "The trees around here have large gashes on the trunks.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom50()
        {
            string examineDescription = "Some of the bushes have tufts of black hair on them.";
            string lookDescription = "While all the vegetation looks healthy several of the small bushes are bent and broken.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom51()
        {
            string examineDescription = "The hole is approximately a foot deep and three feet across.  Several roots are sticking out of the edges of the hole.";
            string lookDescription = "The forest is a full of beautiful green moss growing on the forest floor.  In the center is large hole where something has been digging.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom52()
        {
            string examineDescription = "Each step you take you feet sink into the soft wet ground.  Moss grows up on all the sides of the trees making it impossible to use the old adage \"Moss grows on the north side of the tree.\"";
            string lookDescription = "The ground begins to get more boggy to the east and dryer to the west.  In spots to the east you can see standing water.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom53()
        {
            string examineDescription = "The stream slowly flows to the south but not before extending far to the west in what looks like a bog.";
            string lookDescription = "The stream widens out so much it almost stops.  To the west it seems to flow back and up hill as it always seeks new places to go.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom54()
        {
            string examineDescription = "Cat tails grow up on this side of the pond obstructing the view to the rest of the pond.";
            string lookDescription = "A small pond sits nestled in this corner of the meadow.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom55()
        {
            string examineDescription = "The pond appears to be about 10 feet deep and about 45 feet around.  The west side is covered in cat tails while the east side remains relatively plant free.";
            string lookDescription = "A small pond sits nestled in this corner of the meadow.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            room.AddMobileObjectToRoom(Chipmunk(room));

            return room;
        }

        private IRoom GenerateRoom56()
        {
            string examineDescription = "Pine trees grow to heights of 65 feet and block out most of the light from above leaving the forest floor cool, dark and covered mostly in pine needles and pine cones.";
            string lookDescription = "Forest trees grow to incredible heights as they try to out grow each other to get the most sunlight.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom57()
        {
            string examineDescription = "Beyond the scat you almost stepped in... never mind you did, the grass grows to an almost emerald color.  Signs of animals eating the grass can be seen in spots here and there.";
            string lookDescription = "Some scat lies scattered around the forest floor here.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            room.AddMobileObjectToRoom(Owl(room));

            return room;
        }

        private IRoom GenerateRoom58()
        {
            string examineDescription = "The entrance to the cave is dark and you can only see a few feet in.";
            string lookDescription = "A large mound stands about 5 feet tall and has a rather large opening on the north side.  Several sets of prints can be seen going in and out of the cave.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom59()
        {
            string examineDescription = "Upon further inspection some of the trees that are not broken down instead have broken limbs.  Perhaps something big was trying to climb in them.";
            string lookDescription = "Several trees have broken down.  Some look small enough that wind could have done it but others look to be two big.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom60()
        {
            string examineDescription = "The white bark gives a interesting break of the green grass and the green leaves.";
            string lookDescription = "Aspen trees with their white bark dominate the forest here and provide a bright contrast to the brown bark of other trees.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom61()
        {
            string examineDescription = "The white bark gives a interesting break of the green grass and the green leaves.";
            string lookDescription = "Aspen trees with their white bark dominate the forest here and provide a bright contrast to the brown bark of other trees.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom62()
        {
            string examineDescription = "Small saplings grow ever deeper into the meadow encroaching inch by inch.";
            string lookDescription = "The forest has yielded this spot of land to the meadow before you.  Yet it seems to be taking it back as small saplings encroach ever further into the meadow lands.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom63()
        {
            string examineDescription = "Meadow grasses come to you waist and tickle at your skin as you walk.  A small pond can be seen to the north and a trail can be seen to the south.";
            string lookDescription = "Almost hidden at first a small trail leads into the forest to the south.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom64()
        {
            string examineDescription = "With the large amounts of rocks and low soil levels it is amazing the trees grow so well here.";
            string lookDescription = "Red moss speckles large rocks giving the forest a strange look.  Red moss contrast sharply with the gray rocks and the white trees.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom65()
        {
            string examineDescription = "The path has several prints but they are hard to distinguish from one another.";
            string lookDescription = "A worn path leads to the east through the trees and brush.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            room.AddMobileObjectToRoom(Bear(room));

            return room;
        }

        private IRoom GenerateRoom66()
        {
            string examineDescription = "The path north leads towards a hill.";
            string lookDescription = "A worn path leads to the north and west east through the trees and brush.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom67()
        {
            string examineDescription = "Several trees here have tufts of black hair.";
            string lookDescription = "The forest trees are green and healthy.  Each one growing tall and strong.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom68()
        {
            string examineDescription = "It has dense trees growing in all directions blocking out most of the forest.  A small spring bubbles up in the center and flows to the east.  A large tree stump has been move close the spring as if to allow some one to sit and watch the stream wash their troubles away.";
            string lookDescription = "This part of the forest is very tranquil with dense trees blocking most of the forest.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom69()
        {
            string examineDescription = "The stream has carved out a bit of a gorge here with walls that rise about ten feet on either side.  Damp moss lines the walls making climbing the walls tricky but not impossible.";
            string lookDescription = "A small water fall drop ten feet from the west where it splatters noisily onto rocks.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            room.AddMobileObjectToRoom(Fish(room));

            return room;
        }

        private IRoom GenerateRoom70()
        {
            string examineDescription = "At this point all the trees have begun to blend together making navigation difficult.  At least you can follow your bread crumb trail back... you have been dropping bread crumbs right?";
            string lookDescription = "Tall trees loom high above you in all directions.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            room.AddMobileObjectToRoom(Fox(room));

            return room;
        }

        private IRoom GenerateRoom71()
        {
            string examineDescription = "Some type of animal is using this area as a path way through the forest.";
            string lookDescription = "A game trail runs north and south through the forest.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom72()
        {
            string examineDescription = "There are several animal carcases that have been hung from tree branches here.  They appear to have been dead when they were hung but it is strange to find this in the forest as there does not appear to be anyone to have put them there.";
            string lookDescription = "Several small animals have been hung from branches.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom73()
        {
            string examineDescription = "It is impossible to tell where the water goes.  The hole goes down several feet and then makes a sharp turn under a rock.";
            string lookDescription = "A small spring bubbles up and flows down a rock only to go down a small in the ground.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom74()
        {
            string examineDescription = "Several sets of older trees have had their tops removed.  Their cut off height ascends in an arc from almost at the ground where you stand to a height of over 50 feet.";
            string lookDescription = "The forest is a mixture of normal young trees and older ones that have had their tops removed.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom75()
        {
            string examineDescription = "Almost missed at first the depressions range from ten to fifteen feet in length and are six to nine inches in depth.";
            string lookDescription = "The forest has several large depressions here of unusual size and shape.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom76()
        {
            string examineDescription = "Almost completely covered in vines the signs of a long ago battle sit here whose details are lost with time.";
            string lookDescription = "The forest is trying to bury a large hammer and sword fused together.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IItem SwordHammer()
        {
            string examineDescription = "The sword appears to be made of an unknown type of metal that cut into the iron hammer but became lodged.  The war hammer runes can still be seen quite clearly even with the large amount of weathering that has occurred.  The sword still is as sharp as ever and looks to be untouched by time.";
            string lookDescription = "Each weapon is colossal and could wielded only be something equally as big.  The hammer is impressive at over 30 feet long had to be wielded buy something equally as big while the sword is longer yet.  The chaos that these two fighters must have caused makes you glad they are long gone.";
            string sentenceDescription = "sword fused with a hammer";
            string shortDescription = "A sword fused with a hammer lies partly covered in vines.";

            IItem item = CreateItem<IItem>(examineDescription, lookDescription, sentenceDescription, shortDescription);
            item.Attributes.Add(Item.ItemAttribute.NoGet);
            item.KeyWords.Add("sword");
            item.KeyWords.Add("hammer");

            

            return item;
        }

        private IRoom GenerateRoom77()
        {
            string examineDescription = "The water is cool and clear with small colorful fish swimming through it.";
            string lookDescription = "A stream runs from the north and to the east and through the forest trees.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom78()
        {
            string examineDescription = "A large tree has fallen into the stream creating a small \"ramp\" that allows animals to go down and drink water.";
            string lookDescription = "The water make another turn to the south again before flowing out of sight.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            return room;
        }

        private IRoom GenerateRoom79()
        {
            string examineDescription = "The trees bend inward towards the path almost trying to hide it and definably making it hard to traverse.";
            string lookDescription = "A small path runs east and west.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            room.AddMobileObjectToRoom(Crow(room));

            return room;
        }

        private IRoom GenerateRoom80()
        {
            string examineDescription = "There are a large amount of tracks leading into and out of the cave.";
            string lookDescription = "A cooking fire burns next to an entrance of a cave.";
            string shortDescription = "Deep Wood Forest";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription, 5);

            INonPlayerCharacter npc = KolboldGuard(room);
            room.AddMobileObjectToRoom(npc);

            npc = KolboldGuard(room);
            room.AddMobileObjectToRoom(npc);

            return room;
        }

        private INonPlayerCharacter KolboldGuard(IRoom room)
        {
            string corpseDescription;
            string examineDescription;
            string lookDescription;
            string sentenceDescription;
            string shortDescription;

            corpseDescription = "He died while defending his den and did not run like most kobolds would.";
            examineDescription = "The guard stands about 3 feet tall and is lizard like in his features.";
            lookDescription = "The kobold has a few scars where it has been through tough training.";
            sentenceDescription = "kobold guard";
            shortDescription = "The guard snarls at you trying to make you leave but it does not advance from the entrance.";
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 7, corpseDescription);
            npc.KeyWords.Add("Kobold");
            npc.KeyWords.Add("Guard");
            IGuard guardPersonality = new Guard(Direction.South);
            npc.Personalities.Add(guardPersonality);

            examineDescription = "The shield is surprisingly light for it's size but also feels less durable than originally anticipated.";
            lookDescription = "The shield is made of a wooden ring with leather hides stretched across.";
            sentenceDescription = "a leather shield";
            shortDescription = "A well made leather shields.";
            IShield shield = CreateShield(7, examineDescription, lookDescription, sentenceDescription, shortDescription, new Leather());
            shield.KeyWords.Add("leather");
            shield.KeyWords.Add("shield");
            npc.AddEquipment(shield);

            examineDescription = "The point of this spear is fashioned from animal and lashed to a large stick.";
            lookDescription = "A spear crafted from animal bone and a stick.";
            sentenceDescription = "a spear";
            shortDescription = "A hastily made weapon made of readily available materials.";
            IWeapon weapon = CreateWeapon(WeaponType.Spear, 7, examineDescription, lookDescription, sentenceDescription, shortDescription);
            weapon.KeyWords.Add("spear");
            IDamage damage = new Objects.Damage.Damage();
            damage.Type = DamageType.Pierce;
            weapon.DamageList.Add(damage);
            weapon.FinishLoad();
            npc.AddEquipment(weapon);

            examineDescription = "The bracers are fairly plain but are well made.";
            lookDescription = "The bracers extend up the wearers arm a good ways giving the user extra protection.";
            sentenceDescription = "a pair of leather bracers";
            shortDescription = "A well made pair leather bracers.";
            IArmor armor = CreateArmor(AvalableItemPosition.Arms, 7, examineDescription, lookDescription, sentenceDescription, shortDescription, new Leather());
            armor.KeyWords.Add("leather");
            armor.KeyWords.Add("bracer");
            npc.AddEquipment(armor);

            armor.ExamineDescription = "The vest is a light brown natural leather color. It has four large pockets on the front and the top left one is torn slightly.";
            armor.LookDescription = "The leather vest looks to be as utilitarian as protectant.";
            armor.SentenceDescription = "a leather vest";
            armor.ShortDescription = "A leather vest with several pockets.";
            armor = CreateArmor(AvalableItemPosition.Body, 7, examineDescription, lookDescription, sentenceDescription, shortDescription, new Leather());
            armor.KeyWords.Add("leather");
            armor.KeyWords.Add("vest");
            npc.AddEquipment(armor);
            return npc;
        }
        #endregion Rooms

        #region Animals
        private INonPlayerCharacter Owl(IRoom room)
        {
            string corpseDescription = "This once beautiful owl could mistaken for sleeping if not for the way its body is twisted.";
            string examineDescription = "Its hard to get a good view of the owl as it won't let you get near it.";
            string lookDescription = "The owl is brown in color with some black feathers for camouflage.";
            string sentenceDescription = "an owl";
            string shortDescription = "An owl looks at you as you walk through the forest.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 7, corpseDescription);
            npc.Personalities.Add(new Wanderer());
            npc.KeyWords.Add("Owl");

            return npc;
        }

        private INonPlayerCharacter Mouse(IRoom room)
        {
            string corpseDescription = "This tiny mouse looks cute even in death.";
            string examineDescription = "You would need to catch it first before you could examine it.";
            string lookDescription = "The mouse is {color} in color little pink nose.";
            string sentenceDescription = "a mouse";
            string shortDescription = "The mouse skitters away as you get close.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Other, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 7, corpseDescription);
            npc.Personalities.Add(new Wanderer());
            npc.KeyWords.Add("Mouse");
            npc.FlavorOptions.Add("color", new List<string>() { "brown", "black", "white" });

            return npc;
        }

        private INonPlayerCharacter Squirrel(IRoom room)
        {
            string corpseDescription = "The bushy tail on this squirrel could make a nice pipe cleaner.";
            string examineDescription = "It has a long fluffy tail and gray fur on its back.  The underside is a pale white like snow.";
            string lookDescription = "The Squirrel is runs to and fro looking for nuts.";
            string sentenceDescription = "a squirrel";
            string shortDescription = "A squirrel looks at you for a moment before choosing to ignore you.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Other, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 7, corpseDescription);
            npc.Personalities.Add(new Wanderer());
            npc.KeyWords.Add("Squirrel");
            return npc;
        }

        private INonPlayerCharacter Crow(IRoom room)
        {
            string corpseDescription = "If you killed three crows you could say you murdered a murder of crows.";
            string examineDescription = "It seems to have been born of the night with black feathers, feet and beak. The small black beady eyes are the only thing to reflect any light.";
            string lookDescription = "As you and the crow stare at each other it starts crowing loudly as trying to win a staring contest by making you look away.";
            string sentenceDescription = "a crow";
            string shortDescription = "A black as night crow calls out a warning as you approach.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Other, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 7, corpseDescription);
            npc.Personalities.Add(new Wanderer());
            npc.KeyWords.Add("Crow");

            return npc;
        }

        private INonPlayerCharacter Fox(IRoom room)
        {
            string corpseDescription = "This fox looks to have put up a good fight was bested in the end.";
            string examineDescription = "The fur on the fox looks soft but you will not be able to get close enough to it while it has a choice.";
            string lookDescription = "A red fox scurries along trying to catch mice.";
            string sentenceDescription = "a fox";
            string shortDescription = "A small fox looks at you.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Other, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 7, corpseDescription);
            npc.Personalities.Add(new Wanderer());
            npc.KeyWords.Add("Fox");

            return npc;
        }

        private INonPlayerCharacter Bear(IRoom room)
        {
            string examineDescription = "It looks like a giant black teddy bear but you know this is one teddy you don't want to get a bear hug from.";
            string lookDescription = "The black bear looks to be thirty two inches long weigh over 200 lbs.";
            string sentenceDescription = "a black bear";
            string shortDescription = "The black bear looks at you curiously but cautiously.";
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Other, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 10);
            npc.Personalities.Add(new Wanderer());

            npc.KeyWords.Add("Bear");

            return npc;
        }

        private INonPlayerCharacter Chipmunk(IRoom room)
        {
            string examineDescription = "For a minute it cocks it head to the side and stares at you as if trying to decide if it could fit you in its cheek.";
            string lookDescription = "A little chipmunk runs underneath your feet with its fat cheeks stuffed full of food.";
            string sentenceDescription = "a chipmunk";
            string shortDescription = "A chipmunk with two full cheeks stands here.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Other, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 7);
            npc.Personalities.Add(new Wanderer());
            npc.KeyWords.Add("Chipmunk");

            return npc;
        }

        private INonPlayerCharacter Fish(IRoom room)
        { 
            string examineDescription = "The top of the fish is a bit green in color on top and a bit of orange on its belly.";
            string lookDescription = "It is a silver fish with a blue spot behind its gill.";
            string sentenceDescription = "a chipmunk";
            string shortDescription = "A small fish swims back and forth in the stream";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Other, room, examineDescription, lookDescription,sentenceDescription, shortDescription, 7);
            npc.KeyWords.Add("Fish");

            IWanderer wanderer = new Wanderer();
            wanderer.NavigableRooms.Add(new RoomId(9, 35));
            wanderer.NavigableRooms.Add(new RoomId(9, 27));
            wanderer.NavigableRooms.Add(new RoomId(9, 28));
            wanderer.NavigableRooms.Add(new RoomId(9, 29));
            wanderer.NavigableRooms.Add(new RoomId(9, 21));
            wanderer.NavigableRooms.Add(new RoomId(9, 13));
            wanderer.NavigableRooms.Add(new RoomId(9, 5));
            wanderer.NavigableRooms.Add(new RoomId(9, 45));
            wanderer.NavigableRooms.Add(new RoomId(9, 53));
            wanderer.NavigableRooms.Add(new RoomId(9, 61));
            wanderer.NavigableRooms.Add(new RoomId(9, 69));
            wanderer.NavigableRooms.Add(new RoomId(9, 77));
            wanderer.NavigableRooms.Add(new RoomId(9, 78));

            npc.Personalities.Add(wanderer);

            return npc;
        }
        #endregion Animals

        private void ConnectRooms()
        {
            ZoneHelper.ConnectZone(Zone.Rooms[1], Direction.West, 9, 80);
            ZoneHelper.ConnectZone(Zone.Rooms[36], Direction.North, 10, 1);
            ZoneHelper.ConnectZone(Zone.Rooms[80], Direction.South, 12, 1);
            ZoneHelper.ConnectZone(Zone.Rooms[80], Direction.East, 17, 1);

            for (int h = 0; h < 5; h++)
            {
                for (int i = 1; i < 9; i++)
                {
                    int baseRoom = i + h * 8;
                    if (i != 8)
                    {
                        ZoneHelper.ConnectRoom(Zone.Rooms[baseRoom], Direction.East, Zone.Rooms[baseRoom + 1]);
                    }
                    if (h != 4)
                    {
                        ZoneHelper.ConnectRoom(Zone.Rooms[baseRoom], Direction.North, Zone.Rooms[baseRoom + 8]);
                    }
                }
            }

            for (int h = 0; h < 5; h++)
            {
                for (int i = 1; i < 9; i++)
                {
                    int baseRoom = 72 + i - h * 8;
                    if (i != 8)
                    {
                        ZoneHelper.ConnectRoom(Zone.Rooms[baseRoom], Direction.East, Zone.Rooms[baseRoom + 1]);
                    }
                    if (h != 4)
                    {
                        ZoneHelper.ConnectRoom(Zone.Rooms[baseRoom], Direction.North, Zone.Rooms[baseRoom - 8]);
                    }
                    else
                    {
                        ZoneHelper.ConnectRoom(Zone.Rooms[baseRoom], Direction.North, Zone.Rooms[i]);
                    }
                }
            }
        }
    }
}
