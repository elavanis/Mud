using System;
using System.Collections.Generic;
using System.Text;
using MiscShared;
using Objects;
using Objects.Effect;
using Objects.Effect.Interface;
using Objects.Interface;
using Objects.Item;
using Objects.Item.Interface;
using Objects.Item.Items;
using Objects.Item.Items.Custom.UnderGrandViewCastle;
using Objects.Item.Items.Interface;
using Objects.Language;
using Objects.Magic.Enchantment;
using Objects.Magic.Interface;
using Objects.Material.Materials;
using Objects.Mob;
using Objects.Mob.Interface;
using Objects.Personality;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using Shared.Sound;
using Shared.Sound.Interface;
using static Objects.Global.Direction.Directions;
using static Objects.Item.Item;
using static Shared.TagWrapper.TagWrapper;

namespace GenerateZones.Zones.UnderGrandView
{
    public class UnderGrandViewCastle : BaseZone, IZoneCode
    {
        public UnderGrandViewCastle() : base(25)
        {
        }

        public IZone Generate()
        {
            Zone.Name = nameof(UnderGrandViewCastle);

            BuildRoomsViaReflection(this.GetType());

            ConnectRooms();

            return Zone;
        }

        #region Rooms
        private IRoom GenerateRoom1()
        {
            string examineDescription = "Upon closer inspection the few spots that have not been eaten indicate this was a fine piece of furniture but now its only use is firewood.";
            string lookDescription = "Remnants of a wooden chair have been gnawed on by animals to the point that it hard to tell what it once was.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom2()
        {
            string examineDescription = "Near the clay pieces are the remnants of some wood.  Perhaps maybe this was a cupboard that fell over spilling the clay vessels onto the floor.";
            string lookDescription = "Broken pieces of clay lay shattered on the floor.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom3()
        {
            string examineDescription = "The straw matts are torn and tattered and generally decayed but you can tell what they once were.";
            string lookDescription = "Straw matts lay on the floor in this corner of the room.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom4()
        {
            string examineDescription = "The white shimmering liquid cast reflections on to chamber wall creating dancing lines sparkle to and fro.";
            string lookDescription = "A shimmering pool of white glowing liquid fills the room with light.";
            string shortDescription = "Shimmering Pool";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddItemToRoom(RunicStatue());

            return room;
        }
        private IRoom GenerateRoom5()
        {
            string examineDescription = "The white shimmering liquid cast reflections on to chamber wall creating dancing lines sparkle to and fro.";
            string lookDescription = "A shimmering pool of white glowing liquid fills the room with light.";
            string shortDescription = "Shimmering Pool";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom6()
        {
            string examineDescription = "The white shimmering liquid cast reflections on to chamber wall creating dancing lines sparkle to and fro.";
            string lookDescription = "A shimmering pool of white glowing liquid fills the room with light.";
            string shortDescription = "Shimmering Pool";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddItemToRoom(RunicStatue());

            return room;
        }
        private IRoom GenerateRoom7()
        {
            string examineDescription = "The collapsed wall once had a colorful mosaic on it but has crumbled beyond any home of recognition now.";
            string lookDescription = "Part of the wall has collapsed here making the turn here more difficult to navigate.  Still the path is navigable allowing you to continue on.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom8()
        {
            string examineDescription = "Along the edge of the wall on the floor you can see what looks like dried up flower petals.  It would seem that these used to be flowers adjoining the walls.";
            string lookDescription = "Long dead vines still cling to a lattice on the walls.  They were well maintained in the day and were sure to add some color to the place but now they are just dry and rotting.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom9()
        {
            string examineDescription = "Some rusted hinges still cling to the stone entry way.  The wooden door is gone but the latch sits strewn on the ground nearby.";
            string lookDescription = "This looks like it might have been the entry way to a house at one point.  Stone tiles are on the floor to give it a entrance feel.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom10()
        {
            string examineDescription = "A medium size patch of moss has taken up residence near where the water drips from the ceiling above.  It is soft and green very inviting asking you to curl up on it and drift off to the sound of water dripping.";
            string lookDescription = "Drops of water occasionally fall onto what is left of a cooking hearth.  The ceiling above has a hole cut out where the smoke would rise up but no light can be seen filtering down from it.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom11()
        {
            string examineDescription = "This area would have been close to the fire.  Perhaps it was used for sleeping.  The animal skins might have been furs for sleeping.";
            string lookDescription = "Tatters of old animal furs lie on the floor.  At one time these would have kept a person warm but now they serve little purpose.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);
            room.AddItemToRoom(AnimalSkins());
            room.AddItemToRoom(AnimalSkins());

            return room;
        }
        private IRoom GenerateRoom12()
        {
            string examineDescription = "The white shimmering liquid cast reflections on to chamber wall creating dancing lines sparkle to and fro.";
            string lookDescription = "A shimmering pool of white glowing liquid fills the room with light.";
            string shortDescription = "Shimmering Pool";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom13()
        {
            string examineDescription = "While you can scoop up the white shimmering liquid it does not feel wet and does not cling to you like water does.";
            string lookDescription = "The liquid comes up to your waist and feels warmer than the cool dungeon.";
            string shortDescription = "Shimmering Pool";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom14()
        {
            string examineDescription = "The white shimmering liquid cast reflections on to chamber wall creating dancing lines sparkle to and fro.";
            string lookDescription = "A shimmering pool of white glowing liquid fills the room with light.";
            string shortDescription = "Shimmering Pool";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom15()
        {
            string examineDescription = "The doors seem to be in surprising good condition for all the dampness in this tunnel.";
            string lookDescription = "Once a large wooden pair of doors blocked the way to the west but have since fallen onto the floor.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            examineDescription = "As you approach the doors a soft blue light sputters and dies.  It repeats itself over and over.  As you move around the glowing follows you.  It is like some type of magic was cast on the door but with time has faded away.";
            lookDescription = "The doors are in surprising good condition for having spent such a long time on a damp tunnel floor.";
            string sentenceDescription = "doors";
            shortDescription = "A pair of large wooden doors have fallen off their hinges and lie on the floor.";
            IItem door = CreateItem(examineDescription, lookDescription, sentenceDescription, shortDescription);
            door.Attributes.Add(Item.ItemAttribute.NoGet);
            door.KeyWords.Add("door");

            room.AddItemToRoom(door);

            return room;
        }
        private IRoom GenerateRoom16()
        {
            string examineDescription = "Climbing up the collapsed ceiling rocks you can see up into the hole ten or so feet but nothing of interest is there.";
            string lookDescription = "A large part of the ceiling has fallen in on the north side of the tunnel.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom17()
        {
            string examineDescription = "The ceiling has only a few tiles left as most have fallen to the ground over time.";
            string lookDescription = "Small bits of mosaic tiles litter the floor.  Most have been broken with the fall and by underfoot of people who have traveled down here.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom18()
        {
            string examineDescription = "The dome seems to indicate at one point this cavern use to be on the surface and allowed sun light through during the day.";
            string lookDescription = "A large crystal dome still stands defiantly against the crushing weight of dirt above.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom19()
        {
            string examineDescription = "Dropping a small stone down the hole you count, 1, 2, 3, 4, 5.  The splash echoes up the hole and around the cavern.";
            string lookDescription = "A hole has been dug into the stone floor.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom20()
        {
            string examineDescription = "It is hard to tell what this wall was for.  It was only two feet tall so maybe a small cage or storage area.";
            string lookDescription = "A small bit of a stone wall remains creating a corner.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom21()
        {
            string examineDescription = "The bones have not been disturbed for many years but non the less time has not been kind to them making it impossible to tell what they one belonged to.";
            string lookDescription = "A few animal bones lay in the corner of what appears to once have been an animal pen.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom22()
        {
            string examineDescription = "The white shimmering liquid cast reflections on to chamber wall creating dancing lines sparkle to and fro.";
            string lookDescription = "A shimmering pool of white glowing liquid fills the room with light.";
            string shortDescription = "Shimmering Pool";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddItemToRoom(RunicStatue());

            return room;
        }
        private IRoom GenerateRoom23()
        {
            string examineDescription = "The white shimmering liquid cast reflections on to chamber wall creating dancing lines sparkle to and fro.";
            string lookDescription = "A shimmering pool of white glowing liquid fills the room with light.";
            string shortDescription = "Shimmering Pool";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom24()
        {
            string examineDescription = "The white shimmering liquid cast reflections on to chamber wall creating dancing lines sparkle to and fro.";
            string lookDescription = "A shimmering pool of white glowing liquid fills the room with light.";
            string shortDescription = "Shimmering Pool";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddItemToRoom(RunicButton());

            return room;
        }
        private IRoom GenerateRoom25()
        {
            string examineDescription = "The tapestries each have a silver emblem of a star and moon.";
            string lookDescription = "A pair of purple tapestries line the walls of the hallway.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom26()
        {
            string examineDescription = "The tapestries each have a gold emblem of a pair of minotaurs with a tree between them.";
            string lookDescription = "A pair of red tapestries line the walls of the hallway.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom27()
        {
            string examineDescription = "Each pillar while plain in design is ornately decorated with embedded gems and metals causing it to sparkle and flash as you look examine it.";
            string lookDescription = "A row of stone pillars run north and south supporting the massive chamber's ceiling.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom28()
        {
            string examineDescription = "The platform has four steps each narrower then the one beneath it.";
            string lookDescription = "A large oversized gold throne sits upon an elevated platform.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddItemToRoom(Throne());

            return room;
        }
        private IRoom GenerateRoom29()
        {
            string examineDescription = "Each pillar while plain in design is ornately decorated with embedded gems and metals causing it to sparkle and flash as you look examine it.";
            string lookDescription = "A row of stone pillars run north and south supporting the massive chamber's ceiling.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom30()
        {
            string examineDescription = "The point of the archway has a keystone that holds the whole thing in place.";
            string lookDescription = "White stone archways rise from floor to a point in the ceiling.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom31()
        {
            string examineDescription = "An elaborate door way extends to the east.";
            string lookDescription = "The area is decorated with murals painted on the walls and there are some stone seats built up from the floor here.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom32()
        {
            string examineDescription = "The stone statues appear to have been carved in place from the existing rock.";
            string lookDescription = "Stone statues of minotaurs rise thirty feet in the air crossing battle axes above your head.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom33()
        {
            string examineDescription = "The stone statues appear to have been carved in place from the existing rock.";
            string lookDescription = "Stone statues of minotaurs rise thirty feet in the air crossing battle axes above your head.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom34()
        {
            string examineDescription = "The stone statues appear to have been carved in place from the existing rock.";
            string lookDescription = "Stone statues of minitoaurs rise thirty feet in the air crossing battle axes above your head.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom35()
        {
            string examineDescription = "Each pillar while plain in design is ornately decorated with embedded gems and metals causing it to sparkle and flash as you look examine it.";
            string lookDescription = "A row of stone pillars run north and south supporting the massive chamber's ceiling.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom36()
        {
            string examineDescription = "The chandelier appears to be approximately fifteen feet across with three levels of candles.";
            string lookDescription = "A large iron chandelier hangs from the ceiling.  The candles have long since burned out.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom37()
        {
            string examineDescription = "Each pillar while plain in design is ornately decorated with embedded gems and metals causing it to sparkle and flash as you look examine it.";
            string lookDescription = "A row of stone pillars run north and south supporting the massive chamber's ceiling.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom38()
        {
            string examineDescription = "It is only upon closer inspection that you notice these trees are stone.  Who ever carved them did an magnificent job of making the stone life like.  Each leave is intricately carved with veins and delicately painted.  Each piece of bark looks and feels like real wood save the coldness of the stone.";
            string lookDescription = "Each tree extends up twenty or thirty feet into the air before exploding into a massive green canopy of leaves.";
            string shortDescription = "Underground forest";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);
            room.Sounds.Add(BirdsSound());

            return room;
        }
        private IRoom GenerateRoom39()
        {
            string examineDescription = "Large trees reach up toward the sky with there leaves forming a perfect canopy on either side of you.";
            string lookDescription = "A small path through a forest stretches out before you.";
            string shortDescription = "Underground forest";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);
            room.Sounds.Add(BirdsSound());

            return room;
        }
        private IRoom GenerateRoom40()
        {
            string examineDescription = "It is only upon closer inspection that you notice these trees are stone.  Who ever carved them did an magnificent job of making the stone life like.  Each leave is intricately carved with veins and delicately painted.  Each piece of bark looks and feels like real wood save the coldness of the stone.";
            string lookDescription = "Each tree extends up twenty or thirty feet into the air before exploding into a massive green canopy of leaves.";
            string shortDescription = "Underground forest";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);
            room.Sounds.Add(BirdsSound());

            return room;
        }
        private IRoom GenerateRoom41()
        {
            string examineDescription = "Each pillar while plain in design is ornately decorated with embedded gems and metals causing it to sparkle and flash as you look examine it.";
            string lookDescription = "A row of stone pillars run north and south supporting the massive chamber's ceiling.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom42()
        {
            string examineDescription = "The chandelier appears to be approximately fifteen feet across with three levels of candles.";
            string lookDescription = "A large iron chandelier hangs from the ceiling.  The candles have long since burned out.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom43()
        {
            string examineDescription = "Each pillar while plain in design is ornately decorated with embedded gems and metals causing it to sparkle and flash as you look examine it.";
            string lookDescription = "A row of stone pillars run north and south supporting the massive chamber's ceiling.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom44()
        {
            string examineDescription = "Aside immediate damage from the cave ins from the north the hallway seems in good shape.";
            string lookDescription = "The hallway to the north ends a mere foot after it begins in a a collapsed ceiling.  The path to the east is also collapsing.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom45()
        {
            string examineDescription = "The one statue that still stands has an inscription. \"Sir Malculms \"The Wall\"  Lost in the battle of widows while defiantly holding back a waves of advancing orcs so his troops could escape.";
            string lookDescription = "Part of the north wall has caved in, held up only by one statue that did not break and has held the line.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom46()
        {
            string examineDescription = "The faint sound of dirt can be heard falling into water through the crack in the floor.";
            string lookDescription = "Small avalanches of dirt continue to slide down the mound before disappearing in a crack in the floor.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom47()
        {
            string examineDescription = "Dirt from above still occasionally falls down and one day may fill the hallway.";
            string lookDescription = "The ceiling to the hallway has collapsed and dirt from above has fallen in.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom48()
        {
            string examineDescription = "The dirt from above has filled most of this cavern.  Several small tunnels lead off but most are impassable beyond a few feet.";
            string lookDescription = "A large mound of dirt fills the cavern.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom49()
        {
            string examineDescription = "Each statue is carved from stone and in a regal pose.";
            string lookDescription = "The hallway is lined with statues of knights in their armor.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom50()
        {
            string examineDescription = "Large trees reach up toward the sky with there leaves forming a perfect canopy on either side of you.";
            string lookDescription = "A small path through a forest stretches out before you.";
            string shortDescription = "Underground forest";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);
            room.Sounds.Add(BirdsSound());

            return room;
        }
        private IRoom GenerateRoom51()
        {
            string examineDescription = "Light from the sun warms your body but something is strange...  Its as if the sun never moves.";
            string lookDescription = "Light streams down from the sun high above on to the clearing.";
            string shortDescription = "Underground forest";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);
            room.Sounds.Add(BirdsSound());

            return room;
        }
        private IRoom GenerateRoom52()
        {
            string examineDescription = "Large trees reach up toward the sky with there leaves forming a perfect canopy on either side of you.";
            string lookDescription = "A small path through a forest stretches out before you.";
            string shortDescription = "Underground forest";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);
            room.Sounds.Add(BirdsSound());

            return room;
        }
        private IRoom GenerateRoom53()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom54()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom55()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom56()
        {
            string examineDescription = "Each pillar while plain in design is ornately decorated with embedded gems and metals causing it to sparkle and flash as you look examine it.";
            string lookDescription = "A row of stone pillars run north and south supporting the massive chamber's ceiling.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom57()
        {
            string examineDescription = "The chandelier appears to be approximately fifteen feet across with three levels of candles.";
            string lookDescription = "A large iron chandelier hangs from the ceiling.  The candles have long since burned out.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom58()
        {
            string examineDescription = "Each pillar while plain in design is ornately decorated with embedded gems and metals causing it to sparkle and flash as you look examine it.";
            string lookDescription = "A row of stone pillars run north and south supporting the massive chamber's ceiling.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom59()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom60()
        {
            string examineDescription = "The statues lances cross just below your head such that you would need to bow slightly in reverence to enter the chamber to the south.";
            string lookDescription = "A pair of stone statues have lances crossed above the entrance to the chamber to the south.";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom61()
        {
            string examineDescription = "It is only upon closer inspection that you notice these trees are stone.  Who ever carved them did an magnificent job of making the stone life like.  Each leave is intricately carved with veins and delicately painted.  Each piece of bark looks and feels like real wood save the coldness of the stone.";
            string lookDescription = "Each tree extends up twenty or thirty feet into the air before exploding into a massive green canopy of leaves.";
            string shortDescription = "Underground forest";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);
            room.Sounds.Add(BirdsSound());

            return room;
        }
        private IRoom GenerateRoom62()
        {
            string examineDescription = "Large trees reach up toward the sky with there leaves forming a perfect canopy on either side of you.";
            string lookDescription = "A small path through a forest stretches out before you.";
            string shortDescription = "Underground forest";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);
            room.Sounds.Add(BirdsSound());

            return room;
        }
        private IRoom GenerateRoom63()
        {
            string examineDescription = "It is only upon closer inspection that you notice these trees are stone.  Who ever carved them did an magnificent job of making the stone life like.  Each leave is intricately carved with veins and delicately painted.  Each piece of bark looks and feels like real wood save the coldness of the stone.";
            string lookDescription = "Each tree extends up twenty or thirty feet into the air before exploding into a massive green canopy of leaves.";
            string shortDescription = "Underground forest";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);
            room.Sounds.Add(BirdsSound());

            return room;
        }
        private IRoom GenerateRoom64()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom65()
        {
            string examineDescription = "Sarcophagi line the walls of the chamber.";
            string lookDescription = "A slight bit of fog covers the ground of this chamber.";
            string shortDescription = "Burial Chamber";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom66()
        {
            string examineDescription = "Sarcophagi line the walls of the chamber.";
            string lookDescription = "A slight bit of fog covers the ground of this chamber.";
            string shortDescription = "Burial Chamber";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom67()
        {
            string examineDescription = "Sarcophagi line the walls of the chamber.";
            string lookDescription = "A slight bit of fog covers the ground of this chamber.";
            string shortDescription = "Burial Chamber";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom68()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom69()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom70()
        {
            string examineDescription = "Sarcophagi line the walls of the chamber.";
            string lookDescription = "A slight bit of fog covers the ground of this chamber.";
            string shortDescription = "Burial Chamber";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom71()
        {
            string examineDescription = "The sarcophagus seems slightly out of place only because it ornately decorated while most everything else is plainly decorated.";
            string lookDescription = "The center of the chamber is dominated by a single sarcophagus.";
            string shortDescription = "Burial Chamber";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddItemToRoom(Sarcophagus());

            return room;
        }
        private IRoom GenerateRoom72()
        {
            string examineDescription = "Sarcophagi line the walls of the chamber.";
            string lookDescription = "A slight bit of fog covers the ground of this chamber.";
            string shortDescription = "Burial Chamber";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom73()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom74()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom75()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom76()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom77()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom78()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom79()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom80()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom81()
        {
            string examineDescription = "Sarcophagi line the walls of the chamber.";
            string lookDescription = "A slight bit of fog covers the ground of this chamber.";
            string shortDescription = "Burial Chamber";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom82()
        {
            string examineDescription = "Sarcophagi line the walls of the chamber.";
            string lookDescription = "A slight bit of fog covers the ground of this chamber.";
            string shortDescription = "Burial Chamber";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom83()
        {
            string examineDescription = "Sarcophagi line the walls of the chamber.";
            string lookDescription = "A slight bit of fog covers the ground of this chamber.";
            string shortDescription = "Burial Chamber";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom84()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom85()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom86()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom87()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom88()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom89()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom90()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom91()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom92()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom93()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom94()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom95()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom96()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom97()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom98()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom99()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom100()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom101()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom102()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom103()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom104()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom105()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom106()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom107()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom108()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom109()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom110()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom111()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom112()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom113()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom114()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom115()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom116()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom117()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom118()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom119()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom120()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom121()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom122()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom123()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom124()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom125()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom126()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom127()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom128()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom129()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom130()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom131()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom132()
        {
            string examineDescription = "";
            string lookDescription = "";
            string shortDescription = "Underground cavern";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        #endregion Rooms

        #region Sound
        private ISound BirdsSound()
        {
            ISound sound = new Sound();
            sound.Loop = true;
            sound.SoundName = string.Format("{0}\\{1}", Zone.Name, "Birds.mp3");
            return sound;
        }
        #endregion Sound

        #region Items
        private Container Sarcophagus()
        {
            string openMessage = "The lid of the sarcophagus slides off with a loud thud and cracks slightly.";
            string closeMessage = "Lifting the lid of the sarcophagus was quite hard and you are worn out leaving the lid slightly ajar.";
            string examineDescription = "The coins are made of different materials so it is hard to estimate their worth.";
            string lookDescription = "The head of coins have minotaurs on them and on the back different runes.";
            string sentenceDescription = "coins";
            string shortDescription = "A pile of coins.";

            Container sarcophagus = CreateContainer(openMessage, closeMessage, examineDescription, lookDescription, sentenceDescription, shortDescription);

            IItem money = CreateItem(examineDescription, lookDescription, sentenceDescription, shortDescription);
            money.Value = 1000;
            money.KeyWords.Add("coin");
            money.KeyWords.Add("coins");
         
            money.Enchantments.Add(LoadSkeletonMinotaur(65));
            money.Enchantments.Add(LoadSkeletonMinotaur(66));
            money.Enchantments.Add(LoadSkeletonMinotaur(67));
            money.Enchantments.Add(LoadSkeletonMinotaur(70));
            money.Enchantments.Add(LoadSkeletonMinotaur(72));
            money.Enchantments.Add(LoadSkeletonMinotaur(81));
            money.Enchantments.Add(LoadSkeletonMinotaur(82));
            money.Enchantments.Add(LoadSkeletonMinotaur(83));
            money.Enchantments.Add(CloseBurialChamberDoor(sarcophagus));
            money.Enchantments.Add(OpenBurialChamberDoor(sarcophagus));

            IEnchantment enchantment = new OpenEnchantment();
            enchantment.ActivationPercent = 100;
            enchantment.Effect = new Message();
            ISound sound = new Sound();
            sound.Loop = false;
            sound.SoundName = $"{Zone.Name}\\StoneCoffinOpen.mp3";
            enchantment.Effect.Sound = sound;
            IEffectParameter effectParameter = new EffectParameter();
            effectParameter.RoomMessage = new TranslationMessage("An unknown voice says \"Don't take anything and leave this place.\"", TagType.Communication);
            effectParameter.RoomId = new RoomId(Zone.Id, 71);
            enchantment.Parameter = effectParameter;


            sarcophagus.Attributes.Add(Item.ItemAttribute.NoGet);
            sarcophagus.Items.Add(money);
            sarcophagus.Opened = false;
            sarcophagus.KeyWords.Add("sarcophagus");
            sarcophagus.SentenceDescription = "sarcophagus";
            sarcophagus.ShortDescription = "A sarcophagus with a gold lid.";
            sarcophagus.LookDescription = "This sarcophagus is more ornately decorated then the others. Perhaps there is treasure in this one.";
            sarcophagus.ExamineDescription = "The gold lid is heavy but could be slid off.";
            sarcophagus.Enchantments.Add(enchantment);

            return sarcophagus;
        }

        private IArmor AnimalSkins()
        {
            string examineDescription = "The fact that no one would want to be around you makes you reconsider but maybe you could fashion some armor out of these.  You would not be the winner of a fashion show but the amount of material required would give you decent protection if not restrict you movement some.";
            string lookDescription = "The animal furs are tattered and give off a foul odor making them almost useless.";
            string sentenceDescription = "animals furs";
            string shortDescription = "A tatter of animal furs.";

            IArmor armor = CreateArmor(Equipment.AvalableItemPosition.Body, 5, examineDescription, lookDescription, sentenceDescription, shortDescription, new Leather());
            armor.Dexterity = -2;
            armor.Charisma = -5;
            armor.Bludgeon = 1.5M;
            armor.Necrotic = 1.5M;
            armor.KeyWords.Add("animal");
            armor.KeyWords.Add("fur");
            armor.KeyWords.Add("furs");

            return armor;
        }

        private IItem RunicStatue()
        {
            string examineDescription = "will be calculated later";
            string lookDescription = "The statue is of a priest chanting with their arms spread open.  It appears to be carved from the surrounding stone.";
            string sentenceDescription = "runic statue";
            string shortDescription = "A stone statue of a priest facing the pool of liquid.";

            RunicStatue statue = new RunicStatue(examineDescription, lookDescription, sentenceDescription, shortDescription);
            statue.ZoneId = Zone.Id;
            statue.Id = ItemId++;
            statue.ExamineDescription = statue.CalculateExamDescription();

            return statue;
        }

        private IItem RunicButton()
        {
            string examineDescription = "The ᚹ is carved into the top of the pedestal and can be pressed.";
            string lookDescription = "The stone pedestal rises up from the floor and has a single run on the top.";
            string sentenceDescription = "runic button";
            string shortDescription = "A large runic button on a stone pedestal.";

            RunicButton button = new RunicButton(examineDescription, lookDescription, sentenceDescription, shortDescription);
            button.ZoneId = Zone.Id;
            button.Id = ItemId++;


            return button;
        }

        private IItem Throne()
        {
            string examineDescription = "The gold throne is made entirely of solid gold save a single royal blue sapphire that is as big as a fist that is set at the top of the head rest.  It weights to much to move and must be worth a kings randsom.";
            string lookDescription = "The gold throne is quite large and ornate.";
            string sentenceDescription = "gold throne";
            string shortDescription = "A large gold throne sits here.";

            IItem button = CreateItem(examineDescription, lookDescription, sentenceDescription, shortDescription);
            button.ZoneId = Zone.Id;
            button.Id = ItemId++;
            button.Attributes.Add(ItemAttribute.NoGet);

            button.KeyWords.Add("throne");
            button.KeyWords.Add("gold");
         
            return button;
        }
        #endregion Items

        #region Enchantments
        private IEnchantment LoadSkeletonMinotaur(int roomId)
        {
            IEnchantment enchantment = new GetEnchantment();
            enchantment.ActivationPercent = 100;
            enchantment.Effect = new LoadMob() { RoomId = new BaseObjectId(Zone.Id, roomId) };
            enchantment.Parameter = new EffectParameter() { Performer = Skeleton() };
            return enchantment;
        }

        private IEnchantment CloseBurialChamberDoor(IBaseObject sarcophagus)
        {
            IEnchantment enchantment = new GetEnchantment() { MatchingContainerId = new BaseObjectId(sarcophagus) };
            enchantment.ActivationPercent = 100;
            enchantment.Effect = new CloseDoor();
            enchantment.Parameter = new EffectParameter() { RoomId = new BaseObjectId(Zone.Id, 66), Direction = Direction.North };
            return enchantment;
        }

        private IEnchantment OpenBurialChamberDoor(IBaseObject sarcophagus)
        {
            IEnchantment enchantment = new GetEnchantment() { MatchingContainerId = new BaseObjectId(sarcophagus) };
            enchantment.ActivationPercent = 100;
            enchantment.Effect = new OpenDoor();
            enchantment.Parameter = new EffectParameter() { RoomId = new BaseObjectId(Zone.Id, 66), Direction = Direction.North };
            return enchantment;
        }
        #endregion Enchantments

        #region Mobs
        private IMobileObject Skeleton()
        {
            string examineDescription = "The bones rattle slightly as they move toward you.";
            string lookDescription = "Red beady eyes burn with rage at the desecration of their tomb.";
            string sentenceDescription = "minotaur";
            string shortDescription = "A minotaur skeleton.";

            INonPlayerCharacter npc = new NonPlayerCharacter(null!, examineDescription, lookDescription, sentenceDescription, shortDescription);
            npc.Level = 23;
            npc.Personalities.Add(new Aggressive());
            npc.Personalities.Add(new Wanderer());
            npc.KeyWords.Add("skeleton");
            npc.KeyWords.Add("minotaur");

            return npc;
        }
        #endregion Mobs

        private void ConnectRooms()
        {
            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.East, Zone.Rooms[2]);
            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.South, Zone.Rooms[9]);
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.East, Zone.Rooms[3]);
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.South, Zone.Rooms[10]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.South, Zone.Rooms[11]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.East, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.South, Zone.Rooms[12]);
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.East, Zone.Rooms[6]);
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.South, Zone.Rooms[13]);
            ZoneHelper.ConnectRoom(Zone.Rooms[6], Direction.South, Zone.Rooms[14]);
            ZoneHelper.ConnectRoom(Zone.Rooms[7], Direction.East, Zone.Rooms[8]);
            ZoneHelper.ConnectRoom(Zone.Rooms[7], Direction.South, Zone.Rooms[18]);
            ZoneHelper.ConnectRoom(Zone.Rooms[8], Direction.East, Zone.Rooms[9]);
            ZoneHelper.ConnectRoom(Zone.Rooms[9], Direction.East, Zone.Rooms[10]);
            ZoneHelper.ConnectRoom(Zone.Rooms[9], Direction.South, Zone.Rooms[19]);
            ZoneHelper.ConnectRoom(Zone.Rooms[10], Direction.East, Zone.Rooms[11]);
            ZoneHelper.ConnectRoom(Zone.Rooms[10], Direction.South, Zone.Rooms[20]);
            ZoneHelper.ConnectRoom(Zone.Rooms[11], Direction.South, Zone.Rooms[21]);
            ZoneHelper.ConnectRoom(Zone.Rooms[12], Direction.East, Zone.Rooms[13]);
            ZoneHelper.ConnectRoom(Zone.Rooms[12], Direction.South, Zone.Rooms[22]);
            ZoneHelper.ConnectRoom(Zone.Rooms[13], Direction.East, Zone.Rooms[14]);
            ZoneHelper.ConnectRoom(Zone.Rooms[13], Direction.South, Zone.Rooms[23]);
            ZoneHelper.ConnectRoom(Zone.Rooms[14], Direction.East, Zone.Rooms[15]);
            ZoneHelper.ConnectRoom(Zone.Rooms[14], Direction.South, Zone.Rooms[24]);
            ZoneHelper.ConnectRoom(Zone.Rooms[15], Direction.East, Zone.Rooms[16]);
            ZoneHelper.ConnectRoom(Zone.Rooms[16], Direction.East, Zone.Rooms[17]);
            ZoneHelper.ConnectRoom(Zone.Rooms[17], Direction.East, Zone.Rooms[18]);
            ZoneHelper.ConnectRoom(Zone.Rooms[18], Direction.South, Zone.Rooms[25]);
            ZoneHelper.ConnectRoom(Zone.Rooms[19], Direction.East, Zone.Rooms[20]);
            ZoneHelper.ConnectRoom(Zone.Rooms[20], Direction.East, Zone.Rooms[21]);
            ZoneHelper.ConnectRoom(Zone.Rooms[22], Direction.East, Zone.Rooms[23]);
            ZoneHelper.ConnectRoom(Zone.Rooms[23], Direction.East, Zone.Rooms[24]);
            ZoneHelper.ConnectRoom(Zone.Rooms[25], Direction.South, Zone.Rooms[26]);
            ZoneHelper.ConnectRoom(Zone.Rooms[26], Direction.South, Zone.Rooms[30]);
            ZoneHelper.ConnectRoom(Zone.Rooms[27], Direction.East, Zone.Rooms[28]);
            ZoneHelper.ConnectRoom(Zone.Rooms[27], Direction.South, Zone.Rooms[35]);
            ZoneHelper.ConnectRoom(Zone.Rooms[28], Direction.East, Zone.Rooms[29]);
            ZoneHelper.ConnectRoom(Zone.Rooms[28], Direction.South, Zone.Rooms[36]);
            ZoneHelper.ConnectRoom(Zone.Rooms[29], Direction.South, Zone.Rooms[37]);
            ZoneHelper.ConnectRoom(Zone.Rooms[30], Direction.East, Zone.Rooms[31]);
            ZoneHelper.ConnectRoom(Zone.Rooms[30], Direction.South, Zone.Rooms[39]);
            ZoneHelper.ConnectRoom(Zone.Rooms[31], Direction.East, Zone.Rooms[32]);
            ZoneHelper.ConnectRoom(Zone.Rooms[31], Direction.South, Zone.Rooms[40]);
            ZoneHelper.ConnectRoom(Zone.Rooms[32], Direction.East, Zone.Rooms[33]);
            ZoneHelper.ConnectRoom(Zone.Rooms[33], Direction.East, Zone.Rooms[34]);
            ZoneHelper.ConnectRoom(Zone.Rooms[34], Direction.East, Zone.Rooms[35]);
            ZoneHelper.ConnectRoom(Zone.Rooms[35], Direction.East, Zone.Rooms[36]);
            ZoneHelper.ConnectRoom(Zone.Rooms[35], Direction.South, Zone.Rooms[41]);
            ZoneHelper.ConnectRoom(Zone.Rooms[36], Direction.East, Zone.Rooms[37]);
            ZoneHelper.ConnectRoom(Zone.Rooms[36], Direction.South, Zone.Rooms[42]);
            ZoneHelper.ConnectRoom(Zone.Rooms[37], Direction.South, Zone.Rooms[43]);
            ZoneHelper.ConnectRoom(Zone.Rooms[38], Direction.East, Zone.Rooms[39]);
            ZoneHelper.ConnectRoom(Zone.Rooms[38], Direction.South, Zone.Rooms[50]);
            ZoneHelper.ConnectRoom(Zone.Rooms[39], Direction.East, Zone.Rooms[40]);
            ZoneHelper.ConnectRoom(Zone.Rooms[39], Direction.South, Zone.Rooms[51]);
            ZoneHelper.ConnectRoom(Zone.Rooms[40], Direction.South, Zone.Rooms[52]);
            ZoneHelper.ConnectRoom(Zone.Rooms[41], Direction.East, Zone.Rooms[42]);
            ZoneHelper.ConnectRoom(Zone.Rooms[41], Direction.South, Zone.Rooms[56]);
            ZoneHelper.ConnectRoom(Zone.Rooms[42], Direction.East, Zone.Rooms[43]);
            ZoneHelper.ConnectRoom(Zone.Rooms[42], Direction.South, Zone.Rooms[57]);
            ZoneHelper.ConnectRoom(Zone.Rooms[43], Direction.South, Zone.Rooms[58]);
            ZoneHelper.ConnectRoom(Zone.Rooms[44], Direction.East, Zone.Rooms[45]);
            ZoneHelper.ConnectRoom(Zone.Rooms[44], Direction.South, Zone.Rooms[60]);
            ZoneHelper.ConnectRoom(Zone.Rooms[45], Direction.East, Zone.Rooms[46]);
            ZoneHelper.ConnectRoom(Zone.Rooms[46], Direction.East, Zone.Rooms[47]);
            ZoneHelper.ConnectRoom(Zone.Rooms[47], Direction.East, Zone.Rooms[48]);
            ZoneHelper.ConnectRoom(Zone.Rooms[48], Direction.East, Zone.Rooms[49]);
            ZoneHelper.ConnectRoom(Zone.Rooms[49], Direction.East, Zone.Rooms[50]);
            ZoneHelper.ConnectRoom(Zone.Rooms[50], Direction.East, Zone.Rooms[51]);
            ZoneHelper.ConnectRoom(Zone.Rooms[50], Direction.South, Zone.Rooms[61]);
            ZoneHelper.ConnectRoom(Zone.Rooms[51], Direction.East, Zone.Rooms[52]);
            ZoneHelper.ConnectRoom(Zone.Rooms[51], Direction.South, Zone.Rooms[62]);
            ZoneHelper.ConnectRoom(Zone.Rooms[52], Direction.East, Zone.Rooms[53]);
            ZoneHelper.ConnectRoom(Zone.Rooms[52], Direction.South, Zone.Rooms[63]);
            ZoneHelper.ConnectRoom(Zone.Rooms[53], Direction.East, Zone.Rooms[54]);
            ZoneHelper.ConnectRoom(Zone.Rooms[54], Direction.East, Zone.Rooms[55]);
            ZoneHelper.ConnectRoom(Zone.Rooms[55], Direction.East, Zone.Rooms[56]);
            ZoneHelper.ConnectRoom(Zone.Rooms[56], Direction.East, Zone.Rooms[57]);
            ZoneHelper.ConnectRoom(Zone.Rooms[57], Direction.East, Zone.Rooms[58]);
            ZoneHelper.ConnectRoom(Zone.Rooms[58], Direction.East, Zone.Rooms[59]);
            ZoneHelper.ConnectRoom(Zone.Rooms[59], Direction.South, Zone.Rooms[64]);
            ZoneHelper.ConnectRoom(Zone.Rooms[60], Direction.South, Zone.Rooms[66], new DoorInfo("stone", "", "", true, "The stone perfectly seals the entry way preventing anyone from entering or leaving.", true, true));
            ZoneHelper.ConnectRoom(Zone.Rooms[61], Direction.East, Zone.Rooms[62]);
            ZoneHelper.ConnectRoom(Zone.Rooms[62], Direction.East, Zone.Rooms[63]);
            ZoneHelper.ConnectRoom(Zone.Rooms[62], Direction.South, Zone.Rooms[68]);
            ZoneHelper.ConnectRoom(Zone.Rooms[64], Direction.South, Zone.Rooms[69]);
            ZoneHelper.ConnectRoom(Zone.Rooms[65], Direction.East, Zone.Rooms[66]);
            ZoneHelper.ConnectRoom(Zone.Rooms[65], Direction.South, Zone.Rooms[70]);
            ZoneHelper.ConnectRoom(Zone.Rooms[66], Direction.East, Zone.Rooms[67]);
            ZoneHelper.ConnectRoom(Zone.Rooms[66], Direction.South, Zone.Rooms[71]);
            ZoneHelper.ConnectRoom(Zone.Rooms[67], Direction.South, Zone.Rooms[72]);
            ZoneHelper.ConnectRoom(Zone.Rooms[68], Direction.South, Zone.Rooms[76]);
            ZoneHelper.ConnectRoom(Zone.Rooms[69], Direction.South, Zone.Rooms[80]);
            ZoneHelper.ConnectRoom(Zone.Rooms[70], Direction.East, Zone.Rooms[71]);
            ZoneHelper.ConnectRoom(Zone.Rooms[70], Direction.South, Zone.Rooms[81]);
            ZoneHelper.ConnectRoom(Zone.Rooms[71], Direction.East, Zone.Rooms[72]);
            ZoneHelper.ConnectRoom(Zone.Rooms[71], Direction.South, Zone.Rooms[82]);
            ZoneHelper.ConnectRoom(Zone.Rooms[72], Direction.South, Zone.Rooms[83]);
            ZoneHelper.ConnectRoom(Zone.Rooms[73], Direction.East, Zone.Rooms[74]);
            ZoneHelper.ConnectRoom(Zone.Rooms[73], Direction.South, Zone.Rooms[84]);
            ZoneHelper.ConnectRoom(Zone.Rooms[74], Direction.East, Zone.Rooms[75]);
            ZoneHelper.ConnectRoom(Zone.Rooms[74], Direction.South, Zone.Rooms[85]);
            ZoneHelper.ConnectRoom(Zone.Rooms[75], Direction.South, Zone.Rooms[86]);
            ZoneHelper.ConnectRoom(Zone.Rooms[76], Direction.South, Zone.Rooms[88]);
            ZoneHelper.ConnectRoom(Zone.Rooms[77], Direction.East, Zone.Rooms[78]);
            ZoneHelper.ConnectRoom(Zone.Rooms[77], Direction.South, Zone.Rooms[91]);
            ZoneHelper.ConnectRoom(Zone.Rooms[78], Direction.East, Zone.Rooms[79]);
            ZoneHelper.ConnectRoom(Zone.Rooms[78], Direction.South, Zone.Rooms[92]);
            ZoneHelper.ConnectRoom(Zone.Rooms[79], Direction.South, Zone.Rooms[93]);
            ZoneHelper.ConnectRoom(Zone.Rooms[80], Direction.South, Zone.Rooms[95]);
            ZoneHelper.ConnectRoom(Zone.Rooms[81], Direction.East, Zone.Rooms[82]);
            ZoneHelper.ConnectRoom(Zone.Rooms[82], Direction.East, Zone.Rooms[83]);
            ZoneHelper.ConnectRoom(Zone.Rooms[84], Direction.East, Zone.Rooms[85]);
            ZoneHelper.ConnectRoom(Zone.Rooms[84], Direction.South, Zone.Rooms[97]);
            ZoneHelper.ConnectRoom(Zone.Rooms[85], Direction.East, Zone.Rooms[86]);
            ZoneHelper.ConnectRoom(Zone.Rooms[85], Direction.South, Zone.Rooms[98]);
            ZoneHelper.ConnectRoom(Zone.Rooms[86], Direction.East, Zone.Rooms[87]);
            ZoneHelper.ConnectRoom(Zone.Rooms[86], Direction.South, Zone.Rooms[99]);
            ZoneHelper.ConnectRoom(Zone.Rooms[87], Direction.East, Zone.Rooms[88]);
            ZoneHelper.ConnectRoom(Zone.Rooms[88], Direction.East, Zone.Rooms[89]);
            ZoneHelper.ConnectRoom(Zone.Rooms[88], Direction.South, Zone.Rooms[100]);
            ZoneHelper.ConnectRoom(Zone.Rooms[89], Direction.East, Zone.Rooms[90]);
            ZoneHelper.ConnectRoom(Zone.Rooms[90], Direction.East, Zone.Rooms[91]);
            ZoneHelper.ConnectRoom(Zone.Rooms[91], Direction.East, Zone.Rooms[92]);
            ZoneHelper.ConnectRoom(Zone.Rooms[91], Direction.South, Zone.Rooms[101]);
            ZoneHelper.ConnectRoom(Zone.Rooms[92], Direction.East, Zone.Rooms[93]);
            ZoneHelper.ConnectRoom(Zone.Rooms[92], Direction.South, Zone.Rooms[102]);
            ZoneHelper.ConnectRoom(Zone.Rooms[93], Direction.South, Zone.Rooms[103]);
            ZoneHelper.ConnectRoom(Zone.Rooms[94], Direction.East, Zone.Rooms[95]);
            ZoneHelper.ConnectRoom(Zone.Rooms[94], Direction.South, Zone.Rooms[104]);
            ZoneHelper.ConnectRoom(Zone.Rooms[95], Direction.East, Zone.Rooms[96]);
            ZoneHelper.ConnectRoom(Zone.Rooms[95], Direction.South, Zone.Rooms[105]);
            ZoneHelper.ConnectRoom(Zone.Rooms[96], Direction.South, Zone.Rooms[106]);
            ZoneHelper.ConnectRoom(Zone.Rooms[97], Direction.East, Zone.Rooms[98]);
            ZoneHelper.ConnectRoom(Zone.Rooms[98], Direction.East, Zone.Rooms[99]);
            ZoneHelper.ConnectRoom(Zone.Rooms[99], Direction.South, Zone.Rooms[107]);
            ZoneHelper.ConnectRoom(Zone.Rooms[100], Direction.South, Zone.Rooms[108]);
            ZoneHelper.ConnectRoom(Zone.Rooms[101], Direction.East, Zone.Rooms[102]);
            ZoneHelper.ConnectRoom(Zone.Rooms[102], Direction.East, Zone.Rooms[103]);
            ZoneHelper.ConnectRoom(Zone.Rooms[104], Direction.East, Zone.Rooms[105]);
            ZoneHelper.ConnectRoom(Zone.Rooms[104], Direction.South, Zone.Rooms[109]);
            ZoneHelper.ConnectRoom(Zone.Rooms[105], Direction.East, Zone.Rooms[106]);
            ZoneHelper.ConnectRoom(Zone.Rooms[105], Direction.South, Zone.Rooms[110]);
            ZoneHelper.ConnectRoom(Zone.Rooms[106], Direction.South, Zone.Rooms[111]);
            ZoneHelper.ConnectRoom(Zone.Rooms[107], Direction.South, Zone.Rooms[113]);
            ZoneHelper.ConnectRoom(Zone.Rooms[108], Direction.South, Zone.Rooms[115]);
            ZoneHelper.ConnectRoom(Zone.Rooms[109], Direction.East, Zone.Rooms[110]);
            ZoneHelper.ConnectRoom(Zone.Rooms[110], Direction.East, Zone.Rooms[111]);
            ZoneHelper.ConnectRoom(Zone.Rooms[112], Direction.East, Zone.Rooms[113]);
            ZoneHelper.ConnectRoom(Zone.Rooms[112], Direction.South, Zone.Rooms[116]);
            ZoneHelper.ConnectRoom(Zone.Rooms[113], Direction.East, Zone.Rooms[114]);
            ZoneHelper.ConnectRoom(Zone.Rooms[113], Direction.South, Zone.Rooms[117]);
            ZoneHelper.ConnectRoom(Zone.Rooms[114], Direction.East, Zone.Rooms[115]);
            ZoneHelper.ConnectRoom(Zone.Rooms[114], Direction.South, Zone.Rooms[118]);
            ZoneHelper.ConnectRoom(Zone.Rooms[115], Direction.South, Zone.Rooms[119]);
            ZoneHelper.ConnectRoom(Zone.Rooms[116], Direction.East, Zone.Rooms[117]);
            ZoneHelper.ConnectRoom(Zone.Rooms[116], Direction.South, Zone.Rooms[123]);
            ZoneHelper.ConnectRoom(Zone.Rooms[117], Direction.East, Zone.Rooms[118]);
            ZoneHelper.ConnectRoom(Zone.Rooms[117], Direction.South, Zone.Rooms[124]);
            ZoneHelper.ConnectRoom(Zone.Rooms[118], Direction.East, Zone.Rooms[119]);
            ZoneHelper.ConnectRoom(Zone.Rooms[118], Direction.South, Zone.Rooms[125]);
            ZoneHelper.ConnectRoom(Zone.Rooms[119], Direction.East, Zone.Rooms[120]);
            ZoneHelper.ConnectRoom(Zone.Rooms[119], Direction.South, Zone.Rooms[126]);
            ZoneHelper.ConnectRoom(Zone.Rooms[120], Direction.East, Zone.Rooms[121]);
            ZoneHelper.ConnectRoom(Zone.Rooms[120], Direction.South, Zone.Rooms[127]);
            ZoneHelper.ConnectRoom(Zone.Rooms[121], Direction.East, Zone.Rooms[122]);
            ZoneHelper.ConnectRoom(Zone.Rooms[121], Direction.South, Zone.Rooms[128]);
            ZoneHelper.ConnectRoom(Zone.Rooms[122], Direction.South, Zone.Rooms[129]);
            ZoneHelper.ConnectRoom(Zone.Rooms[123], Direction.East, Zone.Rooms[124]);
            ZoneHelper.ConnectRoom(Zone.Rooms[124], Direction.East, Zone.Rooms[125]);
            ZoneHelper.ConnectRoom(Zone.Rooms[125], Direction.East, Zone.Rooms[126]);
            ZoneHelper.ConnectRoom(Zone.Rooms[126], Direction.East, Zone.Rooms[127]);
            ZoneHelper.ConnectRoom(Zone.Rooms[127], Direction.East, Zone.Rooms[128]);
            ZoneHelper.ConnectRoom(Zone.Rooms[127], Direction.South, Zone.Rooms[130]);
            ZoneHelper.ConnectRoom(Zone.Rooms[128], Direction.East, Zone.Rooms[129]);
            ZoneHelper.ConnectRoom(Zone.Rooms[128], Direction.South, Zone.Rooms[131]);
            ZoneHelper.ConnectRoom(Zone.Rooms[129], Direction.South, Zone.Rooms[132]);
            ZoneHelper.ConnectRoom(Zone.Rooms[130], Direction.East, Zone.Rooms[131]);
            ZoneHelper.ConnectRoom(Zone.Rooms[131], Direction.East, Zone.Rooms[132]);
        }
    }
}
