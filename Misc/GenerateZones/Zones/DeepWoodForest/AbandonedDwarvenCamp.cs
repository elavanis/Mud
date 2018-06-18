using Objects.Damage;
using Objects.Damage.Interface;
using Objects.Global;
using Objects.Item.Items;
using Objects.Item.Items.Interface;
using Objects.LevelRange;
using Objects.Material.Materials;
using Objects.Mob;
using Objects.Mob.Interface;
using Objects.Personality.Personalities;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Zone;
using Objects.Zone.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using static Objects.Global.Direction.Directions;
using static Objects.Item.Items.Equipment;

namespace GenerateZones.Zones.DeepWoodForest
{
    public class AbandonedDwarvenCamp : IZoneCode
    {
        Zone zone = new Zone();
        int roomId = 1;
        int itemId = 1;
        int npcId = 1;

        public IZone Generate()
        {
            zone.Id = 15;
            zone.InGameDaysTillReset = 5;
            zone.Name = nameof(AbandonedDwarvenCamp);

            int methodCount = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Count();
            for (int i = 1; i <= methodCount; i++)
            {
                string methodName = "GenerateRoom" + i;
                MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
                if (method != null)
                {
                    IRoom room = (IRoom)method.Invoke(this, null);
                    room.Zone = zone.Id;
                    ZoneHelper.AddRoom(zone, room);
                }
            }

            AddMobs();

            ConnectRooms();

            return zone;
        }

        private void AddMobs()
        {
            for (int i = 0; i < 50; i++)
            {
                int roomId = GlobalReference.GlobalValues.Random.Next(zone.Rooms.Count) + 1;
                zone.Rooms[roomId].AddMobileObjectToRoom(TunnelGoblin());
            }
        }


        #region Rooms
        private IRoom ZoneRoom(int movementCost)
        {
            IRoom room = new Room();
            room.Id = roomId++;
            room.MovementCost = movementCost;
            room.Attributes.Add(Room.RoomAttribute.Indoor);
            room.Attributes.Add(Room.RoomAttribute.NoLight);
            return room;
        }

        private IRoom GenerateRoom1()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "Each statue is larger than life coming in at eight feet tall.  Their axes raised in a salute touching overhead forming the entrance way in which to walk in or out of the camp.";
            room.LongDescription = "A pair of dwarven statues are carved into the rock faces.";
            room.ShortDescription = "Dwarven Mining Camp";

            return room;
        }

        private IRoom GenerateRoom2()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The cavern begins to open up more here to allow for the larger camp.";
            room.LongDescription = "The dwarven mining camp opens to the east and the camp ends with a pair of statues to the west.";
            room.ShortDescription = "Dwarven Mining Camp";

            return room;
        }

        private IRoom GenerateRoom3()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The store front was carved out the rock but the right half has been ripped apart like it was a kids Lincoln Logs.";
            room.LongDescription = "The old store front of the blacksmith has been partly destroyed.";
            room.ShortDescription = "Dwarven Mining Camp";

            return room;
        }

        private IRoom GenerateRoom4()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The smoke carries the smell of meat and vegetables to your nostrils.";
            room.LongDescription = "Smoke slowly rolls out of what used to be the old dwarven tavern.";
            room.ShortDescription = "Dwarven Mining Camp";

            return room;
        }

        private IRoom GenerateRoom5()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The hole leading down to the water is all that remains.";
            room.LongDescription = "Once upon a time there used to be a well here but has since been removed.";
            room.ShortDescription = "Dwarven Mining Camp";

            return room;
        }

        private IRoom GenerateRoom6()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "Small piles of rubble are all that are left of the stone walls that was one stood high above.";
            room.LongDescription = "Piles of rubble, a dwarven anvil and a long dead forge are all that remain of the dwarven blacksmith hall.";
            room.ShortDescription = "Dwarven Mining Camp";

            return room;
        }

        private IRoom GenerateRoom7()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The place has been ransacked and looted.  A few empty bottles are all that remain of what was surely a well stocked store.";
            room.LongDescription = "You stand in what was a general store of sorts.";
            room.ShortDescription = "Dwarven Mining Camp";

            return room;
        }

        private IRoom GenerateRoom8()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "Not much remains of the bar.  Just a few broken table and chairs.";
            room.LongDescription = "Broken tables are piled in one corner while the booze behind the bar has been emptied.";
            room.ShortDescription = "Dwarven Mining Camp";

            return room;
        }

        private IRoom GenerateRoom9()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "Peering into the cooking pot revels a stew of sorts slowly bubbling.";
            room.LongDescription = "A small cooking fire burns giving light to the room and filling the room with the scent of food.";
            room.ShortDescription = "Dwarven Mining Camp";

            room.Attributes.Add(Room.RoomAttribute.Light);

            room.AddMobileObjectToRoom(Goblin());
            room.AddMobileObjectToRoom(Goblin());

            return room;
        }

        private IRoom GenerateRoom10()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "Faint light comes from one end of the tunnel while inky blackness fills the other end.";
            room.LongDescription = "A natural tunnel was revealed when the wall to the bar was broken.";
            room.ShortDescription = "Cavern Tunnels";

            return room;
        }

        #region Cavern
        private IRoom GenerateRoom11() { return CavernTunnel(); }
        private IRoom GenerateRoom12() { return CavernTunnel(); }
        private IRoom GenerateRoom13() { return CavernTunnel(); }
        private IRoom GenerateRoom14() { return CavernTunnel(); }
        private IRoom GenerateRoom15() { return CavernTunnel(); }
        private IRoom GenerateRoom16() { return CavernTunnel(); }
        private IRoom GenerateRoom17() { return CavernTunnel(); }
        private IRoom GenerateRoom18() { return CavernTunnel(); }
        private IRoom GenerateRoom19() { return CavernTunnel(); }
        private IRoom GenerateRoom20() { return CavernTunnel(); }
        private IRoom GenerateRoom21() { return CavernTunnel(); }
        private IRoom GenerateRoom22() { return CavernTunnel(); }
        private IRoom GenerateRoom23() { return CavernTunnel(); }
        private IRoom GenerateRoom24() { return CavernTunnel(); }
        private IRoom GenerateRoom25() { return CavernTunnel(); }
        private IRoom GenerateRoom26() { return CavernTunnel(); }
        private IRoom GenerateRoom27() { return CavernTunnel(); }
        private IRoom GenerateRoom28() { return CavernTunnel(); }
        private IRoom GenerateRoom29() { return CavernTunnel(); }
        private IRoom GenerateRoom30() { return CavernTunnel(); }
        private IRoom GenerateRoom31() { return CavernTunnel(); }
        private IRoom GenerateRoom32() { return CavernTunnel(); }
        private IRoom GenerateRoom33() { return CavernTunnel(); }
        private IRoom GenerateRoom34() { return CavernTunnel(); }
        private IRoom GenerateRoom35() { return CavernTunnel(); }
        private IRoom GenerateRoom36() { return CavernTunnel(); }
        private IRoom GenerateRoom37() { return CavernTunnel(); }
        private IRoom GenerateRoom38() { return CavernTunnel(); }
        private IRoom GenerateRoom39() { return CavernTunnel(); }
        private IRoom GenerateRoom40() { return CavernTunnel(); }
        private IRoom GenerateRoom41() { return CavernTunnel(); }
        private IRoom GenerateRoom42() { return CavernTunnel(); }
        private IRoom GenerateRoom43() { return CavernTunnel(); }
        private IRoom GenerateRoom44() { return CavernTunnel(); }
        private IRoom GenerateRoom45() { return CavernTunnel(); }
        private IRoom GenerateRoom46() { return CavernTunnel(); }
        private IRoom GenerateRoom47() { return CavernTunnel(); }
        private IRoom GenerateRoom48() { return CavernTunnel(); }
        private IRoom GenerateRoom49() { return CavernTunnel(); }
        private IRoom GenerateRoom50() { return CavernTunnel(); }
        private IRoom GenerateRoom51() { return CavernTunnel(); }
        private IRoom GenerateRoom52() { return CavernTunnel(); }
        private IRoom GenerateRoom53() { return CavernTunnel(); }
        private IRoom GenerateRoom54() { return CavernTunnel(); }
        private IRoom GenerateRoom55() { return CavernTunnel(); }
        private IRoom GenerateRoom56() { return CavernTunnel(); }
        private IRoom GenerateRoom57() { return CavernTunnel(); }
        private IRoom GenerateRoom58() { return CavernTunnel(); }
        private IRoom GenerateRoom59() { return CavernTunnel(); }
        private IRoom GenerateRoom60() { return CavernTunnel(); }
        private IRoom GenerateRoom61() { return CavernTunnel(); }
        private IRoom GenerateRoom62() { return CavernTunnel(); }
        private IRoom GenerateRoom63() { return CavernTunnel(); }
        private IRoom GenerateRoom64() { return CavernTunnel(); }
        private IRoom GenerateRoom65() { return CavernTunnel(); }
        private IRoom GenerateRoom66() { return CavernTunnel(); }
        private IRoom GenerateRoom67() { return CavernTunnel(); }
        private IRoom GenerateRoom68() { return CavernTunnel(); }
        private IRoom GenerateRoom69() { return CavernTunnel(); }
        private IRoom GenerateRoom70() { return CavernTunnel(); }
        private IRoom GenerateRoom71() { return CavernTunnel(); }
        private IRoom GenerateRoom72() { return CavernTunnel(); }
        private IRoom GenerateRoom73() { return CavernTunnel(); }
        private IRoom GenerateRoom74() { return CavernTunnel(); }
        private IRoom GenerateRoom75() { return CavernTunnel(); }
        private IRoom GenerateRoom76() { return CavernTunnel(); }
        private IRoom GenerateRoom77() { return CavernTunnel(); }
        private IRoom GenerateRoom78() { return CavernTunnel(); }
        private IRoom GenerateRoom79() { return CavernTunnel(); }
        private IRoom GenerateRoom80() { return CavernTunnel(); }
        private IRoom GenerateRoom81() { return CavernTunnel(); }
        private IRoom GenerateRoom82() { return CavernTunnel(); }
        private IRoom GenerateRoom83() { return CavernTunnel(); }
        private IRoom GenerateRoom84() { return CavernTunnel(); }
        private IRoom GenerateRoom85() { return CavernTunnel(); }
        private IRoom GenerateRoom86() { return CavernTunnel(); }
        private IRoom GenerateRoom87() { return CavernTunnel(); }
        private IRoom GenerateRoom88() { return CavernTunnel(); }
        private IRoom GenerateRoom89() { return CavernTunnel(); }
        private IRoom GenerateRoom90() { return CavernTunnel(); }
        private IRoom GenerateRoom91() { return CavernTunnel(); }
        private IRoom GenerateRoom92() { return CavernTunnel(); }
        private IRoom GenerateRoom93() { return CavernTunnel(); }
        private IRoom GenerateRoom94() { return CavernTunnel(); }
        private IRoom GenerateRoom95() { return CavernTunnel(); }
        private IRoom GenerateRoom96() { return CavernTunnel(); }
        private IRoom GenerateRoom97() { return CavernTunnel(); }
        private IRoom GenerateRoom98() { return CavernTunnel(); }

        private IRoom CavernTunnel()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The small tunnel was formed by flowing water long since gone.";
            room.LongDescription = "You stand in a small tunnel part of a larger cave system.";
            room.ShortDescription = "Cavern Tunnels";

            return room;
        }
        #endregion Cavern
        #endregion Rooms

        #region NPC
        private INonPlayerCharacter Goblin()
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.Id = npcId++;
            npc.Level = 14;

            npc.ExamineDescription = "The goblin stares at you with fright in its eyes.";
            npc.LongDescription = "The goblin cowers in the corner watching you very intently.";
            npc.ShortDescription = "A frightened goblin.";
            npc.SentenceDescription = "goblin";
            npc.KeyWords.Add("goblin");

            return npc;
        }

        private INonPlayerCharacter TunnelGoblin()
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.Id = npcId++;
            npc.LevelRange = new LevelRange() { LowerLevel = 14, UpperLevel = 16 };

            npc.Personalities.Add(new Aggressive());
            npc.Personalities.Add(new Wanderer());

            npc.ExamineDescription = "The goblin is armed to the teeth and appears to be on some type of war patrol.";
            npc.LongDescription = "A well armed goblin appears before you with weapon drawn.";
            npc.ShortDescription = "An armed goblin.";
            npc.SentenceDescription = "goblin";
            npc.KeyWords.Add("goblin");

            npc.AddEquipment(Dogslicer());
            npc.AddEquipment(Bracer());
            npc.AddEquipment(ChainMail());
            npc.AddEquipment(Helmet());
            npc.AddEquipment(LeathPants());
            return npc;
        }

        private IEquipment Dogslicer()
        {
            IWeapon weapon = new Weapon();
            weapon.Id = itemId++;
            weapon.Level = 15;
            weapon.Type = Weapon.WeaponType.Sword;
            weapon.KeyWords.Add("sword");
            weapon.KeyWords.Add("dog");
            weapon.KeyWords.Add("slicer");
            weapon.LongDescription = "The sword is crudely made with spots of rust where the iron has gotten wet.";
            weapon.ShortDescription = "The goblin Dogslicer has three holes in the blade making it lighter and easier to swing.";
            weapon.SentenceDescription = "Dogslicer";
            weapon.ExamineDescription = "The Dogslicer is poorly made and looks like it will fail in combat at some point.";

            IDamage damage = new Damage();
            damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(weapon.Level);
            damage.Type = Damage.DamageType.Slash;
            weapon.DamageList.Add(damage);

            return weapon;
        }

        private IEquipment Bracer()
        {
            IArmor armor = Armor();
            armor.ItemPosition = AvalableItemPosition.Arms;
            armor.KeyWords.Add("leather");
            armor.KeyWords.Add("bracer");
            armor.Material = new Leather();
            armor.ShortDescription = "A poorly made pair leather bracers.";
            armor.LongDescription = "The bracers extend up the wearers arm a good ways giving the user extra protection.";
            armor.SentenceDescription = "a pair of leather bracers";
            armor.ExamineDescription = "The bracers are fairly plain but are poorly made.";

            return armor;
        }

        private IArmor Armor()
        {
            IArmor armor = new Armor();
            armor.Id = itemId++;
            armor.Level = 15;
            armor.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(armor.Level);

            return armor;
        }

        private IEquipment ChainMail()
        {
            IArmor armor = Armor();
            armor.ItemPosition = AvalableItemPosition.Body;
            armor.KeyWords.Add("chain mail");
            armor.KeyWords.Add("mail");
            armor.KeyWords.Add("shirt");
            armor.Material = new Steel();
            armor.ShortDescription = "A chain mail shirt with a few missing links.";
            armor.LongDescription = "The chain mail looks to be as utilitarian as protectant.";
            armor.SentenceDescription = "a chain mail shirt";
            armor.ExamineDescription = "The chain mail is a light steel color. It has a few missing links but other wise looks good.";

            return armor;
        }

        private IEquipment Helmet()
        {
            IArmor armor = Armor();
            armor.ItemPosition = AvalableItemPosition.Head;
            armor.KeyWords.Add("helmet");
            armor.KeyWords.Add("steel");
            armor.Material = new Steel();
            armor.ShortDescription = "A steel helmet that is more functional then attractive.";
            armor.LongDescription = "The helmet sits crooked on the user slightly obscuring their vision.";
            armor.SentenceDescription = "a steel helmet";
            armor.ExamineDescription = "The helmet is of good quality for goblin craftsmanship.";

            return armor;
        }

        private IEquipment LeathPants()
        {
            IArmor armor = Armor();
            armor.ItemPosition = AvalableItemPosition.Legs;
            armor.KeyWords.Add("leather");
            armor.KeyWords.Add("pants");
            armor.KeyWords.Add("pant");
            armor.Material = new Leather();
            armor.ShortDescription = "Leather pants.";
            armor.LongDescription = "These pants once belonged to a goblin and look well worn.";
            armor.SentenceDescription = "leather pants";
            armor.ExamineDescription = "The leather pants have lots of stains from the life in the caves.";

            return armor;
        }
        #endregion NPC

        private void ConnectRooms()
        {
            zone.RecursivelySetZone();

            ZoneHelper.ConnectZone(zone.Rooms[1], Direction.West, 13, 108);
            //ZoneHelper.ConnectZone(zone.Rooms[47], Direction.North, x, x);

            ZoneHelper.ConnectRoom(zone.Rooms[1], Direction.East, zone.Rooms[2]);
            ZoneHelper.ConnectRoom(zone.Rooms[2], Direction.East, zone.Rooms[3]);
            ZoneHelper.ConnectRoom(zone.Rooms[3], Direction.East, zone.Rooms[4]);
            ZoneHelper.ConnectRoom(zone.Rooms[4], Direction.East, zone.Rooms[5]);
            ZoneHelper.ConnectRoom(zone.Rooms[5], Direction.East, zone.Rooms[6]);

            ZoneHelper.ConnectRoom(zone.Rooms[3], Direction.North, zone.Rooms[7]);
            ZoneHelper.ConnectRoom(zone.Rooms[4], Direction.South, zone.Rooms[8]);
            ZoneHelper.ConnectRoom(zone.Rooms[8], Direction.South, zone.Rooms[9]);
            ZoneHelper.ConnectRoom(zone.Rooms[9], Direction.South, zone.Rooms[10]);

            #region Cavern
            #region Correct Path
            ZoneHelper.ConnectRoom(zone.Rooms[10], Direction.South, zone.Rooms[11]);
            ZoneHelper.ConnectRoom(zone.Rooms[11], Direction.East, zone.Rooms[12]);
            ZoneHelper.ConnectRoom(zone.Rooms[12], Direction.East, zone.Rooms[13]);
            ZoneHelper.ConnectRoom(zone.Rooms[13], Direction.East, zone.Rooms[14]);
            ZoneHelper.ConnectRoom(zone.Rooms[14], Direction.East, zone.Rooms[15]);
            ZoneHelper.ConnectRoom(zone.Rooms[15], Direction.Up, zone.Rooms[16]);
            ZoneHelper.ConnectRoom(zone.Rooms[16], Direction.Up, zone.Rooms[17]);
            ZoneHelper.ConnectRoom(zone.Rooms[17], Direction.West, zone.Rooms[18]);
            ZoneHelper.ConnectRoom(zone.Rooms[18], Direction.West, zone.Rooms[19]);
            ZoneHelper.ConnectRoom(zone.Rooms[19], Direction.West, zone.Rooms[20]);
            ZoneHelper.ConnectRoom(zone.Rooms[20], Direction.North, zone.Rooms[21]);
            ZoneHelper.ConnectRoom(zone.Rooms[21], Direction.Down, zone.Rooms[22]);
            ZoneHelper.ConnectRoom(zone.Rooms[22], Direction.South, zone.Rooms[23]);
            ZoneHelper.ConnectRoom(zone.Rooms[23], Direction.South, zone.Rooms[24]);
            ZoneHelper.ConnectRoom(zone.Rooms[24], Direction.East, zone.Rooms[25]);
            ZoneHelper.ConnectRoom(zone.Rooms[25], Direction.Up, zone.Rooms[26]);
            ZoneHelper.ConnectRoom(zone.Rooms[26], Direction.East, zone.Rooms[27]);
            ZoneHelper.ConnectRoom(zone.Rooms[27], Direction.Up, zone.Rooms[28]);
            ZoneHelper.ConnectRoom(zone.Rooms[28], Direction.West, zone.Rooms[29]);
            ZoneHelper.ConnectRoom(zone.Rooms[29], Direction.West, zone.Rooms[30]);
            ZoneHelper.ConnectRoom(zone.Rooms[30], Direction.South, zone.Rooms[31]);
            ZoneHelper.ConnectRoom(zone.Rooms[31], Direction.South, zone.Rooms[32]);
            ZoneHelper.ConnectRoom(zone.Rooms[32], Direction.Up, zone.Rooms[33]);
            ZoneHelper.ConnectRoom(zone.Rooms[33], Direction.East, zone.Rooms[34]);
            ZoneHelper.ConnectRoom(zone.Rooms[34], Direction.East, zone.Rooms[35]);
            ZoneHelper.ConnectRoom(zone.Rooms[35], Direction.East, zone.Rooms[36]);
            ZoneHelper.ConnectRoom(zone.Rooms[36], Direction.Down, zone.Rooms[37]);
            ZoneHelper.ConnectRoom(zone.Rooms[37], Direction.North, zone.Rooms[38]);
            ZoneHelper.ConnectRoom(zone.Rooms[38], Direction.North, zone.Rooms[39]);
            ZoneHelper.ConnectRoom(zone.Rooms[39], Direction.North, zone.Rooms[40]);
            ZoneHelper.ConnectRoom(zone.Rooms[40], Direction.North, zone.Rooms[41]);
            ZoneHelper.ConnectRoom(zone.Rooms[41], Direction.Up, zone.Rooms[42]);
            ZoneHelper.ConnectRoom(zone.Rooms[42], Direction.South, zone.Rooms[43]);
            ZoneHelper.ConnectRoom(zone.Rooms[43], Direction.West, zone.Rooms[44]);
            ZoneHelper.ConnectRoom(zone.Rooms[44], Direction.Down, zone.Rooms[45]);
            ZoneHelper.ConnectRoom(zone.Rooms[45], Direction.North, zone.Rooms[46]);
            ZoneHelper.ConnectRoom(zone.Rooms[46], Direction.Down, zone.Rooms[47]);
            #endregion Correct Path

            ZoneHelper.ConnectRoom(zone.Rooms[12], Direction.South, zone.Rooms[48]);
            ZoneHelper.ConnectRoom(zone.Rooms[48], Direction.South, zone.Rooms[49]);
            ZoneHelper.ConnectRoom(zone.Rooms[49], Direction.Up, zone.Rooms[50]);
            ZoneHelper.ConnectRoom(zone.Rooms[50], Direction.South, zone.Rooms[51]);
            ZoneHelper.ConnectRoom(zone.Rooms[51], Direction.East, zone.Rooms[52]);
            ZoneHelper.ConnectRoom(zone.Rooms[52], Direction.East, zone.Rooms[53]);
            ZoneHelper.ConnectRoom(zone.Rooms[53], Direction.Down, zone.Rooms[54]);
            ZoneHelper.ConnectRoom(zone.Rooms[54], Direction.North, zone.Rooms[55]);
            ZoneHelper.ConnectRoom(zone.Rooms[55], Direction.North, zone.Rooms[56]);
            ZoneHelper.ConnectRoom(zone.Rooms[56], Direction.North, zone.Rooms[14]);

            ZoneHelper.ConnectRoom(zone.Rooms[51], Direction.West, zone.Rooms[57]);
            ZoneHelper.ConnectRoom(zone.Rooms[57], Direction.North, zone.Rooms[58]);
            ZoneHelper.ConnectRoom(zone.Rooms[58], Direction.Down, zone.Rooms[59]);
            ZoneHelper.ConnectRoom(zone.Rooms[59], Direction.North, zone.Rooms[60]);

            ZoneHelper.ConnectRoom(zone.Rooms[56], Direction.Up, zone.Rooms[61]);
            ZoneHelper.ConnectRoom(zone.Rooms[61], Direction.South, zone.Rooms[62]);
            ZoneHelper.ConnectRoom(zone.Rooms[62], Direction.Up, zone.Rooms[63]);
            ZoneHelper.ConnectRoom(zone.Rooms[63], Direction.Up, zone.Rooms[64]);
            ZoneHelper.ConnectRoom(zone.Rooms[64], Direction.Up, zone.Rooms[65]);
            ZoneHelper.ConnectRoom(zone.Rooms[65], Direction.West, zone.Rooms[66]);
            ZoneHelper.ConnectRoom(zone.Rooms[66], Direction.West, zone.Rooms[67]);
            ZoneHelper.ConnectRoom(zone.Rooms[67], Direction.North, zone.Rooms[68]);
            ZoneHelper.ConnectRoom(zone.Rooms[68], Direction.North, zone.Rooms[69]);
            ZoneHelper.ConnectRoom(zone.Rooms[69], Direction.Down, zone.Rooms[70]);
            ZoneHelper.ConnectRoom(zone.Rooms[70], Direction.North, zone.Rooms[71]);
            ZoneHelper.ConnectRoom(zone.Rooms[71], Direction.West, zone.Rooms[72]);
            ZoneHelper.ConnectRoom(zone.Rooms[72], Direction.Down, zone.Rooms[73]);
            ZoneHelper.ConnectRoom(zone.Rooms[73], Direction.South, zone.Rooms[74]);
            ZoneHelper.ConnectRoom(zone.Rooms[74], Direction.South, zone.Rooms[75]);
            ZoneHelper.ConnectRoom(zone.Rooms[75], Direction.South, zone.Rooms[76]);
            ZoneHelper.ConnectRoom(zone.Rooms[76], Direction.South, zone.Rooms[77]);
            ZoneHelper.ConnectRoom(zone.Rooms[77], Direction.Up, zone.Rooms[78]);
            ZoneHelper.ConnectRoom(zone.Rooms[78], Direction.North, zone.Rooms[79]);

            ZoneHelper.ConnectRoom(zone.Rooms[77], Direction.East, zone.Rooms[80]);
            ZoneHelper.ConnectRoom(zone.Rooms[80], Direction.East, zone.Rooms[81]);
            ZoneHelper.ConnectRoom(zone.Rooms[81], Direction.Up, zone.Rooms[82]);
            ZoneHelper.ConnectRoom(zone.Rooms[82], Direction.North, zone.Rooms[83]);
            ZoneHelper.ConnectRoom(zone.Rooms[83], Direction.Down, zone.Rooms[84]);
            ZoneHelper.ConnectRoom(zone.Rooms[84], Direction.Down, zone.Rooms[85]);
            ZoneHelper.ConnectRoom(zone.Rooms[85], Direction.Down, zone.Rooms[86]);
            ZoneHelper.ConnectRoom(zone.Rooms[86], Direction.North, zone.Rooms[87]);

            ZoneHelper.ConnectRoom(zone.Rooms[69], Direction.East, zone.Rooms[88]);
            ZoneHelper.ConnectRoom(zone.Rooms[88], Direction.North, zone.Rooms[89]);
            ZoneHelper.ConnectRoom(zone.Rooms[89], Direction.Down, zone.Rooms[90]);
            ZoneHelper.ConnectRoom(zone.Rooms[90], Direction.Down, zone.Rooms[91]);
            ZoneHelper.ConnectRoom(zone.Rooms[91], Direction.Down, zone.Rooms[92]);
            ZoneHelper.ConnectRoom(zone.Rooms[92], Direction.South, zone.Rooms[93]);
            ZoneHelper.ConnectRoom(zone.Rooms[93], Direction.East, zone.Rooms[94]);
            ZoneHelper.ConnectRoom(zone.Rooms[94], Direction.North, zone.Rooms[95]);
            ZoneHelper.ConnectRoom(zone.Rooms[95], Direction.Down, zone.Rooms[96]);
            ZoneHelper.ConnectRoom(zone.Rooms[96], Direction.West, zone.Rooms[97]);
            ZoneHelper.ConnectRoom(zone.Rooms[97], Direction.West, zone.Rooms[98]);

            #endregion Cavern
        }
    }
}
