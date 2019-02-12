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
using Objects.Item.Items.Interface;

namespace GenerateZones.Zones
{
    public class GrandViewGarden : BaseZone, IZoneCode
    {
        public GrandViewGarden() : base(11)
        {
        }

        //int npcId = 1;
        public IZone Generate()
        {
            Zone.InGameDaysTillReset = 1;
            Zone.Name = nameof(GrandViewGarden);

            for (int i = 1; i < 20; i++)
            {
                IRoom room = GenerateRoom();
                room.Zone = Zone.Id;
                ZoneHelper.AddRoom(Zone, room);
            }

            Zone.Rooms[6].AddItemToRoom(PrizeRose());
            Zone.Rooms[19].AddItemToRoom(Sign());

            ConnectRooms();

            return Zone;
        }



        private IRoom GenerateRoom()
        {
            IRoom room = CreateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);
            room.ShortDescription = "A rose garden";
            room.LookDescription = "Hedges of roses tower above you blocking your view forming a maze.";
            room.ExamineDescription = "The red rose bush is in full bloom while the white roses are starting to open up.  There is a red and yellow mix close to your feet that is an interesting color while the pink one slightly above your eye level looks to be straight out of a bouquet.";
            return room;
        }

        public IItem PrizeRose()
        {
            IItem rose = CreateItem<IItem>();
            rose.KeyWords.Add("rose");
            rose.SentenceDescription = "rose";
            rose.ShortDescription = "A beautiful prize {color} rose.";
            rose.LookDescription = "This is the prize rose that the Kings Gardner has been growing.";
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
            IEnchantery sign = CreateItem<IEnchantery>();
            sign.KeyWords.Add("sign");
            sign.Attributes.Add(Item.ItemAttribute.NoGet);
            sign.SentenceDescription = "sign";
            sign.ShortDescription = "A sign floats weightlessly in the air.";
            sign.LookDescription = "ƎƠȴɕȶφΩ ЉѣѼѿ ӁқԘհե";
            sign.ExamineDescription = "As you continue to examine the sign it shifts into readable words.  This fountain is used for enchanting items.  To use the fountain use the command \"Enchant [Item].\"";
            return sign;
        }

        private void ConnectRooms()
        {
            Zone.RecursivelySetZone();

            ZoneHelper.ConnectZone(Zone.Rooms[9], Direction.North, 5, 30);

            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.North, Zone.Rooms[2]);
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.North, Zone.Rooms[9]);
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.East, Zone.Rooms[3]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.East, Zone.Rooms[4]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.South, Zone.Rooms[7]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.North, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.South, Zone.Rooms[8]);
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.West, Zone.Rooms[6]);

            ZoneHelper.ConnectRoom(Zone.Rooms[11], Direction.North, Zone.Rooms[12]);
            ZoneHelper.ConnectRoom(Zone.Rooms[12], Direction.North, Zone.Rooms[19]);
            ZoneHelper.ConnectRoom(Zone.Rooms[12], Direction.East, Zone.Rooms[13]);
            ZoneHelper.ConnectRoom(Zone.Rooms[13], Direction.East, Zone.Rooms[14]);
            ZoneHelper.ConnectRoom(Zone.Rooms[13], Direction.South, Zone.Rooms[17]);
            ZoneHelper.ConnectRoom(Zone.Rooms[14], Direction.North, Zone.Rooms[15]);
            ZoneHelper.ConnectRoom(Zone.Rooms[14], Direction.South, Zone.Rooms[18]);
            ZoneHelper.ConnectRoom(Zone.Rooms[15], Direction.West, Zone.Rooms[16]);
        }
    }
}
