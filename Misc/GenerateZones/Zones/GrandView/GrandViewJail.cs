using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Objects.Zone.Interface;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using static Objects.Item.Item;
using static Objects.Global.Direction.Directions;
using MiscShared;
using static Objects.Item.Items.Equipment;

namespace GenerateZones.Zones
{
    public class GrandViewJail : BaseZone, IZoneCode
    {
        public GrandViewJail() : base(6)
        {
        }

        public IZone Generate()
        {
            Zone.Name = nameof(GrandViewJail);

            BuildRoomsViaReflection(this.GetType());

            ConnectRooms();

            return Zone;
        }

        #region Rooms
        #region JailTunnel
        //private BaseEnchantment EnterRoomMessage(string message, TagWrapper.TagType tagType)
        //{
        //    GenericEnchantment enchantment = new GenericEnchantment();
        //    enchantment.ActivationPercent = 100;
        //    EnterRoomMessage effect = new EnterRoomMessage();
        //    effect.TagType = tagType;
        //    effect.Message = message;
        //    enchantment.Effect = effect;

        //    return enchantment;
        //}


        private IRoom GenerateRoom1()
        {
            string examineDescription = "The cell is cold and damp just like you would imagine it to be.  However upon closer inspection you notice that the wall to the east is actually a door.";
            string lookDescription = "You are in what appears to be an old jail cell.  Light flickers from beyond the bars causing your shadow to dance on the walls.";
            string shortDescription = "A cell";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);
            room.Attributes.Add(Room.RoomAttribute.Peaceful);

            //string message = "Perform the following commands." + Environment.NewLine + "OPEN WALL" + Environment.NewLine + "EAST";
            //room.Enchantments.Add(EnterRoomMessage(message, TagWrapper.TagType.Info));
            return room;
        }


        private IRoom GenerateRoom2()
        {
            string examineDescription = "Drops of water can be heard falling in the cavern.  The moisture makes the rocks slippery and would be a slipping hazard if you were able to stand and walk.";
            string lookDescription = "This area is a low tunnel connecting the cell to an underground cavern.";
            string shortDescription = "A tunnel";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            //string message = "Perform the following commands." + Environment.NewLine + "EAST";
            //room.Enchantments.Add(EnterRoomMessage(message, TagWrapper.TagType.Info));
            return room;
        }

        private IRoom GenerateRoom3()
        {
            string examineDescription = "Because of all the glowing mushrooms you can see the cavern ceiling.  Stalactites hang down from the ceiling above stalagmites.  Each trying desperately to reach the other.";
            string lookDescription = "The cavern opens up to a large area here.  Several colorful glowing mushrooms are growing here making it easy to see.  Maybe you should pick some up.";
            string shortDescription = "An underground cavern";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            //string message = "Perform the following commands." + Environment.NewLine + "GET MUSHROOM" + Environment.NewLine + "EAST";
            //room.Enchantments.Add(EnterRoomMessage(message, TagWrapper.TagType.Info));

            room.AddItemToRoom(Room3_Mushroom());
            room.AddItemToRoom(Room3_Mushroom());
            room.AddItemToRoom(Room3_Mushroom());
            room.AddItemToRoom(Room3_Mushroom());
            room.AddItemToRoom(Room3_Mushroom());

            return room;
        }

        private IItem Room3_Mushroom()
        {
            string examineDescription = "A small mushroom about 1.5 inches tall.  It glows with a soft {color} light that is capable of lighting a room without being harsh on the eyes.";
            string lookDescription = "While not very big it does produces a soft {color} glow that is capable of lighting up a room.";
            string sentenceDescription = "mushroom";
            string shortDescription = "A glowing mushroom.";

            IEquipment item = CreateEquipment(AvalableItemPosition.Held, 1, examineDescription, lookDescription, sentenceDescription, shortDescription);
            item.Attributes.Add(ItemAttribute.Light);
            item.KeyWords.Add("Mushroom");
            item.KeyWords.Add("{color}");
            item.FlavorOptions.Add("{color}", new List<string>() { "green", "blue", "white", "yellow", "red", "purple" });

            return item;
        }

        private IRoom GenerateRoom4()
        {
            string examineDescription = "The tunnel here is only slightly taller then you.  At points you have to crawl on your belly to get through.";
            string lookDescription = "The tunnel here is steep and bendy.  The walls seem to close in on you as you make your way through.";
            string shortDescription = "A steep bendy tunnel";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            //string message = "Perform the following commands." + Environment.NewLine + "EQUIP MUSHROOM" + Environment.NewLine + "LOOK" + Environment.NewLine + "EAST";
            //room.Enchantments.Add(EnterRoomMessage(message, TagWrapper.TagType.Info));

            return room;
        }

        private IRoom GenerateRoom5()
        {
            string examineDescription = "You can hear the sounds of water cascading over rocks from the east, however it a sheer cliff that descends into darkness of unknown depth.  A shoddy built wooden ladder made of twine and logs ascends into the darkness above.";
            string lookDescription = "The tunnel makes an abrupt direction change here.  To the east is are cliff and the sound of water.  Up is a ladder and to the west a tunnel of darkness.";
            string shortDescription = "A tunnel";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            //string message = "Perform the following commands." + Environment.NewLine + "UP";
            //room.Enchantments.Add(EnterRoomMessage(message, TagWrapper.TagType.Info));

            return room;
        }

        private IRoom GenerateRoom6()
        {
            string examineDescription = "Looking around you are not sure if this part of the tunnel is natural or not.  You know for sure the ladder is not natural.";
            string lookDescription = "You are on a ladder.  Above you is darkness, below you is darkness.";
            string shortDescription = "A ladder";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            //string message = "Perform the following commands." + Environment.NewLine + "UP";
            //room.Enchantments.Add(EnterRoomMessage(message, TagWrapper.TagType.Info));

            return room;
        }

        private IRoom GenerateRoom7()
        {
            string examineDescription = "Looking around you are not sure if this part of the tunnel is natural or not.  You know for sure the ladder is not natural.";
            string lookDescription = "You are on a ladder.  Above you is darkness, below you is darkness.";
            string shortDescription = "A ladder";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            //string message = "Perform the following commands." + Environment.NewLine + "UP";
            //room.Enchantments.Add(EnterRoomMessage(message, TagWrapper.TagType.Info));

            return room;
        }

        private IRoom GenerateRoom8()
        {
            string examineDescription = "Light comes into the tunnel from cracks between the stone and hole above.";
            string lookDescription = "You can see light above you and darkness below.  Be careful not to fall.";
            string shortDescription = "A ladder";
            IRoom room = IndoorRoomNoLight(Zone.Id, examineDescription, lookDescription, shortDescription);
            room.Attributes.Add(Room.RoomAttribute.NoNPC);

            //string message = "Perform the following commands." + Environment.NewLine + "OPEN STONE" + Environment.NewLine + "UP";
            //room.Enchantments.Add(EnterRoomMessage(message, TagWrapper.TagType.Info));

            return room;
        }
        #endregion JailTunnel


        #endregion End Rooms

        private void ConnectRooms()
        {
            #region Jail/Tunnel
            string openMessage = "With one last push the wall slides open enough for you to pass through.";
            string closeMessage = "The wall slides back shut becoming nearly invisible.";
            string description = "The door blends in perfectly with the wall and makes you wonder how you discovered it in the first place.";
            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.East, Zone.Rooms[2], new DoorInfo("wall", openMessage, closeMessage, true, description));
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.East, Zone.Rooms[3]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.East, Zone.Rooms[4]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.East, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.Up, Zone.Rooms[6]);
            ZoneHelper.ConnectRoom(Zone.Rooms[6], Direction.Up, Zone.Rooms[7]);
            ZoneHelper.ConnectRoom(Zone.Rooms[7], Direction.Up, Zone.Rooms[8]);

            openMessage = "You move the stone out of the way reveling the opening of the tunnel.";
            closeMessage = "You slide the stone back carefully placing it back where it was.";
            description = "The stone is of good weight, enough to discourage people from moving it but light enough to move when needed.";
            ZoneHelper.ConnectZone(Zone.Rooms[8], Direction.Up, 5, 1, new DoorInfo("stone", openMessage, closeMessage, true, description));
            #endregion Jail/Tunnel
        }
    }
}
