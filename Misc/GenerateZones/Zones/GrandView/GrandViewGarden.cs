using Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Magic;
using System.Reflection;
using Objects.Zone.Interface;
using Objects.Zone;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Damage;
using Objects.Global.DefaultValues;
using Objects.Item.Items;
using Objects.Global;
using Objects.Global.Stats;
using Objects.Material.Materials;
using Objects.Personality.Personalities;
using Objects.Mob;
using static Objects.Global.Direction.Directions;
using Objects.Mob.Interface;
using Objects.Item.Interface;
using Objects.Item;
using Objects.Magic.Interface;
using Objects.Effect.Interface;
using Objects.Interface;
using Shared.Sound.Interface;
using System.Diagnostics.CodeAnalysis;
using Objects.Magic.Enchantment;
using Objects.Effect;
using Objects.Effect.Zone.GrandViewGarden;

namespace GenerateZones.Zones
{
    public class GrandViewGarden : IZoneCode
    {
        public static int zoneId = 11;

        IZone zone = new Zone();
        int roomId = 1;
        int itemId = 1;
        //int npcId = 1;
        public IZone Generate()
        {
            zone.Id = zoneId;
            zone.InGameDaysTillReset = 1;
            zone.Name = nameof(GrandViewGarden);

            for (int i = 1; i < 20; i++)
            {
                IRoom room = GenerateRoom();
                room.Zone = zone.Id;
                ZoneHelper.AddRoom(zone, room);
            }

            zone.Rooms[6].Items.Add(PrizeRose());
            zone.Rooms[19].Items.Add(Sign());

            ConnectRooms();

            return zone;
        }



        private IRoom GenerateRoom()
        {
            IRoom room = new Room();
            room.Id = roomId++;
            room.MovementCost = 1;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);
            room.ShortDescription = "A rose garden";
            room.LongDescription = "Hedges of roses tower above you blocking your view forming a maze.";
            room.ExamineDescription = "The red rose bush is in full bloom while the white roses are starting to open up.  There is a red and yellow mix close to your feet that is an interesting color while the pink one slightly above your eye level looks to be straight out of a bouquet.";
            return room;
        }

        public IItem PrizeRose()
        {
            IItem rose = new Item();
            rose.Id = itemId++;
            rose.Level = 0;
            rose.KeyWords.Add("rose");
            rose.SentenceDescription = "rose";
            rose.ShortDescription = "A beautiful prize {color} rose.";
            rose.LongDescription = "This is the prize rose that the Kings Gardner has been growing.";
            rose.ExamineDescription = "The rose has the most perfect flower that is in full boom.  It's no wonder that it is the prize flower in the garden.";

            rose.FlavorOptions.Add("{color}", new List<string>() { "red", "white", "yellow", "pink", "orange", "lilac", "purple" });

            IEnchantment enchantment = new GetEnchantment();
            enchantment.Effect = new Replenish();
            enchantment.ActivationPercent = 100;
            rose.Enchantments.Add(enchantment);

            enchantment = new GetEnchantment();
            enchantment.Effect = new MoveToOtherDimension();
            enchantment.ActivationPercent = 100;
            rose.Enchantments.Add(enchantment);

            enchantment = new DropEnchantment();
            enchantment.Effect = new ReturnToNormalDimension();
            enchantment.ActivationPercent = 100;
            rose.Enchantments.Add(enchantment);

            return rose;
        }

        public IItem Sign()
        {
            IItem sign = new Item();
            sign.Id = itemId++;
            sign.Level = 0;
            sign.KeyWords.Add("sign");
            sign.Attributes.Add(Item.ItemAttribute.NoGet);
            sign.SentenceDescription = "sign";
            sign.ShortDescription = "A sign floats weightlessly in the air.";
            sign.LongDescription = "ƎƠȴɕȶφΩ ЉѣѼѿ ӁқԘհե";
            sign.ExamineDescription = "As you continue to examine the sign it shifts into readable words.  This fountain is used for enchanting items.  To use the fountain use the command \"Enchant [Item].\"";
            return sign;
        }

        private void ConnectRooms()
        {
            zone.RecursivelySetZone();

            ZoneHelper.ConnectRoom(zone.Rooms[1], Direction.North, zone.Rooms[2]);
            ZoneHelper.ConnectRoom(zone.Rooms[2], Direction.North, zone.Rooms[9]);
            ZoneHelper.ConnectRoom(zone.Rooms[2], Direction.East, zone.Rooms[3]);
            ZoneHelper.ConnectRoom(zone.Rooms[3], Direction.East, zone.Rooms[4]);
            ZoneHelper.ConnectRoom(zone.Rooms[3], Direction.South, zone.Rooms[7]);
            ZoneHelper.ConnectRoom(zone.Rooms[4], Direction.North, zone.Rooms[5]);
            ZoneHelper.ConnectRoom(zone.Rooms[4], Direction.South, zone.Rooms[8]);
            ZoneHelper.ConnectRoom(zone.Rooms[5], Direction.West, zone.Rooms[6]);

            ZoneHelper.ConnectRoom(zone.Rooms[11], Direction.North, zone.Rooms[12]);
            ZoneHelper.ConnectRoom(zone.Rooms[12], Direction.North, zone.Rooms[19]);
            ZoneHelper.ConnectRoom(zone.Rooms[12], Direction.East, zone.Rooms[13]);
            ZoneHelper.ConnectRoom(zone.Rooms[13], Direction.East, zone.Rooms[14]);
            ZoneHelper.ConnectRoom(zone.Rooms[13], Direction.South, zone.Rooms[17]);
            ZoneHelper.ConnectRoom(zone.Rooms[14], Direction.North, zone.Rooms[15]);
            ZoneHelper.ConnectRoom(zone.Rooms[14], Direction.South, zone.Rooms[18]);
            ZoneHelper.ConnectRoom(zone.Rooms[15], Direction.West, zone.Rooms[16]);
        }
    }
}
