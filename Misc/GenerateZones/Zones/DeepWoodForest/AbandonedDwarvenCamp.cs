using MiscShared;
using Objects.Damage;
using Objects.Damage.Interface;
using Objects.Global;
using Objects.Item.Items.Interface;
using Objects.LevelRange;
using Objects.Material.Materials;
using Objects.Mob.Interface;
using Objects.Personality;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using System.Linq;
using System.Reflection;
using static Objects.Global.Direction.Directions;
using static Objects.Item.Items.Equipment;
using static Objects.Item.Items.Weapon;
using static Objects.Mob.NonPlayerCharacter;

namespace GenerateZones.Zones.DeepWoodForest
{
    public class AbandonedDwarvenCamp : BaseZone, IZoneCode
    {
        public AbandonedDwarvenCamp() : base(15)
        {
        }

        public IZone Generate()
        {
            Zone.InGameDaysTillReset = 5;
            Zone.Name = nameof(AbandonedDwarvenCamp);

            BuildRoomsViaReflection(this.GetType());

            AddMobs();

            ConnectRooms();

            return Zone;
        }

        private void AddMobs()
        {
            for (int i = 0; i < 50; i++)
            {
                int roomId = GlobalReference.GlobalValues.Random.Next(Zone.Rooms.Count) + 1;
                Zone.Rooms[roomId].AddMobileObjectToRoom(TunnelGoblin(Zone.Rooms[roomId]));
            }
        }


        #region Rooms

        private IRoom GenerateRoom1()
        {
            string examineDescription = "Each statue is larger than life coming in at eight feet tall.  Their axes raised in a salute touching overhead forming the entrance way in which to walk in or out of the camp.";
            string lookDescription = "A pair of dwarven statues are carved into the rock faces.";
            string shortDescription = "Dwarven Mining Camp";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom2()
        {
            string examineDescription = "The cavern begins to open up more here to allow for the larger camp.";
            string lookDescription = "The dwarven mining camp opens to the east and the camp ends with a pair of statues to the west.";
            string shortDescription = "Dwarven Mining Camp";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom3()
        {

            string examineDescription = "The store front was carved out the rock but the right half has been ripped apart like it was a kids Lincoln Logs.";
            string lookDescription = "The old store front of the blacksmith has been partly destroyed.";
            string shortDescription = "Dwarven Mining Camp";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom4()
        {

            string examineDescription = "The smoke carries the smell of meat and vegetables to your nostrils.";
            string lookDescription = "Smoke slowly rolls out of what used to be the old dwarven tavern.";
            string shortDescription = "Dwarven Mining Camp";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom5()
        {

            string examineDescription = "The hole leading down to the water is all that remains.";
            string lookDescription = "Once upon a time there used to be a well here but has since been removed.";
            string shortDescription = "Dwarven Mining Camp";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom6()
        {

            string examineDescription = "Small piles of rubble are all that are left of the stone walls that was one stood high above.";
            string lookDescription = "Piles of rubble, a dwarven anvil and a long dead forge are all that remain of the dwarven blacksmith hall.";
            string shortDescription = "Dwarven Mining Camp";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom7()
        {

            string examineDescription = "The place has been ransacked and looted.  A few empty bottles are all that remain of what was surely a well stocked store.";
            string lookDescription = "You stand in what was a general store of sorts.";
            string shortDescription = "Dwarven Mining Camp";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom8()
        {

            string examineDescription = "Not much remains of the bar.  Just a few broken table and chairs.";
            string lookDescription = "Broken tables are piled in one corner while the booze behind the bar has been emptied.";
            string shortDescription = "Dwarven Mining Camp";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom9()
        {

            string examineDescription = "Peering into the cooking pot revels a stew of sorts slowly bubbling.";
            string lookDescription = "A small cooking fire burns giving light to the room and filling the room with the scent of food.";
            string shortDescription = "Dwarven Mining Camp";
            IRoom room = IndoorRoomLight(examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(Goblin(room));
            room.AddMobileObjectToRoom(Goblin(room));

            return room;
        }

        private IRoom GenerateRoom10()
        {

            string examineDescription = "Faint light comes from one end of the tunnel while inky blackness fills the other end.";
            string lookDescription = "A natural tunnel was revealed when the wall to the bar was broken.";
            string shortDescription = "Cavern Tunnels";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

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

            string examineDescription = "The small tunnel was formed by flowing water long since gone.";
            string lookDescription = "You stand in a small tunnel part of a larger cave system.";
            string shortDescription = "Cavern Tunnels";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }
        #endregion Cavern
        #endregion Rooms

        #region NPC
        private INonPlayerCharacter Goblin(IRoom room)
        {
            string corpseDescription = "A small broken green lies here.";
            string examineDescription = "The goblin stares at you with fright in its eyes.";
            string lookDescription = "The goblin cowers in the corner watching you very intently.";
            string shortDescription = "A frightened goblin.";
            string sentenceDescription = "goblin";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, shortDescription, sentenceDescription, 14, corpseDescription);

            npc.KeyWords.Add("goblin");

            return npc;
        }

        private INonPlayerCharacter TunnelGoblin(IRoom room)
        {
            string corpseDescription = "A small broken green lies here.";
            string examineDescription = "The goblin is armed to the teeth and appears to be on some type of war patrol.";
            string lookDescription = "A well armed goblin appears before you with weapon drawn.";
            string shortDescription = "An armed goblin.";
            string sentenceDescription = "goblin";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 0, corpseDescription);
            npc.LevelRange = new LevelRange() { LowerLevel = 14, UpperLevel = 16 };

            npc.Personalities.Add(new Aggressive());
            npc.Personalities.Add(new Wanderer());

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
            string examineDescription = "The Dogslicer is poorly made and looks like it will fail in combat at some point.";
            string lookDescription = "The sword is crudely made with spots of rust where the iron has gotten wet.";
            string sentenceDescription = "Dogslicer";
            string shortDescription = "The goblin Dogslicer has three holes in the blade making it lighter and easier to swing.";

            IWeapon weapon = CreateWeapon(WeaponType.Sword, 15, examineDescription, lookDescription, sentenceDescription, shortDescription);
            weapon.KeyWords.Add("sword");
            weapon.KeyWords.Add("dog");
            weapon.KeyWords.Add("slicer");


            IDamage damage = new Damage();
            damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(weapon.Level);
            damage.Type = Damage.DamageType.Slash;
            weapon.DamageList.Add(damage);

            return weapon;
        }

        private IEquipment Bracer()
        {
            string examineDescription = "The bracers are fairly plain but are poorly made.";
            string lookDescription = "The bracers extend up the wearers arm a good ways giving the user extra protection.";
            string shortDescription = "A poorly made pair leather bracers.";
            string sentenceDescription = "a pair of leather bracers";

            IArmor armor = CreateArmor(AvalableItemPosition.Arms, 15, examineDescription, lookDescription, sentenceDescription, shortDescription, new Leather());
            armor.KeyWords.Add("leather");
            armor.KeyWords.Add("bracer");

            return armor;
        }

        private IEquipment ChainMail()
        {
            string examineDescription = "The chain mail is a light steel color. It has a few missing links but other wise looks good.";
            string lookDescription = "The chain mail looks to be as utilitarian as protectant.";
            string shortDescription = "A chain mail shirt with a few missing links.";
            string sentenceDescription = "a chain mail shirt";

            IArmor armor = CreateArmor(AvalableItemPosition.Body, 15, examineDescription, lookDescription, sentenceDescription, shortDescription, new Steel());
            armor.KeyWords.Add("chain mail");
            armor.KeyWords.Add("mail");
            armor.KeyWords.Add("shirt");

            return armor;
        }

        private IEquipment Helmet()
        {
            string examineDescription = "The helmet is of good quality for goblin craftsmanship.";
            string lookDescription = "The helmet sits crooked on the user slightly obscuring their vision.";
            string shortDescription = "A steel helmet that is more functional then attractive.";
            string sentenceDescription = "a steel helmet";

            IArmor armor = CreateArmor( AvalableItemPosition.Head, 15, examineDescription, lookDescription, sentenceDescription, shortDescription, new Steel());
            armor.KeyWords.Add("helmet");
            armor.KeyWords.Add("steel");

            return armor;
        }

        private IEquipment LeathPants()
        {
            string examineDescription = "The leather pants have lots of stains from the life in the caves.";
            string lookDescription = "These pants once belonged to a goblin and look well worn.";
            string shortDescription = "Leather pants.";
            string sentenceDescription = "leather pants";

            IArmor armor = CreateArmor(AvalableItemPosition.Legs, 15, examineDescription, lookDescription, sentenceDescription, shortDescription, new Leather());
            armor.KeyWords.Add("leather");
            armor.KeyWords.Add("pants");
            armor.KeyWords.Add("pant");

            return armor;
        }
        #endregion NPC

        private void ConnectRooms()
        {
            ZoneHelper.ConnectZone(Zone.Rooms[1], Direction.West, 13, 108);
            //ZoneHelper.ConnectZone(Zone.Rooms[47], Direction.North, x, x);

            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.East, Zone.Rooms[2]);
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.East, Zone.Rooms[3]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.East, Zone.Rooms[4]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.East, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.East, Zone.Rooms[6]);

            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.North, Zone.Rooms[7]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.South, Zone.Rooms[8]);
            ZoneHelper.ConnectRoom(Zone.Rooms[8], Direction.South, Zone.Rooms[9]);
            ZoneHelper.ConnectRoom(Zone.Rooms[9], Direction.South, Zone.Rooms[10]);

            #region Cavern
            #region Correct Path
            ZoneHelper.ConnectRoom(Zone.Rooms[10], Direction.South, Zone.Rooms[11]);
            ZoneHelper.ConnectRoom(Zone.Rooms[11], Direction.East, Zone.Rooms[12]);
            ZoneHelper.ConnectRoom(Zone.Rooms[12], Direction.East, Zone.Rooms[13]);
            ZoneHelper.ConnectRoom(Zone.Rooms[13], Direction.East, Zone.Rooms[14]);
            ZoneHelper.ConnectRoom(Zone.Rooms[14], Direction.East, Zone.Rooms[15]);
            ZoneHelper.ConnectRoom(Zone.Rooms[15], Direction.Up, Zone.Rooms[16]);
            ZoneHelper.ConnectRoom(Zone.Rooms[16], Direction.Up, Zone.Rooms[17]);
            ZoneHelper.ConnectRoom(Zone.Rooms[17], Direction.West, Zone.Rooms[18]);
            ZoneHelper.ConnectRoom(Zone.Rooms[18], Direction.West, Zone.Rooms[19]);
            ZoneHelper.ConnectRoom(Zone.Rooms[19], Direction.West, Zone.Rooms[20]);
            ZoneHelper.ConnectRoom(Zone.Rooms[20], Direction.North, Zone.Rooms[21]);
            ZoneHelper.ConnectRoom(Zone.Rooms[21], Direction.Down, Zone.Rooms[22]);
            ZoneHelper.ConnectRoom(Zone.Rooms[22], Direction.South, Zone.Rooms[23]);
            ZoneHelper.ConnectRoom(Zone.Rooms[23], Direction.South, Zone.Rooms[24]);
            ZoneHelper.ConnectRoom(Zone.Rooms[24], Direction.East, Zone.Rooms[25]);
            ZoneHelper.ConnectRoom(Zone.Rooms[25], Direction.Up, Zone.Rooms[26]);
            ZoneHelper.ConnectRoom(Zone.Rooms[26], Direction.East, Zone.Rooms[27]);
            ZoneHelper.ConnectRoom(Zone.Rooms[27], Direction.Up, Zone.Rooms[28]);
            ZoneHelper.ConnectRoom(Zone.Rooms[28], Direction.West, Zone.Rooms[29]);
            ZoneHelper.ConnectRoom(Zone.Rooms[29], Direction.West, Zone.Rooms[30]);
            ZoneHelper.ConnectRoom(Zone.Rooms[30], Direction.South, Zone.Rooms[31]);
            ZoneHelper.ConnectRoom(Zone.Rooms[31], Direction.South, Zone.Rooms[32]);
            ZoneHelper.ConnectRoom(Zone.Rooms[32], Direction.Up, Zone.Rooms[33]);
            ZoneHelper.ConnectRoom(Zone.Rooms[33], Direction.East, Zone.Rooms[34]);
            ZoneHelper.ConnectRoom(Zone.Rooms[34], Direction.East, Zone.Rooms[35]);
            ZoneHelper.ConnectRoom(Zone.Rooms[35], Direction.East, Zone.Rooms[36]);
            ZoneHelper.ConnectRoom(Zone.Rooms[36], Direction.Down, Zone.Rooms[37]);
            ZoneHelper.ConnectRoom(Zone.Rooms[37], Direction.North, Zone.Rooms[38]);
            ZoneHelper.ConnectRoom(Zone.Rooms[38], Direction.North, Zone.Rooms[39]);
            ZoneHelper.ConnectRoom(Zone.Rooms[39], Direction.North, Zone.Rooms[40]);
            ZoneHelper.ConnectRoom(Zone.Rooms[40], Direction.North, Zone.Rooms[41]);
            ZoneHelper.ConnectRoom(Zone.Rooms[41], Direction.Up, Zone.Rooms[42]);
            ZoneHelper.ConnectRoom(Zone.Rooms[42], Direction.South, Zone.Rooms[43]);
            ZoneHelper.ConnectRoom(Zone.Rooms[43], Direction.West, Zone.Rooms[44]);
            ZoneHelper.ConnectRoom(Zone.Rooms[44], Direction.Down, Zone.Rooms[45]);
            ZoneHelper.ConnectRoom(Zone.Rooms[45], Direction.North, Zone.Rooms[46]);
            ZoneHelper.ConnectRoom(Zone.Rooms[46], Direction.Down, Zone.Rooms[47]);
            #endregion Correct Path

            ZoneHelper.ConnectRoom(Zone.Rooms[12], Direction.South, Zone.Rooms[48]);
            ZoneHelper.ConnectRoom(Zone.Rooms[48], Direction.South, Zone.Rooms[49]);
            ZoneHelper.ConnectRoom(Zone.Rooms[49], Direction.Up, Zone.Rooms[50]);
            ZoneHelper.ConnectRoom(Zone.Rooms[50], Direction.South, Zone.Rooms[51]);
            ZoneHelper.ConnectRoom(Zone.Rooms[51], Direction.East, Zone.Rooms[52]);
            ZoneHelper.ConnectRoom(Zone.Rooms[52], Direction.East, Zone.Rooms[53]);
            ZoneHelper.ConnectRoom(Zone.Rooms[53], Direction.Down, Zone.Rooms[54]);
            ZoneHelper.ConnectRoom(Zone.Rooms[54], Direction.North, Zone.Rooms[55]);
            ZoneHelper.ConnectRoom(Zone.Rooms[55], Direction.North, Zone.Rooms[56]);
            ZoneHelper.ConnectRoom(Zone.Rooms[56], Direction.North, Zone.Rooms[14]);

            ZoneHelper.ConnectRoom(Zone.Rooms[51], Direction.West, Zone.Rooms[57]);
            ZoneHelper.ConnectRoom(Zone.Rooms[57], Direction.North, Zone.Rooms[58]);
            ZoneHelper.ConnectRoom(Zone.Rooms[58], Direction.Down, Zone.Rooms[59]);
            ZoneHelper.ConnectRoom(Zone.Rooms[59], Direction.North, Zone.Rooms[60]);

            ZoneHelper.ConnectRoom(Zone.Rooms[56], Direction.Up, Zone.Rooms[61]);
            ZoneHelper.ConnectRoom(Zone.Rooms[61], Direction.South, Zone.Rooms[62]);
            ZoneHelper.ConnectRoom(Zone.Rooms[62], Direction.Up, Zone.Rooms[63]);
            ZoneHelper.ConnectRoom(Zone.Rooms[63], Direction.Up, Zone.Rooms[64]);
            ZoneHelper.ConnectRoom(Zone.Rooms[64], Direction.Up, Zone.Rooms[65]);
            ZoneHelper.ConnectRoom(Zone.Rooms[65], Direction.West, Zone.Rooms[66]);
            ZoneHelper.ConnectRoom(Zone.Rooms[66], Direction.West, Zone.Rooms[67]);
            ZoneHelper.ConnectRoom(Zone.Rooms[67], Direction.North, Zone.Rooms[68]);
            ZoneHelper.ConnectRoom(Zone.Rooms[68], Direction.North, Zone.Rooms[69]);
            ZoneHelper.ConnectRoom(Zone.Rooms[69], Direction.Down, Zone.Rooms[70]);
            ZoneHelper.ConnectRoom(Zone.Rooms[70], Direction.North, Zone.Rooms[71]);
            ZoneHelper.ConnectRoom(Zone.Rooms[71], Direction.West, Zone.Rooms[72]);
            ZoneHelper.ConnectRoom(Zone.Rooms[72], Direction.Down, Zone.Rooms[73]);
            ZoneHelper.ConnectRoom(Zone.Rooms[73], Direction.South, Zone.Rooms[74]);
            ZoneHelper.ConnectRoom(Zone.Rooms[74], Direction.South, Zone.Rooms[75]);
            ZoneHelper.ConnectRoom(Zone.Rooms[75], Direction.South, Zone.Rooms[76]);
            ZoneHelper.ConnectRoom(Zone.Rooms[76], Direction.South, Zone.Rooms[77]);
            ZoneHelper.ConnectRoom(Zone.Rooms[77], Direction.Up, Zone.Rooms[78]);
            ZoneHelper.ConnectRoom(Zone.Rooms[78], Direction.North, Zone.Rooms[79]);

            ZoneHelper.ConnectRoom(Zone.Rooms[77], Direction.East, Zone.Rooms[80]);
            ZoneHelper.ConnectRoom(Zone.Rooms[80], Direction.East, Zone.Rooms[81]);
            ZoneHelper.ConnectRoom(Zone.Rooms[81], Direction.Up, Zone.Rooms[82]);
            ZoneHelper.ConnectRoom(Zone.Rooms[82], Direction.North, Zone.Rooms[83]);
            ZoneHelper.ConnectRoom(Zone.Rooms[83], Direction.Down, Zone.Rooms[84]);
            ZoneHelper.ConnectRoom(Zone.Rooms[84], Direction.Down, Zone.Rooms[85]);
            ZoneHelper.ConnectRoom(Zone.Rooms[85], Direction.Down, Zone.Rooms[86]);
            ZoneHelper.ConnectRoom(Zone.Rooms[86], Direction.North, Zone.Rooms[87]);

            ZoneHelper.ConnectRoom(Zone.Rooms[69], Direction.East, Zone.Rooms[88]);
            ZoneHelper.ConnectRoom(Zone.Rooms[88], Direction.North, Zone.Rooms[89]);
            ZoneHelper.ConnectRoom(Zone.Rooms[89], Direction.Down, Zone.Rooms[90]);
            ZoneHelper.ConnectRoom(Zone.Rooms[90], Direction.Down, Zone.Rooms[91]);
            ZoneHelper.ConnectRoom(Zone.Rooms[91], Direction.Down, Zone.Rooms[92]);
            ZoneHelper.ConnectRoom(Zone.Rooms[92], Direction.South, Zone.Rooms[93]);
            ZoneHelper.ConnectRoom(Zone.Rooms[93], Direction.East, Zone.Rooms[94]);
            ZoneHelper.ConnectRoom(Zone.Rooms[94], Direction.North, Zone.Rooms[95]);
            ZoneHelper.ConnectRoom(Zone.Rooms[95], Direction.Down, Zone.Rooms[96]);
            ZoneHelper.ConnectRoom(Zone.Rooms[96], Direction.West, Zone.Rooms[97]);
            ZoneHelper.ConnectRoom(Zone.Rooms[97], Direction.West, Zone.Rooms[98]);

            #endregion Cavern
        }
    }
}
